<% @Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script runat="server">
  private static bool ShouldShow(ModelMetadata metadata, ViewDataDictionary viewData)
  {
    return metadata.ShowForDisplay && !metadata.ModelType.Equals(typeof(System.Data.EntityState)) && !metadata.IsComplexType && !viewData.TemplateInfo.Visited(metadata);
  }
</script>
<tr>
    <% foreach (var property in ViewData.ModelMetadata.Properties.Where(pm => ShouldShow(pm, ViewData))) { %>
    <td>
        <%: Html.Display(property.PropertyName) %>
    </td>
    <% } %>
</tr>