<%@ Page Language="C#" MasterPageFile="~/TrialBalance.Master" AutoEventWireup="true" EnableViewState="false" CodeBehind="Builds.aspx.cs" Inherits="PaulStovell.TrialBalance.Website.Builds" 
Title="Downloads - TrialBalance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceholder" runat="server">
    <asp:PlaceHolder runat="server" Visible="false" ID="_downloadAreaPlaceholder">
        <h1>Download</h1>
        <div class="contentBlock">
            <p>
                The latest stable release of TrialBalance is version <%= this.LatestStableBuild.Version %>, released on <%= this.LatestStableBuild.BuildDate.ToLongDateString() %>.
            </p>
            <p>
                <a href="<%= this.LatestStableBuild.DownloadLink %>"><img style="vertical-align: middle;" width="16" height="16" runat="server" alt="Download Now" src="~/Images/world_go.png" /></a>
                <a href="<%= this.LatestStableBuild.DownloadLink %>">TrialBalance - Version <%= this.LatestStableBuild.Version %></a>
            </p>
        </div>
    </asp:PlaceHolder>
    
    <h1>Builds</h1>
    <div class="contentBlock">
        <p>
            The source code for TrialBalance is stored in a <a href="http://msdn.microsoft.com/vstudio/teamsystem/team/">Microsoft Team Foundation Server</a> 
            hosted by <a href="http://www.readify.net/">Readify</a>. This server has been configured so that every time I check-in some source code for 
            TrialBalance, everything is compiled, tested, and uploaded to this web site automatically. You can find all past builds of TrialBalance below:
        </p>
        <p>
            <a href="BuildsRss.ashx"><img style="vertical-align: middle;" width="16" height="16" alt="Subscribe to Builds RSS Feed" runat="server" src="~/Images/Feed.png" /></a>
            <a href="BuildsRss.ashx">
                Subscribe to Builds RSS Feed
            </a>
        </p>
        <asp:DataGrid runat="server" ID="_buildsDataGrid" AutoGenerateColumns="false" Width="100%" GridLines="none" HeaderStyle-Font-Bold="true">
            <Columns>
                <asp:BoundColumn DataField="BuildNumber" HeaderText="Build Number" />
                <asp:BoundColumn DataField="Version" HeaderText="Version" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundColumn DataField="BuildDate" HeaderText="Build Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundColumn DataField="BuildStatus" HeaderText="Build Quality" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                <asp:TemplateColumn HeaderText="Successful" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <img runat="server" width="16" height="16" alt="<%# GetSuccessfulImageAlt(Container) %>" src="<%# GetSuccessfulImageSource(Container) %>" />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:HyperLinkColumn DataNavigateUrlField="DownloadLink" Text="Download" HeaderText="Download" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:HyperLinkColumn DataNavigateUrlField="EditLink" Text="Edit" HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
            </Columns>
        </asp:DataGrid>
    </div>
</asp:Content>
