<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ManageProducts.aspx.cs" Inherits="CREEMESTUDIO.ADMIN.ManageProducts" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadTitle" runat="server">
    <link href="~/Content/adminproduct.css" rel="stylesheet" type="css" />
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="AdminContent" runat="server">
    <h2 class="page-title">Manage Products</h2>
  <!-- Add / Edit form -->
  <asp:Panel ID="pnlForm" runat="server" CssClass="prod-form">
    <asp:HiddenField ID="hfProductId" runat="server" />

    <div class="row g-3">
      <div class="col-md-4">
        <label class="form-label">Name</label>
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="120" />
      </div>
      <div class="col-md-4">
        <label class="form-label">Brand</label>
        <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control" MaxLength="60" />
      </div>
      <div class="col-md-4">
        <label class="form-label">Category</label>
        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" />
      </div>

      <div class="col-md-3">
        <label class="form-label">Price (₹)</label>
        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" />
      </div>
      <div class="col-md-3">
        <label class="form-label">Stock</label>
        <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" />
      </div>
      <div class="col-md-6">
        <label class="form-label">Image URL</label>
        <asp:TextBox ID="txtImageUrl" runat="server" CssClass="form-control" />
      </div>

      <div class="col-12">
        <label class="form-label">Description</label>
        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
      </div>

      <div class="col-md-3 d-flex align-items-center gap-2">
        <asp:CheckBox ID="chkActive" runat="server" />
        <span>Active</span>
      </div>

      <div class="col-12 d-flex gap-2">
        <asp:Button ID="btnSave" runat="server" Text="Save"
                    CssClass="btn btn-primary" OnClick="btnSave_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear"
                    CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger ms-2" />
      </div>
    </div>
  </asp:Panel>

  <hr />

  <!-- Grid -->
  <h3 class="mt-3">All Products</h3>
  <asp:GridView ID="gvProducts" runat="server"
                CssClass="table table-striped"
                AutoGenerateColumns="False"
                DataKeyNames="ProductID"
                OnRowCommand="gvProducts_RowCommand">
    <Columns>
      <asp:BoundField DataField="ProductID" HeaderText="#" />
      <asp:BoundField DataField="ProductName" HeaderText="Name" />
      <asp:BoundField DataField="BrandName" HeaderText="Brand" />
      <asp:BoundField DataField="CategoryName" HeaderText="Category" />
      <asp:BoundField DataField="Price" HeaderText="Price (₹)" DataFormatString="{0:N2}" />
      <asp:BoundField DataField="Stock" HeaderText="Stock" />
      <asp:CheckBoxField DataField="IsActive" HeaderText="Active" ReadOnly="true" />
      <asp:TemplateField HeaderText="Actions">
        <ItemTemplate>
          <asp:LinkButton ID="cmdEdit" runat="server" CommandName="editrow"
                          CommandArgument='<%# Eval("ProductID") %>' CssClass="btn-link">Edit</asp:LinkButton>
          |
          <asp:LinkButton ID="cmdDel" runat="server" CommandName="deleterow"
                          CommandArgument='<%# Eval("ProductID") %>'
                          OnClientClick="return confirm('Delete this product?');"
                          CssClass="btn-link text-danger">Delete</asp:LinkButton>
        </ItemTemplate>
      </asp:TemplateField>
    </Columns>
  </asp:GridView>
</asp:Content>