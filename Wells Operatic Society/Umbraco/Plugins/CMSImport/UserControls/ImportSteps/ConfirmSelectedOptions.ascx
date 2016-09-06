<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfirmSelectedOptions.ascx.cs" Inherits="CMSImport.Controls.ImportSteps.ConfirmSelectedOptions" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbraco" %>
<%@ Register TagPrefix="CMSImport" Namespace="CMSImportLibrary.UIControls" Assembly="CMSImportLibrary" %>
<umbraco:Feedback ID="StructureFreedBack" runat="server" />
<umbraco:Feedback ID="DeleteFeedback" runat="server" />
<CMSImport:ResourceHeaderControl id="DataSourceHeader" runat="server" ResourceKey="Confirm_DataSourceHeader"/>
<umbraco:Pane ID="ConfirmDatasourcePane" runat="server">
       <asp:Repeater runat="server" ID="DatasourceRepeater">
        <ItemTemplate>
        <CMSImport:PersistedCaptionPropertyPanel ID="MapDocumentItemPropertyPanel"  Caption='<%# FormatCaption(Eval("Name"),Eval("Description"))%>' runat="server"><%#Eval("Value") %></CMSImport:PersistedCaptionPropertyPanel>
        </ItemTemplate>
        </asp:Repeater>
</umbraco:Pane>
<CMSImport:ResourceHeaderControl id="ImportOptionsHeader" runat="server" ResourceKey="Confirm_ImportOptionsHeader"/>
<umbraco:Pane ID="MapDynamicPropertiesPane" runat="server">
    <asp:Repeater runat="server" ID="ImportOptionsRepeater">
        <ItemTemplate>
        <CMSImport:PersistedCaptionPropertyPanel ID="MapDocumentItemPropertyPanel"  Caption='<%# FormatCaption(Eval("Name"),Eval("Description"))%>' runat="server"><%#Eval("Value") %></CMSImport:PersistedCaptionPropertyPanel>
</ItemTemplate>

    </asp:Repeater>
</umbraco:Pane>

<CMSImport:ResourceHeaderControl id="ResourceHeaderControl1" runat="server" ResourceKey="ConfirmMapping"/>
<umbraco:Pane ID="MappingPane" runat="server">
<umbraco:PropertyPanel ID="MapDocumentPropertyPanel" runat="server"><asp:Literal ID="MapDatabaseColumnLiteral" runat="server" /></umbraco:PropertyPanel>
<asp:Repeater ID="MappingRepeater" runat="server" >
<ItemTemplate>
<umbraco:PropertyPanel ID="MappingIdPropertyPanel" runat="server" Text='<%#ParseFixedPropertyAlias(Eval("key")) %>'><%#Eval("value") %><%#GetAdvancedSettings(Eval("key").ToString())%></umbraco:PropertyPanel>
</ItemTemplate>
</asp:Repeater>
</umbraco:Pane>













