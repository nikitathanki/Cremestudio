using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace CREEMESTUDIO
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBrands();                             // fill brands first  
                                                          // honor any existing selections from QS  
                string brand = Request.QueryString["brand"] ?? "";
                string sort = (Request.QueryString["sort"] ?? "new").ToLower();
                if (ddlBrand.Items.FindByValue(brand) != null) ddlBrand.SelectedValue = brand;
                if (ddlSort.Items.FindByValue(sort) != null) ddlSort.SelectedValue = sort; LoadProducts(ddlBrand.SelectedValue, ddlSort.SelectedValue);
            }
        }

        void BindBrands()
        {
            string dept = (Request.QueryString["dept"] ?? "").ToLower().Trim();
            string catName = null;
            switch (dept)
            {
                case "makeup": catName = "Makeup"; break;
                case "skin": catName = "Skin"; break;
                case "hair": catName = "Hair"; break;
                case "fragrance": catName = "Fragrance"; break;
                case "bath": catName = "Bath & Body"; break;
            }

            using (var con = Db.GetConnection())
            {
                con.Open();

                // If a category tab is active → show brands only from that category.      
                // If no tab (dept empty) → show all brands sitewide.      
                string sql = @"      
SELECT BrandName FROM (      
    SELECT '' AS BrandName        -- value for 'All Brands'      
    UNION ALL      
    SELECT DISTINCT RTRIM(LTRIM(p.BrandName))      
    FROM dbo.Products p      
    INNER JOIN dbo.Categories c ON c.CategoryID = p.CategoryID      
    WHERE p.IsActive = 1 {0}      
) B      
ORDER BY CASE WHEN BrandName='' THEN 0 ELSE 1 END, BrandName;";


                sql = string.Format(sql, string.IsNullOrEmpty(catName) ? "" : "AND c.CategoryName = @cat");

                using (var cmd = new SqlCommand(sql, con))
                {
                    if (!string.IsNullOrEmpty(catName))
                        cmd.Parameters.AddWithValue("@cat", catName);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);

                        ddlBrand.DataSource = dt;
                        ddlBrand.DataTextField = "BrandName";
                        ddlBrand.DataValueField = "BrandName";
                        ddlBrand.DataBind();

                        // Make first item display nicely      
                        if (ddlBrand.Items.Count > 0 && ddlBrand.Items[0].Value == "")
                            ddlBrand.Items[0].Text = "All Brands";
                    }
                }
            }

        }

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e) => ReloadWithFilters();
        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e) => ReloadWithFilters();

        void ReloadWithFilters()
        {
            // preserve current dept when changing brand/sort
            var qs = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            qs["brand"] = ddlBrand.SelectedValue;             // "" means all
            qs["sort"] = ddlSort.SelectedValue;

            string url = "Products.aspx";
            string query = qs.ToString();
            Response.Redirect(string.IsNullOrEmpty(query) ? url : (url + "?" + query));

        }

        private void LoadProducts(string brand = null, string sort = null)
        {
            string dept = (Request.QueryString["dept"] ?? "").ToLower().Trim();

            string catName = null;
            switch (dept)
            {
                case "makeup": catName = "Makeup"; break;
                case "skin": catName = "Skincare"; break;
                case "hair": catName = "Hair"; break;
                case "fragrance": catName = "Fragrance"; break;
                case "bath": catName = "Bath & Body"; break;
            }

            try
            {
                using (SqlConnection con = Db.GetConnection())
                {
                    con.Open();

                    string sql =
                        @"SELECT p.ProductID, p.ProductName, p.BrandName, p.Price, p.ImageUrl      
              FROM Products p      
              INNER JOIN Categories c ON p.CategoryID = c.CategoryID      
              WHERE p.IsActive = 1 {0} {1}      
              ORDER BY {2};";

                    string whereExtra = "";
                    string orderBy = "p.CreatedAt DESC";

                    if (!string.IsNullOrEmpty(catName))
                        whereExtra = "AND c.CategoryName = @cat";

                    if (!string.IsNullOrEmpty(brand))
                        whereExtra += " AND p.BrandName = @brand";

                    if (!string.IsNullOrEmpty(sort))
                    {
                        switch (sort)
                        {
                            case "plh": orderBy = "p.Price ASC"; break;
                            case "phl": orderBy = "p.Price DESC"; break;
                            case "az": orderBy = "p.ProductName ASC"; break;
                            default: orderBy = "p.CreatedAt DESC"; break;
                        }
                    }

                    sql = string.Format(sql, whereExtra, "", orderBy);
                    SqlCommand cmd = new SqlCommand(sql, con);

                    if (!string.IsNullOrEmpty(catName))
                        cmd.Parameters.AddWithValue("@cat", catName);
                    if (!string.IsNullOrEmpty(brand))
                        cmd.Parameters.AddWithValue("@brand", brand);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        rptProducts.DataSource = dt;
                        rptProducts.DataBind();

                        lblMsg.Text = (dt.Rows.Count == 0) ? "No products found." : "";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error: " + ex.Message;
            }

        }

    }

}