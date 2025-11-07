<%@ Page Title="Login • Crème Studio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CREEMESTUDIO.Login" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="/Content/auth.css" rel="stylesheet" />

  <section class="container auth-wrap">
    <div class="auth-card">
      <h2 class="auth-title">Login</h2>

      <asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnLogin">

        <div class="mb-2">
          <label>Email address</label>
          <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
        </div>

        <div class="mb-3">
          <label>Password</label>
          <div class="pw-box">
            <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" />
            <span class="toggle-eye" onclick="togglePw('<%=txtPass.ClientID%>',this)">👁</span>
          </div>
        </div>

        <div class="mb-3">
          <label>Role</label>
          <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
            <asp:ListItem Text="User" Value="User" Selected="True" />
            <asp:ListItem Text="Admin" Value="Admin" />
          </asp:DropDownList>
        </div>

        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-gold"
                    OnClick="btnLogin_Click" />
        <a class="ms-2" href="Forgetpassword.aspx">Forgot password?</a>

      </asp:Panel>

      <asp:Label ID="lblError" runat="server" CssClass="auth-msg text-danger mt-2 d-block" EnableViewState="false" />

      <div class="mt-2">Don’t have an account? <a href="Register.aspx">Join us</a></div>
    </div>
  </section>

  <style>
    .pw-box{position:relative}
    .toggle-eye{position:absolute;right:10px;top:50%;transform:translateY(-50%);cursor:pointer;user-select:none}
  </style>
  <script>
      function togglePw(id, el) {
          const t = document.getElementById(id);
          t.type = (t.type === "password") ? "text" : "password";
          el.style.opacity = t.type === "text" ? 0.6 : 1;
      }
  </script>
</asp:Content>