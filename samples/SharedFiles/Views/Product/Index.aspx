<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Demo.Web.ProductDisplayModel>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= LocalizedTexts.List %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= LocalizedTexts.List %></h2>
    <table>
        <tr>
            <th></th>
            <th>
                <%= LocalizedTexts.Name %>
            </th>
            <th>
                <%= LocalizedTexts.Category %>
            </th>
            <th>
                <%= LocalizedTexts.Supplier %>
            </th>
            <th>
                <%= LocalizedTexts.Price %>
            </th>
            <th></th>
        </tr>
    <% foreach (var item in Model) { %>
        <tr>
            <td>
                <%= Html.ActionLink(LocalizedTexts.Edit, "Edit", new { id = item.Id })%> |
                <%= Html.ActionLink(LocalizedTexts.Details, "Details", new { id = item.Id })%>
            </td>
            <td>
                <%= Html.Encode(item.Name) %>
            </td>
            <td>
                <%= Html.Encode(item.CategoryName) %>
            </td>
            <td>
                <%= Html.Encode(item.SupplierName) %>
            </td>
            <td>
                <%= Html.Encode(string.Format("{0:c}", item.Price)) %>
            </td>
            <td>
                <%= Html.ActionLink(LocalizedTexts.Delete, "Delete", new { id = item.Id })%>
            </td>
        </tr>
    <% } %>
    </table>
    <p>
        <%= Html.ActionLink(LocalizedTexts.Create, "Create")%>
    </p>
</asp:Content>