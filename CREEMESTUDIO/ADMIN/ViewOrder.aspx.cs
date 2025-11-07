using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace CREEMESTUDIO.ADMIN
{
    public partial class ViewOrder : Page
    {
        private string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                Response.Redirect("~/Login.aspx?return=/ADMIN/ViewOrder.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["id"], out int id))
                    LoadOrder(id);
                else
                    Response.Redirect("~/ADMIN/ManageOrders.aspx");
            }
        }

        private bool IsAdmin()
        {
            var role = (Session["Role"] ?? "").ToString();
            return role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        private void LoadOrder(int id)
        {
            const string sql = @"
SELECT o.OrderID, o.OrderDate, o.TotalAmount, o.Couponcode, o.Status, o.PaymentMethod,
       o.ShipName, o.ShipPhone, o.AddressLine1, o.AddressLine2, o.City, o.Pincode, o.State,
       u.FullName
FROM Orders o
JOIN Users u ON o.UserID = u.UserID
WHERE o.OrderID=@Id;";

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return;

                    lblOrderId.Text = r["OrderID"].ToString();
                    lblCustomer.Text = r["FullName"].ToString();
                    lblDate.Text = Convert.ToDateTime(r["OrderDate"]).ToString("yyyy-MM-dd");
                    lblStatus.Text = r["Status"].ToString();
                    lblPayment.Text = r["PaymentMethod"].ToString();
                    lblCoupon.Text = r["Couponcode"].ToString();
                    lblTotal.Text = Convert.ToDecimal(r["TotalAmount"]).ToString("0.00");

                    lblShipName.Text = r["ShipName"].ToString();
                    lblShipPhone.Text = r["ShipPhone"].ToString();
                    lblAddr1.Text = r["AddressLine1"].ToString();
                    lblAddr2.Text = r["AddressLine2"].ToString();
                    lblCity.Text = r["City"].ToString();
                    lblPin.Text = r["Pincode"].ToString();
                    lblState.Text = r["State"].ToString();
                }
            }

            // Items
            const string sqlItems = @"
SELECT p.ProductName, p.BrandName, oi.Quantity,
       (oi.Quantity * oi.UnitPrice) AS Total,
       oi.UnitPrice
FROM OrderItems oi
JOIN Products p ON oi.ProductID = p.ProductID
WHERE oi.OrderID=@Id;";

            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(sqlItems, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@Id", id);
                var dt = new DataTable();
                da.Fill(dt);
                gvItems.DataSource = dt;
                gvItems.DataBind();
            }
        }
    }
}