<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddItems.aspx.cs" Inherits="Budget_Tracker.Administrator.AddItems" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DropDownList ID="PossibleItems" OnSelectedIndexChanged="PossibleItems_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
</asp:Content>
