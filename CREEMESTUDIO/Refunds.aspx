<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Refunds.aspx.cs" Inherits="CREEMESTUDIO.Refunds" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="Content/policy.css" rel="stylesheet" />
  <section class="doc container">
    <h1 class="doc-title">Returns & Refunds</h1>
    <p class="doc-sub">Easy returns within 7 days of delivery for eligible items.</p>

    <div class="doc-card">
      <h3>Eligibility</h3>
      <ul>
        <li>Unopened, unused items in original packaging</li>
        <li>Wrong/defective items received (with unboxing proof)</li>
        <li>Non-returnable: used items, hygiene-sensitive products, gift cards</li>
      </ul>
    </div>

    <div class="doc-card">
      <h3>How to Request a Return</h3>
      <ol>
        <li>Write to <strong>support@cremestudio.example</strong> with Order #, reason and photos (if applicable).</li>
        <li>We’ll arrange pickup or share a drop-off address.</li>
        <li>Once inspected, refund/store credit will be issued.</li>
      </ol>
    </div>

    <div class="doc-card">
      <h3>Refund Timelines</h3>
      <ul>
        <li>Prepaid (UPI/Card/NetBanking): 3–7 business days after approval</li>
        <li>COD refunds: issued as store credit or to bank (if provided)</li>
      </ul>
    </div>

    <div class="doc-card">
      <h3>Damaged / Missing Items</h3>
      <p>Please share unboxing video/photos within 48 hours of delivery for swift resolution.</p>
    </div>
  </section>
</asp:Content>