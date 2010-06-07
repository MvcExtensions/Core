<% @Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList>" %>
<%@ Import Namespace="MvcExtensions.Scaffolding.EntityFramework" %>
<script runat="server">
  private bool ShouldShow(ModelMetadata metadata)
  {
      return metadata.CanShow() && !ViewData.TemplateInfo.Visited(metadata);
  }
</script>
<% var properties = ModelMetadata.FromLambdaExpression(m => m[0], ViewData).Properties.Where(ShouldShow); %>
<table>
    <tr>
        <% foreach (var property in properties) { %>
            <th>
                <%: property.GetDisplayName() %>
            </th>
        <% } %>
    </tr>
    <% for(var i = 0; i < Model.Count; i++) { %>
            <% var counter = i; %>
            <%: Html.DisplayFor(m => m[counter], "ListItem")%>
    <% } %>
</table>