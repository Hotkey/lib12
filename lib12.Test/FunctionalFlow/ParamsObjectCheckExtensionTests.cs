﻿using FluentAssertions;
using lib12.Extensions;
using lib12.FunctionalFlow;
using Should;
using Xunit;

namespace lib12.Test.FunctionalFlow
{
    enum TestEnum
    {
        First,
        Second,
        Third,
        Fourth,
        Fifth
    }

    public class ParamsObjectCheckExtensionTests
    {
        [Fact]
        public void is_one_element_true()
        {
            var value = TestEnum.First;

            value.Is(TestEnum.First).Should().BeTrue();
        }

        [Fact]
        public void is_one_element_false()
        {
            var value = TestEnum.First;

            value.Is(TestEnum.Second).Should().BeFalse();
        }

        [Fact]
        public void is_in_collection_true()
        {
            var value = TestEnum.First;

            value.Is(TestEnum.First, TestEnum.Second, TestEnum.Fifth).Should().BeTrue();
        }

        [Fact]
        public void is_in_collection_false()
        {
            var value = TestEnum.First;

            value.Is(TestEnum.Fourth, TestEnum.Second, TestEnum.Fifth).Should().BeFalse();
        }

        [Fact]
        public void is_not_one_element_true()
        {
            var value = TestEnum.First;

            value.IsNot(TestEnum.Second).Should().BeTrue();
        }

        [Fact]
        public void is_not_one_element_false()
        {
            var value = TestEnum.First;

            value.IsNot(TestEnum.First).Should().BeFalse();
        }

        [Fact]
        public void is_not_in_collection_true()
        {
            var value = TestEnum.First;

            value.IsNot(TestEnum.Third, TestEnum.Second, TestEnum.Fifth).Should().BeTrue();
        }

        [Fact]
        public void is_not_in_collection_false()
        {
            var value = TestEnum.First;

            value.IsNot(TestEnum.First, TestEnum.Second, TestEnum.Fifth).Should().BeFalse();
        }

        [Fact]
        public void is_int_test()
        {
            4.IsNot(5, 6, 6).ShouldBeTrue();
        }
    }
}