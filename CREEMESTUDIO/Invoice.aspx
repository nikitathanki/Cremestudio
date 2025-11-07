<%@ Page Title="Invoice • Crème Studio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="CREEMESTUDIO.Invoice" %>


<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="Content/invoice.css" rel="stylesheet" />
  <section class="container inv-wrap">
    <asp:Label ID="lblMsg" runat="server" CssClass="msg"></asp:Label>

    <asp:Panel ID="pnl" runat="server" Visible="false" CssClass="inv-card">
      <div class="inv-head">
        <div>
          <h2>Crème Studio</h2>
          <small>Luxury beauty, thoughtfully curated.</small>
        </div>
        <div class="inv-meta">
          <div><strong>Invoice</strong> <asp:Label ID="lblId" runat="server" /></div>
          <div><strong>Date</strong> <asp:Label ID="lblDate" runat="server" /></div>
          <div><strong>Status</strong> <asp:Label ID="lblStatus" runat="server" /></div>
        </div>
      </div>

      <div class="inv-ship">
        <div><strong>Ship To</strong><br />
          <asp:Label ID="lblName" runat="server" /><br />
          <asp:Label ID="lblAddr" runat="server" />
        </div>
        <div><strong>Payment</strong><br />
          <asp:Label ID="lblPay" runat="server" />
        </div>
      </div>

      <asp:Repeater ID="rpt" runat="server">
        <HeaderTemplate>
          <table class="inv-tab"><thead>
            <tr><th>Product</th><th class="t-r">Qty</th><th class="t-r">Price</th><th class="t-r">Amount</th></tr>
          </thead><tbody>
        </HeaderTemplate>
        <ItemTemplate>
          <tr>
            <td><%# Eval("ProductName") %></td>
            <td class="t-r"><%# Eval("Quantity") %></td>
            <td class="t-r">₹ <%# Eval("UnitPrice","{0:N2}") %></td>
            <td class="t-r">₹ <%# Eval("LineTotal","{0:N2}") %></td>
          </tr>
        </ItemTemplate>
        <FooterTemplate></tbody></table></FooterTemplate>
      </asp:Repeater>

      <div class="inv-total">
        <div><span>Subtotal</span><strong>₹ <asp:Label ID="lblSub" runat="server" /></strong></div>
        <div><span>Discount</span><strong>₹ <asp:Label ID="lblDisc" runat="server" /></strong></div>
        <div class="grand"><span>Total</span><strong>₹ <asp:Label ID="lblTotal" runat="server" /></strong></div>
      </div>

      <div class="mt-2">
        <button class="btn-outline" onclick="window.print()">Print</button>
        <a class="btn-gold" href="Orders.aspx">Back to Orders</a>
      </div>
    </asp:Panel>
  </section>
</asp:Content>