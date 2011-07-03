#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;

    using Moq;
    using Xunit;

    public class HttpContextBaseExtensionsTests
    {
        private readonly Mock<HttpContextBase> httpContext;

        public HttpContextBaseExtensionsTests()
        {
            httpContext = new Mock<HttpContextBase>();
        }

        [Fact]
        public void Cache_should_set_cacheability_to_public()
        {
            var cache = new Mock<HttpCachePolicyBase>();
            httpContext.SetupGet(c => c.Response.Cache).Returns(cache.Object);

            httpContext.Object.Cache(new TimeSpan());

            cache.Verify(c => c.SetCacheability(HttpCacheability.Public), Times.Once());
        }

        [Fact]
        public void Cache_should_set_omit_vary_star_to_true()
        {
            var cache = new Mock<HttpCachePolicyBase>();
            httpContext.SetupGet(c => c.Response.Cache).Returns(cache.Object);

            httpContext.Object.Cache(new TimeSpan());

            cache.Verify(c => c.SetOmitVaryStar(true), Times.Once());
        }

        [Fact]
        public void Cache_should_set_expires_to_based_upon_context_timestamp()
        {
            var cache = new Mock<HttpCachePolicyBase>();
            httpContext.SetupGet(c => c.Response.Cache).Returns(cache.Object);

            var duration = new TimeSpan(1, 0, 0);
            var expectedDateTime = httpContext.Object.Timestamp.Add(duration);

            httpContext.Object.Cache(duration);

            cache.Verify(c => c.SetExpires(expectedDateTime), Times.Once());
        }

        [Fact]
        public void Cache_should_set_cache_valid_until_expires_to_true()
        {
            var cache = new Mock<HttpCachePolicyBase>();
            httpContext.SetupGet(c => c.Response.Cache).Returns(cache.Object);

            httpContext.Object.Cache(new TimeSpan());

            cache.Verify(c => c.SetValidUntilExpires(true), Times.Once());
        }

        [Fact]
        public void Cache_should_set_cache_last_modified_to_context_timestamp()
        {
            var cache = new Mock<HttpCachePolicyBase>();
            httpContext.SetupGet(c => c.Response.Cache).Returns(cache.Object);
            httpContext.SetupGet(c => c.Timestamp).Returns(DateTime.Now);

            httpContext.Object.Cache(new TimeSpan(1, 0, 0));

            cache.Verify(c => c.SetLastModified(httpContext.Object.Timestamp), Times.Once());
        }

        [Fact]
        public void Cache_should_set_cache_last_modified_from_file_dependencies()
        {
            var cache = new Mock<HttpCachePolicyBase>();
            httpContext.SetupGet(c => c.Response.Cache).Returns(cache.Object);

            httpContext.Object.Cache(new TimeSpan(1, 0, 0));

            cache.Verify(c => c.SetLastModifiedFromFileDependencies(), Times.Once());
        }

        [Fact]
        public void Cache_should_set_revalidation_to_all_caches()
        {
            var cache = new Mock<HttpCachePolicyBase>();
            httpContext.SetupGet(c => c.Response.Cache).Returns(cache.Object);

            httpContext.Object.Cache(new TimeSpan());

            cache.Verify(c => c.SetRevalidation(HttpCacheRevalidation.AllCaches), Times.Once());
        }

        [Fact]
        public void Should_not_compress_when_browser_is_ie6_or_lower()
        {
            httpContext.SetupGet(c => c.Request.Browser.MajorVersion).Returns(6);

            httpContext.Setup(c => c.Request.Browser.IsBrowser("IE")).Returns(true);
            httpContext.Setup(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()));

            httpContext.Object.Compress();

            httpContext.Verify(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void Should_not_compress_when_accept_encoding_is_blank()
        {
            httpContext.SetupGet(c => c.Request.Browser.MajorVersion).Returns(7);
            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection());

            httpContext.Setup(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()));

            httpContext.Object.Compress();

            httpContext.Verify(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void Should_not_compress_when_accept_encoding_is_identity()
        {
            httpContext.SetupGet(c => c.Request.Browser.MajorVersion).Returns(7);
            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection { { "Accept-Encoding", "identity, deflate, gzip" } });
            httpContext.SetupGet(c => c.Response.Filter).Returns(new MemoryStream());

            httpContext.Setup(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()));

            httpContext.Object.Compress();

            httpContext.Verify(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void Should_not_compress_when_redirect_location_is_set()
        {
            httpContext.SetupGet(c => c.Request.Browser.MajorVersion).Returns(7);
            httpContext.SetupGet(c => c.Response.RedirectLocation).Returns("http://foobar.com");
            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection { { "Accept-Encoding", "identity, deflate, gzip" } });

            httpContext.Setup(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()));

            httpContext.Object.Compress();

            httpContext.Verify(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void Should_not_throw_exception_when_compression_fails()
        {
            httpContext.SetupGet(c => c.Request.Browser.MajorVersion).Returns(7);
            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection { { "Accept-Encoding", "*" } });

            httpContext.Setup(c => c.Response.AppendHeader(It.IsAny<string>(), It.IsAny<string>())).Throws<HttpException>();

            Assert.DoesNotThrow(() => httpContext.Object.Compress());
        }

        [Fact(Skip = "for build debug")]
        public void Should_compress_as_gzip_when_prefered_encoding_is_wildcard()
        {
            httpContext.SetupGet(c => c.Request.Browser.MajorVersion).Returns(7);
            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection { { "Accept-Encoding", "*, deflate, gzip" } });
            httpContext.SetupGet(c => c.Response.Filter).Returns(new MemoryStream());

            httpContext.Setup(c => c.Response.AppendHeader("Content-encoding", "gzip")).Verifiable();
            httpContext.SetupSet(c => c.Response.Filter).Verifiable();

            httpContext.Object.Compress();

            httpContext.Verify();
        }

        [Fact(Skip = "for build debug")]
        public void Should_compress_as_gzip_when_prefered_encoding_is_gzip()
        {
            httpContext.SetupGet(c => c.Request.Browser.MajorVersion).Returns(7);
            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection { { "Accept-Encoding", "gzip, deflate" } });
            httpContext.SetupGet(c => c.Response.Filter).Returns(new MemoryStream());

            httpContext.Setup(c => c.Response.AppendHeader("Content-encoding", "gzip")).Verifiable();
            httpContext.SetupSet(c => c.Response.Filter).Verifiable();

            httpContext.Object.Compress();

            httpContext.Verify();
        }

        [Fact(Skip = "for build debug")]
        public void Should_compress_as_deflate_when_prefered_encoding_is_deflate()
        {
            httpContext.SetupGet(c => c.Request.Browser.MajorVersion).Returns(7);
            httpContext.SetupGet(c => c.Request.Headers).Returns(new NameValueCollection { { "Accept-Encoding", "deflate, gzip" } });
            httpContext.SetupGet(c => c.Response.Filter).Returns(new MemoryStream());

            httpContext.Setup(c => c.Response.AppendHeader("Content-encoding", "deflate")).Verifiable();
            httpContext.SetupSet(c => c.Response.Filter).Verifiable();

            httpContext.Object.Compress();

            httpContext.Verify();
        }
    }
}