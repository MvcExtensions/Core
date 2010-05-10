<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="MvcExtensions"%>
<script runat="server">
    string GetSelectedText()
    {
        ExtendedModelMetadata metadata = ViewData.ModelMetadata as ExtendedModelMetadata;

        if (metadata != null)
        {
            ModelMetadataItemSelectableElementSetting setting = metadata.Metadata
                                                                        .AdditionalSettings
                                                                        .OfType<ModelMetadataItemSelectableElementSetting>()
                                                                        .SingleOrDefault();

            if (setting != null)
            {
                IEnumerable<SelectListItem> selectList = ViewData.Eval(setting.ViewDataKey) as IEnumerable<SelectListItem>;

                if ((selectList != null) && selectList.Any())
                {
                    return selectList.Where(item => item.Selected)
                                     .Select(item => item.Text)
                                     .FirstOrDefault();
                }
            }
        }

        return null;
    }
</script>
<% string selectedText = GetSelectedText(); %>
<% if (selectedText != null) {%>
    <%= Html.Encode(selectedText) %>
<% }%>
<% else {%>
    <%= Html.Encode(ViewData.TemplateInfo.FormattedModelValue) %>
<% }%>