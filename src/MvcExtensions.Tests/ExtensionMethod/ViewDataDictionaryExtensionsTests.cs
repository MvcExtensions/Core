#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Web.Mvc;

    using Xunit;

    public class ViewDataDictionaryExtensionsTests
    {
        private readonly ViewDataDictionary viewData;

        public ViewDataDictionaryExtensionsTests()
        {
            viewData = new ViewDataDictionary();
        }

        [Fact]
        public void Get_should_return_default_value_when_type_does_not_exist()
        {
            Assert.Equal(0, viewData.Get<int>());
        }

        [Fact]
        public void Get_should_return_default_value_when_key_does_not_exist()
        {
            Assert.Equal(0, viewData.Get<int>("foo"));
        }

        [Fact]
        public void Get_should_return_the_provided_default_value_when_key_does_not_exist()
        {
            Assert.Equal(5, viewData.Get("foo", 5));
        }

        [Fact]
        public void Should_be_able_to_set_and_get_value()
        {
            viewData.Set(5);

            Assert.Equal(5, viewData.Get<int>());
        }

        [Fact]
        public void Should_be_able_to_set_and_get_value_by_key()
        {
            viewData.Set("foo", 5);

            Assert.Equal(5, viewData.Get<int>("foo"));
        }

        [Fact]
        public void Should_be_able_to_check_whether_a_type_exists()
        {
            Assert.False(viewData.Contains<int>());
        }

        [Fact]
        public void Should_be_able_to_remove_a_type()
        {
            viewData.Set(5);
            viewData.Remove<int>();

            Assert.False(viewData.Contains<int>());
        }

        [Fact]
        public void Should_be_able_to_convert_to_serializable_type()
        {
            viewData.Add(@"foo", @"bar");
            viewData.Model = new { baz = @"quz" };

            viewData.ModelState.AddModelError(@"foo", @"foo cannot have bar");
            viewData.ModelState.AddModelError(@"baz", @"baz cannot have quz");

            var serialized = viewData.AsSerializable();

            Assert.True(serialized.GetType().GetProperty("viewData") != null);
            Assert.True(serialized.GetType().GetProperty("model") != null);
            Assert.True(serialized.GetType().GetProperty("modelStates") != null);
        }

        [Fact]
        public void Should_be_able_to_convert_to_json()
        {
            Assert.NotEqual(string.Empty, viewData.ToJson());
        }
    }
}