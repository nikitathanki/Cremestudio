using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace CREEMESTUDIO.ADMIN
{
    public partial class ManageOrders : Page
    {
        private string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                Response.Redirect("~/Login.aspx?return=/ADMIN/ManageOrders.aspx");
                return;
            }
            if (!IsPostBack) BindOrders();
        }

        private bool IsAdmin()
        {
            var role = (Session["Role"] ?? "").ToString();
            return role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        private void BindOrders()
        {
            const string sql = @"
SELECT o.OrderID, o.OrderDate, o.TotalAmount, o.Couponcode, o.Status,
       o.PaymentMethod, o.ShipPhone, o.City, u.FullName
FROM Orders o
JOIN Users u ON o.UserID = u.UserID
ORDER BY o.OrderID DESC;";

            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(sql, con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                gvOrders.DataSource = dt;
                gvOrders.DataBind();
            }
        }

        protected void gvOrders_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int orderId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "viewrow")
            {
                Response.Redirect("~/ADMIN/ViewOrder.aspx?id=" + orderId);
            }
            else if (e.CommandName == "editrow")
            {
                Response.Redirect("~/ADMIN/EditOrder.aspx?id=" + orderId);
            }
            else if (e.CommandName == "deleterow")
            {
                DeleteOrder(orderId);
            }
        }

        private void DeleteOrder(int id)
        {
            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand("DELETE FROM Orders WHERE OrderID=@Id;", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            BindOrders();
            lblMsg.CssClass = "text-success";
            lblMsg.Text = "Order deleted.";
        }
    }
}