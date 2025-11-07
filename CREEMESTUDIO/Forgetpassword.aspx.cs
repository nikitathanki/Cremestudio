using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CREEMESTUDIO
{
    public partial class Forgetpasssword : System.Web.UI.Page
    {
            protected void btnNext_Click(object sender, EventArgs e)
            {
            lblMsg.Text = "";
            lnkReset.Visible = false;

            var email = (txtEmail.Text ?? "").Trim();
            if (email.Length == 0) { lblMsg.Text = "Please enter your email."; return; }

            var cs = ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

            try
            {
                Guid token = Guid.NewGuid();
                DateTime exp = DateTime.UtcNow.AddMinutes(30);

                using (var con = new SqlConnection(cs))
                using (var cmd = new SqlCommand(
                    @"UPDATE Users
                      SET ResetToken = @T, ResetExpiry = @E
                      WHERE Email = @Eml AND IsActive = 1", con))
                {
                    cmd.Parameters.AddWithValue("@T", token);
                    cmd.Parameters.AddWithValue("@E", exp);
                    cmd.Parameters.AddWithValue("@Eml", email);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        lblMsg.Text = "We couldn't find an active account with that email.";
                        return;
                    }
                }

                // Build the reset URL (works on localhost)
                var url = ResolveUrl("~/ResetPassword.aspx?token=" + token);
                lnkReset.NavigateUrl = url;
                lnkReset.Visible = true;

                // For local projects we just show the link.
                lblMsg.Text = "Reset link generated (valid for 30 minutes). Click the link below.";

                // If you later add SMTP, send 'url' by email here.
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Something went wrong. " + ex.Message;
            }
        }
    }
}