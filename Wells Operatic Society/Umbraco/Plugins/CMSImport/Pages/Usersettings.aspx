<%@ Page Language="C#"  ValidateRequest="false" AutoEventWireup="true" CodeBehind="Usersettings.aspx.cs" MasterPageFile="/umbraco/masterpages/umbracoPage.Master"
    Inherits="CMSImport.Pages.Usersettings" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbraco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <umbraco:UmbracoPanel ID="UmbracoPanel" runat="server">
    <umbraco:Pane ID="IntroPane" runat="server" Text="">
    <h3><asp:literal ID="UserImportConfigurationTitle" runat="server"></asp:literal></h3>
    <asp:label ID="IntroLabel" runat="server" Text="">
    
    </asp:label>
    </umbraco:Pane>
        <umbraco:Pane ID="UserSettingsPane" runat="server" Text="">
            <umbraco:PropertyPanel ID="UserImportDefinitions" runat="server" Text="UserImportEnableDefinitions">
                <br />
                <asp:PlaceHolder ID="MultiDropdownPlaceHolder" runat="server" />
            </umbraco:PropertyPanel>
        </umbraco:Pane>
   </umbraco:UmbracoPanel>
</asp:Content>
