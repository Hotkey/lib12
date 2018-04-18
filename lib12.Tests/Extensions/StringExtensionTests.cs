﻿using System;
using lib12.Extensions;
using Shouldly;
using Xunit;

namespace lib12.Tests.Extensions
{
    public class StringExtensionTests
    {
        [Fact]
        public void recover_null_returns_empty_string()
        {
            ((string)null).Recover().ShouldBe(string.Empty);
        }

        [Fact]
        public void recover_non_empty_string_returns_same_string()
        {
            const string test = "Test string";
            test.Recover().ShouldBe(test);
        }

        [Fact]
        public void equalsignorecase_two_equal_same_case_returns_true()
        {
            const string first = "Test string";
            const string second = "Test string";

            first.EqualsIgnoreCase(second).ShouldBeTrue();
        }

        [Fact]
        public void equalsignorecase_two_equal_different_case_returns_true()
        {
            const string first = "Test string";
            const string second = "tEst sTrinG";

            first.EqualsIgnoreCase(second).ShouldBeTrue();
        }

        [Fact]
        public void equalsignorecase_two_different_returns_false()
        {
            const string first = "Test string";
            const string second = "Different test string";

            first.EqualsIgnoreCase(second).ShouldBeFalse();
        }

        [Fact]
        public void equalsmatchcase_two_equal_same_case_returns_true()
        {
            const string first = "Test string";
            const string second = "Test string";

            first.EqualsMatchCase(second).ShouldBeTrue();
        }

        [Fact]
        public void equalsmatchcase_two_equal_different_case_returns_flase()
        {
            const string first = "Test string";
            const string second = "tEst sTrinG";

            first.EqualsMatchCase(second).ShouldBeFalse();
        }

        [Fact]
        public void equalsmatchcase_two_different_returns_false()
        {
            const string first = "Test string";
            const string second = "Different test string";

            first.EqualsMatchCase(second).ShouldBeFalse();
        }

        [Fact]
        public void truncate_throws_exception_if_max_length_is_equal_or_less_than_zero()
        {
            const string text = "text5";
            Assert.Throws<ArgumentOutOfRangeException>(() => text.Truncate(0).ShouldBe(text));
        }

        [Fact]
        public void truncate_on_null_returns_empty_string()
        {
            string text = null;
            text.Truncate(12).ShouldBeEmpty();
        }

        [Fact]
        public void truncate_on_empty_string_returns_empty_string()
        {
            string.Empty.Truncate(12).ShouldBeEmpty();
        }

        [Fact]
        public void truncate_on_string_with_length_lower_than_max_returns_same_string()
        {
            const string text = "text5";
            text.Truncate(12).ShouldBe(text);
        }

        [Fact]
        public void truncate_on_string_with_length_bigger_than_max_returns_truncated_string()
        {
            const string text = "text5";
            text.Truncate(2).ShouldBe("te");
        }

        [Theory]
        [InlineData(null, null, false)]
        [InlineData("", null, false)]
        [InlineData(null, "", false)]
        [InlineData("", "", true)]
        [InlineData("text", "", true)]
        [InlineData("", "text", false)]
        [InlineData("sample text", "text", true)]
        [InlineData("sample Text", "text", true)]
        public void contains_ignore_case_theory(string source, string toCheck, bool expectedResult)
        {
            const string text = "text5";
            text.Truncate(2).ShouldBe("te");
        }

        [Fact]
        public void remove_diacritics_on_null_string_returns_empty_string()
        {
            string text = null;
            text.RemoveDiacritics().ShouldBeEmpty();
        }

        [Fact]
        public void remove_diacritics_on_empty_string_returns_empty_string()
        {
            string.Empty.RemoveDiacritics().ShouldBeEmpty();
        }

        [Fact]
        public void remove_diacritics_happy_path()
        {
            const string withDiacritics = "crème brûlée";
            const string withoutDiacritics = "creme brulee";

            withDiacritics.RemoveDiacritics().ShouldBe(withoutDiacritics);
        }

        [Theory]
        [InlineData(null, null, 0)]
        [InlineData("", null, 0)]
        [InlineData(null, "", 0)]
        [InlineData("", "", 0)]
        [InlineData("", "", 0)]
        [InlineData("test", "t", 2)]
        [InlineData("test", "ch", 0)]
        [InlineData("It will be long and harsh winter! Better be prepared!", "be", 2)]
        public void get_number_of_occurences_theory(string source, string text, int expectedResult)
        {
            
        }
    }
}