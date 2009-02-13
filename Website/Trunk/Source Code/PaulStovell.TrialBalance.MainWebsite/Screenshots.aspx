<%@ Page Language="C#" MasterPageFile="~/TrialBalance.Master" AutoEventWireup="true" EnableViewState="false" CodeBehind="Screenshots.aspx.cs" 
    Inherits="PaulStovell.TrialBalance.MainWebsite.Screenshots" 
    Title="Screenshots - TrialBalance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceholder" runat="server">

    <h1>Screnshots</h1>
    <div class="contentBlock">
    
        <asp:LoginView ID="LoginView1" runat="server">
            <LoggedInTemplate>
                <p>
                    <a href="/Administration/EditScreenshot.aspx">Add Screenshot</a>
                </p>
            </LoggedInTemplate>
        </asp:LoginView>
        
        <asp:Repeater runat="server" ID="_screenshotRepeater">
            <ItemTemplate>
                <div style="margin: 7px; margin-left: auto; margin-right: auto; width: 500px; background: #FAFAFA; border: 1px solid #999999; text-align: center;">
                    <div style="margin: 7px; border: 1px solid #999999; background: #FFFFFF">
                    <div style="margin-left: auto; margin-right: auto;">
                        <a href="<%# Eval("LargeImageLink") %>" target="_blank">
                            <img src="<%# Eval("SmallImageLink") %>" alt="<%# Eval("Caption") %>" />
                        </a>
                    </div>
                    </div>
                    <div>
                        <p><%# Eval("Caption") %>
                        <asp:LoginView runat="server">
                            <LoggedInTemplate>
                                <a href="<%# Eval("EditScreenshotLink") %>">Edit Screenshot</a>
                            </LoggedInTemplate>
                        </asp:LoginView>
                        </p>
                    </div>
                </div>
                
            </ItemTemplate>
        </asp:Repeater>
        
    </div>

</asp:Content>
