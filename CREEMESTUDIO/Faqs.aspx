<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Faqs.aspx.cs" Inherits="CREEMESTUDIO.Faqs" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="Content/policy.css" rel="stylesheet" />
  <section class="doc container">
    <h1 class="doc-title">Frequently Asked Questions</h1>
    <p class="doc-sub">Can’t find what you need? <a href="Help.aspx">Visit Help Centre</a>.</p>

    <div class="accordion" id="faqAcc">
      <!-- Q1 -->
      <div class="accordion-item">
        <h2 class="accordion-header" id="q1h">
          <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#q1" aria-expanded="true" aria-controls="q1">
            How do I track my order?
          </button>
        </h2>
        <div id="q1" class="accordion-collapse collapse show" aria-labelledby="q1h" data-bs-parent="#faqAcc">
          <div class="accordion-body">
            Go to <a href="Track.aspx">Track Order</a> and enter your Order # to see live status and invoice.
          </div>
        </div>
      </div>

      <!-- Q2 -->
      <div class="accordion-item">
        <h2 class="accordion-header" id="q2h">
          <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#q2" aria-expanded="false" aria-controls="q2">
            What payment methods are accepted?
          </button>
        </h2>
        <div id="q2" class="accordion-collapse collapse" aria-labelledby="q2h" data-bs-parent="#faqAcc">
          <div class="accordion-body">
            We accept UPI, Cards, NetBanking and Cash on Delivery (COD) for eligible orders.
          </div>
        </div>
      </div>

      <!-- Q3 -->
      <div class="accordion-item">
        <h2 class="accordion-header" id="q3h">
          <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#q3" aria-expanded="false" aria-controls="q3">
            Can I cancel or edit my order?
          </button>
        </h2>
        <div id="q3" class="accordion-collapse collapse" aria-labelledby="q3h" data-bs-parent="#faqAcc">
          <div class="accordion-body">
            Orders can be cancelled before dispatch. For changes, contact support quickly with your Order #.
          </div>
        </div>
      </div>

      <!-- Q4 -->
      <div class="accordion-item">
        <h2 class="accordion-header" id="q4h">
          <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#q4" aria-expanded="false" aria-controls="q4">
            What is your return policy?
          </button>
        </h2>
        <div id="q4" class="accordion-collapse collapse" aria-labelledby="q4h" data-bs-parent="#faqAcc">
          <div class="accordion-body">
            Most unopened items are eligible within 7 days of delivery. See <a href="Refunds.aspx">Refunds</a> for details.
          </div>
        </div>
      </div>

      <!-- Q5 -->
      <div class="accordion-item">
        <h2 class="accordion-header" id="q5h">
          <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#q5" aria-expanded="false" aria-controls="q5">
            Do you ship to my location?
          </button>
        </h2>
        <div id="q5" class="accordion-collapse collapse" aria-labelledby="q5h" data-bs-parent="#faqAcc">
          <div class="accordion-body">
            We ship PAN-India via trusted partners. Enter your pincode at checkout to confirm serviceability.
          </div>
        </div>
      </div>
    </div>
  </section>
</asp:Content>