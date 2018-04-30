<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddItems.aspx.cs" Inherits="Budget_Tracker.Administrator.AddItems" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:DropDownList ID="PossibleItems" OnSelectedIndexChanged="PossibleItems_SelectedIndexChanged" AutoPostBack="true" runat="server">
        <asp:ListItem Selected="true" Value="defaultValue">Select an item</asp:ListItem>
        <asp:ListItem Value="Purpose" Text="Purpose"></asp:ListItem>
        <asp:ListItem Value="Currency" Text="Currency"></asp:ListItem>
        <asp:ListItem Value="PaymentMethod" Text="Payment Method"></asp:ListItem>
    </asp:DropDownList>

    <!-- list all the items of a table -->
    <asp:GridView ID="ItemNamesList" runat="server"></asp:GridView>
    <asp:Label ID="ItemName" runat="server" Text=""></asp:Label>
    <asp:Panel ID="AddItemForm" runat="server">
        <!-- form to add a purpose -->
        <asp:Panel ID="PurposePanel" runat="server" Visible="false">
            <asp:Label ID="purposeName" runat="server" Text="Purpose name : "></asp:Label>
            <asp:TextBox ID="purposeNameTB" runat="server"></asp:TextBox>
            <asp:Button ID="CreatePurposeBTN" OnClick="addPurpose" runat="server" Text="Create" />
        </asp:Panel>

        <!-- form to add a currency -->
        <asp:Panel ID="CurrencyPanel" runat="server" Visible="false">
            <asp:Label ID="currencyName" runat="server" Text="Currency name : "></asp:Label>
            <asp:TextBox ID="currencyNameTB" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="currencyNameValidator" ValidationExpression="[A-Z]{1,4}" runat="server" ControlToValidate="currencyNameTB" ErrorMessage="Please enter a valid name (max. 4 uppercase letters)"></asp:RegularExpressionValidator>
            <br />
            <asp:Label ID="currencySymbol" runat="server" Text="Currency symbol : "></asp:Label>
            <asp:TextBox ID="currencySymbolTB" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression=".{1,3}" runat="server" ControlToValidate="currencySymbolTB" ErrorMessage="Please enter a valid symbol (max. 3 characters)"></asp:RegularExpressionValidator>
            <br />
            <asp:Button ID="CreateCurrencyBTN" OnClick="addCurrency" runat="server" Text="Create" />
        </asp:Panel>

        <!-- form to add a payment method -->
        <asp:Panel ID="PaymentMethodPanel" runat="server" Visible="false">
            <asp:Label ID="PMName" runat="server" Text="Payment method name : "></asp:Label>
            <asp:TextBox ID="PMNameTB" runat="server"></asp:TextBox>
            <asp:Button ID="CreatePMBTN" OnClick="addPaymentMethod" runat="server" Text="Create" />
        </asp:Panel>
    </asp:Panel>

</asp:Content>
