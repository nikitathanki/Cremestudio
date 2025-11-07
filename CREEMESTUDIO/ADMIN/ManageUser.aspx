<%@ Page Title="" Language="C#" MasterPageFile="~/ADMIN/Admin.Master" AutoEventWireup="true" CodeBehind="ManageUser.aspx.cs" Inherits="CREEMESTUDIO.ADMIN.ManageUser" %>

<asp:Content ID="head" ContentPlaceHolderID="HeadTitle" runat="server">
  Manage Users
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="AdminContent" runat="server">
  <h2 class="page-title">Manage Users</h2>

  <!-- Users Grid (no custom code-behind logic) -->
  <asp:GridView ID="gvUsers" runat="server"
      CssClass="table table-striped"
      AutoGenerateColumns="False"
      DataKeyNames="UserID"
      DataSourceID="dsUsers">
    <Columns>
      <asp:BoundField DataField="UserID" HeaderText="ID" ReadOnly="true" />

      <asp:BoundField DataField="FullName" HeaderText="Name" />
      <asp:BoundField DataField="Email" HeaderText="Email" />
      <asp:BoundField DataField="Phone" HeaderText="Phone" />
      <asp:BoundField DataField="Address" HeaderText="Address" />

      <asp:TemplateField HeaderText="Active">
        <ItemTemplate>
          <asp:CheckBox ID="chkActiveRO" runat="server"
                        Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>' Enabled="false" />
        </ItemTemplate>
        <EditItemTemplate>
          <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("IsActive") %>' />
        </EditItemTemplate>
      </asp:TemplateField>

      <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
    </Columns>
  </asp:GridView>

  <!-- Data source: handles Select/Update/Delete itself -->
  <asp:SqlDataSource ID="dsUsers" runat="server"
      ConnectionString="<%$ ConnectionStrings:CremeDb %>"
      SelectCommand="
        SELECT UserID, FullName, Email, Phone, Address, IsActive
        FROM Users
        ORDER BY UserID DESC"
      UpdateCommand="
        UPDATE Users
           SET FullName=@FullName,
               Email=@Email,
               Phone=@Phone,
               Address=@Address,
               IsActive=@IsActive
         WHERE UserID=@UserID"
      DeleteCommand="
        DELETE FROM Users
         WHERE UserID=@UserID
           AND NOT EXISTS(SELECT 1 FROM Orders WHERE UserID=@UserID)">
    <UpdateParameters>
      <asp:Parameter Name="FullName" Type="String" />
      <asp:Parameter Name="Email" Type="String" />
      <asp:Parameter Name="Phone" Type="String" />
      <asp:Parameter Name="Address" Type="String" />
      <asp:Parameter Name="IsActive" Type="Boolean" />
      <asp:Parameter Name="UserID" Type="Int32" />
    </UpdateParameters>
    <DeleteParameters>
      <asp:Parameter Name="UserID" Type="Int32" />
    </DeleteParameters>
  </asp:SqlDataSource>

  <div class="text-muted" style="margin-top:.5rem;font-size:12px;">
    Note: Delete will be ignored if the user has any orders.
  </div>
</asp:Content>