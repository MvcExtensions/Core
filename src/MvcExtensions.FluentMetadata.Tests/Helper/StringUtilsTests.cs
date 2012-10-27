#region Copyright
// Copyright (c) 2009 - 2012, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>, 2011 - 2012 hazzik <hazzik@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.FluentMetadata.Tests.Helper
{
    using Xunit;

    public class StringUtilsTests
    {
        public class SplitUpperCaseToString
        {
            [Fact]
            public void Should_not_double_split_the_sentence()
            {
                // arrange
                const string Original = "This Is a Normal Sentence";

                // act
                var result = Original.SplitUpperCaseToString();

                // assert
                Assert.Equal("This Is a Normal Sentence", result);
            }

            [Fact]
            public void Should_not_split_the_abbreviations()
            {
                // arrange
                const string Original = "a EntityID";

                // act
                var result = Original.SplitUpperCaseToString();

                // assert
                Assert.Equal("a Entity ID", result);
            }

            [Fact]
            public void Should_return_empty_string_for_null_empty()
            {
                // arrange
                var original = string.Empty;

                // act
                var result = original.SplitUpperCaseToString();

                // assert
                Assert.Equal(string.Empty, result);
            }

            [Fact]
            public void Should_return_null_for_null_string()
            {
                // arrange
                string original = null;

                // act
                // ReSharper disable ExpressionIsAlwaysNull
                var result = original.SplitUpperCaseToString();
                // ReSharper restore ExpressionIsAlwaysNull

                // assert
                Assert.Equal(null, result);
            }

            [Fact]
            public void Should_split_camel_cased_word_with_spaces()
            {
                // arrange
                const string Original = "camelCasedWord";

                // act
                var result = Original.SplitUpperCaseToString();

                // assert
                Assert.Equal("camel Cased Word", result);
            }

            [Fact]
            public void Should_split_pascal_cased_word_with_spaces()
            {
                // arrange
                const string Original = "PascalCasedWord";

                // act
                var result = Original.SplitUpperCaseToString();

                // assert
                Assert.Equal("Pascal Cased Word", result);
            }
        }
    }
}
