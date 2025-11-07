using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.Configuration;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CREEMESTUDIO
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If already logged in, optionally bounce to their landing page.
            if (!IsPostBack && Session["UserID"] != null)
            {
                // Prevent infinite loops when return=/Login.aspx
                var returnUrl = (Request.QueryString["return"] ?? string.Empty).ToLower();
                if (!returnUrl.Contains("login.aspx"))
                {
                    RedirectAfterLogin(Session["Role"] as string, returnUrl);
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            var email = (txtEmail.Text ?? "").Trim();
            var pass = (txtPass.Text ?? "").Trim();

            if (email.Length == 0 || pass.Length == 0)
            {
                lblError.Text = "Please enter email and password.";
                return;
            }

            var cs = ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

            try
            {
                using (var con = new SqlConnection(cs))
                using (var cmd = new SqlCommand(
                    @"SELECT TOP 1 UserID, FullName, Role, PasswordHash
                      FROM Users
                      WHERE Email = @E AND IsActive = 1", con))
                {
                    cmd.Parameters.AddWithValue("@E", email);
                    con.Open();

                    using (var rd = cmd.ExecuteReader())
                    {
                        if (!rd.Read())
                        {
                            lblError.Text = "Invalid email or password.";
                            return;
                        }

                        // NOTE: You are storing plaintext in PasswordHash in this project.
                        // If you later hash, replace this check with a proper hash-verify.
                        var dbPass = rd["PasswordHash"].ToString();
                        if (!string.Equals(dbPass, pass))
                        {
                            lblError.Text = "Invalid email or password.";
                            return;
                        }

                        // Success → set session
                        Session["UserID"] = rd["UserID"].ToString();
                        Session["FullName"] = rd["FullName"].ToString();
                        Session["Role"] = rd["Role"].ToString();
                    }
                }

                // Decide where to go
                var returnUrl = Request.QueryString["return"];
                RedirectAfterLogin(Session["Role"] as string, returnUrl);
            }
            catch (Exception ex)
            {
                lblError.Text = "Something went wrong. " + ex.Message;
            }
        }

        private void RedirectAfterLogin(string role, string returnUrl)
        {
            // Safe custom return (and avoid loops)
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                var lower = returnUrl.ToLower();
                if (!lower.Contains("login.aspx") && lower.StartsWith("/"))
                {
                    Response.Redirect(returnUrl);
                    return;
                }
            }

            // Default by role
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
                Response.Redirect("~/ADMIN/Dashboard.aspx");
            else
                Response.Redirect("Home.aspx");
        }
    }
}