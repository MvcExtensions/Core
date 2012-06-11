#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>, AlexBar <abarbashin@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.Tests
{
    using System.Web.Mvc;
    using Xunit;

    public class RemoteValidationMetadataTests : ValidationMetadataTestsBase
    {
        [Fact]
        public void Should_be_able_to_create_validator()
        {
            var metadata = new RemoteValidationMetadata { RouteName = "someroute"};
            var validator = metadata.CreateValidator(CreateMetadata(), new ControllerContext());

            Assert.NotNull(validator);
        }

        [Fact]
        public void Should_be_able_to_create_validator_with_contrller_and_action()
        {
            var metadata = new RemoteValidationMetadata { Action = "action", Controller = "controller"};
            var validator = metadata.CreateValidator(CreateMetadata(), new ControllerContext());

            Assert.NotNull(validator);
        }
    }
}