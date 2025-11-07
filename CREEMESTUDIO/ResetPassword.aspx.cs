using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;

namespace CREEMESTUDIO
{

    public partial class Reset : System.Web.UI.Page
    {
        private Guid _token;
        private int _userId;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            // Validate token in query
            var t = Request.QueryString["token"];
            if (!Guid.TryParse(t, out _token))
            {
                lblMsg.Text = "Invalid reset link.";
                return;
            }

            if (!IsPostBack)
            {
                // Verify token in DB
                var cs = ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

                using (var con = new SqlConnection(cs))
                using (var cmd = new SqlCommand(
                    @"SELECT TOP 1 UserID
                      FROM Users
                      WHERE ResetToken = @T
                        AND ResetExpiry IS NOT NULL
                        AND ResetExpiry > SYSUTCDATETIME()", con))
                {
                    cmd.Parameters.AddWithValue("@T", _token);
                    con.Open();

                    var r = cmd.ExecuteScalar();
                    if (r == null)
                    {
                        lblMsg.Text = "This reset link is invalid or has expired.";
                        pnlForm.Visible = false;
                        return;
                    }

                    _userId = Convert.ToInt32(r);
                    ViewState["uid"] = _userId;   // persist across postback
                    pnlForm.Visible = true;
                }
            }
            else
            {
                // on postback, read from ViewState
                _userId = (ViewState["uid"] != null) ? (int)ViewState["uid"] : 0;
                if (_userId == 0)
                {
                    lblMsg.Text = "Session expired. Please generate the link again.";
                    pnlForm.Visible = false;
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            var p1 = (txtPass1.Text ?? "").Trim();
            var p2 = (txtPass2.Text ?? "").Trim();

            if (p1.Length < 6) { lblMsg.Text = "Password must be at least 6 characters."; return; }
            if (p1 != p2) { lblMsg.Text = "Passwords do not match."; return; }

            var cs = ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

            try
            {
                using (var con = new SqlConnection(cs))
                using (var cmd = new SqlCommand(
                    @"UPDATE Users
                      SET PasswordHash = @PW,       -- plain for this project; hash later
                          ResetToken = NULL,
                          ResetExpiry = NULL
                      WHERE UserID = @ID AND ResetToken = @T", con))
                {
                    cmd.Parameters.AddWithValue("@PW", p1);
                    cmd.Parameters.AddWithValue("@ID", (int)ViewState["uid"]);
                    cmd.Parameters.AddWithValue("@T", _token);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                    {
                        lblMsg.Text = "Reset failed. The link may have expired.";
                        return;
                    }
                }

                lblMsg.Text = "Password updated. You can now sign in.";
                pnlForm.Visible = false;
                Response.AddHeader("REFRESH", "2;URL=Login.aspx");
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Something went wrong. " + ex.Message;
            }
        }
    }
}