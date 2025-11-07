<%@ Page Title="Create account • Crème Studio" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CREEMESTUDIO.Registration" %>

<asp:Content ID="cMain" ContentPlaceHolderID="MainContent" runat="server">
  <link href="/Content/auth.css" rel="stylesheet" />

  <section class="container auth-wrap">
    <div class="auth-card">
      <h2 class="auth-title">Create your account</h2>

      <div class="mb-2">
        <label>Full name</label>
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="80" />
      </div>

      <div class="mb-2">
        <label>Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" MaxLength="120" />
      </div>

      <div class="mb-2">
        <label>Phone (optional)</label>
        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" MaxLength="15" />
      </div>

      <div class="mb-2">
        <label>Password</label>
        <div class="pw-box">
          <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" />
          <span class="toggle-eye" onclick="togglePw('<%=txtPass.ClientID%>',this)">👁</span>
        </div>
      </div>

      <div class="mb-3">
        <label>Confirm password</label>
        <div class="pw-box">
          <asp:TextBox ID="txtPass2" runat="server" CssClass="form-control" TextMode="Password" />
          <span class="toggle-eye" onclick="togglePw('<%=txtPass2.ClientID%>',this)">👁</span>
        </div>
      </div>

      <!-- Role selector -->
      <div class="mb-3">
        <label>Role</label>
        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
          <asp:ListItem Text="User" Value="User" Selected="True" />
          <asp:ListItem Text="Admin" Value="Admin" />
        </asp:DropDownList>
      </div>

      <asp:Button ID="btnRegister" runat="server" Text="Create account"
                  CssClass="btn-gold" OnClick="btnReg_Click" />
      <asp:Label ID="lblMsg" runat="server" CssClass="auth-msg ms-2" />
    </div>
  </section>

  <style>
    .pw-box{position:relative}
    .toggle-eye{position:absolute;right:10px;top:50%;transform:translateY(-50%);
      cursor:pointer;user-select:none}
  </style>
  <script>
      function togglePw(id, el) { const t = document.getElementById(id); t.type = t.type === "password" ? "text" : "password"; el.style.opacity = t.type === "text" ? 0.6 : 1; }
  </script>
</asp:Content>