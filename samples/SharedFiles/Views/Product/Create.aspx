<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Demo.Web.ProductEditModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= LocalizedTexts.Create %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= LocalizedTexts.Create %></h2>
    <%= Html.ValidationSummary(false, LocalizedTexts.CreateValidationSummary)%>
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <fieldset>
            <%= Html.EditorForModel() %>
            <p>
                <input type="submit" value="<%= LocalizedTexts.Create %>" />
            </p>
        </fieldset>
    <% } %>
    <div>
        <%= Html.ActionLink(LocalizedTexts.BackToList, "Index")%>
    </div>
</asp:Content>