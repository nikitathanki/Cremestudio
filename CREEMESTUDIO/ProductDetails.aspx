<%@ Page Title="Product Details • Crème Studio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="CREEMESTUDIO.ProductDetails" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="Content/productdetail.css" rel="stylesheet" />

  <section class="pdp-wrap">
    <asp:Label ID="lblMsg" runat="server" CssClass="msg"></asp:Label>

    <!-- Not Found -->
    <asp:PlaceHolder ID="phNotFound" runat="server" Visible="false">
      <div class="pdp-card">
        <p>Product not found.</p>
        <a href="Products.aspx" class="btn-gold">Back to Products</a>
      </div>
    </asp:PlaceHolder>

    <!-- Product Details -->
    <asp:PlaceHolder ID="phProduct" runat="server" Visible="false">
      <div class="pdp-card">
        <div class="pdp-grid">
          <div class="pdp-media">
            <asp:Image ID="imgProd" runat="server" CssClass="pdp-img" />
          </div>
          <div class="pdp-info">
            <h2><asp:Label ID="lblName" runat="server" /></h2>
            <div>Brand: <asp:Label ID="lblBrand" runat="server" /></div>
            <div>₹ <asp:Label ID="lblPrice" runat="server" /></div>
            <div>Availability: <asp:Label ID="lblAvail" runat="server" /></div>

            <div class="pdp-actions">
              Qty:
              <asp:TextBox ID="txtQty" runat="server" Text="1" CssClass="qty-box" />
              <asp:Button ID="btnAddCart" runat="server" Text="Add to Cart" CssClass="btn-gold" OnClick="btnAddCart_Click" />
              <asp:Button ID="btnBuyNow" runat="server" Text="Buy Now" CssClass="btn-outline" OnClick="btnBuyNow_Click" />
            </div>

            <h4>About this item</h4>
            <p><asp:Label ID="lblDesc" runat="server" /></p>
          </div>
        </div>
      </div>
    </asp:PlaceHolder>
  </section>
</asp:Content>