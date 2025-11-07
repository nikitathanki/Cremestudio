using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CREEMESTUDIO
{
    public partial class Checkout : System.Web.UI.Page
    {
        private string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // require login
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx?returnUrl=Checkout.aspx");
                return;
            }
            if (!IsPostBack) LoadSummary();
        }

        private void LoadSummary()
        {
            lblMsg.Text = "";
            int userId = Convert.ToInt32(Session["UserID"]);

            const string sql = @"
SELECT ci.CartItemID, p.ProductName, ci.Quantity, ci.UnitPrice,
       CAST(ci.Quantity * ci.UnitPrice AS decimal(10,2)) AS LineTotal
FROM CartItems ci
JOIN Carts c   ON ci.CartID = c.CartID
JOIN Products p ON ci.ProductID = p.ProductID
WHERE c.UserID = @UserID
ORDER BY ci.CartItemID DESC;";

            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(sql, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@UserID", userId);
                var dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlEmpty.Visible = true;
                    pnlCheckout.Visible = false;
                    return;
                }

                rptItems.DataSource = dt;
                rptItems.DataBind();

                // totals
                decimal sub = 0m;
                foreach (DataRow r in dt.Rows)
                    sub += Convert.ToDecimal(r["LineTotal"], CultureInfo.InvariantCulture);

                decimal disc = 0m; // apply real coupon logic if needed
                lblSub.Text = sub.ToString("N2");
                lblDisc.Text = disc.ToString("N2");
                lblTotal.Text = (sub - disc).ToString("N2");

                pnlEmpty.Visible = false;
                pnlCheckout.Visible = true;
            }
        }

        protected void btnPlace_Click(object sender, EventArgs e)
        {
            lblMsg.CssClass = "msg";
            lblMsg.Text = "";

            if (!ValidateForm())
            {
                lblMsg.CssClass = "msg err";
                lblMsg.Text = "Please fill all required fields.";
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            string payMethod = ddlPay.SelectedValue;
            string coupon = txtCoupon.Text.Trim();
            string shipName = txtName.Text.Trim();
            string shipPhone = txtPhone.Text.Trim();
            string addr1 = txtAddr1.Text.Trim();
            string addr2 = txtAddr2.Text.Trim();
            string city = txtCity.Text.Trim();
            string state = txtState.Text.Trim();
            string pin = txtPin.Text.Trim();

            // get numbers again
            decimal subtotal, discount;
            if (!decimal.TryParse(lblSub.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out subtotal))
                subtotal = 0m;
            if (!decimal.TryParse(lblDisc.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out discount))
                discount = 0m;

            using (var con = new SqlConnection(CS))
            {
                con.Open();
                using (var tx = con.BeginTransaction())
                using (var cmd = con.CreateCommand())
                {
                    cmd.Transaction = tx;
                    try
                    {
                        // 1) Insert Order (TotalAmount is computed in DB; do not touch)
                        cmd.CommandText = @"
INSERT INTO Orders
(UserID, OrderDate, Status, PaymentMethod, CouponCode, Subtotal, DiscountAmt,
 ShipName, ShipPhone, AddressLine1, AddressLine2, City, State, Pincode, CreatedAt)
VALUES
(@UserID, SYSUTCDATETIME(), @Status, @Method, @Coupon, @Subtotal, @Discount,
 @ShipName, @ShipPhone, @Addr1, @Addr2, @City, @State, @Pin, SYSUTCDATETIME());
SELECT CAST(SCOPE_IDENTITY() AS int);";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@Status", "Placed");
                        cmd.Parameters.AddWithValue("@Method", payMethod);
                        cmd.Parameters.AddWithValue("@Coupon", string.IsNullOrWhiteSpace(coupon) ? (object)DBNull.Value : coupon);
                        cmd.Parameters.AddWithValue("@Subtotal", subtotal);
                        cmd.Parameters.AddWithValue("@Discount", discount);
                        cmd.Parameters.AddWithValue("@ShipName", shipName);
                        cmd.Parameters.AddWithValue("@ShipPhone", (object)shipPhone ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Addr1", addr1);
                        cmd.Parameters.AddWithValue("@Addr2", string.IsNullOrWhiteSpace(addr2) ? (object)DBNull.Value : addr2);
                        cmd.Parameters.AddWithValue("@City", city);
                        cmd.Parameters.AddWithValue("@State", state);
                        cmd.Parameters.AddWithValue("@Pin", pin);

                        int orderId = (int)cmd.ExecuteScalar();

                        // 2) Insert OrderItems from the user's cart
                        cmd.CommandText = @"
INSERT INTO OrderItems (OrderID, ProductID, Quantity, UnitPrice)
SELECT @OrderID, ci.ProductID, ci.Quantity, ci.UnitPrice
FROM CartItems ci
JOIN Carts c ON ci.CartID = c.CartID
WHERE c.UserID = @UserID;";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@OrderID", orderId);
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.ExecuteNonQuery();

                        // 3) Clear the cart (items + cart row)
                        cmd.CommandText = @"
DELETE ci FROM CartItems ci JOIN Carts c ON ci.CartID=c.CartID WHERE c.UserID=@UserID;
DELETE FROM Carts WHERE UserID=@UserID;";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.ExecuteNonQuery();

                        tx.Commit();

                        // go to My Orders
                        Response.Redirect("~/Orders.aspx?placed=" + orderId);
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        lblMsg.CssClass = "msg err";
                        lblMsg.Text = "Order failed: " + ex.Message;
                    }
                }
            }
        }

        private bool ValidateForm()
        {
            bool ok = true;
            if (string.IsNullOrWhiteSpace(txtName.Text)) ok = false;
            if (string.IsNullOrWhiteSpace(txtAddr1.Text)) ok = false;
            if (string.IsNullOrWhiteSpace(txtCity.Text)) ok = false;
            if (string.IsNullOrWhiteSpace(txtState.Text)) ok = false;
            if (string.IsNullOrWhiteSpace(txtPin.Text)) ok = false;
            return ok;
        }
    }
}