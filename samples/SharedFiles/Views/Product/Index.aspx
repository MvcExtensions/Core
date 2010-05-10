<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Demo.Web.ProductDisplayModel>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Index</h2>
    <table>
        <tr>
            <th></th>
            <th>
                Name
            </th>
            <th>
                Category
            </th>
            <th>
                Supplier
            </th>
            <th>
                Price
            </th>
            <th></th>
        </tr>
    <% foreach (var item in Model) { %>
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id = item.Id }) %> |
                <%= Html.ActionLink("Details", "Details", new { id = item.Id })%>
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
                <%= Html.ActionLink("Delete", "Delete", new { id = item.Id })%>
            </td>
        </tr>
    <% } %>
    </table>
    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>
</asp:Content>