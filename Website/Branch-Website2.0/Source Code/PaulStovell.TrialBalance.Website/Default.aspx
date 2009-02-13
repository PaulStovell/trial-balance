<%@ Page Language="C#" MasterPageFile="~/TrialBalance.Master" AutoEventWireup="true" EnableViewState="false"
    Codebehind="Default.aspx.cs" Inherits="PaulStovell.TrialBalance.Website.Default"
    Title="Home - TrialBalance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <div> 
        <h1>What is TrialBalance?</h1>
        <div class="contentBlock">
            <%
            if (this.MainScreenshot != null) 
            {
            %>
            <div class="thumbnailImageContainer">
                <a href="<%= this.MainScreenshot.LargeImageLink %>" target="_blank">
                    <img src="<%= this.MainScreenshot.SmallImageLink %>" alt="<%= this.MainScreenshot.Caption %>" />
                </a>
            </div>
            <% 
            } 
            %>
            <p>
                TrialBalance is a free, shared-source accounting application that is designed to <b>teach</b> basic double-entry
                bookkeeping to High School and University students. TrialBalance is 
                designed to fit around the New South Wales high-school accounting curriculum. 
            </p>
            <p>
                TrialBalance is an educational product, and is not designed to compete with products such as Quicken or MYOB. 
            </p>
            <p>
                <a href="Builds.aspx"><img style="vertical-align: middle;" runat="server" alt="Download Now" src="~/Images/world_go.png" /></a>
                <a href="Builds.aspx" style="font-weight: bold;">Download TrialBalance...</a>
            </p>
        </div>
        
        <h1>Who makes it?</h1>
        <div class="contentBlock">
            <p>
                TrialBalance is a spare-time project developed by me, <a href="http://www.paulstovell.net/">Paul Stovell</a>, a Sydney based 
                software developer. By day I work for <a href="http://www.readify.net/">Readify</a>, a team of elite consultants specializing in 
                technical readiness, and by night I keep my skills sharp by working on TrialBalance. 
            </p>
        </div>
    </div>
</asp:Content>
