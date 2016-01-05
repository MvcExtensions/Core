#region Copyright
// Copyright (c) 2009 - 2011, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class ConventionsTests
    {
        private const string PropertyName = "Name";
        private readonly Type modelType;
        private readonly ModelMetadataRegistry registry;

        public ConventionsTests()
        {
            registry = new ModelMetadataRegistry();
            modelType = typeof(DummyModelWithConventions);
        }

        [Fact]
        public void Should_apply_conventions_when_condition_is_met()
        {
            // arrange
            registry.RegisterConvention(new TestPropertyModelMetadataConvention());
            registry.RegisterModelProperties(modelType, new Dictionary<string, Func<ModelMetadataItem>>());

            // act
            var result = registry.GetModelPropertyMetadata(modelType, PropertyName);

            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Validations);
            Assert.NotNull(result.IsRequired);
            Assert.True(result.IsRequired.Value);
        }

        [Fact]
        public void Should_apply_overwrite_existing_metadata_validation()
        {
            // arrange
            registry.RegisterConvention(new TestPropertyModelMetadataConvention());

            var items = new Dictionary<string, Func<ModelMetadataItem>>();
            const int Expected = 200;
            var builder = new ModelMetadataItemBuilder<string>(new ModelMetadataItem());
            builder.MaximumLength(Expected);
            items.Add(PropertyName, () => builder.Item);

            registry.RegisterModelProperties(modelType, items);

            // act
            var result = registry.GetModelPropertyMetadata(modelType, PropertyName);

            // assert
            var val = result.Validations.OfType<StringLengthValidationMetadata>().FirstOrDefault();
            Assert.NotNull(val);
            Assert.Equal(Expected, val.Maximum);
        }

        [Fact]
        public void Should_apply_convenstions_for_inheritance_even_if_it_was_called_for_inherited_class_firstly()
        {
            // arrange
            registry.RegisterConvention(new InheritanceModelModelMetadataConvention());
            registry.RegisterModelProperties(typeof(BaseModel), new Dictionary<string, Func<ModelMetadataItem>>());

            // act
            var result = registry.GetModelPropertyMetadata(typeof(InheritedModel), PropertyName);

            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Validations);
            Assert.NotNull(result.IsRequired);
            Assert.True(result.IsRequired.Value);
        }

        [Fact]
        public void Should_not_apply_convenstions_for_inacceptable_property()
        {
            // arrange
            registry.RegisterConvention(new InheritanceModelModelMetadataConvention());
            registry.RegisterModelProperties(typeof(BaseModel), new Dictionary<string, Func<ModelMetadataItem>>());

            // act
            var result = registry.GetModelPropertyMetadata(typeof(InheritedModel), "ShouldNotApply");

            // assert
            Assert.Null(result);
        }

        [Fact]
        public void Should_not_apply_convention_for_types_that_do_not_have_metadata_configuration()
        {
            // arrange
            registry.ConventionAcceptor = new DefaultModelConventionAcceptor();
            registry.RegisterConvention(new TestPropertyModelMetadataConvention());

            // act
            var result = registry.GetModelPropertyMetadata(modelType, PropertyName);

            // assert
            Assert.Null(result);
        }
        
        [Fact]
        public void Should_apply_conventions_when_no_metadata_convention_is_set_but_it_is_accepted_by_model_convensions()
        {
            // arrange
            registry.ConventionAcceptor = new TestModelConventionAcceptor();
            registry.RegisterConvention(new TestPropertyModelMetadataConvention());

            // act
            var result = registry.GetModelPropertyMetadata(typeof(TestModelWitoutMetadata), PropertyName);

            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Validations);
            Assert.NotNull(result.IsRequired);
            Assert.True(result.IsRequired.Value);
        }
        
        #region Test data
        
        #region // regular test for model with metadata

        public class DummyModelWithConventions
        {
            public string Name { get; set; }
        }

        public class TestPropertyModelMetadataConvention : DefaultPropertyModelMetadataConvention<string>
        {
            public override bool IsApplicable(PropertyInfo propertyInfo)
            {
                return propertyInfo.Name == "Name";
            }

            protected override void Apply(PropertyInfo property, ModelMetadataItemBuilder<string> builder)
            {
                builder
                    .Required()
                    .MaximumLength(50);
            }
        }

        #endregion

        #region // for test without conventions

        public class TestModelWitoutMetadata : IForm
        {
            public string Name { get; set; }
        }

        public interface IForm
        {
        }

        public class TestModelConventionAcceptor : DefaultModelConventionAcceptor
        {
            public override bool CanAcceptConventions(AcceptorContext context)
            {
                return base.CanAcceptConventions(context) || typeof(IForm).IsAssignableFrom(context.ModelType);
            }
        }
        #endregion

        #region // type conventions for inheritance

        public class BaseModel
        {
            public string Name { get; set; }
        }

        public class InheritedModel : BaseModel
        {
            public string Name2 { get; set; }
            public string ShouldNotApply { get; set; }
        }

        public class InheritanceModelModelMetadataConvention : DefaultPropertyModelMetadataConvention<string>
        {
            public override bool IsApplicable(PropertyInfo propertyInfo)
            {
                return base.IsApplicable(propertyInfo) && propertyInfo.Name.StartsWith("Name");
            }

            protected override void Apply(PropertyInfo property, ModelMetadataItemBuilder<string> builder)
            {
                builder
                    .Required()
                    .MaximumLength(50);
            }
        }
        #endregion



        #endregion
    }
}
