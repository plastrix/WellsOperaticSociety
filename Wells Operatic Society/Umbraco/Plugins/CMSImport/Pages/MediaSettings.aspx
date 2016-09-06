<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MediaSettings.aspx.cs" MasterPageFile="/umbraco/masterpages/umbracoPage.Master" ValidateRequest="false" Inherits="CMSImport.Pages.MediaSettings" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="cc1" %>
<%@ Register TagPrefix="CMSImport" Namespace="CMSImportLibrary.UIControls" Assembly="CMSImportLibrary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <cc1:UmbracoPanel ID="UmbracoPanel" runat="server">
        <cc1:Pane ID="MediaSettingsPane" runat="server" Text="">
        <h3><asp:literal ID="AdditionalSettingsTitle" runat="server" /></h3>
            <p><asp:literal ID="AdditionalSettingsIntro" runat="server" /></p>
         <cc1:PropertyPanel ID="MediaImportDefaultExtensions" runat="server" Text="MediaImportDefaultExtensions"><asp:TextBox ID="ExtensionTextBox"  runat="server" Width="400" CssClass="guiInputText umb-editor" />&nbsp;<asp:LinkButton OnClick="AddButton_Click" ID="AddButton" runat="server" CssClass="btn" /><br />
         <asp:PlaceHolder ID="MultiDropdownPlaceHolder" runat="server" />
          </cc1:PropertyPanel>
          </cc1:Pane>
          <cc1:Pane ID="Pane2" runat="server">
                   <cc1:PropertyPanel ID="MediaImportAllowedDomains" runat="server" Text="MediaImportAllowedDomains"><asp:TextBox ID="DomainTextBox"  runat="server" Width="400" CssClass="guiInputText umb-editor" Text="http://" />&nbsp;<asp:LinkButton OnClick="AddDomainButton_Click" ID="AddDomainButton" runat="server" CssClass="btn" /><br />
         <asp:PlaceHolder ID="MultiDomainPlaceholder" runat="server" />
          </cc1:PropertyPanel>

        </cc1:Pane>
            <cc1:Pane ID="MediaImportLocationPane" runat="server">
                   <cc1:PropertyPanel ID="MediaImportLocationPanel" runat="server" ><asp:TextBox ID="MediaImportLocationTextBox"  runat="server" CssClass="guiInputText umb-editor umb-textstring"  />
          </cc1:PropertyPanel>
        </cc1:Pane>
        <cc1:Pane ID="MediaImportKeepFolderStructurePane" runat="server">
                   <cc1:PropertyPanel ID="MediaImportKeepFolderStructurePanel" runat="server" ><asp:CheckBox runat="server" ID="MediaImportKeepFolderStructureCheckBox"/>
          </cc1:PropertyPanel>
        </cc1:Pane>
        <CMSImport:ResourceHeaderControl id="ResourceHeaderControl1" runat="server" ResourceKey="MediaTypeSettings"/>
         <cc1:Pane ID="ImageTypePane" runat="server">
                   <cc1:PropertyPanel ID="ImageTypePanel" runat="server" ><asp:DropDownList ID="ImageTypeDropdown" runat="Server" />
          </cc1:PropertyPanel>
        </cc1:Pane>
        
        <cc1:Pane ID="FileTypePane" runat="server">
                   <cc1:PropertyPanel ID="FileTypePanel" runat="server" ><asp:DropDownList ID="FileTypeDropdown" runat="Server" />
          </cc1:PropertyPanel>
        </cc1:Pane>
        
        <cc1:Pane ID="FolderTypePane" runat="server">
                   <cc1:PropertyPanel ID="FolderTypePanel" runat="server" ><asp:DropDownList ID="FolderTypeDropdown" runat="Server" />
          </cc1:PropertyPanel>
        </cc1:Pane>

    </cc1:UmbracoPanel>
</asp:Content>
