<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleTask.aspx.cs" Inherits="CMSImport.Pages.ScheduleTask" MasterPageFile="/umbraco/masterpages/umbracoPage.Master" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/StateError.ascx" TagName="StateError"
    TagPrefix="CMSImport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <cc1:UmbracoPanel ID="ScheduledTaskPanel" runat="server">
        <cc1:Feedback Id="SchedulerFeedback" runat="server"/>
        <cc1:Pane ID="HeaderPane" runat="server">
            <asp:PlaceHolder runat="server" ID="LegacyHeaderPlaceholder">
                <asp:Image ID="LogoImage" CssClass="cmsimportLogo" runat="server" />
                <h2>
                    <asp:Literal ID="LegacyTitleLiteral" runat="server" /></h2>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="HeaderPlaceHolder">
                <h3><asp:Literal ID="TitleLiteral" runat="server" /></h3>
            </asp:PlaceHolder>
            <CMSImport:StateError id="StateError" runat="server" />
            <asp:ValidationSummary ID="ScheduleValSum" runat="server"  CssClass="error"/>
            <asp:HiddenField ID="importIDHiddenField" runat="server" />
        </cc1:Pane>
        <cc1:Pane ID="SavedImportNamePane" runat="server">
            <cc1:PropertyPanel ID="SavedImportNamePanel" runat="server" Text="">
                <asp:Literal ID="SavedImportNameLiteral" runat="server" />
            </cc1:PropertyPanel>
        </cc1:Pane>

        <cc1:Pane ID="TaskNamePane" runat="server">
            <cc1:PropertyPanel ID="TaskNamePanel" runat="server" Text="">
                <asp:TextBox ID="TaskNameTextBox" runat="server" MaxLength="50" CssClass="umbEditorTextField" /><asp:RequiredFieldValidator ID="TaskNameValidator" runat="server" Text="*" ControlToValidate="TaskNameTextBox" />
            </cc1:PropertyPanel>
        </cc1:Pane>

        <cc1:Pane ID="NotifyPane" runat="server">
            <cc1:PropertyPanel ID="NotifyPanel" runat="server" Text="">
                <asp:TextBox ID="NotifyTextbox" runat="server" MaxLength="250" CssClass="umbEditorTextField" /><asp:RequiredFieldValidator ID="EmailRequiredValidator" runat="server" Text="*" ControlToValidate="NotifyTextbox" /><asp:CustomValidator ID="NotifyAdressValidator" runat="server" ControlToValidate="NotifyTextbox" Text="*"  OnServerValidate="NotifyAdressValidator_OnServerValidate" />
            </cc1:PropertyPanel>
        </cc1:Pane>

        <cc1:Pane ID="RunAsPane" runat="server">
            <cc1:PropertyPanel ID="RunAsPanel" runat="server" Text="">
                <asp:DropDownList ID="RunAsDropdown" runat="Server" />
            </cc1:PropertyPanel>
        </cc1:Pane>

        <cc1:Pane ID="ExecuteEveryPane" runat="server">
            <cc1:PropertyPanel ID="ExecuteEveryPanel" runat="server" Text="Execute every">
                <asp:RadioButtonList ID="ExecuteEveryRadioList" runat="server" AutoPostBack="true" CausesValidation="false">
                </asp:RadioButtonList>
            </cc1:PropertyPanel>
        </cc1:Pane>

        <cc1:Pane ID="DaysPane" runat="server">
            <cc1:PropertyPanel ID="DaysProperties" runat="server" Text="ON">
                <asp:CheckBoxList ID="DaysCheckboxList" runat="server">
                </asp:CheckBoxList><asp:CustomValidator ID="DaysValidator" runat="server" Text="*" OnServerValidate="DaysValidator_Validate" />
            </cc1:PropertyPanel>

        </cc1:Pane>

        <cc1:Pane ID="TimePane" runat="server">

            <cc1:PropertyPanel ID="TimePanel" runat="server" Text="Time">
                <asp:DropDownList ID="HourDropdown" runat="server" CssClass="smallDropDown" />
                :
                <asp:DropDownList ID="MinuteDropdown" runat="server" CssClass="smallDropDown" />
            </cc1:PropertyPanel>

        </cc1:Pane>

    </cc1:UmbracoPanel>
</asp:Content>
