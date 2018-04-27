<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Budget.aspx.cs" Inherits="Budget_Tracker.Member.Budget" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="BudgetGridView" OnRowCreated="BudgetGridView_RowCreated" OnRowDataBound="BudgetGridView_RowDataBound" runat="server">
    </asp:GridView>
</asp:Content>
