<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ViewOrder.aspx.cs" Inherits="CREEMESTUDIO.ADMIN.ViewOrder" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadTitle" runat="server">
  <style>
    .page-title{margin:8px 0 16px;font-weight:600;font-size:22px}
    .section{margin-bottom:1.5rem;padding:1rem;border:1px solid #ddd;border-radius:6px}
    .section h4{margin-bottom:.75rem;font-size:18px}
    .table td,.table th{vertical-align:middle}
    .fw-bold{font-weight:600}
  </style>
</asp:Content>

<asp:Content ID="bodyContent" ContentPlaceHolderID="AdminContent" runat="server">
  <h2 class="page-title">Order Details</h2>

  <asp:Panel ID="pnlOrder" runat="server" CssClass="section">
    <h4>General Info</h4>
    <p><span class="fw-bold">Order #:</span> <asp:Label ID="lblOrderId" runat="server" /></p>
    <p><span class="fw-bold">Customer:</span> <asp:Label ID="lblCustomer" runat="server" /></p>
    <p><span class="fw-bold">Date:</span> <asp:Label ID="lblDate" runat="server" /></p>
    <p><span class="fw-bold">Status:</span> <asp:Label ID="lblStatus" runat="server" /></p>
    <p><span class="fw-bold">Payment:</span> <asp:Label ID="lblPayment" runat="server" /></p>
    <p><span class="fw-bold">Coupon:</span> <asp:Label ID="lblCoupon" runat="server" /></p>
    <p><span class="fw-bold">Total:</span> ₹<asp:Label ID="lblTotal" runat="server" /></p>
  </asp:Panel>

  <asp:Panel ID="pnlShip" runat="server" CssClass="section">
    <h4>Shipping Info</h4>
    <p><span class="fw-bold">Name:</span> <asp:Label ID="lblShipName" runat="server" /></p>
    <p><span class="fw-bold">Phone:</span> <asp:Label ID="lblShipPhone" runat="server" /></p>
    <p><span class="fw-bold">Address:</span>
      <asp:Label ID="lblAddr1" runat="server" /> <asp:Label ID="lblAddr2" runat="server" />,
      <asp:Label ID="lblCity" runat="server" /> -
      <asp:Label ID="lblPin" runat="server" />,
      <asp:Label ID="lblState" runat="server" />
    </p>
  </asp:Panel>

  <asp:Panel ID="pnlItems" runat="server" CssClass="section">
    <h4>Items</h4>
    <asp:GridView ID="gvItems" runat="server" CssClass="table table-bordered"
                  AutoGenerateColumns="False">
      <Columns>
        <asp:BoundField DataField="ProductName" HeaderText="Product" />
        <asp:BoundField DataField="BrandName" HeaderText="Brand" />
        <asp:BoundField DataField="Quantity" HeaderText="Qty" />
        <asp:BoundField DataField="UnitPrice" HeaderText="Price (₹)" DataFormatString="{0:N2}" />
        <asp:BoundField DataField="Total" HeaderText="Total (₹)" DataFormatString="{0:N2}" />
      </Columns>
    </asp:GridView>
  </asp:Panel>

  <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="~/ADMIN/ManageOrders.aspx" Text="← Back to Orders" />
</asp:Content>