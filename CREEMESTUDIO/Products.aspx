<%@ Page Title="Products • Crème Studio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="CREEMESTUDIO.Products" %>

<asp:Content ID="c1" ContentPlaceHolderID="MainContent" runat="server">

  <link href="Content/products.css" rel="stylesheet" />    <section class="products-wrap">      
    <h2 class="page-title">Our Products</h2>  <!-- FILTER BAR -->      
<% var d = (Request["dept"] ?? "").ToLower(); %>      
<div class="filter-bar">      
  <div class="tabs">      
    <a class="tab <%= d==""?"active":"" %>"         href="Products.aspx">All</a>      
    <a class="tab <%= d=="makeup"?"active":"" %>"    href="Products.aspx?dept=makeup">Makeup</a>      
    <a class="tab <%= d=="skin"?"active":"" %>"      href="Products.aspx?dept=skin">Skin</a>      
    <a class="tab <%= d=="hair"?"active":"" %>"      href="Products.aspx?dept=hair">Hair</a>      
    <a class="tab <%= d=="fragrance"?"active":"" %>" href="Products.aspx?dept=fragrance">Fragrance</a>      
    <a class="tab <%= d=="bath"?"active":"" %>"      href="Products.aspx?dept=bath">Bath &amp; Body</a>      
  </div>      <div class="filters-right">      
    <asp:DropDownList ID="ddlBrand" runat="server" AutoPostBack="true" CssClass="flt"      
                      OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged" />      
    <asp:DropDownList ID="ddlSort" runat="server" AutoPostBack="true" CssClass="flt"      
                      OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">      
      <asp:ListItem Text="Newest" Value="new" Selected="True" />      
      <asp:ListItem Text="Price: Low → High" Value="plh" />      
      <asp:ListItem Text="Price: High → Low" Value="phl" />      
      <asp:ListItem Text="Name: A → Z" Value="az" />      
    </asp:DropDownList>      
  </div>      
</div>    
      <asp:Label ID="lblMsg" runat="server" CssClass="msg" />  <!-- GRID -->    <div class="products-grid">      <asp:Repeater ID="rptProducts" runat="server">
<ItemTemplate>
  <article class="card-prod">
    <!-- Removed the price-badge -->

    <a class="img-wrap" href='ProductDetails.aspx?id=<%# Eval("ProductID") %>'>
      <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("ProductName") %>' />
    </a>

    <div class="info">
      <h4 class="name"><%# Eval("ProductName") %></h4>
      <div class="brand"><%# Eval("BrandName") %></div>

      <div class="foot">
        <div class="price">&#8377;<%# Eval("Price","{0:0.##}") %></div>
        <a class="btn-view" href='ProductDetails.aspx?id=<%# Eval("ProductID") %>'>View</a>
      </div>
    </div>
  </article>
</ItemTemplate>
                                                                                                                 </asp:Repeater>
    
</div>  
</section> 
    
</asp:Content>