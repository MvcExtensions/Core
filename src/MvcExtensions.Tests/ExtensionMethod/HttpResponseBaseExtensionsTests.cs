#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Web;

    using Moq;
    using Xunit;

    public class HttpResponseBaseExtensionsTests
    {
        [Fact]
        public void Should_be_able_to_write_json()
        {
            var httpResponse = new Mock<HttpResponseBase>();

            httpResponse.Setup(r => r.Clear());
            httpResponse.SetupSet(r => r.ContentType = "application/json");
            httpResponse.Setup(r => r.Write(It.IsAny<string>()));

            httpResponse.Object.WriteJson("{ \"foo\" : \"bar\" }");

            httpResponse.VerifyAll();
        }
    }
}