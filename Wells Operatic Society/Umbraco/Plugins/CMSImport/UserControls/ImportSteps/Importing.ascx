<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Importing.ascx.cs" Inherits="CMSImport.Controls.ImportSteps.Importing" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbraco" %>
<umbraco:Pane ID="InfoPane" runat="server">
<umbraco:PropertyPanel ID="ImportDurationPropertyPanel" runat="server"><asp:Literal ID="ImportDurationValueLiteral" runat="server" /></umbraco:PropertyPanel>
<umbraco:PropertyPanel ID="ImportRecordCountPropertyPanel" runat="server"><asp:Literal ID="ImportRecordCountValueLiteral" runat="server" /></umbraco:PropertyPanel>
<umbraco:PropertyPanel ID="ImportSuccesCountPropertyPanel" runat="server"><asp:Literal ID="ImportSuccesCountValueLiteral" runat="server" /></umbraco:PropertyPanel>
    <asp:PlaceHolder ID="UpdatedPlaceholder" runat="server" Visible="false"><umbraco:PropertyPanel ID="ImportUpdatedCountPropertyPanel" runat="server"><asp:Literal ID="ImportUpdatedCountValueLiteral" runat="server" /></umbraco:PropertyPanel></asp:PlaceHolder>
    <asp:PlaceHolder ID="skippedPlaceholder" runat="server" Visible="false"><umbraco:PropertyPanel ID="ImportSkippedCountPropertyPanel" runat="server"><asp:Literal ID="ImportSkippedCountValueLiteral" runat="server" /></umbraco:PropertyPanel></asp:PlaceHolder>
<umbraco:PropertyPanel ID="ImportDeletedPropertyPanel" runat="server"><asp:Literal ID="ImportDeletedCountValueLiteral" runat="server" /></umbraco:PropertyPanel>
    <umbraco:PropertyPanel ID="ImportErrorCounPropertyPanel" runat="server"><asp:Literal ID="ImportErrorCountValueLiteral" runat="server" /></umbraco:PropertyPanel>
</umbraco:Pane>
<asp:PlaceHolder ID="ErroPlaceholder" runat="server" Visible="false">
<umbraco:Pane ID="ErrorPane" runat="server">
<umbraco:PropertyPanel ID="ErrorPropertyPanel" runat="server">
<strong><asp:Literal ID="ErrorPropertyLiteral" runat="server" /></strong><br /><br />
<asp:Repeater id="ImportErrorRepeater" runat="server">
<ItemTemplate> <%#Container.DataItem%> <br /></ItemTemplate>
</asp:Repeater>
</umbraco:PropertyPanel>
</umbraco:Pane>
</asp:PlaceHolder>