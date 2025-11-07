<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="CREEMESTUDIO.Help" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="Content/policy.css" rel="stylesheet" />
  <section class="doc container">
    <h1 class="doc-title">Help Centre</h1>
    <p class="doc-sub">We’re here to help with orders, returns, payments and more.</p>

    <div class="row g-4">
      <div class="col-md-6">
        <div class="doc-card">
          <h3>Order & Tracking</h3>
          <p>Track your order status, delivery timeline, and invoice.</p>
          <a class="btn-gold" href="Track.aspx">Track Order</a>
        </div>
      </div>
      <div class="col-md-6">
        <div class="doc-card">
          <h3>Returns & Refunds</h3>
          <p>Understand our return window, eligibility and refund timelines.</p>
          <a class="btn-outline" href="Refunds.aspx">View Refund Policy</a>
        </div>
      </div>

      <div class="col-md-6">
        <div class="doc-card">
          <h3>Shipping & Delivery</h3>
          <p>Delivery speeds, charges and serviceable locations.</p>
          <a class="btn-outline" href="Shipping.aspx">Shipping Details</a>
        </div>
      </div>
      <div class="col-md-6">
        <div class="doc-card">
          <h3>FAQs</h3>
          <p>Quick answers to the most common questions.</p>
          <a class="btn-outline" href="Faqs.aspx">Browse FAQs</a>
        </div>
      </div>
    </div>

    <div class="help-contact">
      <h3>Contact Us</h3>
      <p>Email: support@cremestudio.example • Mon–Sat, 10am–7pm IST</p>
    </div>
  </section>
</asp:Content>