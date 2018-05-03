<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Budget.aspx.cs" Inherits="Budget_Tracker.Member.Budget" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 id="BudgetHeaderText" runat="server">Your budget!</h1>
    <asp:GridView ID="BudgetGridView" OnRowDataBound="BudgetGridView_RowDataBound" runat="server">
    </asp:GridView>
</asp:Content>
