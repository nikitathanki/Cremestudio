using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CREEMESTUDIO
{
    public partial class Cart : Page
    {
        private string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try { BindCart(); }
                catch (Exception ex)
                {
                    lblMsg.CssClass = "text-danger";
                    lblMsg.Text = "Cart load failed: " + ex.Message;
                    pnlCart.Visible = false;
                    pnlEmpty.Visible = true;
                }
            }
        }

        private void BindCart()
        {
            int cartId = EnsureCartId();   // returns 0 if not logged in → empty state
            if (cartId == 0) { pnlCart.Visible = false; pnlEmpty.Visible = true; return; }

            const string sql = @"
SELECT
    ci.CartItemID,
    p.ProductID,
    p.ProductName,
    p.ImageUrl,
    CAST(ci.UnitPrice AS decimal(18,2)) AS UnitPrice,
    ci.Quantity,
    CAST(ci.UnitPrice * ci.Quantity AS decimal(18,2)) AS LineTotal
FROM CartItems ci
JOIN Products p ON p.ProductID = ci.ProductID
WHERE ci.CartID = @cid
ORDER BY ci.CartItemID DESC;";

            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(sql, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@cid", cartId);

                var dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    pnlCart.Visible = false;
                    pnlEmpty.Visible = true;
                    lblMsg.Text = "";
                    return;
                }

                pnlEmpty.Visible = false;
                pnlCart.Visible = true;

                rptCart.DataSource = dt;
                rptCart.DataBind();

                decimal subtotal = 0m;
                foreach (DataRow r in dt.Rows)
                    subtotal += Convert.ToDecimal(r["LineTotal"]);

                lblSubtotal.Text = subtotal.ToString("N2");
                lblTotal.Text = subtotal.ToString("N2");
                lblMsg.Text = "";
            }
        }

        protected void rptCart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument); // CartItems.CartItemID

            if (e.CommandName == "inc")
                ExecNonQuery("UPDATE CartItems SET Quantity = Quantity + 1 WHERE CartItemID=@id;", "@id", id);
            else if (e.CommandName == "dec")
                ExecNonQuery(@"
UPDATE CartItems
SET Quantity = CASE WHEN Quantity > 1 THEN Quantity - 1 ELSE 1 END
WHERE CartItemID=@id;", "@id", id);
            else if (e.CommandName == "del")
                ExecNonQuery("DELETE FROM CartItems WHERE CartItemID=@id;", "@id", id);

            BindCart();
        }

        private int EnsureCartId()
        {
            if (Session["UserID"] == null) return 0;

            int uid = Convert.ToInt32(Session["UserID"]);
            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM Carts WHERE UserID=@u)
  SELECT CartID FROM Carts WHERE UserID=@u;
ELSE BEGIN
  INSERT INTO Carts(UserID, CreatedAt) VALUES(@u, SYSUTCDATETIME());
  SELECT SCOPE_IDENTITY();
END", con))
            {
                cmd.Parameters.AddWithValue("@u", uid);
                con.Open();
                int cartId = Convert.ToInt32(cmd.ExecuteScalar());
                Session["CartID"] = cartId;
                return cartId;
            }
        }

        private void ExecNonQuery(string sql, string pName, object pValue)
        {
            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue(pName, pValue);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}