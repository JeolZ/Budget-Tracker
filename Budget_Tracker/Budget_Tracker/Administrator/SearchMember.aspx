﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SearchMember.aspx.cs" Inherits="Budget_Tracker.Administrator.SearchMember" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Search members!</h1>
    <asp:GridView ID="MemberGridView" OnRowDataBound="MemberGridView_RowDataBound" runat="server"></asp:GridView>
</asp:Content>