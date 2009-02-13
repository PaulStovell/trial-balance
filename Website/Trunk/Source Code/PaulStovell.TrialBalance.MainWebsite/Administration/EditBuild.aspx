<%@ Page Language="C#" MasterPageFile="~/TrialBalance.Master" AutoEventWireup="true" CodeBehind="EditBuild.aspx.cs" Inherits="PaulStovell.TrialBalance.MainWebsite.Administration.EditBuild" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    
    <h1>Edit Build</h1>
    
    <div class="contentBlock">
        
        <table width="100%" border="0" cellpadding="7" cellspacing="0">
            <tr>
                <td>Build Number:</td>
                <td><b><%= this.Build.BuildNumber %></b></td>
            </tr>
            <tr>
                <td>Downloads:</td>
                <td><%= this.Build.Downloads.ToString("n0") %></td>
            </tr>
            <tr>
                <td>Successful:</td>
                <td><asp:CheckBox runat="server" ID="_successfulCheckBox" /></td>
            </tr>
            <tr>
                <td>Stable:</td>
                <td><asp:CheckBox runat="server" ID="_stableCheckbox" /></td>
            </tr>
            <tr>
                <td>Public:</td>
                <td><asp:CheckBox runat="server" ID="_publicCheckbox" /></td>
            </tr>
        </table>
        
        <div>
            <asp:Button Text="Save Build" runat="server" OnClick="SaveButton_Clicked" />
        </div>
    </div>

</asp:Content>
