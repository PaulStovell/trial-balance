<%@ Page Language="C#" MasterPageFile="~/TrialBalance.Master" AutoEventWireup="true" EnableViewState="false"
    Codebehind="Default.aspx.cs" Inherits="PaulStovell.TrialBalance.MainWebsite.Default"
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
                TrialBalance is a free, shared-source accounting application that is designed to teach basic double-entry
                bookkeeping to High School and University students.
            </p>
            <p>
                TrialBalance is not designed to run your business
                or to do your tax, although, if you knew enough about accounting, it could. TrialBalance simply provides
                a chart of accounts and allows you to record transactions against the five account types. TrialBalance is 
                designed to fit around the New South Wales high-school accounting curriculum. 
            </p>
            <p>
                <a href="Builds.aspx"><img style="vertical-align: middle;" runat="server" alt="Download Now" src="~/Images/world_go.png" /></a>
                <a href="Builds.aspx">Download TrialBalance...</a>
            </p>
        </div>
        
        <h1>Features</h1>
        <div class="contentBlock">
            <p>
                The planned features for the initial TrialBalance release are:
            </p>
            <ul>
                <li>Creating and storing of the five major account types - <strong>Assets</strong>, <strong>Liabilities</strong>, <strong>Revenue</strong>, <strong>Expenses</strong> and <strong>Equity</strong>.</li>
                <li>Recording <strong>transactions</strong> using the double-entry system.</li>
            </ul>
        </div>
        
        <h1>Who makes it?</h1>
        <div class="contentBlock">
            <p>
                TrialBalance is a spare-time project developed by me, <a href="http://www.paulstovell.net/">Paul Stovell</a>, a Sydney-based 
                software developer. By day I work for <a href="http://www.readify.net/">Readify</a>, a team of elite consultants specializing in 
                technical readiness, and by night I keep my skills sharp by working on TrialBalance. 
            </p>
        </div>
    </div>
</asp:Content>
