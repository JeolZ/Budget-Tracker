﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="Budget_Tracker.Signup" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" OnCreatedUser="CreateUserWizard1_OnCreatedUser">
        <wizardsteps> 
            <asp:CreateUserWizardStep runat="server"/> <asp:CompleteWizardStep runat="server"/>
        </wizardsteps>
    </asp:CreateUserWizard>
</asp:Content>
