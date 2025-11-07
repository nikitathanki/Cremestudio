using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CREEMESTUDIO
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        private string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["id"], out int pid))
                    LoadProduct(pid);
                else
                    phNotFound.Visible = true;
            }
        }

        private void LoadProduct(int pid)
        {
            string sql = "SELECT * FROM Products WHERE ProductID=@id AND IsActive=1";
            using (SqlConnection con = new SqlConnection(CS))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", pid);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                    phProduct.Visible = true;
                    lblName.Text = r["ProductName"].ToString();
                    lblBrand.Text = r["BrandName"].ToString();
                    lblPrice.Text = Convert.ToDecimal(r["Price"]).ToString("0.00");
                    lblDesc.Text = r["Description"].ToString();
                    lblAvail.Text = (Convert.ToInt32(r["Stock"]) > 0) ? "In stock" : "Out of stock";
                    imgProd.ImageUrl = r["ImageUrl"].ToString();
                    ViewState["Price"] = Convert.ToDecimal(r["Price"]);
                    ViewState["ProductID"] = pid;
                }
                else
                {
                    phNotFound.Visible = true;
                }
            }
        }

        protected void btnAddCart_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                int userId = Convert.ToInt32(Session["UserID"]);
                int productId = (int)ViewState["ProductID"];
                int qty = int.TryParse(txtQty.Text, out int q) ? q : 1;
                decimal price = (decimal)ViewState["Price"];

                int cartId;
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT CartID FROM Carts WHERE UserID=@u", con);
                    cmd.Parameters.AddWithValue("@u", userId);
                    object cid = cmd.ExecuteScalar();
                    if (cid == null)
                    {
                        SqlCommand cNew = new SqlCommand("INSERT INTO Carts(UserID,CreatedAt) OUTPUT INSERTED.CartID VALUES(@u,SYSUTCDATETIME())", con);
                        cNew.Parameters.AddWithValue("@u", userId);
                        cartId = (int)cNew.ExecuteScalar();
                    }
                    else cartId = Convert.ToInt32(cid);

                    SqlCommand ci = new SqlCommand(@"
IF EXISTS (SELECT 1 FROM CartItems WHERE CartID=@c AND ProductID=@p)
    UPDATE CartItems SET Quantity=Quantity+@q WHERE CartID=@c AND ProductID=@p
ELSE
    INSERT INTO CartItems(CartID,ProductID,Quantity,UnitPrice)
    VALUES(@c,@p,@q,@pr)", con);

                    ci.Parameters.AddWithValue("@c", cartId);
                    ci.Parameters.AddWithValue("@p", productId);
                    ci.Parameters.AddWithValue("@q", qty);
                    ci.Parameters.AddWithValue("@pr", price);
                    ci.ExecuteNonQuery();
                }
                lblMsg.CssClass = "ok"; lblMsg.Text = "Added to cart ✓";
            }
            catch (Exception ex) { lblMsg.CssClass = "err"; lblMsg.Text = "Error: " + ex.Message; }
        }

        protected void btnBuyNow_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                int userId = Convert.ToInt32(Session["UserID"]);
                int productId = (int)ViewState["ProductID"];
                int qty = int.TryParse(txtQty.Text, out int q) ? q : 1;
                decimal price = (decimal)ViewState["Price"];
                decimal subtotal = qty * price;

                int orderId;
                using (SqlConnection con = new SqlConnection(CS))
                {
                    con.Open();
                    SqlCommand oNew = new SqlCommand(@"
INSERT INTO Orders(UserID, OrderDate, Status, PaymentMethod, Subtotal, DiscountAmt, ShipName, ShipPhone, AddressLine1, City, State, Pincode, CreatedAt)
OUTPUT INSERTED.OrderID
VALUES(@u, SYSUTCDATETIME(),'Placed','COD',@sub,0,'Test User','9999999999','Default Address','City','State','000000',SYSUTCDATETIME())", con);

                    oNew.Parameters.AddWithValue("@u", userId);
                    oNew.Parameters.AddWithValue("@sub", subtotal);
                    orderId = (int)oNew.ExecuteScalar();

                    SqlCommand oi = new SqlCommand("INSERT INTO OrderItems(OrderID,ProductID,Quantity,UnitPrice) VALUES(@o,@p,@q,@pr)", con);
                    oi.Parameters.AddWithValue("@o", orderId);
                    oi.Parameters.AddWithValue("@p", productId);
                    oi.Parameters.AddWithValue("@q", qty);
                    oi.Parameters.AddWithValue("@pr", price);
                    oi.ExecuteNonQuery();
                }
                Response.Redirect("~/Checkout.aspx");
            }
            catch (Exception ex) { lblMsg.CssClass = "err"; lblMsg.Text = "Buy Now failed: " + ex.Message; }
        }
    }
}