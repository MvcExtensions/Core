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
        public void Should_add_conventions_when_condition_is_matched()
        {
            // arrange
            registry.RegisterConvention(new TestPropertyMetadataConvention());
            registry.RegisterModelProperties(modelType, new Dictionary<string, ModelMetadataItem>());

            // act
            var result = registry.GetModelPropertyMetadata(modelType, PropertyName);

            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Validations);
            Assert.NotNull(result.IsRequired);
            Assert.True(result.IsRequired.Value);
        }

        [Fact]
        public void Should_not_overwrite_existing_metadata_validation()
        {
            // arrange
            registry.RegisterConvention(new TestPropertyMetadataConvention());

            var items = new Dictionary<string, ModelMetadataItem>();
            var metadataItem = new ModelMetadataItem();
            const int expected = 200;
            new ModelMetadataItemBuilder<string>(metadataItem).MaximumLength(expected);
            items.Add(PropertyName, metadataItem);

            registry.RegisterModelProperties(modelType, items);

            // act
            var result = registry.GetModelPropertyMetadata(modelType, PropertyName);

            // assert
            var val = result.Validations.OfType<StringLengthValidationMetadata>().FirstOrDefault();
            Assert.NotNull(val);
            Assert.Equal(expected, val.Maximum);
        }

        [Fact]
        public void Should_add_conventions_when_no_metadata_convention_is_set_but_it_is_accepted_by_model_convensions()
        {
            // arrange
            registry.ConventionAcceptor = new TestModelConventionAcceptor();
            registry.RegisterConvention(new TestPropertyMetadataConvention());

            // act
            var result = registry.GetModelPropertyMetadata(typeof(TestModelWitoutMetadata), PropertyName);

            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Validations);
            Assert.NotNull(result.IsRequired);
            Assert.True(result.IsRequired.Value);
        }

        [Fact]
        public void Should_not_add_conventions_when_no_metadata_convention_is_set_and_it_is_not_accepted_by_model_convensions()
        {
            // arrange
            registry.ConventionAcceptor = new DefaultModelConventionAcceptor();
            registry.RegisterConvention(new TestPropertyMetadataConvention());

            // act
            var result = registry.GetModelPropertyMetadata(typeof(TestModelWitoutMetadata), PropertyName);

            // assert
            Assert.Null(result);
        }

        [Fact]
        public void Should_apply_convenstions_for_inheritance_even_if_it_was_called_for_inherited_class_firstly()
        {
            // arrange
            registry.RegisterConvention(new InheritanceModelMetadataConvention());
            registry.RegisterModelProperties(typeof(BaseModel), new Dictionary<string, ModelMetadataItem>());

            // act
            var result = registry.GetModelPropertyMetadata(typeof(InheritedModel), PropertyName);

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

        public class TestPropertyMetadataConvention : DefaultPropertyMetadataConvention<string>
        {
            protected override bool CanBeAcceptedCore(PropertyInfo propertyInfo)
            {
                return propertyInfo.Name == "Name";
            }

            protected override void CreateMetadataRulesCore(ModelMetadataItemBuilder<string> builder)
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
        }

        public class InheritanceModelMetadataConvention : DefaultPropertyMetadataConvention<string>
        {
            protected override bool CanBeAcceptedCore(PropertyInfo propertyInfo)
            {
                return propertyInfo.Name.StartsWith("Name");
            }

            protected override void CreateMetadataRulesCore(ModelMetadataItemBuilder<string> builder)
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
