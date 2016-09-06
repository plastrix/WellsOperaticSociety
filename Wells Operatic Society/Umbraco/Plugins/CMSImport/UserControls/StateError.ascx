<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StateError.ascx.cs" Inherits="CMSImport.Controls.StateError" %>
<asp:PlaceHolder ID="ErrorPlaceholder" runat="server">
<div class="error"><h3><asp:Literal runat="server" ID="StateHeaderErrorLiteral" /></h3><p><asp:Literal ID="StateIntroErrorLiteral" runat="server"/><asp:BulletedList ID="ErrorList" runat="server" /></p></div>
</asp:PlaceHolder>