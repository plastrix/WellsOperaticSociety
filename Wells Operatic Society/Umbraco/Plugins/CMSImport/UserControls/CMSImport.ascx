<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CMSImport.ascx.cs" Inherits="CMSImport.Controls.CMSImport" %>
<%@ Register src="ImportSteps/Intro.ascx" tagname="Intro" tagprefix="CMSImport" %>
<%@ Register src="ImportSteps/SelectDataSourceType.ascx" tagname="SelectDataSourceType" tagprefix="CMSImport" %>
<%@ Register src="ImportSteps/SelectDataSource.ascx" tagname="SelectDataSource" tagprefix="CMSImport" %>
<%@ Register src="ImportSteps/MapProperties.ascx" tagname="MapProperties" tagprefix="CMSImport" %>
<%@ Register src="ImportSteps/ConfirmSelectedOptions.ascx" tagname="Confirm" tagprefix="CMSImport" %>
<%@ Register src="ImportSteps/Importing.ascx" tagname="Importing" tagprefix="CMSImport" %>
<%@ Register src="ImportSteps/SelectImportProviderOptions.ascx" tagname="SelectImportProviderOptions" tagprefix="CMSImport" %>

<%@ Register TagPrefix="umbraco" Namespace="umbraco.uicontrols" Assembly="controls" %>
<umbraco:Pane ID="HeaderPane" runat="server">
<asp:PlaceHolder runat="server" ID="LegacyHeaderPlaceHolder">
    <asp:Image ID="LogoImage" CssClass="cmsimportLogo" runat="server" />
    <h2><asp:Literal ID="LegacyStepTitleLiteral" runat="server"/></h2>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="HeaderPlaceHolder">
    <h3><asp:Literal ID="StepTitleLiteral" runat="server"/></h3>
</asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="IntroPlaceHolder"><p><asp:Literal runat="server" ID="IntroLiteral"/></p></asp:PlaceHolder>
    <asp:CustomValidator ID="ValidateStep" runat="server" OnServerValidate="Validate" CssClass="error cmsimportError"  Display="Dynamic" />
    </umbraco:Pane>
<asp:PlaceHolder ID="Step0PlaceHolder" runat="server" Visible="False"><CMSImport:Intro ID="IntroStep" runat="server" /></asp:PlaceHolder>
<asp:PlaceHolder ID="Step1PlaceHolder" runat="server" Visible="False"><CMSImport:SelectDataSourceType ID="SelectDataSourceTypeStep" runat="server" /></asp:PlaceHolder>
<asp:PlaceHolder ID="Step2PlaceHolder" runat="server" Visible="False"><CMSImport:SelectDataSource ID="SelectDataSourceStep" runat="server" /></asp:PlaceHolder>
<asp:PlaceHolder ID="Step3PlaceHolder" runat="server" Visible="False"><CMSImport:SelectImportProviderOptions ID="SelectImportProviderOptions" runat="server" /></asp:PlaceHolder>
<asp:PlaceHolder ID="Step4PlaceHolder" runat="server" Visible="False"><CMSImport:MapProperties ID="MapPropertiesStep" runat="server" /></asp:PlaceHolder>
<asp:PlaceHolder ID="Step5PlaceHolder" runat="server" Visible="False"><CMSImport:Confirm ID="ConfirmStep" runat="server" /></asp:PlaceHolder>
<asp:PlaceHolder ID="Step6PlaceHolder" runat="server" Visible="False"><CMSImport:Importing ID="ImportStep" runat="server" /></asp:PlaceHolder>
<umbraco:Pane ID="ButtonsPane" runat="server">
<asp:Panel ID="ButtonsPanel"  runat="server" style="position:relative;">
<asp:Button ID="PreviousButton" runat="server"  Text="" CausesValidation="false" OnClick="PreviousButton_Click" CssClass="btn wizardButton"  /><asp:Button ID="NextButton" runat="server" Text=""  OnClick="NextButton_Click" CssClass="btn btn-success wizardButton wizardNextButton"  />
<div id="ShowInprogressDialog" style="position:absolute; top:-120px;Left:10px;width:400px;height:200px;border-style: solid;background-color:#F8F8FA;z-index:99;border-color:#D9D7D7;border-width:1px;display:none;"><div style="padding-top:40px;padding-left:80px;"><p><asp:Literal ID="ImportingDialogLiteral" runat="server" Text="Importing Data don't interrupt this process"/></p>
		<img src="/umbraco_client/images/progressBar.gif" alt="Importing"/><br /><br /><span id="cmsImportStatus"></span></div></div>
</asp:Panel>
    </umbraco:Pane>


