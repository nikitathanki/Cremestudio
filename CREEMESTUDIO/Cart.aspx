<%@ Page Title="" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="CREEMESTUDIO.Cart" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <!-- External CSS for Cart -->
  <link href="Content/cart.css" rel="stylesheet" />

  <asp:Label ID="lblMsg" runat="server" />

  <!-- Empty cart -->
  <asp:Panel ID="pnlEmpty" runat="server" Visible="false">
    <h2>Your cart is empty</h2>
    <a href="Products.aspx" class="btn-gold">Continue shopping</a>
  </asp:Panel>

  <!-- Cart -->
  <asp:Panel ID="pnlCart" runat="server" Visible="false">
    <h2>Your Cart</h2>
    <asp:Repeater ID="rptCart" runat="server" OnItemCommand="rptCart_ItemCommand">
      <ItemTemplate>
        <div class="cart-item">
          <span class="prod-name"><%# Eval("ProductName") %></span>
          <span class="prod-price">₹ <%# Eval("UnitPrice") %></span>
          <span class="prod-qty">× <%# Eval("Quantity") %></span>
          <span class="line-total">= ₹ <%# Eval("LineTotal") %></span>

          <asp:LinkButton ID="btnDec" runat="server" CommandName="dec"
              CommandArgument='<%# Eval("CartItemID") %>' CssClass="btn-qty">−</asp:LinkButton>
          <asp:LinkButton ID="btnInc" runat="server" CommandName="inc"
              CommandArgument='<%# Eval("CartItemID") %>' CssClass="btn-qty">+</asp:LinkButton>
          <asp:LinkButton ID="btnDel" runat="server" CommandName="del"
              CommandArgument='<%# Eval("CartItemID") %>' CssClass="btn-remove">Remove</asp:LinkButton>
        </div>
      </ItemTemplate>
    </asp:Repeater>

    <div class="cart-summary">
      Subtotal: ₹ <asp:Label ID="lblSubtotal" runat="server" />
      <br />
      Total: ₹ <asp:Label ID="lblTotal" runat="server" />
    </div>

    <a href="Checkout.aspx" class="btn-gold">Checkout</a>
  </asp:Panel>
</asp:Content>