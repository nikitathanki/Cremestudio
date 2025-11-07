<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="CREEMESTUDIO.Search" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="Content/search.css" rel="stylesheet" />

  <div class="search-page">
    <!-- Header -->
    <div class="search-header">
      <h2>Search Results</h2>
      <asp:Label ID="lblCount" runat="server" CssClass="result-count" />
    </div>

    <!-- Sidebar -->
    <aside class="search-sidebar">
      <h4>Categories</h4>
      <asp:Repeater ID="rptCategories" runat="server">
        <ItemTemplate>
          <a href='Search.aspx?category=<%# HttpUtility.UrlEncode(Eval("CategoryName").ToString()) %>'>
            <%# Eval("CategoryName") %>
          </a>
        </ItemTemplate>
      </asp:Repeater>

      <h4>Brands (Top 5)</h4>
      <asp:Repeater ID="rptBrands" runat="server">
        <ItemTemplate>
          <a href='Search.aspx?brand=<%# HttpUtility.UrlEncode(Eval("BrandName").ToString()) %>'>
            <%# Eval("BrandName") %>
          </a>
        </ItemTemplate>
      </asp:Repeater>
    </aside>

    <!-- Results -->
    <section class="search-results">
      <asp:Repeater ID="rptProducts" runat="server">
        <ItemTemplate>
          <div class="product-card">
            <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("ProductName") %>' />
            <h5><%# Eval("ProductName") %></h5>
            <p class="brand"><%# Eval("Brand") %></p>
            <p class="price">₹ <%# Eval("Price","{0:N2}") %></p>
            <a class="btn-view" href='ProductDetails.aspx?id=<%# Eval("ProductID") %>'>View</a>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </section>
  </div>
</asp:Content>