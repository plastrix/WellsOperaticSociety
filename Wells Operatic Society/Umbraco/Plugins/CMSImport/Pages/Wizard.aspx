<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Wizard.aspx.cs" Trace="false" MasterPageFile="/umbraco/masterpages/umbracoPage.Master"
    Inherits="CMSImport.Pages.Wizard" %>
    <%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbraco" %>
<%@ Register Src="../UserControls/CMSImport.ascx" TagName="Wizard"
    TagPrefix="CMSImport" %>
    <%@ Register Src="../UserControls/StateError.ascx" TagName="StateError"
    TagPrefix="CMSImport" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="cc1" %>
<asp:Content runat="server" ID="Head" ContentPlaceHolderID="head">
    <script type="text/javascript">
        
        var importModel = null;
        $(document).ready(function () {
            
            importModel = 
            {
                importButtonControlId: '<%=NextButtonControlId%>',
                guid: $('#<%=ImportStateId.ClientID%>').val(),
                state: $('#<%=ImportState.ClientID%>').val(),
                importUrl: 'wizard.aspx/Import',
                statusUrl: 'wizard.aspx/GetStatus'
            };

        });
        function showResult(model) {
            __doPostBack(importModel.importButtonControlId, '');
        }

    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <asp:HiddenField runat="server" ID="ImportState"/>
    <asp:HiddenField runat="server" ID="ImportStateId"/>
    <cc1:UmbracoPanel ID="UmbracoPanel" runat="server">
   <umbraco:Feedback ID="ProEditionInfo" runat="server" />
        <div id="clientImportErrorDiv"><umbraco:Feedback runat="server" ID="ClientErrorFeedback" type="error"/></div>
            <asp:Panel runat="server" ID="CSSPanel">
            <asp:PlaceHolder ID="CMSImportPlaceHolder" runat="server">
                <div id="wizardD">
                <CMSImport:StateError id="StateError" runat="server" />
                    <CMSImport:Wizard id="CMSImport" runat="server" />
                    </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="SaveStatePlaceholder" runat="server">
                <umbraco:Pane ID="SavePane" runat="server">
                    <asp:PlaceHolder runat="server" ID="LegacySaveHeaderPlaceHolder">
    <h2><asp:Literal ID="LegacySaveHeader" runat="server"/></h2>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="SaveHeaderPlaceHolder">
    <h3><asp:Literal ID="SaveHeader" runat="server"/></h3>
</asp:PlaceHolder>
                   
                <asp:ValidationSummary ID="saveSummary" runat="server" ValidationGroup="SaveState" />
                <umbraco:PropertyPanel runat="server" ID="SaveOptionsPanel">
                    <asp:TextBox ID="SaveAsTextbox" ValidationGroup="SaveState" MaxLength="250" runat="server" /><asp:RequiredFieldValidator
                                ID="SaveAsNameValidator" ControlToValidate="SaveAsTextbox" ValidationGroup="SaveState"
                                runat="server" Text="*"></asp:RequiredFieldValidator>

                </umbraco:PropertyPanel>
                    <umbraco:PropertyPanel runat="server" ID="SaveAsPanel"><asp:CheckBox runat="server" ID="CreateCopyCheckBox"/></umbraco:PropertyPanel>
                     </umbraco:Pane>
            </asp:PlaceHolder>
                </asp:Panel>
    </cc1:UmbracoPanel>
</asp:Content>
