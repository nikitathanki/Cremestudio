using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;   // <-- for HttpUtility.UrlDecode

namespace CREEMESTUDIO
{
    public partial class Search : System.Web.UI.Page
    {
        // Make sure this matches your Web.config connectionString name
        private readonly string CS =
            ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategories();
                BindBrands();
                BindProducts();
            }
        }

        private void BindCategories()
        {
            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(
                "SELECT CategoryName FROM Categories WHERE IsActive=1 ORDER BY CategoryName", con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                rptCategories.DataSource = dt;
                rptCategories.DataBind();
            }
        }

        private void BindBrands()
        {
            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(@"
                SELECT TOP 5 BrandName
                FROM Products
                WHERE BrandName IS NOT NULL AND LTRIM(RTRIM(BrandName)) <> ''
                GROUP BY BrandName
                ORDER BY COUNT(*) DESC;", con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                rptBrands.DataSource = dt;
                rptBrands.DataBind();
            }
        }

        private void BindProducts()
        {
            // Base query
            string sql = @"
SELECT p.ProductID, p.ProductName, p.Price,
       ISNULL(p.ImageUrl,'') AS ImageUrl,
       ISNULL(p.BrandName,'') AS Brand,
       c.CategoryName
FROM Products p
JOIN Categories c ON p.CategoryID = c.CategoryID
WHERE p.IsActive = 1";

            // Read & DECODE filters (fix for 'Bath & Body', etc.)
            string qRaw = Request.QueryString["q"];
            string catRaw = Request.QueryString["category"];
            string brandRaw = Request.QueryString["brand"];

            string q = string.IsNullOrEmpty(qRaw) ? null : HttpUtility.UrlDecode(qRaw).Trim();
            string cat = string.IsNullOrEmpty(catRaw) ? null : HttpUtility.UrlDecode(catRaw).Trim();
            string brand = string.IsNullOrEmpty(brandRaw) ? null : HttpUtility.UrlDecode(brandRaw).Trim();

            if (!string.IsNullOrWhiteSpace(q))
                sql += " AND (p.ProductName LIKE @q OR ISNULL(p.BrandName,'') LIKE @q)";
            if (!string.IsNullOrWhiteSpace(cat))
                sql += " AND c.CategoryName = @cat";
            if (!string.IsNullOrWhiteSpace(brand))
                sql += " AND p.BrandName = @brand";

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                if (!string.IsNullOrWhiteSpace(q))
                    cmd.Parameters.AddWithValue("@q", "%" + q + "%");
                if (!string.IsNullOrWhiteSpace(cat))
                    cmd.Parameters.AddWithValue("@cat", cat);
                if (!string.IsNullOrWhiteSpace(brand))
                    cmd.Parameters.AddWithValue("@brand", brand);

                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                rptProducts.DataSource = dt;
                rptProducts.DataBind();
                lblCount.Text = dt.Rows.Count == 1 ? "1 item" : $"{dt.Rows.Count} items";
            }
        }
    }
}