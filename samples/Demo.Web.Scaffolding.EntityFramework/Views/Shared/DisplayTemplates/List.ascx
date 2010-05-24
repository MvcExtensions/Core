<% @Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList>" %>
<script runat="server">
  public static bool ShouldShow(ModelMetadata metadata, ViewDataDictionary viewData)
  {
    return metadata.ShowForDisplay && !metadata.ModelType.Equals(typeof(System.Data.EntityState)) && !metadata.IsComplexType && !viewData.TemplateInfo.Visited(metadata);
  }
</script>
<% var properties = ModelMetadata.FromLambdaExpression(m => m[0], ViewData).Properties.Where(pm => ShouldShow(pm, ViewData)); %>
<table>
    <tr>
        <% foreach (var property in properties) { %>
            <th>
                <%= property.GetDisplayName() %>
            </th>
        <% } %>
    </tr>
    <% for(int i = 0; i < Model.Count; i++) {
        var counter = i;
        var itemMetadata = ModelMetadata.FromLambdaExpression(m => m[counter], ViewData); %>
        <tr>
            <% foreach (var property in properties) { %>
            <td>
                <% var localProperty = property; %>
                <% var propertyMetadata = itemMetadata.Properties.Single(m => m.PropertyName == localProperty.PropertyName); %>
                <%= Html.DisplayFor(m => propertyMetadata.Model) %>
            </td>
            <% } %>
        </tr>
    <% } %>
</table>