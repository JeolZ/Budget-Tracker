<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Budget_Tracker.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Welcome to the Budget Tracker!</h1>
    <br />
    <br />
    The idea of this website is to allow people to track their budgets as in their income and expenditures over time so that they can easily analyse it.
    <br />

    <!-- Login Templates -->
    <div id="LoginTemplates">
        <!-- If user is a non-logged-in visitor (AnonymousTemplate), then... -->
        <!-- Show the login form -->
        <!-- Show the Signup form -->
        <asp:LoginView ID="SignedupView" runat="server">
            <AnonymousTemplate>
                <asp:Login ID="Login" runat="server" DisplayRememberMe="False"></asp:Login>
                <asp:HyperLink ID="SignupHyperlink" NavigateUrl="~/Signup.aspx" runat="server">Signup!</asp:HyperLink>
            </AnonymousTemplate>
        </asp:LoginView>
    </div>

</asp:Content>
