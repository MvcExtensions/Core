<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="MvcExtensions"%>
<script runat="server">
    ModelMetadataItemSelectableElementSetting GetSelectableElementSetting()
    {
        ExtendedModelMetadata metadata = ViewData.ModelMetadata as ExtendedModelMetadata;

        return (metadata != null) ? metadata.Metadata.AdditionalSettings
                                                     .OfType<ModelMetadataItemSelectableElementSetting>()
                                                     .SingleOrDefault() : null;
    }
</script>
<% ModelMetadataItemSelectableElementSetting setting = GetSelectableElementSetting();%>
<% if (setting != null) {%>
    <%= Html.ListBox(null, ViewData.Eval(setting.ViewDataKey) as IEnumerable<SelectListItem>)%>
<% }%>
<% else {%>
    <%= Html.DisplayForModel()%>
<% }%>