using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CREEMESTUDIO
{
    public partial class Track : System.Web.UI.Page
    {
        string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void btnFind_Click(object sender, EventArgs e)
        {
            lblMsg.Text = ""; pnl.Visible = false;
            if (!int.TryParse(txtOrder.Text.Trim(), out int id)) { lblMsg.Text = "Enter a valid order number."; return; }

            const string sql = @"SELECT OrderID, OrderDate, Status, Subtotal, DiscountAmt, (Subtotal-DiscountAmt) AS Total
                                 FROM Orders WHERE OrderID=@id";
            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        pnl.Visible = true;
                        lblId.Text = r["OrderID"].ToString();
                        lblDate.Text = Convert.ToDateTime(r["OrderDate"]).ToString("dd-MMM-yyyy HH:mm");
                        lblStatus.Text = r["Status"].ToString();
                        lblTotal.Text = r["Subtotal"].ToString();

                        // ✅ Set invoice link dynamically
                        lnkInvoice.NavigateUrl = "Invoice.aspx?id=" + lblId.Text;
                    }
                }
            }
            pnl.Visible = true;
        }
    }
}