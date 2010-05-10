#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Moq;
    using Xunit;

    public class XmlResultTests
    {
        private readonly Mock<HttpContextBase> httpContext;
        private readonly ControllerContext controllerContext;

        private readonly XmlResult actionResult;

        public XmlResultTests()
        {
            httpContext = new Mock<HttpContextBase>();
            controllerContext = new ControllerContext(httpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            actionResult = new XmlResult();
        }

        [Fact]
        public void ExecuteResult_should_set_content_type()
        {
            httpContext.SetupSet(c => c.Response.ContentType = "application/xml").Verifiable();

            actionResult.ExecuteResult(controllerContext);

            httpContext.Verify();
        }

        [Fact]
        public void ExecuteResult_should_write_xml_in_response()
        {
            var output = new Mock<TextWriter>();

            output.Setup(o => o.Write(It.IsAny<string>())).Verifiable();
            httpContext.SetupGet(c => c.Response.Output).Returns(output.Object);

            actionResult.Data = new DummyObject { Property1 = "foobar" };

            actionResult.ExecuteResult(controllerContext);

            output.Verify();
        }

        public class DummyObject
        {
            public string Property1 { get; set; }
        }
    }
}