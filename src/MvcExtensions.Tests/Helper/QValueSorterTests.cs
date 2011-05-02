#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Linq;

    using Xunit;
    using Xunit.Extensions;

    public class QValueSorterTests
    {
        [Theory]
        [InlineData(@"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8", @"text/html")]
        [InlineData(@"application/xml;q=0.5,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5", @"application/xhtml+xml")]
        [InlineData(@"image/gif,image/jpeg,image/pjpeg,application/x-ms-application,application/vnd.ms-xpsdocument,application/xaml+xml,application/x-ms-xbap, application/x-shockwave-flash,application/vnd.ms-excel,application/msword,application/vnd.ms-powerpoint,*/*", @"image/gif")]
        public void Should_be_able_sort_according_to_q_value(string acceptTypes, string result)
        {
            var orderedTypes = QValueSorter.Sort(acceptTypes.Split(new[] { ',' }), false);
            var firstType = orderedTypes.First();

            Assert.Equal(result, firstType);
        }
    }
}