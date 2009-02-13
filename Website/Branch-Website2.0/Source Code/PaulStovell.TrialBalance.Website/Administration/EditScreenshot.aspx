<%@ Page Language="C#" MasterPageFile="~/TrialBalance.Master" AutoEventWireup="true" CodeBehind="EditScreenshot.aspx.cs" Inherits="PaulStovell.TrialBalance.Website.Administration.EditScreenshot" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <h1>Edit Screenshot</h1>
    
    <div class="contentBlock">
        
        <table width="100%" border="0" cellpadding="7" cellspacing="0">
            <tr>
                <td>ID:</td>
                <td><b><%= this.Screenshot.ScreenshotID %></b></td>
            </tr>
            <tr>
                <td>Caption:</td>
                <td><asp:TextBox runat="server" ID="_captionTextBox" /></td>
            </tr>
            <tr>
                <td>Date Taken:</td>
                <td><%= this.Screenshot.DateTaken %></td>
            </tr>
            <tr>
                <td>Image Upload:</td>
                <td>
                    <asp:FileUpload runat="server" ID="_imageFileUpload" />
                </td>
            </tr>
        </table>
        
        <div>
            <asp:Button ID="Button1" Text="Save Screenshot" runat="server" OnClick="SaveButton_Clicked" />
            <asp:Button ID="Button2" Text="Delete" runat="server" OnClick="DeleteButton_Clicked" />
        </div>
    </div>
</asp:Content>
