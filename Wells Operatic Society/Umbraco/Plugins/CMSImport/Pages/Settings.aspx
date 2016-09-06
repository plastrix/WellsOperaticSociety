<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Settings.aspx.cs"  MasterPageFile="/umbraco/masterpages/umbracoPage.Master" ValidateRequest="false" Inherits="CMSImport.Pages.Settings" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbraco" %>
<%@ Register Assembly="CMSImportLibrary" Namespace="CMSImportLibrary.UIControls" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <umbraco:UmbracoPanel ID="UmbracoPanel" runat="server">
   <umbraco:Feedback ID="ProEditionInfo" runat="server" />
         <umbraco:Pane ID="Pane1" runat="server">
         <umbraco:PropertyPanel ID="EmailFromAdressPropertyPanel" runat="server" Text="emailFromAdressPropertyPanel"><asp:TextBox ID="EmailFromAdressTextBox" runat="server" Width="300" /> </umbraco:PropertyPanel>
         <umbraco:PropertyPanel ID="EmaillSubjectPropertyPanel" runat="server" Text="emaillSubjectPropertyPanel"><asp:TextBox ID="EmaillSubjectTextbox" runat="server" Width="300" /> </umbraco:PropertyPanel>
         <umbraco:PropertyPanel ID="EmailBodyPropertyPanel" runat="server" Text="emailBodyPropertyPanel">&nbsp;</umbraco:PropertyPanel>
         <umbraco:PropertyPanel ID="PropertyPanel1" runat="server" Text=""><umbraco:CodeArea ID="EmailBodyTextBox" runat="server"  AutoResize="true" OffSetX="47" OffSetY="55" /></umbraco:PropertyPanel>
        </umbraco:Pane>
    </umbraco:UmbracoPanel>
</asp:Content>
