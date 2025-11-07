using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CREEMESTUDIO.ADMIN
{
    public partial class ManageProducts : System.Web.UI.Page
    {
        private string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // simple guard: only admins
            if (!IsAdmin())
            {
                Response.Redirect("~/Login.aspx?return=/ADMIN/ManageProducts.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCategories();
                BindProducts();
            }
        }

        private bool IsAdmin()
        {
            var role = (Session["Role"] ?? "").ToString();
            return role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        /* ---------- Data Bind ---------- */

        private void LoadCategories()
        {
            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter("SELECT CategoryID, CategoryName FROM Categories WHERE IsActive=1 ORDER BY CategoryName", con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
            }
        }

        private void BindProducts()
        {
            const string sql = @"
SELECT p.ProductID, p.ProductName, p.BrandName, p.Price, p.Stock, p.IsActive,
       c.CategoryName
FROM Products p
JOIN Categories c ON p.CategoryID = c.CategoryID
ORDER BY p.ProductID DESC;";

            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(sql, con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        /* ---------- CRUD ---------- */

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            // validate numbers
            if (!decimal.TryParse(txtPrice.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var price) || price < 0)
            { lblMsg.Text = "Enter a valid price."; return; }

            if (!int.TryParse(txtStock.Text.Trim(), out var stock) || stock < 0)
            { lblMsg.Text = "Enter a valid stock."; return; }

            int catId = int.Parse(ddlCategory.SelectedValue);
            bool active = chkActive.Checked;

            using (var con = new SqlConnection(CS))
            using (var cmd = con.CreateCommand())
            {
                con.Open();

                if (string.IsNullOrEmpty(hfProductId.Value))
                {
                    // INSERT
                    cmd.CommandText = @"
INSERT INTO Products (CategoryID, ProductName, BrandName, Description, Price, Stock, ImageUrl, IsActive, CreatedAt)
VALUES (@CategoryID, @Name, @Brand, @Desc, @Price, @Stock, @ImageUrl, @IsActive, SYSUTCDATETIME());";
                }
                else
                {
                    // UPDATE
                    cmd.CommandText = @"
UPDATE Products
SET CategoryID=@CategoryID, ProductName=@Name, BrandName=@Brand, Description=@Desc,
    Price=@Price, Stock=@Stock, ImageUrl=@ImageUrl, IsActive=@IsActive
WHERE ProductID=@Id;";
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(hfProductId.Value));
                }

                cmd.Parameters.AddWithValue("@CategoryID", catId);
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Brand", (object)txtBrand.Text.Trim() ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Desc", (object)txtDesc.Text.Trim() ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Stock", stock);
                cmd.Parameters.AddWithValue("@ImageUrl", (object)txtImageUrl.Text.Trim() ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", active);

                cmd.ExecuteNonQuery();
            }

            ClearForm();
            BindProducts();
            lblMsg.CssClass = "text-success";
            lblMsg.Text = "Saved.";
        }

        protected void gvProducts_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editrow")
            {
                LoadForEdit(Convert.ToInt32(e.CommandArgument));
            }
            else if (e.CommandName == "deleterow")
            {
                DeleteProduct(Convert.ToInt32(e.CommandArgument));
            }
        }

        private void LoadForEdit(int id)
        {
            const string sql = @"
SELECT ProductID, CategoryID, ProductName, BrandName, Description, Price, Stock, ImageUrl, IsActive
FROM Products WHERE ProductID=@Id;";

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return;

                    hfProductId.Value = r["ProductID"].ToString();
                    ddlCategory.SelectedValue = r["CategoryID"].ToString();
                    txtName.Text = r["ProductName"].ToString();
                    txtBrand.Text = r["BrandName"].ToString();
                    txtDesc.Text = r["Description"].ToString();
                    txtPrice.Text = Convert.ToDecimal(r["Price"]).ToString("0.##");
                    txtStock.Text = r["Stock"].ToString();
                    txtImageUrl.Text = r["ImageUrl"].ToString();
                    chkActive.Checked = Convert.ToBoolean(r["IsActive"]);
                }
            }
        }

        private void DeleteProduct(int id)
        {
            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand("DELETE FROM Products WHERE ProductID=@Id;", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            BindProducts();
        }

        protected void btnClear_Click(object sender, EventArgs e) => ClearForm();

        private void ClearForm()
        {
            hfProductId.Value = "";
            txtName.Text = txtBrand.Text = txtDesc.Text = txtImageUrl.Text = "";
            txtPrice.Text = txtStock.Text = "";
            chkActive.Checked = true;
            lblMsg.Text = "";
        }
    }
}