<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CMSImportInstaller.ascx.cs" Inherits="CMSImport.Controls.CMSImportInstaller" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="cc1" %>
<cc1:Pane ID="ReadPane" runat="server">
<asp:Literal ID="InstalledLiteral" runat="server" />
<p><button onclick="window.parent.location.href = '/umbraco/#developer'; return false;" class="btn btn-success">Start using CMSImport</button></p>
</cc1:Pane>
