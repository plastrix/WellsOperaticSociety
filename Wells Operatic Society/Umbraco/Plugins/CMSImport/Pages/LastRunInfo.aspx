<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LastRunInfo.aspx.cs" MasterPageFile="~/umbraco/masterpages/umbracoDialog.Master"
    Inherits="CMSImport.Pages.LastRunInfo" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umbraco" %>
<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <umbraco:Feedback Id="SchedulerFeedback" runat="server"/>
    <umbraco:Pane ID="ScheduledTaskInfoPane" runat="server">
        <umbraco:PropertyPanel runat="server" ID="InfoPanel">
    <table>
        <tr><td style="padding-right: 10px;"><strong><asp:Literal ID="ScheduledTaskNameCaptionLiteral" runat="server" /></strong></td><td><asp:Literal ID="ScheduledTaskNameLiteral" runat="server" /></td></tr>
        <tr><td style="padding-right: 10px;"><strong><asp:Literal ID="ScheduledTimeCaptionLiteral" runat="server" /></strong></td><td><asp:Literal ID="ScheduledTimeLiteral" runat="server" /></td></tr>
        <tr><td style="padding-right: 10px;"><strong><asp:Literal ID="NextRunCaptionLiteral" runat="server" /></strong></td><td><asp:Literal ID="NextRunLiteral" runat="server" /></td></tr>
        <tr><td style="padding-right: 10px;"><strong><asp:Literal ID="LastRunCaptionLiteral" runat="server" /></strong></td><td><asp:Literal ID="LastRunLiteral" runat="server" /></td></tr>
    </table>
            </umbraco:PropertyPanel>
    </umbraco:Pane>
    <umbraco:Pane ID="HistoryPane" runat="server">
        <umbraco:PropertyPanel ID="HistoryPanel" runat="server">
            <asp:ListView ID="HistoryList" runat="server" OnPagePropertiesChanged="HistoryList_PagePropertiesChanged">
                <LayoutTemplate>
                    <table>
                    <tr><td style="width:250px;"><strong>Scheduled</strong></td><td><strong>Executed</strong></td></tr>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("scheduledOn", "{0:f}")%>
                        </td>
                        <td>
                            <%#Eval("executedon", "{0:f}")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            <asp:DataPager PagedControlID="HistoryList" ID="BottomPager" runat="server">
                <Fields>
                    <asp:NumericPagerField ButtonType="Link" ButtonCount="100" />
                </Fields>
            </asp:DataPager>
        </umbraco:PropertyPanel>
        <br/>
        <p><asp:LinkButton ID="ClearHistoryButton" runat="server" OnClientClick="return  confirm('Are you sure?');" OnClick="ClearHistoryButton_Click" CssClass="btn"></asp:LinkButton></p>
    </umbraco:Pane>
</asp:Content>
