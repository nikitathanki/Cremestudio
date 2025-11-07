using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace CREEMESTUDIO
{
    public partial class Invoice : System.Web.UI.Page
    {
        string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (!int.TryParse(Request.QueryString["id"], out int id))
            { lblMsg.Text = "Missing order id."; return; }

            LoadInvoice(id);
        }

        private void LoadInvoice(int orderId)
        {
            const string headSql = @"SELECT TOP 1 *
                                     FROM Orders WHERE OrderID=@id;";
            const string itemsSql = @"
SELECT oi.OrderItemID, p.ProductName, oi.Quantity, oi.UnitPrice,
       CAST(oi.Quantity*oi.UnitPrice AS decimal(10,2)) AS LineTotal
FROM OrderItems oi
JOIN Products p ON oi.ProductID=p.ProductID
WHERE oi.OrderID=@id;";

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(headSql, con))
            {
                cmd.Parameters.AddWithValue("@id", orderId);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) { lblMsg.Text = "Order not found."; return; }

                    pnl.Visible = true;
                    lblId.Text = r["OrderID"].ToString();
                    lblDate.Text = Convert.ToDateTime(r["OrderDate"]).ToString("dd MMM yyyy HH:mm");
                    lblStatus.Text = r["Status"].ToString();
                    lblPay.Text = r["PaymentMethod"].ToString();

                    // shipping address
                    var sb = new StringBuilder();
                    sb.AppendLine(r["AddressLine1"].ToString());
                    if (r["AddressLine2"] != DBNull.Value && r["AddressLine2"].ToString() != "")
                        sb.AppendLine(r["AddressLine2"].ToString());
                    sb.AppendLine($"{r["City"]}, {r["State"]} {r["Pincode"]}");
                    lblName.Text = r["ShipName"].ToString();
                    lblAddr.Text = sb.ToString().Replace(Environment.NewLine, "<br/>");
                    lblAddr.Text = sb.ToString().Replace("\n", "<br/>");

                    decimal sub = Convert.ToDecimal(r["Subtotal"], CultureInfo.InvariantCulture);
                    decimal disc = Convert.ToDecimal(r["DiscountAmt"], CultureInfo.InvariantCulture);
                    lblSub.Text = sub.ToString("N2");
                    lblDisc.Text = disc.ToString("N2");
                    lblTotal.Text = (sub - disc).ToString("N2");
                }
                // items
                using (var da = new SqlDataAdapter(itemsSql, con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@id", orderId);
                    var dt = new DataTable();
                    da.Fill(dt);
                    rpt.DataSource = dt;
                    rpt.DataBind();
                }
            }
        }
    }
}