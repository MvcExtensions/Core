#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class ConfigurationsScannerTests
    {
        [Fact]
        public void Should_find_configurations_for_types()
        {
            var scanner =
                new ConfigurationsScanner(
                    new[]
                        {
                            typeof(DummyModel1MetadataConfiguration),
                            typeof(DummyModel2MetadataConfiguration)
                        });
    
            var results = scanner.Select(r => r.MetadataConfigurationType).ToList();

            Assert.Contains(typeof(DummyModel1MetadataConfiguration), results);
            Assert.Contains(typeof(DummyModel2MetadataConfiguration), results);
        }

        [Fact]
        public void Should_be_able_to_go_over_types_via_foreach()
        {
            var scanner =
               new ConfigurationsScanner(
                   new[]
                        {
                            typeof(DummyModel1MetadataConfiguration),
                            typeof(DummyModel2MetadataConfiguration)
                        });
            var results = new List<Type>();
            scanner.ForEach(d => results.Add(d.MetadataConfigurationType));

            Assert.Contains(typeof(DummyModel1MetadataConfiguration), results);
            Assert.Contains(typeof(DummyModel2MetadataConfiguration), results);
        }

        [Fact]
        public void Should_not_find_non_public_configurations_for_types()
        {
            var scanner =
               new ConfigurationsScanner(
                   new[]
                        {
                            typeof(DummyModel1MetadataConfiguration),
                            typeof(DummyModel2MetadataConfiguration),
                            typeof(DummyModel3PrivateMetadataConfiguration)
                        });

            var results = scanner.Select(r => r.MetadataConfigurationType).ToList();

            Assert.DoesNotContain(typeof(DummyModel3PrivateMetadataConfiguration), results);
            Assert.Contains(typeof(DummyModel1MetadataConfiguration), results);
            Assert.Contains(typeof(DummyModel2MetadataConfiguration), results);
        }
        
        [Fact]
        public void Should_not_find_internal_configurations_for_types()
        {
            var scanner =
               new ConfigurationsScanner(
                   new[]
                        {
                            typeof(DummyModel1MetadataConfiguration),
                            typeof(DummyModel2MetadataConfiguration),
                            typeof(DummyModel4InternalMetadataConfiguration)
                        });

            var results = scanner.Select(r => r.MetadataConfigurationType).ToList();

            Assert.DoesNotContain(typeof(DummyModel4InternalMetadataConfiguration), results);
            Assert.Contains(typeof(DummyModel1MetadataConfiguration), results);
            Assert.Contains(typeof(DummyModel2MetadataConfiguration), results);
        }

        [Fact]
        public void Should_not_find_nested_public_configurations_for_internal_class()
        {
            var scanner =
               new ConfigurationsScanner(
                   new[]
                        {
                            typeof(InternalConfigurationsScannerTestsData.DummyModel5MetadataConfiguration)
                        });

            var results = scanner.Select(r => r.MetadataConfigurationType).ToList();

            Assert.DoesNotContain(typeof(DummyModel4InternalMetadataConfiguration), results);
        }

        public class DummyModel1
        {
        }

        public class DummyModel1MetadataConfiguration : ModelMetadataConfiguration<DummyModel1>
        {
        }

        public class DummyModel2
        {
        }

        public class DummyModel2MetadataConfiguration : ModelMetadataConfiguration<DummyModel2>
        {
        }

        public class DummyModel3Private
        {
        }

        private class DummyModel3PrivateMetadataConfiguration : ModelMetadataConfiguration<DummyModel3Private>
        {
        }

        public class DummyModel4Internal
        {
        }

        private class DummyModel4InternalMetadataConfiguration : ModelMetadataConfiguration<DummyModel4Internal>
        {
        }

        
    }
    internal class InternalConfigurationsScannerTestsData
    {
        public class DummyModel5
        {
        }

        public class DummyModel5MetadataConfiguration : ModelMetadataConfiguration<DummyModel5>
        {
        }
    }
   
}
