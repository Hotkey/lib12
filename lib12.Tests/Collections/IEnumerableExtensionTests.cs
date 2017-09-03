﻿using System;
using System.Collections.Generic;
using System.Linq;
using lib12.Collections;
using Shouldly;
using Xunit;

namespace lib12.Tests.Collections
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
            var twoItemsArray = new[] { 1, 2 };

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
            var list = new List<int> { 3, 4, 12 };
            list.Recover().Count().ShouldBe(3);
        }

        [Fact]
        public void maxby_happy_path()
        {
            var list = new List<Item> {
                new Item{ Value = 3 },
                new Item { Value = 4 },
                new Item { Value = 12 } };

            list.MaxBy(x => x.Value).ShouldBe(list[2]);
        }

        [Fact]
        public void maxby_throws_argument_null_exception_if_enumerable_is_null()
        {
            List<Item> list = null;

            Assert.Throws<ArgumentNullException>(() => list.MaxBy(x => x.Value));
        }

        [Fact]
        public void maxby_throws_argument_null_exception_if_selector_is_null()
        {
            var list = new List<Item> {
                new Item{ Value = 3 },
                new Item { Value = 4 },
                new Item { Value = 12 } };

            Assert.Throws<ArgumentNullException>(() => list.MaxBy((Func<Item, int>)null));
        }

        [Fact]
        public void maxby_throws_invalid_operation_exception_if_collection_is_empty()
        {
            var list = new List<Item>();

            Assert.Throws<InvalidOperationException>(() => list.MaxBy(x => x.Value));
        }

        [Fact]
        public void minby_happy_path()
        {
            var list = new List<Item> {
                new Item{ Value = 3 },
                new Item { Value = 4 },
                new Item { Value = 12 } };

            list.MinBy(x => x.Value).ShouldBe(list[0]);
        }

        [Fact]
        public void minby_throws_argument_null_exception_if_enumerable_is_null()
        {
            List<Item> list = null;

            Assert.Throws<ArgumentNullException>(() => list.MinBy(x => x.Value));
        }

        [Fact]
        public void minby_throws_argument_null_exception_if_selector_is_null()
        {
            var list = new List<Item> {
                new Item{ Value = 3 },
                new Item { Value = 4 },
                new Item { Value = 12 } };

            Assert.Throws<ArgumentNullException>(() => list.MinBy((Func<Item, int>)null));
        }

        [Fact]
        public void minby_throws_invalid_operation_exception_if_collection_is_empty()
        {
            var list = new List<Item>();

            Assert.Throws<InvalidOperationException>(() => list.MinBy(x => x.Value));
        }

        [Fact]
        public void distinctby_happy_path()
        {
            var list = new List<Item> {
                new Item{ Value = 3 },
                new Item { Value = 3 },
                new Item { Value = 12 } };

            var result = list.DistinctBy(x => x.Value);
            result.Count().ShouldBe(2);
            result.ElementAt(0).Value.ShouldBe(3);
            result.ElementAt(1).Value.ShouldBe(12);
        }

        [Fact]
        public void distinctby_throws_argument_null_exception_if_selector_is_null()
        {
            var list = new List<Item> {
                new Item{ Value = 3 },
                new Item { Value = 3 },
                new Item { Value = 12 } };

            Assert.Throws<ArgumentNullException>(() => list.DistinctBy((Func<Item, int>)null).ToArray());
        }

        [Fact]
        public void distinctby_returns_empty_collection_if_enumerable_is_null()
        {
            List<Item> list = null;
            list.DistinctBy(x => x.Value).ShouldBeEmpty();
        }

        [Fact]
        public void distinctby_returns_empty_collectionn_if_enumerable_is_empty()
        {
            var list = new List<Item>();
            list.DistinctBy(x => x.Value).ShouldBeEmpty();
        }

        [Fact]
        public void findindex_happy_path()
        {
            var list = new List<Item> {
                new Item { Value = 3 },
                new Item { Value = 4 },
                new Item { Value = 12 } };

            list.Select(x => x)
                .FindIndex(x => x.Value == 4)
                .ShouldBe(1);
        }

        [Fact]
        public void findindex_throws_argument_null_exception_if_selector_is_null()
        {
            var list = new List<Item> {
                new Item{ Value = 3 },
                new Item { Value = 4 },
                new Item { Value = 12 } };

            Assert.Throws<ArgumentNullException>(() => list.Select(x => x).FindIndex(null));
        }

        [Fact]
        public void findindex_finds_first_element()
        {
            var list = new List<Item> {
                new Item { Value = 3 },
                new Item { Value = 4 },
                new Item { Value = 12 } };

            list.Select(x => x)
                .FindIndex(x => x.Value == 3)
                .ShouldBe(0);
        }

        [Fact]
        public void findindex_finds_last_element()
        {
            var list = new List<Item> {
                new Item { Value = 3 },
                new Item { Value = 4 },
                new Item { Value = 12 } };

            list.Select(x => x)
                .FindIndex(x => x.Value == 12)
                .ShouldBe(2);
        }

        [Fact]
        public void findindex_returns_negative_one_if_cant_find_element()
        {
            var list = new List<Item> {
                new Item { Value = 3 },
                new Item { Value = 4 },
                new Item { Value = 12 } };

            list.Select(x => x)
                .FindIndex(x => x.Value == 99)
                .ShouldBe(-1);
        }

        [Fact]
        public void findindex_returns_negative_one_if_collection_is_null()
        {
            IEnumerable<Item> list = null;

            list
                .FindIndex(x => x.Value == 12)
                .ShouldBe(-1);
        }

        [Fact]
        public void findindex_returns_negative_one_if_collection_is_empty()
        {
            List<Item> list = new List<Item>();

            list.Select(x => x)
                .FindIndex(x => x.Value == 12)
                .ShouldBe(-1);
        }

        [Fact]
        public void partition_happy_path()
        {
            var list = new List<Item> {
                new Item{ Value = 3 },
                new Item { Value = 4 },
                new Item { Value = 12 } };

            var result = list.Partition(x => x.Value < 6);
            result.True.Count.ShouldBe(2);
            result.False.Count.ShouldBe(1);
            result.True[0].ShouldBe(list[0]);
            result.True[1].ShouldBe(list[1]);
            result.False[0].ShouldBe(list[2]);
        }

        [Fact]
        public void partition_throws_argument_null_exception_if_selector_is_null()
        {
            var list = new List<Item> {
                new Item{ Value = 3 },
                new Item { Value = 3 },
                new Item { Value = 12 } };

            Assert.Throws<ArgumentNullException>(() => list.Partition(null));
        }

        [Fact]
        public void partition_returns_empty_collection_if_enumerable_is_null()
        {
            List<Item> list = null;
            var result = list.Partition(x => x.Value < 6);
            
            result.True.ShouldBeEmpty();
            result.False.ShouldBeEmpty();
        }

        [Fact]
        public void partition_returns_empty_collectionn_if_enumerable_is_empty()
        {
            var list = new List<Item>();
            var result = list.Partition(x => x.Value < 6);

            result.True.ShouldBeEmpty();
            result.False.ShouldBeEmpty();
        }
    }
}
