<% @Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="MvcExtensions.Scaffolding.EntityFramework" %>
<script runat="server">
  private bool ShouldShow(ModelMetadata metadata)
  {
      return metadata.CanShow() && !ViewData.TemplateInfo.Visited(metadata);
  }
</script>
<tr>
    <% foreach (var property in ViewData.ModelMetadata.Properties.Where(ShouldShow)) { %>
    <td>
        <%: Html.Display(property.PropertyName) %>
    </td>
    <% } %>
</tr>