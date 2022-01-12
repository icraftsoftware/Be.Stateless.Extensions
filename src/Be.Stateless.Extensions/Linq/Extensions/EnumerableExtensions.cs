#region Copyright & License

// Copyright © 2012 - 2021 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Be.Stateless.Linq.Extensions
{
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Returns distinct elements from a sequence by using a specified <paramref name="comparer"/> to compare values.
		/// </summary>
		/// <typeparam name="TSource">
		/// The type of the elements of source.
		/// </typeparam>
		/// <param name="source">
		/// The sequence to remove duplicate elements from.
		/// </param>
		/// <param name="comparer">
		/// A lambda to compare values for equality.
		/// </param>
		/// <returns>
		/// An <see cref="IEnumerable{T}"/> that contains distinct elements from the source sequence.
		/// </returns>
		public static IEnumerable<TSource> Distinct<TSource>(
			this IEnumerable<TSource> source,
			Func<TSource, TSource, bool> comparer)
		{
			return source.Distinct(new LambdaComparer<TSource>(comparer));
		}

		/// <summary>
		/// Produces the set difference of two sequences by using the specified <paramref name="comparer"/> to compare values.
		/// </summary>
		/// <typeparam name="TSource">
		/// The type of the elements of the input sequences.
		/// </typeparam>
		/// <param name="first">
		/// An <see cref="IEnumerable{T}"/> whose elements that are not also in second will be returned.
		/// </param>
		/// <param name="second">
		/// An <see cref="IEnumerable{T}"/> whose elements that also occur in the first sequence will cause those elements to be
		/// removed from the returned sequence.
		/// </param>
		/// <param name="comparer">
		/// A lambda to compare values for equality.
		/// </param>
		/// <returns>
		/// A sequence that contains the set difference of the elements of two sequences.
		/// </returns>
		public static IEnumerable<TSource> Except<TSource>(
			this IEnumerable<TSource> first,
			IEnumerable<TSource> second,
			Func<TSource, TSource, bool> comparer)
		{
			return first.Except(second, new LambdaComparer<TSource>(comparer));
		}

		/// <summary>
		/// Calls an <see cref="Action{T}"/> delegate for each element of a sequence.
		/// </summary>
		/// <typeparam name="TSource">
		/// The type of the elements of <paramref name="source"/>.
		/// </typeparam>
		/// <param name="source">
		/// A sequence of values to invoke an action delegate on.
		/// </param>
		/// <param name="action">
		/// An <see cref="Action{T}"/> delegate to apply to each element.
		/// </param>
		public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			foreach (var element in source)
			{
				action?.Invoke(element);
			}
		}

		/// <summary>
		/// Calls an <see cref="Action{T}"/> delegate for each element of a sequence by incorporating the element's index.
		/// </summary>
		/// <typeparam name="TSource">
		/// The type of the elements of <paramref name="source"/>.
		/// </typeparam>
		/// <param name="source">
		/// A sequence of values to invoke an action delegate on.
		/// </param>
		/// <param name="action">
		/// An <see cref="Action{T}"/> delegate to apply to each element; the second parameter of the action represents the index
		/// of the source element.
		/// </param>
		public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<int, TSource> action)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			var index = 0;
			foreach (var element in source)
			{
				action?.Invoke(index++, element);
			}
		}

		/// <summary>
		/// Returns the only element of a sequence, and throws an exception if there is not exactly one element in the sequence.
		/// </summary>
		/// <typeparam name="TSource">
		/// </typeparam>
		/// <param name="source">
		/// An <see cref="IEnumerable{T}"/> to return the single element of.
		/// </param>
		/// <param name="noElementExceptionFactory">
		/// The exception to throw if the input sequence contains no element.
		/// </param>
		/// <param name="moreThanOneElementExceptionFactory">
		/// The exception to throw if the input sequence contains more than one element.
		/// </param>
		/// <returns>
		/// The single element of the input sequence.
		/// </returns>
		public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<Exception> noElementExceptionFactory, Func<Exception> moreThanOneElementExceptionFactory)
		{
			if (noElementExceptionFactory == null) throw new ArgumentNullException(nameof(noElementExceptionFactory));
			if (moreThanOneElementExceptionFactory == null) throw new ArgumentNullException(nameof(moreThanOneElementExceptionFactory));
			using (var enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext()) throw noElementExceptionFactory();
				var single = enumerator.Current;
				return enumerator.MoveNext() ? throw moreThanOneElementExceptionFactory() : single;
			}
		}

		/// <summary>
		/// Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one
		/// such element exists.
		/// </summary>
		/// <typeparam name="TSource">
		/// A function to test an element for a condition.
		/// </typeparam>
		/// <param name="source">
		/// An <see cref="IEnumerable{T}"/> to return the single element of.
		/// </param>
		/// <param name="predicate">
		/// A function to test an element for a condition.
		/// </param>
		/// <param name="noElementExceptionFactory">
		/// The exception to throw if the input sequence contains no element that satisfies the condition.
		/// </param>
		/// <param name="moreThanOneElementExceptionFactory">
		/// The exception to throw if the input sequence contains more than one element that satisfies the condition.
		/// </param>
		/// <returns>
		/// The single element of the input sequence that satisfies a condition.
		/// </returns>
		public static TSource Single<TSource>(
			this IEnumerable<TSource> source,
			Func<TSource, bool> predicate,
			Func<Exception> noElementExceptionFactory,
			Func<Exception> moreThanOneElementExceptionFactory)
		{
			return source.Where(predicate).Single(noElementExceptionFactory, moreThanOneElementExceptionFactory);
		}

		/// <summary>
		/// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception
		/// if there is more than one element in the sequence.
		/// </summary>
		/// <typeparam name="TSource">
		/// The type of the elements of source.
		/// </typeparam>
		/// <param name="source">
		/// An <see cref="IEnumerable{T}"/> to return the single element of.
		/// </param>
		/// <param name="moreThanOneElementExceptionFactory">
		/// The exception to throw if the input sequence contains more than one element.
		/// </param>
		/// <returns>
		/// The single element of the input sequence, or default(TSource) if the sequence contains no elements.
		/// </returns>
		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<Exception> moreThanOneElementExceptionFactory)
		{
			if (moreThanOneElementExceptionFactory == null) throw new ArgumentNullException(nameof(moreThanOneElementExceptionFactory));
			using (var enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext()) return default;
				var single = enumerator.Current;
				return enumerator.MoveNext() ? throw moreThanOneElementExceptionFactory() : single;
			}
		}

		/// <summary>
		/// Returns the only element of a sequence that satisfies a specified condition or a default value if no such element
		/// exists; this method throws an exception if more than one element satisfies the condition.
		/// </summary>
		/// <typeparam name="TSource">
		/// The type of the elements of source.
		/// </typeparam>
		/// <param name="source">
		/// An <see cref="IEnumerable{T}"/> to return the single element of.
		/// </param>
		/// <param name="predicate">
		/// A function to test an element for a condition.
		/// </param>
		/// <param name="moreThanOneElementExceptionFactory">
		/// The exception to throw if the input sequence contains more than one element that satisfies the condition.
		/// </param>
		/// <returns>
		/// The single element of the input sequence that satisfies the condition, or default(TSource) if no such element is
		/// found.
		/// </returns>
		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<Exception> moreThanOneElementExceptionFactory)
		{
			return source.Where(predicate).SingleOrDefault(moreThanOneElementExceptionFactory);
		}
	}
}
