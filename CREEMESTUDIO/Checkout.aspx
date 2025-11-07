<%@ Page Title="Checkout • Crème Studio" Language="C#" MasterPageFile="/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="CREEMESTUDIO.Checkout" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <!-- page css -->
  <link href="/Content/checkout.css" rel="stylesheet" />

  <section class="co-wrap container">
    <h2 class="page-title">Checkout</h2>
    <asp:Label ID="lblMsg" runat="server" CssClass="msg"></asp:Label>

    <!-- Empty -->
    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="card empty">
      Your cart is empty.
      <a class="btn-gold" href="Products.aspx">Continue shopping</a>
    </asp:Panel>

    <!-- Form + Summary -->
    <asp:Panel ID="pnlCheckout" runat="server" Visible="false">
      <div class="co-grid">
        <!-- Shipping / Payment -->
        <div class="co-card">
          <h4 class="co-h">Shipping Details</h4>

          <div class="row g-3">
            <div class="col-md-6">
              <label class="form-label">Full name</label>
              <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="80" />
            </div>
            <div class="col-md-6">
              <label class="form-label">Phone</label>
              <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" MaxLength="15" />
            </div>

            <div class="col-12">
              <label class="form-label">Address line 1</label>
              <asp:TextBox ID="txtAddr1" runat="server" CssClass="form-control" MaxLength="120" />
            </div>
            <div class="col-12">
              <label class="form-label">Address line 2 (optional)</label>
              <asp:TextBox ID="txtAddr2" runat="server" CssClass="form-control" MaxLength="120" />
            </div>

            <div class="col-md-4">
              <label class="form-label">City</label>
              <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" MaxLength="60" />
            </div>
            <div class="col-md-4">
              <label class="form-label">State</label>
              <asp:TextBox ID="txtState" runat="server" CssClass="form-control" MaxLength="60" />
            </div>
            <div class="col-md-4">
              <label class="form-label">Pincode</label>
              <asp:TextBox ID="txtPin" runat="server" CssClass="form-control" MaxLength="10" />
            </div>

            <div class="col-md-6">
              <label class="form-label">Payment Method</label>
              <asp:DropDownList ID="ddlPay" runat="server" CssClass="form-select">
                <asp:ListItem Text="Cash on Delivery" Value="COD" />
                <asp:ListItem Text="UPI" Value="UPI" />
                <asp:ListItem Text="Card" Value="Card" />
                <asp:ListItem Text="NetBanking" Value="NetBanking" />
              </asp:DropDownList>
            </div>

            <div class="col-md-6">
              <label class="form-label">Coupon (optional)</label>
              <asp:TextBox ID="txtCoupon" runat="server" CssClass="form-control" MaxLength="30" />
            </div>
          </div>

          <div class="mt-3">
            <asp:Button ID="btnPlace" runat="server" Text="Place Order" CssClass="btn-gold"
                        OnClick="btnPlace_Click" />
          </div>
        </div>

        <!-- Summary -->
        <div class="co-card">
          <h4 class="co-h">Order Summary</h4>

          <asp:Repeater ID="rptItems" runat="server">
            <HeaderTemplate><ul class="sum-list"></HeaderTemplate>
            <ItemTemplate>
              <li class="sum-row">
                <span class="name"><%# Eval("ProductName") %></span>
                <span class="qty">× <%# Eval("Quantity") %></span>
                <span class="amt">₹ <%# Eval("LineTotal", "{0:N2}") %></span>
              </li>
            </ItemTemplate>
            <FooterTemplate></ul></FooterTemplate>
          </asp:Repeater>

          <div class="sum-totals">
            <div><span>Subtotal</span><strong>₹ <asp:Label ID="lblSub" runat="server" /></strong></div>
            <div><span>Discount</span><strong>₹ <asp:Label ID="lblDisc" runat="server" /></strong></div>
            <div class="grand"><span>Total</span><strong>₹ <asp:Label ID="lblTotal" runat="server" /></strong></div>
          </div>
        </div>
      </div>
    </asp:Panel>
  </section>
</asp:Content>