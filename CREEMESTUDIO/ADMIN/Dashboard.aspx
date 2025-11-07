<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CREEMESTUDIO.ADMIN.Dashboard" %>

<asp:Content ID="cMain" ContentPlaceHolderID="AdminContent" runat="server">
  <link href="~/Content/admin.css" rel="stylesheet" />
  <style>
    /* Fallback in case admin.css is not present */
    .dash-wrap{padding:20px}
    .cards{display:grid;grid-template-columns:repeat(4, minmax(180px,1fr));gap:14px}
    .card{background:#fff;border:1px solid #eee;border-radius:14px;padding:16px;box-shadow:0 1px 3px rgba(0,0,0,.05)}
    .card h4{margin:0 0 8px;font-size:14px;color:#666}
    .card .num{font-size:26px;font-weight:700}
    .row{margin-top:22px;display:grid;grid-template-columns:2fr 1fr;gap:16px}
    .panel{background:#fff;border:1px solid #eee;border-radius:14px;padding:16px}
    .panel h3{margin:0 0 12px;font-size:18px}
    table{width:100%;border-collapse:collapse}
    th,td{padding:10px;border-bottom:1px solid #f0f0f0;text-align:left}
    .badge{padding:2px 8px;border-radius:999px;font-size:12px;background:#eee}
    .paid{background:#e6ffed}
    .pending{background:#fff6db}
  </style>

  <div class="dash-wrap">
    <h2>Dashboard</h2>

    <!-- Summary cards -->
    <div class="cards">
      <div class="card">
        <h4>Total Users</h4>
        <div class="num"><asp:Label ID="lblUsers" runat="server" Text="0" /></div>
      </div>
      <div class="card">
        <h4>Active Products</h4>
        <div class="num"><asp:Label ID="lblProducts" runat="server" Text="0" /></div>
      </div>
      <div class="card">
        <h4>Total Orders</h4>
        <div class="num"><asp:Label ID="lblOrders" runat="server" Text="0" /></div>
      </div>
      <div class="card">
        <h4>Total Revenue (₹)</h4>
        <div class="num"><asp:Label ID="lblRevenue" runat="server" Text="0" /></div>
      </div>
    </div>

    <div class="row">
      <!-- Recent Orders -->
      <div class="panel">
        <h3>Recent Orders</h3>
        <asp:Repeater ID="rpRecent" runat="server">
          <HeaderTemplate>
            <table>
              <thead>
                <tr>
                  <th>#</th>
                  <th>Date</th>
                  <th>Customer</th>
                  <th>Total (₹)</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td><%# Eval("OrderID") %></td>
              <td><%# Convert.ToDateTime(Eval("OrderDate")).ToString("dd MMM yyyy") %></td>
              <td><%# Eval("FullName") %></td>
              <td><%# string.Format("{0:0.##}", Eval("TotalAmount")) %></td>
              <td>
                <span class='badge <%# (Eval("PayStatus").ToString()=="Success"?"paid":"pending") %>'>
                  <%# Eval("PayStatus") %>
                </span>
              </td>
            </tr>
          </ItemTemplate>
          <FooterTemplate>
              </tbody>
            </table>
          </FooterTemplate>
        </asp:Repeater>
      </div>

      <!-- Quick links -->
      <div class="panel">
        <h3>Quick Actions</h3>
        <ul style="margin:0;padding-left:18px;line-height:1.9">
          <li><a href="ManageProducts.aspx">Add / Edit Products</a></li>
          <li><a href="ManageCategories.aspx">Manage Categories</a></li>
          <li><a href="ManageOrders.aspx">Process Orders</a></li>
          <li><a href="ManageUsers.aspx">Users</a></li>
          <li><a href="ManageCoupons.aspx">Coupons</a></li>
        </ul>
      </div>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger d-block mt-2" />
  </div>
</asp:Content>