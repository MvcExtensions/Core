<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Demo.Web.ProductDisplayModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Delete
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Delete</h2>
    <% using (Html.BeginForm()) {%>
        Are you sure you want to delete <%= Html.Encode(Model.Name) %> ?
        <br />
        <br />
        <input type="submit" value="Submit"/>
    <% } %>
    <p>
        <%= Html.ActionLink("Back to List", "Index") %>
    </p>
</asp:Content>