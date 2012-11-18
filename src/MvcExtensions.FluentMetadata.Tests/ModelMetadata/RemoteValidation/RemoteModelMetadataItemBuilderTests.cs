#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>, AlexBar <abarbashin@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

// ReSharper disable Mvc.ControllerNotResolved
// ReSharper disable Mvc.AreaNotResolved
// ReSharper disable Mvc.ActionNotResolved
namespace MvcExtensions.FluentMetadata.Tests
{
    using System.Web.Mvc;
    using System.Linq;
    using Xunit;

    public class RemoteModelMetadataItemBuilderTests
    {
         private readonly ModelMetadataItem item;
         private readonly ModelMetadataItemBuilder<string> builder;

        public RemoteModelMetadataItemBuilderTests()
        {
            item = new ModelMetadataItem();
            builder = new ModelMetadataItemBuilder<string>(item);
        }

        [Fact]
        public void Should_be_able_to_set_remote()
        {
            builder.Remote(c => c.For<DummyController>(x => x.CheckUsername1));

            Assert.NotEmpty(item.Validations);
            Assert.IsType<RemoteValidationMetadata>(item.Validations.First());
        }

        [Fact]
        public void Should_be_able_to_set_remote_with_http_method()
        {
            const string HttpMethod = "POST";
            builder.Remote(c => c.HttpMethod(HttpMethod).For<DummyController>(x => x.CheckUsername1));

            var metadata = (RemoteValidationMetadata)item.Validations.First();
            Assert.Equal(metadata.HttpMethod, HttpMethod);
        }

        [Fact]
        public void Should_not_save_http_method_if_no_any_other_params_are_set()
        {
            const string HttpMethod = "POST";
            builder.Remote(c => c.HttpMethod(HttpMethod));

            Assert.Equal(0, item.Validations.Count);
        }

        [Fact]
        public void Should_be_able_to_set_remote_for_action()
        {
            builder.Remote(c => c.For<DummyController>(x => x.CheckUsername1));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal("Dummy", metadata.Controller);
            Assert.Equal("CheckUsername1", metadata.Action);
            Assert.Equal(null, metadata.Area);
        }

        [Fact]
        public void Should_be_able_to_set_remote_for_action_with_area()
        {
            const string AreaName = "area";

            builder.Remote(c => c.For<DummyController>(x => x.CheckUsername1, AreaName));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal("Dummy", metadata.Controller);
            Assert.Equal("CheckUsername1", metadata.Action);
            Assert.Equal(AreaName, metadata.Area);
        }

        [Fact]
        public void Should_be_able_to_set_remote_for_route()
        {
            builder.Remote(c => c.For("routeName"));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal("routeName", metadata.RouteName);
        }

        [Fact]
        public void Should_be_able_to_set_remote_for_route_with_additional_fields()
        {
            builder.Remote(c => c.For("routeName", new[] { "Id", "Id2" }));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal("routeName", metadata.RouteName);
            Assert.Equal("Id,Id2", metadata.AdditionalFields);
        }

        [Fact]
        public void Should_be_able_to_set_remote_for_controller_action_via_string_methods()
        {
            const string Controller = "controller1";
            const string Action = "action1";
            builder.Remote(c => c.For(Controller, Action));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal(Controller, metadata.Controller);
            Assert.Equal(Action, metadata.Action);
            Assert.Null(metadata.Area);
        }

        [Fact]
        public void Should_be_able_to_set_remote_for_controller_action_area_via_string_methods()
        {
            const string Controller = "controller1";
            const string Action = "action1";
            const string AreaName = "area1";
            builder.Remote(c => c.For(Controller, Action, AreaName));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal(Controller, metadata.Controller);
            Assert.Equal(Action, metadata.Action);
            Assert.Equal(AreaName, metadata.Area);
        }

        [Fact]
        public void Should_be_able_to_set_remote_for_controller_action_additional_fields_via_string_methods()
        {
            const string Controller = "controller1";
            const string Action = "action1";
            const string AdditionalFields = "Id2";
            builder.Remote(c => c.For(Controller, Action, new[] { AdditionalFields }));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal(Controller, metadata.Controller);
            Assert.Equal(Action, metadata.Action);
            Assert.Equal(null, metadata.Area);
            Assert.Equal(AdditionalFields, metadata.AdditionalFields);
        }

        [Fact]
        public void Should_be_able_to_set_remote_for_controller_action_with_two_additional_fields_via_string_methods()
        {
            const string Controller = "controller1";
            const string Action = "action1";
            const string AdditionalField1 = "Id";
            const string AdditionalField2 = "Id2";
            builder.Remote(c => c.For(Controller, Action, new[] { AdditionalField1, AdditionalField2 }));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal(string.Format("{0},{1}", AdditionalField1, AdditionalField2), metadata.AdditionalFields);
        }

        [Fact]
        public void Should_be_able_to_set_remote_for_controller_action_area_additional_fields_via_string_methods()
        {
            const string Controller = "controller1";
            const string Action = "action1";
            const string AdditionalFields = "Id";
            const string AreaName = "area1";
            builder.Remote(c => c.For(Controller, Action, AreaName, new[] { AdditionalFields }));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal(Controller, metadata.Controller);
            Assert.Equal(Action, metadata.Action);
            Assert.Equal(AreaName, metadata.Area);
            Assert.Equal(AdditionalFields, metadata.AdditionalFields);
        }

        [Fact]
        public void Should_set_all_controller_action_and_additional_fields_automatically()
        {
            builder.Remote(c => c.For<DummyController>(a => a.CheckUsername2(null, null)));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal("Dummy", metadata.Controller);
            Assert.Equal("CheckUsername2", metadata.Action);
            Assert.Null(metadata.Area);
            Assert.Equal("Id2,Id3", metadata.AdditionalFields);
        }

        [Fact]
        public void Should_set_all_controller_action_area_and_additional_fields_automatically()
        {
            builder.Remote(c => c.For<DummyController>(a => a.CheckUsername2(null, null), "area"));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal("Dummy", metadata.Controller);
            Assert.Equal("CheckUsername2", metadata.Action);
            Assert.Equal("area", metadata.Area);
            Assert.Equal("Id2,Id3", metadata.AdditionalFields);
        }

        [Fact]
        public void Should_set_all_controller_action_automatically_and_set_specified_additional_fields()
        {
            const string AdditionalField1 = "Id";
            const string AdditionalField2 = "Id2";
            builder.Remote(c => c.For<DummyController>(a => a.CheckUsername2(null, null), new[] { AdditionalField1, AdditionalField2 }));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal("Dummy", metadata.Controller);
            Assert.Equal("CheckUsername2", metadata.Action);
            Assert.Null(metadata.Area);
            Assert.Equal(string.Format("{0},{1}", AdditionalField1, AdditionalField2), metadata.AdditionalFields);
        }
        
        [Fact]
        public void Should_set_all_controller_action_area_automatically_and_set_specified_additional_fields()
        {
            const string AdditionalField1 = "Id";
            const string AdditionalField2 = "Id2";
            builder.Remote(c => c.For<DummyController>(a => a.CheckUsername2(null, null), "area", new[] { AdditionalField1, AdditionalField2 }));

            var metadata = (RemoteValidationMetadata)item.Validations.First();

            Assert.Equal("Dummy", metadata.Controller);
            Assert.Equal("CheckUsername2", metadata.Action);
            Assert.Equal("area", metadata.Area);
            Assert.Equal(string.Format("{0},{1}", AdditionalField1, AdditionalField2), metadata.AdditionalFields);
        }

        /// <summary>
        /// DummyController for tests
        /// </summary>
        private class DummyController : Controller
        {
            public JsonResult CheckUsername1(string name)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            public JsonResult CheckUsername2(int? id2, int? id3)
            {
                if (!id2.HasValue && !id3.HasValue)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
// ReSharper restore Mvc.AreaNotResolved
// ReSharper restore Mvc.ControllerNotResolved
// ReSharper restore Mvc.ActionNotResolved