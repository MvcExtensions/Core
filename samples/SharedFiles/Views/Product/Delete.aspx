<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Demo.Web.ProductDisplayModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= LocalizedTexts.Delete %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Delete</h2>
    <% using (Html.BeginForm()) {%>
        <%= LocalizedTexts.DeletePrompt %> <%= Html.Encode(Model.Name) %> ?
        <br />
        <br />
        <input type="submit" value="<%= LocalizedTexts.Delete %>"/>
    <% } %>
    <p>
        <%= Html.ActionLink(LocalizedTexts.BackToList, "Index")%>
    </p>
</asp:Content>