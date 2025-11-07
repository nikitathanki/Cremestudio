<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Track.aspx.cs" Inherits="CREEMESTUDIO.Track" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="Content/orders.css" rel="stylesheet" />
  <section class="container mt-2">
    <h2 class="page-title">Track Order</h2>
    <asp:Label ID="lblMsg" runat="server" CssClass="msg"></asp:Label>

    <div class="track-box">
      <label>Enter Order #</label>
      <div class="d-flex gap-2">
        <asp:TextBox ID="txtOrder" runat="server" CssClass="form-control" />
        <asp:Button ID="btnFind" runat="server" CssClass="btn-gold" Text="Track" OnClick="btnFind_Click" />
      </div>
    </div>

    <asp:Panel ID="pnl" runat="server" Visible="false" CssClass="order-card">
      <h4>Order#<asp:Label ID="lblId" runat="server" /></h4>
      <div>Date: <asp:Label ID="lblDate" runat="server" /></div>
      <div>Status: <asp:Label ID="lblStatus" runat="server" CssClass="status" /></div>
      <div>Total: ₹ <asp:Label ID="lblTotal" runat="server" /></div>
<asp:HyperLink ID="lnkInvoice" runat="server" CssClass="btn-outline" Text="View Invoice"></asp:HyperLink>
        
    </asp:Panel>
  </section>
</asp:Content>