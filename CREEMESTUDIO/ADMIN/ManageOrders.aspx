<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ManageOrders.aspx.cs" Inherits="CREEMESTUDIO.ADMIN.ManageOrders" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadTitle" runat="server">
  <style>
    .page-title{margin:8px 0 16px;font-weight:600;font-size:22px}
    .table td,.table th{vertical-align:middle}
    .btn-link{border:0;background:none;padding:0;cursor:pointer}
    .btn-link.text-danger{color:#c00}
    .text-success{color:green}.text-danger{color:red}
    .mt-3{margin-top:1rem}
    .ms-2{margin-left:.5rem}
    .gap-2{gap:.5rem}
  </style>
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="AdminContent" runat="server">
  <h2 class="page-title">Manage Orders</h2>

  <asp:GridView ID="gvOrders" runat="server"
      CssClass="table table-striped"
      AutoGenerateColumns="False"
      DataKeyNames="OrderID"
      OnRowCommand="gvOrders_RowCommand">
    <Columns>
      <asp:BoundField DataField="OrderID" HeaderText="Order #" />
      <asp:BoundField DataField="FullName" HeaderText="Customer" />
      <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
      <asp:BoundField DataField="TotalAmount" HeaderText="Total (₹)" DataFormatString="{0:N2}" />
      <asp:BoundField DataField="Couponcode" HeaderText="Coupon" />
      <asp:BoundField DataField="Status" HeaderText="Status" />
      <asp:BoundField DataField="PaymentMethod" HeaderText="Payment" />
      <asp:BoundField DataField="ShipPhone" HeaderText="Phone" />
      <asp:BoundField DataField="City" HeaderText="City" />
      <asp:TemplateField HeaderText="Actions">
        <ItemTemplate>
          <asp:LinkButton ID="cmdView" runat="server" CommandName="viewrow"
                          CommandArgument='<%# Eval("OrderID") %>' CssClass="btn-link">View</asp:LinkButton>
          |
          <asp:LinkButton ID="cmdEdit" runat="server" CommandName="editrow"
                          CommandArgument='<%# Eval("OrderID") %>' CssClass="btn-link">Update</asp:LinkButton>
          |
          <asp:LinkButton ID="cmdDel" runat="server" CommandName="deleterow"
                          CommandArgument='<%# Eval("OrderID") %>'
                          OnClientClick="return confirm('Delete this order?');"
                          CssClass="btn-link text-danger">Delete</asp:LinkButton>
        </ItemTemplate>
      </asp:TemplateField>
    </Columns>
  </asp:GridView>

  <asp:Label ID="lblMsg" runat="server" CssClass="text-danger mt-3" />
</asp:Content>