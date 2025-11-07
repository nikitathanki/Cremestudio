using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace CREEMESTUDIO.ADMIN
{
    public partial class ManageCategories : Page
    {
        private string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                Response.Redirect("~/Login.aspx?return=/ADMIN/ManageCategories.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindCategories();
                chkActive.Checked = true;
            }
        }

        private bool IsAdmin()
        {
            var role = (Session["Role"] ?? "").ToString();
            return role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        /* ---------- Data Bind ---------- */

        private void BindCategories()
        {
            const string sql = @"
SELECT c.CategoryID, c.CategoryName, c.Description, c.IsActive,
       (SELECT COUNT(*) FROM Products p WHERE p.CategoryID = c.CategoryID) AS ProductCount
FROM Categories c
ORDER BY c.CategoryID DESC;";

            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(sql, con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                gvCategories.DataSource = dt;
                gvCategories.DataBind();
            }
        }

        /* ---------- CRUD ---------- */

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            var name = (txtName.Text ?? "").Trim();
            var desc = (txtDesc.Text ?? "").Trim();
            var active = chkActive.Checked;

            if (string.IsNullOrWhiteSpace(name))
            {
                lblMsg.Text = "Category name is required.";
                return;
            }

            // check duplicate (case-insensitive)
            if (IsDuplicateName(name, string.IsNullOrEmpty(hfCategoryId.Value) ? (int?)null : Convert.ToInt32(hfCategoryId.Value)))
            {
                lblMsg.Text = "Category with the same name already exists.";
                return;
            }

            using (var con = new SqlConnection(CS))
            using (var cmd = con.CreateCommand())
            {
                con.Open();

                if (string.IsNullOrEmpty(hfCategoryId.Value))
                {
                    cmd.CommandText = @"
INSERT INTO Categories (CategoryName, Description, IsActive)
VALUES (@Name, @Desc, @IsActive);";
                }
                else
                {
                    cmd.CommandText = @"
UPDATE Categories
SET CategoryName=@Name, Description=@Desc, IsActive=@IsActive
WHERE CategoryID=@Id;";
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(hfCategoryId.Value));
                }

                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Desc", (object)desc ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", active);

                cmd.ExecuteNonQuery();
            }

            ClearForm();
            BindCategories();
            lblMsg.CssClass = "text-success";
            lblMsg.Text = "Saved.";
        }

        private bool IsDuplicateName(string name, int? skipId)
        {
            const string sql = @"
SELECT COUNT(*) FROM Categories
WHERE LOWER(CategoryName) = LOWER(@Name)
AND (@SkipId IS NULL OR CategoryID <> @SkipId);";

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@SkipId", (object)skipId ?? DBNull.Value);
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        protected void gvCategories_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editrow")
            {
                LoadForEdit(Convert.ToInt32(e.CommandArgument));
            }
            else if (e.CommandName == "deleterow")
            {
                DeleteCategory(Convert.ToInt32(e.CommandArgument));
            }
        }

        private void LoadForEdit(int id)
        {
            const string sql = @"
SELECT CategoryID, CategoryName, Description, IsActive
FROM Categories WHERE CategoryID=@Id;";

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return;

                    hfCategoryId.Value = r["CategoryID"].ToString();
                    txtName.Text = r["CategoryName"].ToString();
                    txtDesc.Text = r["Description"].ToString();
                    chkActive.Checked = Convert.ToBoolean(r["IsActive"]);
                }
            }
        }

        private void DeleteCategory(int id)
        {
            // block delete if products exist
            const string chk = "SELECT COUNT(*) FROM Products WHERE CategoryID=@Id;";
            const string del = "DELETE FROM Categories WHERE CategoryID=@Id;";

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(chk, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                var count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    lblMsg.CssClass = "text-danger";
                    lblMsg.Text = "Cannot delete: products exist in this category.";
                    return;
                }
            }

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(del, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            BindCategories();
            lblMsg.CssClass = "text-success";
            lblMsg.Text = "Category deleted.";
        }

        protected void btnClear_Click(object sender, EventArgs e) => ClearForm();

        private void ClearForm()
        {
            hfCategoryId.Value = "";
            txtName.Text = txtDesc.Text = "";
            chkActive.Checked = true;
            lblMsg.Text = "";
        }
    }
}