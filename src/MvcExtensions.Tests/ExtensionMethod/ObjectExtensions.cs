#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using Xunit;

    public class ObjectExtensions
    {
        [Fact]
        public void Should_be_able_to_convert_to_json()
        {
            var obj = new { foo = "bar" };
            var json = obj.ToJson();

            Assert.NotEqual(string.Empty, json);
        }
    }
}