<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ManageCategories.aspx.cs" Inherits="CREEMESTUDIO.ADMIN.ManageCategories" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadTitle" runat="server">
    <style>
        .page-title {
            margin: 8px 0 16px;
            font-weight: 600;
            font-size: 22px;
        }
        .cat-form {
            background: #fff;
            padding: 16px;
            border: 1px solid #eee;
            border-radius: 10px;
            margin-bottom: 16px;
        }
        .cat-form .form-label {
            font-weight: 500;
        }
        .table td, .table th {
            vertical-align: middle;
        }
        .btn-link {
            border: 0;
            background: none;
            padding: 0;
            cursor: pointer;
        }
        .btn-link.text-danger {
            color: #c00;
        }
        .text-success { color: green; }
        .text-danger { color: red; }
        .mt-3 { margin-top: 1rem; }
        .ms-2 { margin-left: .5rem; }
        .gap-2 { gap: .5rem; }
    </style>
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="AdminContent" runat="server">
    <h2 class="page-title">Manage Categories</h2>

    <!-- Add / Edit form -->
    <asp:Panel ID="pnlForm" runat="server" CssClass="cat-form">
        <asp:HiddenField ID="hfCategoryId" runat="server" />

        <div class="row g-3">
            <div class="col-md-5">
                <label class="form-label">Category Name</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="80" />
            </div>
            <div class="col-md-7">
                <label class="form-label">Description</label>
                <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
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
    <h3 class="mt-3">All Categories</h3>
    <asp:GridView ID="gvCategories" runat="server"
        CssClass="table table-striped"
        AutoGenerateColumns="False"
        DataKeyNames="CategoryID"
        OnRowCommand="gvCategories_RowCommand">
        <Columns>
            <asp:BoundField DataField="CategoryID" HeaderText="#" />
            <asp:BoundField DataField="CategoryName" HeaderText="Name" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:CheckBoxField DataField="IsActive" HeaderText="Active" ReadOnly="true" />
            <asp:BoundField DataField="ProductCount" HeaderText="Products" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:LinkButton ID="cmdEdit" runat="server" CommandName="editrow"
                        CommandArgument='<%# Eval("CategoryID") %>' CssClass="btn-link">Edit</asp:LinkButton>
                    |
                    <asp:LinkButton ID="cmdDel" runat="server" CommandName="deleterow"
                        CommandArgument='<%# Eval("CategoryID") %>'
                        OnClientClick="return confirm('Delete this category? This will be blocked if products exist.');"
                        CssClass="btn-link text-danger">Delete</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>