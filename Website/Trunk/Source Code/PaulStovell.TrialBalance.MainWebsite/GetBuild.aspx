<%@ Page Language="C#" MasterPageFile="~/TrialBalance.Master" AutoEventWireup="true" EnableViewState="false" CodeBehind="GetBuild.aspx.cs" 
    Inherits="PaulStovell.TrialBalance.MainWebsite.GetBuild" 
    Title="Download - TrialBalance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceholder" runat="server">

    <h1>
        Download
    </h1>
    <div class="tableBlock">
        <table border="0" cellpadding="7" cellspacing="0">
            <tr>
                <td></td>
                <td>Version:</td>
                <td><%= this.Build.Version %></td>
            </tr>
            <tr>
                <td></td>
                <td>Built successfully:</td>
                <td><%= (this.Build.IsSuccessful) ? "Yes" : "No" %></td>
            </tr>
            <tr>
                <td></td>
                <td>Quality:</td>
                <td><%= this.Build.BuildStatus %></td>
            </tr>
            <tr>
                <td></td>
                <td>Release date:</td>
                <td><%= this.Build.BuildDate %></td>
            </tr>
            <% if (this.Build.HasClickOnce) { %>
            <tr>
                <td><a href="<%= this.Build.ClickOnceUrl %>"><img style="vertical-align: middle;" runat="server" alt="Try Online!" src="~/Images/world_go.png" /></a></td>
                <td colspan="2"><a href="<%= this.Build.ClickOnceUrl %>"><b>Try online</b></a></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2">This is a ClickOnce application that you can run online, without having to download the installer.</td>
            </tr>
            <% } %>
            <% if (this.Build.HasInstaller) { %>
            <tr>
                <td><a href="<%= this.Build.InstallerUrl %>"><img style="vertical-align: middle;" runat="server" alt="Download Now" src="~/Images/application_put.png" /></a></td>
                <td colspan="2"><a href="<%= this.Build.InstallerUrl %>"><b>Download Installer</b></a></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2">This installation wizard will guide you through the installation process.</td>
            </tr>
            <% } %>
            <% if (this.Build.HasSourceCode) { %>
            <tr>
                <td><a href="<%= this.Build.SourceCodeUrl %>"><img style="vertical-align: middle;" runat="server" alt="Download Now" src="~/Images/compress.png" /></a></td>
                <td colspan="2"><a href="<%= this.Build.SourceCodeUrl %>"><b>Download Source Code</b></a></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2">Source code releases are designed for software developers and other people who are interested in learning how TrialBalance was made.</td>
            </tr>
            <% } %>
            <% if (!this.Build.HasSourceCode && !this.Build.HasInstaller && !this.Build.HasClickOnce) { %>
            <tr>
                <td></td>
                <td colspan="2">No download files are available for this release. They may have been removed to save on disk space. Please go back to the <a href="Builds.aspx">Download Page</a> 
                to download the latest version of TrialBalance.</td>
            </tr>
            <% } %>
        </table>
        
    </div>
    
    <h1>
        Release Notes
    </h1>
    <div class="contentBlock">
        <%= this.Build.ReleaseNotes %>
    </div>

</asp:Content>
