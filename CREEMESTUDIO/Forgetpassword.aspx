<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Forgetpassword.aspx.cs" Inherits="CREEMESTUDIO.Forgetpasssword" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="/Content/auth.css" rel="stylesheet" />

  <section class="container auth-wrap">
    <div class="auth-card">
      <h2 class="auth-title">Forgot password?</h2>
      <p class="mb-3">Enter your account email. We’ll generate a reset link for you.</p>

      <div class="mb-3">
        <label>Email address</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
      </div>

      <asp:Button ID="btnSend" runat="server" Text="Generate reset link" CssClass="btn-gold"
                  OnClick="btnNext_Click" />
      <asp:Label ID="lblMsg" runat="server" CssClass="auth-msg d-block mt-2" />
      <asp:HyperLink ID="lnkReset" runat="server" CssClass="d-block mt-2" Visible="false" Text="Open reset link" />
    </div>
  </section>
</asp:Content>