#region Copyright & License

// Copyright © 2012 - 2020 François Chabot
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
using System.Globalization;
using System.IO;
using static System.Char;

namespace Be.Stateless.Extensions
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name")]
	public static class StringExtensions
	{
		/// <summary>
		/// Performs an <see cref="Action{T}"/> delegate on the <paramref name="string"/> if it is not null nor empty.
		/// </summary>
		/// <param name="string">
		/// The string to test and to pass as argument to the <paramref name="action"/> delegate.
		/// </param>
		/// <param name="action">
		/// The <see cref="Action{T}"/> delegate to perform.
		/// </param>
		public static void IfNotNullOrEmpty(this string @string, Action<string> action)
		{
			if (action == null) throw new ArgumentNullException(nameof(action));
			if (!@string.IsNullOrEmpty()) action(@string);
		}

		/// <summary>
		/// Performs an <see cref="Func{TResult}"/> delegate on the <paramref name="string"/> if it is not null nor empty
		/// and returns the <paramref name="function"/> delegate value.
		/// </summary>
		/// <typeparam name="TR">
		/// The return type of the <see cref="Func{TResult}"/> delegate.
		/// </typeparam>
		/// <param name="string">
		/// The string to test and to pass as argument to the <paramref name="function"/> delegate.
		/// </param>
		/// <param name="function">
		/// The <see cref="Func{TResult}"/> delegate to perform.
		/// </param>
		/// <returns>
		/// The result of the <paramref name="function"/> delegate, or <c>default(TR)</c> if the <paramref name="string"/>
		/// is null or empty.
		/// </returns>
		public static TR IfNotNullOrEmpty<TR>(this string @string, Func<string, TR> function)
		{
			if (function == null) throw new ArgumentNullException(nameof(function));
			return @string.IsNullOrEmpty() ? default : function(@string);
		}

		/// <summary>
		/// Performs an <see cref="Action{T}"/> delegate on the <paramref name="string"/> if it is not null or white space.
		/// </summary>
		/// <param name="string">
		/// The string to test and to pass as argument to the <paramref name="action"/> delegate.
		/// </param>
		/// <param name="action">
		/// The <see cref="Action{T}"/> delegate to perform.
		/// </param>
		public static void IfNotNullOrWhiteSpace(this string @string, Action<string> action)
		{
			if (action == null) throw new ArgumentNullException(nameof(action));
			if (!@string.IsNullOrWhiteSpace()) action(@string);
		}

		/// <summary>
		/// Performs an <see cref="Func{TResult}"/> delegate on the <paramref name="string"/> if it is not null or white space
		/// and returns the <paramref name="function"/> delegate value.
		/// </summary>
		/// <typeparam name="TR">
		/// The return type of the <see cref="Func{TResult}"/> delegate.
		/// </typeparam>
		/// <param name="string">
		/// The string to test and to pass as argument to the <paramref name="function"/> delegate.
		/// </param>
		/// <param name="function">
		/// The <see cref="Func{TResult}"/> delegate to perform.
		/// </param>
		/// <returns>
		/// The result of the <paramref name="function"/> delegate, or <c>default(TR)</c> if the <paramref name="string"/> is
		/// null or white space.
		/// </returns>
		public static TR IfNotNullOrWhiteSpace<TR>(this string @string, Func<string, TR> function)
		{
			if (function == null) throw new ArgumentNullException(nameof(function));
			return @string.IsNullOrWhiteSpace() ? default : function(@string);
		}

		/// <summary>
		/// Indicates whether the specified string is null or an <see cref="string.Empty"/> string.
		/// </summary>
		/// <param name="string">
		/// The string to test.
		/// </param>
		/// <returns>
		/// <c>true</c> if the <paramref name="string"/> argument is null or an <see cref="string.Empty"/>; otherwise,
		/// <c>false</c>.
		/// </returns>
		public static bool IsNullOrEmpty(this string @string)
		{
			return string.IsNullOrEmpty(@string);
		}

		/// <summary>
		/// Indicates whether a specified string is null, empty, or consists only of white-space characters.
		/// </summary>
		/// <param name="string">
		/// The string to test.
		/// </param>
		/// <returns>
		/// <c>true</c> if the <paramref name="string"/> argument is null or an <see cref="string.Empty"/>, or if it
		/// consists exclusively of white-space characters; otherwise <c>false</c>.
		/// </returns>
		public static bool IsNullOrWhiteSpace(this string @string)
		{
			return string.IsNullOrWhiteSpace(@string);
		}

		/// <summary>
		/// Verifies that the <paramref name="name"/> string is a valid file name.
		/// </summary>
		/// <param name="name">
		/// The file name to verify.
		/// </param>
		/// <returns>
		/// <c>true</c> if it is a valid file name; <c>false</c> otherwise.
		/// </returns>
		public static bool IsValidFileName(this string name)
		{
			return name.IfNotNullOrEmpty(n => n.IndexOfAny(Path.GetInvalidFileNameChars()) < 0);
		}

		/// <summary>
		/// Converts the string representation of the name or numeric value of one or more enumerated constants to an
		/// equivalent enumerated object.
		/// </summary>
		/// <typeparam name="T">
		/// An enumeration type.
		/// </typeparam>
		/// <param name="value">
		/// A string containing the name or value to convert.
		/// </param>
		/// <param name="ignoreCase">
		/// Whether to ignore case or not; <c>true</c> by default.
		/// </param>
		/// <returns>
		/// One or more enumerated constants of type <typeparamref name="T"/> that represents <paramref name="value"/>.
		/// </returns>
		public static T Parse<T>(this string value, bool ignoreCase = true) where T : struct
		{
			return (T) Enum.Parse(typeof(T), value, ignoreCase);
		}

		/// <summary>
		/// Extract the first or last <c>length</c> characters of a string, whether <c>length</c> is respectively positive
		/// or negative.
		/// </summary>
		/// <param name="string">The string to extract characters of.</param>
		/// <param name="length">The number of characters to extract.</param>
		/// <returns>The substring of the input string.</returns>
		/// <remarks>
		/// Returns an empty string if the input string is null or empty. If the length of the input string is less than
		/// <c>length</c>, the whole string is returned.
		/// </remarks>
		public static string SubstringEx(this string @string, int length)
		{
			if (@string.IsNullOrEmpty()) return string.Empty;
			var startIndex = @string.Length - Math.Abs(length);
			return startIndex < 0
				? @string
				: length < 0
					? @string.Substring(startIndex)
					: @string.Substring(0, length);
		}

		/// <summary>
		/// Converts a Pascal-case string to a camel-case one.
		/// </summary>
		/// <param name="string">
		/// A Pascal-case string.
		/// </param>
		/// <returns>
		/// The camel-case equivalent string.
		/// </returns>
		public static string ToCamelCase(this string @string)
		{
			return @string.IsNullOrEmpty()
				? string.Empty
				: ToLower(@string[0], CultureInfo.InvariantCulture) + @string.Substring(1);
		}
	}
}
