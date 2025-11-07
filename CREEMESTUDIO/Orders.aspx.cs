using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace CREEMESTUDIO
{
    public partial class Orders : System.Web.UI.Page
    {
        private string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Require login – prevents NullReference on Session
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx?return=Orders.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            lblMsg.Text = "";
            int userId = Convert.ToInt32(Session["UserID"]);

            const string sql = @"
SELECT 
    OrderID,
    OrderDate,
    Status,
    Subtotal,
    DiscountAmt,
    TotalAmount   -- may be NULL if it's a computed column not filled yet, we handle it in UI
FROM Orders
WHERE UserID = @uid
ORDER BY OrderDate DESC;";

            try
            {
                using (var con = new SqlConnection(CS))
                using (var da = new SqlDataAdapter(sql, con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@uid", userId);
                    var dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        pnlEmpty.Visible = true;
                        pnlList.Visible = false;
                        return;
                    }

                    pnlEmpty.Visible = false;
                    pnlList.Visible = true;
                    rptOrders.DataSource = dt;
                    rptOrders.DataBind();
                }
            }
            catch (Exception ex)
            {
                pnlEmpty.Visible = true;
                pnlList.Visible = false;
                lblMsg.CssClass = "msg err";
                lblMsg.Text = "Error loading orders: " + ex.Message;
            }
        }

        // Bind items for each order row
        protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var row = (DataRowView)e.Item.DataItem;
            int orderId = Convert.ToInt32(row["OrderID"]);
            var rpt = (Repeater)e.Item.FindControl("rptItems");

            const string itemsSql = @"
SELECT 
    oi.Quantity,
    oi.UnitPrice,
    p.ProductName
FROM OrderItems oi
JOIN Products p ON p.ProductID = oi.ProductID
WHERE oi.OrderID = @oid;";

            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(itemsSql, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@oid", orderId);
                var items = new DataTable();
                da.Fill(items);
                rpt.DataSource = items;
                rpt.DataBind();
            }
        }
    }
}