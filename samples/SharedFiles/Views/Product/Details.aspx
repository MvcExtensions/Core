<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Demo.Web.ProductDisplayModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Details</h2>
    <fieldset>
        <legend>Fields</legend>
        <%= Html.DisplayForModel()%>
    </fieldset>
    <p>
        <%= Html.ActionLink("Edit", "Edit", new { id = Model.Id }) %> |
        <%= Html.ActionLink("Delete", "Delete", new { id = Model.Id })%> |
        <%= Html.ActionLink("Back to List", "Index") %>
    </p>
</asp:Content>