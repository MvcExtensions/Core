<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Demo.Web.ProductEditModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Edit</h2>
    <%= Html.ValidationSummary(false, "Edit was unsuccessful. Please correct the errors and try again.") %>
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <fieldset>
            <legend>Fields</legend>
            <%= Html.EditorForModel() %>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>
    <% } %>
    <div>
        <%= Html.ActionLink("Back to List", "Index") %>
    </div>
</asp:Content>