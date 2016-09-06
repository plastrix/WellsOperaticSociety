<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleSettings.aspx.cs"   MasterPageFile="/umbraco/masterpages/umbracoPage.Master" ValidateRequest="false" Inherits="CMSImport.Pages.ScheduleSettings" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <cc1:UmbracoPanel ID="UmbracoPanel" runat="server">
        <cc1:Pane ID="Pane1" runat="server">
         <cc1:PropertyPanel ID="EmailFromAdressPropertyPanel" runat="server" Text="emailFromAdressPropertyPanel"><asp:TextBox ID="EmailFromAdressTextBox" runat="server" Width="300" CssClass="guiInputText" /> </cc1:PropertyPanel>
         <cc1:PropertyPanel ID="EmaillSubjectPropertyPanel" runat="server" Text="emaillSubjectPropertyPanel"><asp:TextBox ID="EmaillSubjectTextbox" runat="server" Width="300" CssClass="guiInputText" /> </cc1:PropertyPanel>
         <cc1:PropertyPanel ID="EmailBodyPropertyPanel" runat="server" Text="emailBodyPropertyPanel">&nbsp;</cc1:PropertyPanel>
         <cc1:PropertyPanel ID="PropertyPanel1" runat="server" Text=""><cc1:CodeArea ID="EmailBodyTextBox"  AutoResize="true" OffSetX="47" OffSetY="55" runat="server" /></cc1:PropertyPanel>
        </cc1:Pane>
    </cc1:UmbracoPanel>
</asp:Content>

