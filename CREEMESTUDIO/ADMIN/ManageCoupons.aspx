<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ManageCoupons.aspx.cs" Inherits="CREEMESTUDIO.ADMIN.ManageCoupons" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadTitle" runat="server">
  <style>
    .page-title{margin:8px 0 16px;font-weight:600;font-size:22px}
    .coupon-form{background:#fff;padding:16px;border:1px solid #eee;border-radius:10px;margin-bottom:16px}
    .coupon-form .form-label{font-weight:500}
    .table td,.table th{vertical-align:middle}
    .btn-link{border:0;background:none;padding:0;cursor:pointer}
    .btn-link.text-danger{color:#c00}
    .text-success{color:green}.text-danger{color:red}
    .mt-3{margin-top:1rem}.ms-2{margin-left:.5rem}.gap-2{gap:.5rem}
    .small-hint{font-size:12px;color:#666}
  </style>
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="AdminContent" runat="server">
  <h2 class="page-title">Manage Coupons</h2>

  <asp:Panel ID="pnlForm" runat="server" CssClass="coupon-form">
    <asp:HiddenField ID="hfCouponId" runat="server" />
    <div class="row g-3">
      <div class="col-md-3">
        <label class="form-label">Code</label>
        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="80" />
        <div class="small-hint">Example: FESTIVE10 (unique, UPPERCASE)</div>
      </div>
      <div class="col-md-5">
        <label class="form-label">Title</label>
        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" MaxLength="80" />
      </div>
      <div class="col-md-2">
        <label class="form-label">Discount (₹)</label>
        <asp:TextBox ID="txtAmt" runat="server" CssClass="form-control" />
      </div>
      <div class="col-md-2 d-flex align-items-center gap-2">
        <asp:CheckBox ID="chkActive" runat="server" />
        <span>Active</span>
      </div>

      <div class="col-md-3">
        <label class="form-label">Valid From</label>
        <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" TextMode="Date" />
      </div>
      <div class="col-md-3">
        <label class="form-label">Valid To</label>
        <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" TextMode="Date" />
      </div>

      <div class="col-12 d-flex gap-2">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger ms-2" />
      </div>
    </div>
  </asp:Panel>

  <hr />

  <h3 class="mt-3">All Coupons</h3>
  <asp:GridView ID="gvCoupons" runat="server"
    CssClass="table table-striped"
    AutoGenerateColumns="False"
    DataKeyNames="CouponID"
    OnRowCommand="gvCoupons_RowCommand">
    <Columns>
      <asp:BoundField DataField="CouponID" HeaderText="#" />
      <asp:BoundField DataField="Code" HeaderText="Code" />
      <asp:BoundField DataField="Title" HeaderText="Title" />
      <asp:BoundField DataField="DiscountAmt" HeaderText="Discount (₹)" DataFormatString="{0:N2}" />
      <asp:BoundField DataField="ValidFrom" HeaderText="From" DataFormatString="{0:yyyy-MM-dd}" />
      <asp:BoundField DataField="ValidTo" HeaderText="To" DataFormatString="{0:yyyy-MM-dd}" />
      <asp:CheckBoxField DataField="IsActive" HeaderText="Active" ReadOnly="true" />
      <asp:BoundField DataField="UsageCount" HeaderText="Used (orders)" />
      <asp:TemplateField HeaderText="Actions">
        <ItemTemplate>
          <asp:LinkButton ID="cmdEdit" runat="server" CommandName="editrow" CommandArgument='<%# Eval("CouponID") %>' CssClass="btn-link">Edit</asp:LinkButton>
          |
          <asp:LinkButton ID="cmdDel" runat="server" CommandName="deleterow" CommandArgument='<%# Eval("CouponID") %>'
            OnClientClick="return confirm('Delete this coupon? Blocked if used in any order.');"
            CssClass="btn-link text-danger">Delete</asp:LinkButton>
        </ItemTemplate>
      </asp:TemplateField>
    </Columns>
  </asp:GridView>
</asp:Content>