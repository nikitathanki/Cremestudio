using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.Configuration;
using System.Configuration;

namespace CREEMESTUDIO
{
    public partial class Registration : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["UserId"] != null)
            {
                // already signed in — go home
                Response.Redirect("~/Home.aspx");
            }
        }

        protected void btnReg_Click(object sender, EventArgs e)
        {
            // 1) simple validation
            if (txtPass.Text.Length < 6) { lblMsg.Text = "Password must be at least 6 characters."; return; }
            if (!string.Equals(txtPass.Text, txtPass2.Text)) { lblMsg.Text = "Passwords do not match."; return; }

            var cs = ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

            try
            {
                using (var con = new SqlConnection(cs))
                {
                    con.Open();

                    // 2) email unique?
                    using (var chk = new SqlCommand("SELECT COUNT(1) FROM Users WHERE Email=@E", con))
                    {
                        chk.Parameters.AddWithValue("@E", txtEmail.Text.Trim());
                        if ((int)chk.ExecuteScalar() > 0)
                        {
                            lblMsg.Text = "Email already registered.";
                            return;
                        }
                    }

                    // 3) insert user with selected role
                    using (var cmd = new SqlCommand(
                        @"INSERT INTO Users (FullName,Email,Phone,PasswordHash,Role,CreatedAt,IsActive)
                          VALUES (@N,@E,@P,@PW,@R,sysutcdatetime(),1);
                          SELECT SCOPE_IDENTITY();", con))
                    {
                        cmd.Parameters.AddWithValue("@N", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@E", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@P", string.IsNullOrWhiteSpace(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@PW", txtPass.Text); // TODO: replace with a proper hash
                        cmd.Parameters.AddWithValue("@R", ddlRole.SelectedValue);

                        var newId = Convert.ToInt32(cmd.ExecuteScalar());

                        // 4) auto-login (Session)
                        Session["UserID"] = newId.ToString();
                        Session["FullName"] = txtName.Text.Trim();
                        Session["Role"] = ddlRole.SelectedValue;

                        // 5) redirect by role
                        if (string.Equals(ddlRole.SelectedValue, "Admin", StringComparison.OrdinalIgnoreCase))
                            Response.Redirect("~/ADMIN/Dashboard.aspx");
                        else
                            Response.Redirect("~/Home.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Something went wrong. " + ex.Message;
            }
        }
    }
}