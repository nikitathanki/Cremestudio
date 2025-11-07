<%@ Page Title="My Orders • Crème Studio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="CREEMESTUDIO.Orders" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="Content/orderes.css" rel="stylesheet" />

  <h2 class="page-title">My Orders</h2>
  <asp:Label ID="lblMsg" runat="server" CssClass="msg"></asp:Label>

  <!-- Empty -->
  <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="card empty">
    <p>You don’t have any orders yet.</p>
    <a class="btn-gold" href="Products.aspx">Start shopping</a>
  </asp:Panel>

  <!-- Orders -->
  <asp:Panel ID="pnlList" runat="server" Visible="false">
    <asp:Repeater ID="rptOrders" runat="server" OnItemDataBound="rptOrders_ItemDataBound">
      <ItemTemplate>
        <div class="order-card">
          <div class="order-head">
            <div><strong>Order #</strong><%# Eval("OrderID") %></div>
            <div><strong>Date:</strong> <%# Convert.ToDateTime(Eval("OrderDate")).ToString("dd MMM yyyy HH:mm") %></div>
            <div><strong>Status:</strong> <%# Eval("Status") %></div>
          </div>

          <div class="order-body">
            <asp:Repeater ID="rptItems" runat="server">
              <HeaderTemplate><ul class="items"></HeaderTemplate>
              <ItemTemplate>
                <li class="item">
                  <span class="name"><%# Eval("ProductName") %></span>
                  <span class="qty">× <%# Eval("Quantity") %></span>
                  <span class="price">₹ <%# string.Format("{0:N2}", Eval("UnitPrice")) %></span>
                  <span class="line">= ₹ <%# string.Format("{0:N2}", Convert.ToDecimal(Eval("Quantity")) * Convert.ToDecimal(Eval("UnitPrice"))) %></span>
                </li>
              </ItemTemplate>
              <FooterTemplate></ul></FooterTemplate>
            </asp:Repeater>
          </div>

          <div class="order-foot">
            <div>Subtotal: ₹ <%# string.Format("{0:N2}", Eval("Subtotal")) %></div>
            <div>Discount: ₹ <%# string.Format("{0:N2}", Eval("DiscountAmt")) %></div>
            <div class="total">
              Total: ₹
              <%# Eval("TotalAmount") is DBNull ? 
                    string.Format("{0:N2}", Convert.ToDecimal(Eval("Subtotal")) - Convert.ToDecimal(Eval("DiscountAmt"))) :
                    string.Format("{0:N2}", Eval("TotalAmount")) %>
            </div>
          </div>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </asp:Panel>
</asp:Content>