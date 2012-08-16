﻿#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System.Web.Mvc;
    using Xunit;

    public class StringLengthValidationMetadataTests : ValidationMetadataTestsBase
    {
        [Fact]
        public void Should_be_able_to_create_validator()
        {
            var metadata = new StringLengthValidationMetadata { Maximum = 10 };
            var validator = metadata.CreateValidator(CreateMetadata(), new ControllerContext());

            Assert.NotNull(validator);
        }
    }
}