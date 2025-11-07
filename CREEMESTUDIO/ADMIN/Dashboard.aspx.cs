using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;


namespace CREEMESTUDIO.ADMIN
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only Admins allowed
            var role = (Session["role"] ?? "").ToString();
            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("/Login.aspx?return=/Admin/Dashboard.aspx");
                return;
            }

            if (!IsPostBack) LoadDashboard();
        }

        void LoadDashboard()
        {
            try
            {
                var connStr =
                    (WebConfigurationManager.ConnectionStrings["CremeDb"] ??
                     WebConfigurationManager.ConnectionStrings["CS"] ??
                     WebConfigurationManager.ConnectionStrings["CremestudioDb"]).ConnectionString;

                using (var con = new SqlConnection(connStr))
                {
                    con.Open();

                    // --- Summary numbers (1 round-trip) ---
                    using (var cmd = new SqlCommand(@"
SELECT 
  (SELECT COUNT(*) FROM dbo.Users WHERE IsActive=1) AS UsersCount,
  (SELECT COUNT(*) FROM dbo.Products WHERE IsActive=1) AS ProductsCount,
  (SELECT COUNT(*) FROM dbo.Orders) AS OrdersCount,
  (SELECT ISNULL(SUM(TotalAmount),0) FROM dbo.Orders) AS RevenueTotal;", con))
                    {
                        using (var rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                lblUsers.Text = Convert.ToInt32(rd["UsersCount"]).ToString();
                                lblProducts.Text = Convert.ToInt32(rd["ProductsCount"]).ToString();
                                lblOrders.Text = Convert.ToInt32(rd["OrdersCount"]).ToString();
                                lblRevenue.Text = Convert
                                   .ToDecimal(rd["RevenueTotal"]).ToString("0.##");
                            }
                        }
                    }

                    // --- Recent orders (TOP 8) ---
                    using (var cmd = new SqlCommand(@"
SELECT TOP 8 
   o.OrderID, o.OrderDate, o.TotalAmount,
   u.FullName,
   ISNULL(p.Status,'Pending') AS PayStatus
FROM dbo.Orders o
JOIN dbo.Users  u ON u.UserID = o.UserID
LEFT JOIN dbo.Payments p ON p.OrderID = o.OrderID
ORDER BY o.OrderDate DESC, o.OrderID DESC;", con))
                    {
                        var dt = new DataTable();
                        new SqlDataAdapter(cmd).Fill(dt);
                        rpRecent.DataSource = dt;
                        rpRecent.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error loading dashboard: " + ex.Message;
            }
        }
    }
}