<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="EditOrder.aspx.cs" Inherits="CREEMESTUDIO.ADMIN.EditOrder" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadTitle" runat="server">
    Edit Order
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="AdminContent" runat="server">
    <h2 class="page-title">Edit Order</h2>

    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger"></asp:Label>

    <div class="form-group">
        <label>Status</label>
        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
            <asp:ListItem>Placed</asp:ListItem>
            <asp:ListItem>Processing</asp:ListItem>
            <asp:ListItem>Shipped</asp:ListItem>
            <asp:ListItem>Delivered</asp:ListItem>
            <asp:ListItem>Cancelled</asp:ListItem>
            <asp:ListItem>Refunded</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="form-group">
        <label>Ship Name</label>
        <asp:TextBox ID="txtShipName" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Ship Phone</label>
        <asp:TextBox ID="txtShipPhone" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Address Line 1</label>
        <asp:TextBox ID="txtAddr1" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Address Line 2</label>
        <asp:TextBox ID="txtAddr2" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>City</label>
        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>State</label>
        <asp:TextBox ID="txtState" runat="server" CssClass="form-control" />
    </div>

    <div class="form-group">
        <label>Pincode</label>
        <asp:TextBox ID="txtPin" runat="server" CssClass="form-control" />
    </div>

    <asp:Button ID="btnSave" runat="server" Text="Save Changes"
        CssClass="btn btn-primary mt-3"
        OnClick="SaveOrder_Click" />
</asp:Content>