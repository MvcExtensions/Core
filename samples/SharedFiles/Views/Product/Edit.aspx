<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Demo.Web.ProductEditModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= LocalizedTexts.Edit %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= LocalizedTexts.Edit %></h2>
    <%= Html.ValidationSummary(false, LocalizedTexts.EditValidationSummary)%>
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <fieldset>
            <%= Html.EditorForModel() %>
            <p>
                <input type="submit" value="<%= LocalizedTexts.Edit %>" />
            </p>
        </fieldset>
    <% } %>
    <div>
        <%= Html.ActionLink(LocalizedTexts.BackToList, "Index")%>
    </div>
</asp:Content>