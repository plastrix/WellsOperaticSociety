<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapProperties.ascx.cs" Inherits="CMSImport.Controls.ImportSteps.MapProperties" %>
<%@ Register Assembly="CMSImportLibrary" Namespace=" CMSImportLibrary.UIControls.ProperyMapping" TagPrefix="CMSImport" %>
<%@ Register Assembly="CMSImportLibrary" Namespace=" CMSImportLibrary.UIControls" TagPrefix="CMSImport" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbraco" %>
<%@ Register assembly="CMSImportLibrary" namespace="CMSImportLibrary.UIControls.ProperyMapping" tagprefix="cc1" %>

<!--Dynamic Data-->

 <asp:Repeater ID="dtRepeater" runat="server"  >
<ItemTemplate>
    <%#RenderTab(Eval("PropertyTypeGroup")) %>
    <umbraco:Pane ID="MapDynamicPropertiesPane" runat="server">
<CMSImport:PersistedCaptionPropertyPanel ID="MapDocumentItemPropertyPanel"  Caption='<%# FormatCaption(Eval("PropertyName"),Eval("PropertyAlias"))%>' runat="server"><CMSImport:CommandDropdownlist ID="datasourceDropdown"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "PropertyAlias") %>'  OnSelectedIndexChanged="ColumnDropdown_SelectedIndexChanged"  ondatabinding="ColumnDropdown_DataBinding" OnDataBound="ColumnDropdown_DataBound" runat="server" /> <CMSImport:PropertyMapping id="mapping" PropertyAlias='<%# Eval("PropertyAlias") %>' PropertEditorAlias='<%#GetPropertEditorAlias(Container.DataItem) %>' ContentTypeAlias='<%#Eval("ContentTypeAlias") %>' runat="server"  /> </CMSImport:PersistedCaptionPropertyPanel>
</umbraco:Pane>
</ItemTemplate>
</asp:Repeater>

