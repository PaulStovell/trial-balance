<%@ Page Language="C#" MasterPageFile="~/TrialBalance.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PaulStovell.TrialBalance.Website.Login" 
    Title="Login - TrialBalance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceholder" runat="server">

    <asp:Login runat="server" OnAuthenticate="Login_Authenticate" />

</asp:Content>
