﻿using System.Collections.Generic;
using System.Linq;
using lib12.Collections;
using Shouldly;
using Xunit;

namespace lib12.Test.Collections
{
    public class IEnumerableExtensionTests
    {
        [Fact]
        public void formatting_collection_with_one_item()
        {
            const string expected = "test";
            var result = new[] { "test" }.ToDelimitedString("_");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void formatting_collection_with_two_item()
        {
            const string expected = "test_test2";
            var result = new[] { "test", "test2" }.ToDelimitedString("_");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void contains_one_element_returns_false_when_passing_null()
        {
            IEnumerable<int> enumerable = null;

            Assert.False(enumerable.ContainsOneElement());
        }

        [Fact]
        public void contains_one_element_returns_false_when_passing_empty_array()
        {
            var emptyArray = new int[0];

            Assert.False(emptyArray.ContainsOneElement());
        }

        [Fact]
        public void contains_one_element_returns_true_when_passing_enumerable_with_one_item()
        {
            var oneItemArray = new[] { 1 };

            Assert.True(oneItemArray.ContainsOneElement());
        }

        [Fact]
        public void contains_one_element_returns_false_when_passing_enumerable_with_two_items()
        {
            var twoItemsArray = new[] {1, 2};

            Assert.False(twoItemsArray.ContainsOneElement());
        }

        [Fact]
        public void recover_test_on_null_collection()
        {
            List<int> list = null;
            list.Recover().Count().ShouldBe(0);
        }

        [Fact]
        public void recover_on_not_null_collection()
        {
            var list = new List<int> {3, 4, 12};
            list.Recover().Count().ShouldBe(3);
        }
    }
}
