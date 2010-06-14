<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Demo.Web.ProductDisplayModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= LocalizedTexts.Details %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= LocalizedTexts.Details %></h2>
    <fieldset>
        <%= Html.DisplayForModel()%>
    </fieldset>
    <p>
        <%= Html.ActionLink(LocalizedTexts.Edit, "Edit", new { id = Model.Id })%> |
        <%= Html.ActionLink(LocalizedTexts.Delete, "Delete", new { id = Model.Id })%> |
        <%= Html.ActionLink(LocalizedTexts.BackToList, "Index")%>
    </p>
</asp:Content>