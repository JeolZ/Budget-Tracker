<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Budget.aspx.cs" Inherits="Budget_Tracker.Member.Budget" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 id="BudgetHeaderText" runat="server">Your budget!</h1>

    <asp:Button ID="StatsButton" OnClick="StatsButton_Click" runat="server" Text="Check the statistics!" />

    <asp:GridView ID="BudgetGridView" OnRowDataBound="BudgetGridView_RowDataBound" runat="server">
    </asp:GridView>

</asp:Content>
