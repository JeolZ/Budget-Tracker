<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddChange.aspx.cs" Inherits="Budget_Tracker.Member.AddChange" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- form to add a change (expenditure or income) -->
    <div id="AddChange">
        <h1>Add a change!</h1>

        <!-- indicate which type of change -->
        <div id="ChangeType">
            Type:
            <asp:RadioButton ID="ExpenditureBTN" Text="Expenditure" GroupName="ExpOrInc" runat="server" />
            <asp:RadioButton ID="IncomeBTN" Text="Income" GroupName="ExpOrInc" runat="server" />
        </div>

        <!-- indicate the amount of the change -->
        <div id="ChangeAmount">
            Amount:
            <asp:TextBox ID="MoneyAmount" runat="server"></asp:TextBox>
            <!-- Checks that the user enters a valid number (decimal or numerical) -->
            <asp:RegularExpressionValidator ID="MoneyAmountRegex" runat="server" ValidationExpression="^[1-9]\d*(\.\d+)?$"
                ErrorMessage="Please enter valid number." ControlToValidate="MoneyAmount" />
        </div>
        
        <!-- indicate the currency of the change -->
        <div id="ChangeCurrency">
            Currency:
            <asp:DropDownList ID="ChangeCurrencyDD" runat="server" DataSourceID="CurrencyDataSource" DataTextField="currencyName" DataValueField="currencyName"></asp:DropDownList>
            <asp:SqlDataSource ID="CurrencyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [currencyName] FROM [Currency]"></asp:SqlDataSource>
        </div>

        <!-- indicate the date of the change -->
        <div id="ChangeDate">
            Date:
            <asp:TextBox ID="ChangeDateTB" runat="server"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="ChangeDateExtender" TargetControlID="ChangeDateTB" Format="dd/MM/yyyy" runat="server" />
        </div>

        <!-- indicate the time of the change -->
        <div id="ChangeTime">
            Time:
            <asp:TextBox ID="ChangeTimeTB" Text="00:00:00" runat="server"></asp:TextBox>
            <ajaxToolkit:MaskedEditExtender ID="ChangeTimeExtender" TargetControlID="ChangeTimeTB" MaskType="Time" Mask="99:99:99" runat="server" />
            <ajaxToolkit:MaskedEditValidator ID="ChangeTimeValidator" ControlExtender="ChangeTimeExtender" ControlToValidate="ChangeTimeTB" IsValidEmpty="False" InvalidValueMessage="Not a valid time!" EmptyValueMessage="Please enter a valid date (dd-MM-yy)" runat="server"></ajaxToolkit:MaskedEditValidator>
        </div>

        <!-- indicate the payment method of the change -->
        <div id="ChangePaymentMethod">
            Payment Method:
            <asp:DropDownList ID="ChangePaymentMethodDD" runat="server" DataSourceID="PaymentMethodDataSource" DataTextField="paymentMethodName" DataValueField="paymentMethodName"></asp:DropDownList>
            <asp:SqlDataSource ID="PaymentMethodDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [paymentMethodName] FROM [PaymentMethod]"></asp:SqlDataSource>
        </div>

        <!-- indicate the purpose of the change -->
        <div id="ChangePurpose">
            Purpose:
            <asp:DropDownList ID="ChangePurposeDD" runat="server" DataSourceID="PurposeDataSource" DataTextField="PurposeName" DataValueField="PurposeName"></asp:DropDownList>
            <asp:SqlDataSource ID="PurposeDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [PurposeName] FROM [Purpose]"></asp:SqlDataSource>
        </div>

        <!-- indicate the recipent of the change -->
        <div id="ChangeRecipient">
            Recipient:
            <asp:TextBox ID="ChangeRecipientTB" runat="server"></asp:TextBox>
        </div>

        <!-- indicate a comment for the change -->
        <div id="ChangeComment">
            Comment:
            <asp:TextBox ID="ChangeCommentTB" runat="server"></asp:TextBox>
        </div>

        <!-- button to confirm the change -->
        <div id="ChangeConfirm">
            <asp:Button ID="ConfirmButton" OnClick="ConfirmButton_Click" runat="server" Text="Confirm" />
        </div>

        <asp:Label ID="ResultLabel" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>
