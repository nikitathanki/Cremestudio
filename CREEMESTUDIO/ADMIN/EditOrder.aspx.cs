using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CREEMESTUDIO.ADMIN
{
    public partial class EditOrder : System.Web.UI.Page
    {
        private string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int orderId = int.Parse(Request.QueryString["id"]);
                    ViewState["OrderID"] = orderId;
                    LoadOrder(orderId);
                }
                else
                {
                    lblMsg.Text = "Order ID missing.";
                    lblMsg.CssClass = "text-danger";
                }
            }
        }

        private void LoadOrder(int id)
        {
            const string sql = @"SELECT OrderID, Status, ShipName, ShipPhone, 
                                        AddressLine1, AddressLine2, City, State, Pincode
                                 FROM Orders WHERE OrderID=@Id";

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        ddlStatus.SelectedValue = r["Status"].ToString();
                        txtShipName.Text = r["ShipName"].ToString();
                        txtShipPhone.Text = r["ShipPhone"].ToString();
                        txtAddr1.Text = r["AddressLine1"].ToString();
                        txtAddr2.Text = r["AddressLine2"].ToString();
                        txtCity.Text = r["City"].ToString();
                        txtState.Text = r["State"].ToString();
                        txtPin.Text = r["Pincode"].ToString();
                    }
                }
            }
        }

        protected void SaveOrder_Click(object sender, EventArgs e)
        {
            if (ViewState["OrderID"] == null)
            {
                lblMsg.Text = "Order not found.";
                lblMsg.CssClass = "text-danger";
                return;
            }

            int id = (int)ViewState["OrderID"];

            const string sql = @"
UPDATE Orders
SET Status=@Status,
    ShipName=@ShipName,
    ShipPhone=@ShipPhone,
    AddressLine1=@Addr1,
    AddressLine2=@Addr2,
    City=@City,
    State=@State,
    Pincode=@Pin
WHERE OrderID=@Id;";

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@ShipName", txtShipName.Text.Trim());
                cmd.Parameters.AddWithValue("@ShipPhone", txtShipPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@Addr1", txtAddr1.Text.Trim());
                cmd.Parameters.AddWithValue("@Addr2", txtAddr2.Text.Trim());
                cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                cmd.Parameters.AddWithValue("@Pin", txtPin.Text.Trim());
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMsg.Text = "Order updated successfully.";
            lblMsg.CssClass = "text-success";
        }
    }
}