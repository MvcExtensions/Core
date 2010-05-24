<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Encode(ViewData["Message"]) %></h2>
    <ul>
        <li>IoC Containers rules everywhere. Currently has adapter for Autofac, Ninject, StructureMap, Unity and Windsor.</li>
        <li>Supports multiple Modelmetadata provider, a fluent ModelMetadata provider is included.</li>
        <li>Fluent Action Filter registration, dependency injection for decorated action filters.</li>
        <li>Bootstrapper can execute arbitrary tasks in predefined order when application starts.</li>
        <li>PerWebRequest which excutes on request begin and ends.</li>
    </ul>
    <p>
        To learn more about ASP.NET MVC Extensions visit <a href="http://mvcextension.net">http://mvcextension.net</a>.
    </p>
</asp:Content>