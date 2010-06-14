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
    <%= Html.DropDownList(null, ViewData.Eval(setting.ViewDataKey) as IEnumerable<SelectListItem>, (setting.OptionLabel != null) ? setting.OptionLabel() : null)%>
<% }%>
<% else {%>
    <%= Html.DisplayForModel()%>
<% }%>