using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;

namespace CREEMESTUDIO.ADMIN
{
    public partial class ManageCoupons : Page
    {
        private string CS => ConfigurationManager.ConnectionStrings["CremeDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                Response.Redirect("~/Login.aspx?return=/ADMIN/ManageCoupons.aspx");
                return;
            }
            if (!IsPostBack)
            {
                BindCoupons();
                chkActive.Checked = true;
            }
        }

        private bool IsAdmin()
        {
            var role = (Session["Role"] ?? "").ToString();
            return role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        private void BindCoupons()
        {
            // UsageCount via Orders.Couponcode (text match), safe even if some orders have null/empty
            const string sql = @"
SELECT c.CouponID, c.Code, c.Title, c.DiscountAmt, c.ValidFrom, c.ValidTo, c.IsActive,
       (SELECT COUNT(*) FROM Orders o WHERE UPPER(o.Couponcode) = UPPER(c.Code)) AS UsageCount
FROM Coupons c
ORDER BY c.CouponID DESC;";
            using (var con = new SqlConnection(CS))
            using (var da = new SqlDataAdapter(sql, con))
            {
                var dt = new DataTable();
                da.Fill(dt);
                gvCoupons.DataSource = dt;
                gvCoupons.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMsg.Text = ""; lblMsg.CssClass = "text-danger";

            string code = (txtCode.Text ?? "").Trim().ToUpperInvariant();
            string title = (txtTitle.Text ?? "").Trim();
            bool active = chkActive.Checked;

            if (string.IsNullOrWhiteSpace(code)) { lblMsg.Text = "Code is required."; return; }
            if (!decimal.TryParse((txtAmt.Text ?? "").Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal amount) || amount < 0)
            { lblMsg.Text = "Enter a valid discount amount (₹)."; return; }

            if (!TryParseDateNullable(txtFrom.Text, out DateTime? validFrom) ||
                !TryParseDateNullable(txtTo.Text, out DateTime? validTo))
            { lblMsg.Text = "Enter valid dates for From/To."; return; }
            if (validFrom.HasValue && validTo.HasValue && validFrom.Value.Date > validTo.Value.Date)
            { lblMsg.Text = "Valid From cannot be after Valid To."; return; }

            int? editingId = string.IsNullOrEmpty(hfCouponId.Value) ? (int?)null : Convert.ToInt32(hfCouponId.Value);

            if (IsDuplicateCode(code, editingId)) { lblMsg.Text = "Coupon code already exists."; return; }

            string sql = editingId == null
                ? @"INSERT INTO Coupons (Code, Title, DiscountAmt, ValidFrom, ValidTo, IsActive)
                    VALUES (@Code, @Title, @DiscountAmt, @From, @To, @Active);"
                : @"UPDATE Coupons
                    SET Code=@Code, Title=@Title, DiscountAmt=@DiscountAmt,
                        ValidFrom=@From, ValidTo=@To, IsActive=@Active
                    WHERE CouponID=@Id;";

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 80).Value = code;
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 80).Value = (object)title ?? DBNull.Value;

                var pAmt = cmd.Parameters.Add("@DiscountAmt", SqlDbType.Decimal);
                pAmt.Precision = 10; pAmt.Scale = 2; pAmt.Value = amount;

                var pFrom = cmd.Parameters.Add("@From", SqlDbType.Date);
                pFrom.Value = (object)validFrom ?? DBNull.Value;

                var pTo = cmd.Parameters.Add("@To", SqlDbType.Date);
                pTo.Value = (object)validTo ?? DBNull.Value;

                cmd.Parameters.Add("@Active", SqlDbType.Bit).Value = active;

                if (editingId != null)
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = editingId.Value;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            ClearForm();
            BindCoupons();
            lblMsg.CssClass = "text-success";
            lblMsg.Text = "Saved.";
        }

        protected void gvCoupons_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editrow")
                LoadForEdit(Convert.ToInt32(e.CommandArgument));
            else if (e.CommandName == "deleterow")
                DeleteCoupon(Convert.ToInt32(e.CommandArgument));
        }

        private void LoadForEdit(int id)
        {
            const string sql = @"SELECT CouponID, Code, Title, DiscountAmt, ValidFrom, ValidTo, IsActive
                                 FROM Coupons WHERE CouponID=@Id;";
            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return;
                    hfCouponId.Value = r["CouponID"].ToString();
                    txtCode.Text = r["Code"].ToString();
                    txtTitle.Text = r["Title"].ToString();
                    txtAmt.Text = Convert.ToDecimal(r["DiscountAmt"]).ToString("0.##");
                    txtFrom.Text = r["ValidFrom"] == DBNull.Value ? "" : Convert.ToDateTime(r["ValidFrom"]).ToString("yyyy-MM-dd");
                    txtTo.Text = r["ValidTo"] == DBNull.Value ? "" : Convert.ToDateTime(r["ValidTo"]).ToString("yyyy-MM-dd");
                    chkActive.Checked = Convert.ToBoolean(r["IsActive"]);
                }
            }
        }

        private void DeleteCoupon(int id)
        {
            // We block delete if any order uses this coupon *by code* (Orders.Couponcode)
            string code;
            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand("SELECT Code FROM Coupons WHERE CouponID=@Id;", con))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                code = cmd.ExecuteScalar() as string;
                if (string.IsNullOrEmpty(code)) return;
            }

            const string chk = "SELECT COUNT(*) FROM Orders WHERE UPPER(Couponcode)=UPPER(@Code);";
            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(chk, con))
            {
                cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 80).Value = code;
                con.Open();
                var count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    lblMsg.CssClass = "text-danger";
                    lblMsg.Text = "Cannot delete: this coupon code is used in orders.";
                    return;
                }
            }

            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand("DELETE FROM Coupons WHERE CouponID=@Id;", con))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
            }
            BindCoupons();
            lblMsg.CssClass = "text-success";
            lblMsg.Text = "Coupon deleted.";
        }

        protected void btnClear_Click(object sender, EventArgs e) => ClearForm();

        private void ClearForm()
        {
            hfCouponId.Value = "";
            txtCode.Text = txtTitle.Text = txtAmt.Text = "";
            txtFrom.Text = txtTo.Text = "";
            chkActive.Checked = true;
            lblMsg.Text = "";
        }

        private static bool TryParseDateNullable(string input, out DateTime? value)
        {
            value = null;
            var s = (input ?? "").Trim();
            if (string.IsNullOrEmpty(s)) return true;
            if (DateTime.TryParse(s, out var d)) { value = d.Date; return true; }
            return false;
        }

        private bool IsDuplicateCode(string code, int? skipId)
        {
            const string sql = @"SELECT COUNT(*) FROM Coupons
                                 WHERE UPPER(Code)=UPPER(@Code)
                                 AND (@SkipId IS NULL OR CouponID<>@SkipId);";
            using (var con = new SqlConnection(CS))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 80).Value = code;
                cmd.Parameters.Add("@SkipId", SqlDbType.Int).Value = (object)skipId ?? DBNull.Value;
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
    }
}