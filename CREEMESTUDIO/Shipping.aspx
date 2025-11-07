<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shipping.aspx.cs" Inherits="CREEMESTUDIO.Shipping" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="Content/policy.css" rel="stylesheet" />
  <section class="doc container">
    <h1 class="doc-title">Shipping & Delivery</h1>
    <p class="doc-sub">Pan-India delivery with real-time tracking.</p>

    <div class="doc-card">
      <h3>Processing Time</h3>
      <p>Orders are processed within 24–48 business hours. During launches/sales, processing may take a bit longer.</p>
    </div>

    <div class="doc-card">
      <h3>Delivery Speed</h3>
      <ul>
        <li>Metro cities: 2–4 business days</li>
        <li>Rest of India: 3–7 business days</li>
        <li>Remote locations: 5–9 business days</li>
      </ul>
    </div>

    <div class="doc-card">
      <h3>Shipping Charges</h3>
      <p>Free shipping above ₹699. A nominal fee applies below this threshold (shown at checkout).</p>
    </div>

    <div class="doc-card">
      <h3>Order Tracking</h3>
      <p>Use your Order # at <a href="Track.aspx">Track Order</a> for status, ETA and invoice.</p>
    </div>
  </section>
</asp:Content>