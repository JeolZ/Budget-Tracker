<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BudgetStats.aspx.cs" Inherits="Budget_Tracker.Member.BudgetStats" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 runat="server">Budget statistics!</h1>

    <div id="totalAmountdiv">
        TOTAL AMOUNT:
        <asp:Label ID="TotalAmount" runat="server"></asp:Label>
    </div>

    <ajaxToolkit:LineChart ID="BudgetLineChart" runat="server"></ajaxToolkit:LineChart>

    <br />

    <ajaxToolkit:PieChart ID="BudgetPieChart" ChartType="Column" runat="server"></ajaxToolkit:PieChart>

    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
</asp:Content>
