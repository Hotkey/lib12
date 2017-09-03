﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lib12.FunctionalFlow;

namespace lib12.Collections
{
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Determines whether the specified enumerable is empty.
        /// </summary>
        /// <param name="enumerable">The enumerable to check</param>
        /// <returns>True if enumerable is empty</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }

        /// <summary>
        /// Determines whether enumerable is null or empty
        /// </summary>
        /// <param name="enumerable">The enumerable to check</param>
        /// <returns>True if enumerable is null or empty</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        /// <summary>
        /// Determines whether enumerable is not empty
        /// </summary>
        /// <param name="enumerable">The enumerable to check</param>
        /// <returns>True if enumerable is not empty</returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Any();
        }

        /// <summary>
        /// Determines whether enumerable is not null and not empty
        /// </summary>
        /// <param name="enumerable">The enumerable to check</param>
        /// <returns>True if enumerable is not null and not empty</returns>
        public static bool IsNotNullAndNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Any();
        }

        /// <summary>
        /// Invoke action for each element in enumerable
        /// </summary>
        /// <param name="enumeration">The enumeration of items to invoke action on</param>
        /// <param name="action">The action to invoke</param>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
                action(item);

            return enumeration;
        }

        /// <summary>
        /// Converterts enumerable to delimited string
        /// </summary>
        /// <param name="enumerable">The enumerable to convert</param>
        /// <param name="delimiter">The delimiter to use</param>
        /// <returns>Delimited string</returns>
        public static string ToDelimitedString<T>(this IEnumerable<T> enumerable, string delimiter)
        {
            if (enumerable.IsEmpty())
                return string.Empty;

            var sbuilder = new StringBuilder();
            foreach (var item in enumerable)
            {
                sbuilder.AppendFormat("{0}{1}", item, delimiter);
            }

            sbuilder.Remove(sbuilder.Length - delimiter.Length, delimiter.Length);
            return sbuilder.ToString();
        }

        /// <summary>
        /// Determines if two collections have the same content, but not necessary in the same order
        /// http://stackoverflow.com/questions/50098/comparing-two-collections-for-equality
        /// </summary>
        /// <param name="first">The first collection.</param>
        /// <param name="second">The second collection.</param>
        /// <returns>True if both collections have the same content, false otherwise.</returns>
        public static bool SequenceContentEqual<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            // Declare a dictionary to count the occurence of the items in the collection
            var itemCounts = new Dictionary<T, int>();
            
            // Increase the count for each occurence of the item in the first collection
            foreach (T item in first)
            {
                if (itemCounts.ContainsKey(item))
                {
                    itemCounts[item]++;
                }
                else
                {
                    itemCounts[item] = 1;
                }
            }

            // Wrap the keys in a searchable list
            var keys = new List<T>(itemCounts.Keys);

            // Decrease the count for each occurence of the item in the second collection
            foreach (var item in second)
            {
                // Try to find a key for the item
                // The keys of a dictionary are compared by reference, so we have to
                // find the original key that is equivalent to the "item"
                // You may want to override ".Equals" to define what it means for
                // two "T" objects to be equal
                var key = keys.Find(
                    delegate (T listKey)
                    {
                        return listKey.Equals(item);
                    });

                // Check if a key was found
                if (key != null)
                {
                    itemCounts[key]--;
                }
                else
                {
                    // There was no occurence of this item in the first collection, thus the collections are not equal
                    return false;
                }
            }

            // The count of each item should be 0 if the contents of the collections are equal
            foreach (int value in itemCounts.Values)
            {
                if (value != 0)
                {
                    return false;
                }
            }

            // The collections are equal
            return true;
        }

        /// <summary>
        /// Casts object to generic IEnumerable
        /// </summary>
        /// <param name="toCast">Object to cast</param>
        /// <returns>Generic IEnumerable or null if it's possible</returns>
        public static IEnumerable<T> CastToEnumerable<T>(object toCast)
        {
            if (toCast is IEnumerable enumerable)
                return enumerable.Cast<T>();
            else
                return null;
        }

        /// <summary>
        /// Determines whether collection contains exactly one element
        /// </summary>
        /// <param name="enumerable">Collection to check</param>
        public static bool ContainsOneElement<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.IsNotNullAndNotEmpty() && enumerable.Skip(1).IsEmpty();
        }

        /// <summary>
        /// Determines whether collection contains exactly more than one element
        /// </summary>
        /// <param name="enumerable">Collection to check</param>
        public static bool ContainsMultipleElements<T>(this IEnumerable<T> enumerable)
        {
            return enumerable != null && enumerable.Skip(1).IsNotEmpty();
        }

        /// <summary>
        /// Gets the next element after given or default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable to search</param>
        /// <param name="currentElement">The current element</param>
        /// <returns></returns>
        public static T GetNextElementOrDefault<T>(this IEnumerable<T> enumerable, T currentElement)
        {
            var returnNextElement = false;
            foreach (var item in enumerable)
            {
                if (item.Equals(currentElement))
                {
                    returnNextElement = true;
                    continue;
                }
                else if (returnNextElement)
                {
                    return item;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Gets the previous element before given or default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable to search</param>
        /// <param name="currentElement">The current element</param>
        /// <returns></returns>
        public static T GetPreviousElementOrDefault<T>(this IEnumerable<T> enumerable, T currentElement)
        {
            var prevoiusElement = default(T);
            foreach (var item in enumerable)
            {
                if (item.Equals(currentElement))
                    return prevoiusElement;
            }

            return default(T);
        }

        /// <summary>
        /// Takes last X elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable to take elements from</param>
        /// <param name="count">The count to take</param>
        /// <returns></returns>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> enumerable, int count)
        {
            return enumerable.Skip(Math.Max(0, enumerable.Count() - count));
        }

        /// <summary>
        /// If enumerable is null convert it into empty collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static IEnumerable<T> Recover<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Get item with maximum value of given property
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="selector">The selector to get value by which maximum is searched</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// enumerable
        /// or
        /// selector
        /// </exception>
        /// <exception cref="InvalidOperationException">Sequence contains no elements</exception>
        public static TItem MaxBy<TItem, TKey>(this IEnumerable<TItem> enumerable, Func<TItem, TKey> selector) where TKey : IComparable
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            using (var enumerator = enumerable.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException("Sequence contains no elements");

                var maxKey = enumerator.Current;
                var maxValue = selector(maxKey);
                while (enumerator.MoveNext())
                {
                    var currentValue = selector(enumerator.Current);
                    if (maxValue.CompareTo(currentValue) < 0)
                    {
                        maxValue = currentValue;
                        maxKey = enumerator.Current;
                    }
                }
                return maxKey;
            }
        }

        /// <summary>
        /// Get item with minimum value of given property
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="selector">The selector to get value by which minimum is searched</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// enumerable
        /// or
        /// selector
        /// </exception>
        /// <exception cref="InvalidOperationException">Sequence contains no elements</exception>
        public static TItem MinBy<TItem, TKey>(this IEnumerable<TItem> enumerable, Func<TItem, TKey> selector) where TKey : IComparable
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            using (var enumerator = enumerable.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    throw new InvalidOperationException("Sequence contains no elements");

                var minKey = enumerator.Current;
                var minValue = selector(minKey);
                while (enumerator.MoveNext())
                {
                    var currentValue = selector(enumerator.Current);
                    if (minValue.CompareTo(currentValue) > 0)
                    {
                        minValue = currentValue;
                        minKey = enumerator.Current;
                    }
                }
                return minKey;
            }
        }

        /// <summary>
        /// Computes the collection of distinct elements by specific property
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// enumerable
        /// or
        /// selector
        /// </exception>
        public static IEnumerable<TItem> DistinctBy<TItem, TKey>(this IEnumerable<TItem> enumerable, Func<TItem, TKey> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var set = new HashSet<TKey>();
            foreach (var item in enumerable.Recover())
            {
                var value = selector(item);
                if (set.Add(value))
                    yield return item;
            }
        }

        /// <summary>
        /// Finds the index using predicate
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// enumerable
        /// or
        /// predicate
        /// </exception>
        public static int FindIndex<TItem>(this IEnumerable<TItem> enumerable, Predicate<TItem> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var index = 0;
            using (var enumerator = enumerable.Recover().GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return -1;

                do
                {
                    if (predicate(enumerator.Current))
                        return index;
                    else
                        index++;
                } while (enumerator.MoveNext());

                return -1;
            }
        }

        /// <summary>
        /// Splits collection into two collections based on whether or not they pass the condition provided
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">predicate</exception>
        public static (List<TItem> True, List<TItem> False) Partition<TItem>(this IEnumerable<TItem> enumerable, Predicate<TItem> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var trueList = new List<TItem>();
            var falseList = new List<TItem>();
            using (var enumerator = enumerable.Recover().GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return (trueList, falseList);

                do
                {
                    if (predicate(enumerator.Current))
                        trueList.Add(enumerator.Current);
                    else
                        falseList.Add(enumerator.Current);
                } while (enumerator.MoveNext());

                return (trueList, falseList);
            }
        }

        /// <summary>
        /// Returns items from enumerable batched in arrays with given size
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="size">The size of batch</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">size - The batch must have at least 1 length</exception>
        public static IEnumerable<TItem[]> Batch<TItem>(this IEnumerable<TItem> enumerable, int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "The batch must have at least 1 length");

            var count = 0;
            TItem[] batch = null;
            foreach (var item in enumerable.Recover())
            {
                if (batch == null)
                    batch = new TItem[size];

                batch[count++] = item;
                if (count == size)
                {
                    yield return batch;

                    batch = null;
                    count = 0;
                }
            }

            if (batch.NotNull() && count > 0)
            {
                Array.Resize(ref batch, count);
                yield return batch;
            }
        }
    }
}
