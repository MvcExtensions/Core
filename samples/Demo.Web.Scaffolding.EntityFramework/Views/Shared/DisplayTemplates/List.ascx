<% @Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList>" %>
<script runat="server">
  private static bool ShouldShow(ModelMetadata metadata, ViewDataDictionary viewData)
  {
    return metadata.ShowForDisplay && !metadata.ModelType.Equals(typeof(System.Data.EntityState)) && !metadata.IsComplexType && !viewData.TemplateInfo.Visited(metadata);
  }
</script>
<% var properties = ModelMetadata.FromLambdaExpression(m => m[0], ViewData).Properties.Where(pm => ShouldShow(pm, ViewData)); %>
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