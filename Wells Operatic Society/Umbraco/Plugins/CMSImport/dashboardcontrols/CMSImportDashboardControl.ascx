<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CMSImportDashboardControl.ascx.cs" Inherits="CMSImport.Dashboard.CMSImportDashboardControl" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbraco" %>
<asp:PlaceHolder runat="server" ID="ImportJsPlaceHolder">
<script type="text/javascript">

        var importModel = null;
        $(document).ready(function () {

            importModel =
            {
                importButtonControlId: '<%=ImportButton.UniqueID%>',
                guid: $('#<%=ImportStateId.ClientID%>').val(),
                state: $('#<%=ImportStateField.ClientID%>').val(),
                importUrl: '../plugins/cmsimport/pages/wizard.aspx/Import',
                statusUrl: '../plugins/cmsimport/pages/wizard.aspx/GetStatus'
            };

            startImport();
        });

    function showResult(model) {
        $('.importFinishedReport').html(model.Status);
        $('#ShowInprogressDialog').hide();
        if (model.ErrorCount > 0) {
            $('#clientImportErrorDiv').show();
        } else {
            $('#clientImportFinishedDiv').show();
        }
    }

    </script>
    </asp:PlaceHolder>    
<asp:panel id="CMSImportDashBoard" runat="server">
    <asp:HiddenField runat="server" ID="ImportStateField"/>
    <asp:HiddenField runat="server" ID="ImportStateId"/>
    <div id="clientImportFinishedDiv"><umbraco:Feedback ID="ImportFeedback" runat="server"></umbraco:Feedback></div>
    <div id="clientImportErrorDiv"><umbraco:Feedback runat="server" ID="ClientErrorFeedback" type="error"/></div>

<asp:ValidationSummary ID="DashboardSummary" runat="server" ValidationGroup ="CMSImportDashboard" CssClass="error" />
    <div id="wizardD">
<umbraco:Pane ID="StartImportPane" runat="server">
<asp:Image ID="logoImage" CssClass="cmsimportDashboardLogo" runat="server" />
<asp:Literal ID="IntroLiteral" runat="server"></asp:Literal><br /><br />
<asp:PlaceHolder ID="NoDefinitionsPlaceHolder" runat="server"><asp:Literal ID="NoDefinitionsLiteral" runat="server" /></asp:PlaceHolder>
</umbraco:Pane>
<umbraco:Pane ID="SelectDefinitionPane" runat="server"><umbraco:PropertyPanel ID="SelectImportDefinition"  runat="server">
    <asp:DropDownList ID="SelectImportDefinitionDropdownlist" runat="server" 
        AutoPostBack="true" CausesValidation="false" 
        onselectedindexchanged="SelectImportDefinitionDropdownlist_SelectedIndexChanged" /></umbraco:PropertyPanel></umbraco:Pane>
<umbraco:Pane ID="UploadFilePane" runat="server">
<umbraco:PropertyPanel ID="UploadFilePropertyPanel"  runat="server"><asp:FileUpload ID="DatasourceUpload" runat="server" /><asp:RequiredFieldValidator ID="DataSourceRequiredField" runat="server" ControlToValidate="DatasourceUpload" ErrorMessage="Datasource is required" Text="*"  ValidationGroup="CMSImportDashboard"/>
    <asp:CustomValidator ID="DataSourceValidator"  runat="server" 
        ControlToValidate="DatasourceUpload" 
        Text="*"  ValidationGroup="CMSImportDashboard" 
        onservervalidate="DataSourceValidator_ServerValidate" /></umbraco:PropertyPanel>
</umbraco:Pane>
<umbraco:Pane ID="UploadMediaPane" runat="server">
<umbraco:PropertyPanel ID="UploadMediaPanel" Text="Upload media" runat="server"><asp:FileUpload ID="MediaUpload" runat="server" /><asp:CustomValidator ID="MediaValidator"  runat="server" 
        ControlToValidate="DatasourceUpload"  
        Text="*"  ValidationGroup="CMSImportDashboard" 
        onservervalidate="MediaValidator_ServerValidate" /></umbraco:PropertyPanel>
</umbraco:Pane>
<umbraco:Pane ID="ActionPane" runat="server">
<umbraco:PropertyPanel ID="ActionPropertyPanel" Text="" runat="server">
    <div style="position: relative">
    <asp:Button ID="ImportButton" runat="server" onclick="ImportButton_Click"  ValidationGroup="CMSImportDashboard" CssClass="btn btn-success" />&nbsp;&nbsp;&nbsp;
    <div id="ShowInprogressDialog" style="position:absolute; top:-120px;Left:0;width:400px;height:200px;border-style: solid;background-color:#F8F8FA;z-index:99;border-color:#D9D7D7;border-width:1px;display:none;"><div style="padding-top:40px;padding-left:80px;"><p><asp:Literal ID="Literal1" runat="server" Text="Importing Data don't interrupt this process"/></p>
        <img src="/umbraco_client/images/progressBar.gif" alt="Importing"/><br /><br /><span id="cmsImportStatus"></span></div></div>
        </div>
    </umbraco:PropertyPanel>
    </umbraco:Pane>
</div>
    </asp:panel>
