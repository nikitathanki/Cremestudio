 <%@ Page Title="Home • Crème Studio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="CREEMESTUDIO.Home" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/home.css" rel="stylesheet" />

    <!-- Hero (single banner, no JS) -->
    <section class="hero">
        <div class="hero-card" style="background-image:url('Images/serum-p.jpg')">
            <div class="hero-text">
                <h2>Glow Season</h2>
                <p>Flat 20% off on bestsellers</p>
                <a href="Products.aspx" class="btn-gold">Shop Now</a>
            </div>
        </div>
    </section>

    <!-- Top Categories (simple grid) -->
    <section class="section">
        <div class="section-head"><h3>Top Categories</h3></div>
        <div class="cat-grid">
            <a class="cat-card" href="Products.aspx?dept=makeup">
                <img src="Images/makeup.jpg" alt="Makeup" /><span>Makeup</span>
            </a>
            <a class="cat-card" href="Products.aspx?dept=skin">
                <img src="Images/serum-p.jpg" alt="Skincare" /><span>Skincare</span>
            </a>
            <a class="cat-card" href="Products.aspx?dept=hair">
                <img src="Images/hair-care.jpg" alt="Hair" /><span>Hair</span>
            </a>
            <a class="cat-card" href="Products.aspx?dept=fragrance">
                <img src="Images/perfume.jpg" alt="Fragrance" /><span>Fragrance</span>
            </a>
            <a class="cat-card" href="Products.aspx?dept=bath">
                <img src="Images/bath&body.jpg" alt="Bath &amp; Body" /><span>Bath &amp; Body</span>
            </a>
        </div>
    </section>

    <!-- Spotlight / Offers -->
    <section class="section">
        <div class="section-head"><h3>Spotlight — Crème Exclusives</h3></div>
        <div class="offer-grid">
            <asp:Repeater ID="rpOffers" runat="server">
                <ItemTemplate>
                    <div class="offer-card">
                        <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("Title") %>' />
                        <div class="offer-meta">
                            <h4><%# Eval("Title") %></h4>
                            <p><%# Eval("Subtitle") %></p>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>
</asp:Content>