     <%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="CREEMESTUDIO.Reset" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="/Content/auth.css" rel="stylesheet" />

  <section class="container auth-wrap">
    <div class="auth-card">
      <h2 class="auth-title">Reset password</h2>

      <asp:Panel ID="pnlForm" runat="server" Visible="false">
        <div class="mb-2">
          <label>New password</label>
          <asp:TextBox ID="txtPass1" runat="server" TextMode="Password" CssClass="form-control" />
        </div>
        <div class="mb-3">
          <label>Confirm new password</label>
          <asp:TextBox ID="txtPass2" runat="server" TextMode="Password" CssClass="form-control" />
        </div>
        <asp:Button ID="btnReset" runat="server" Text="Update password" CssClass="btn-gold"
                    OnClick="btnReset_Click" />
      </asp:Panel>

      <asp:Label ID="lblMsg" runat="server" CssClass="auth-msg d-block mt-2" />
    </div>
  </section>
</asp:Content>