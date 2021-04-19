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

namespace Be.Stateless.Linq
{
	/// <summary>
	/// Provides a <see cref="IEqualityComparer{T}"/> wrapper that delegates to lambdas that support the comparison of objects
	/// for equality.
	/// </summary>
	/// <typeparam name="T">
	/// The type of objects to compare.
	/// </typeparam>
	/// <remarks>
	/// It is recommended to derive from the <see cref="EqualityComparer{T}"/> class instead of implementing the <see
	/// cref="IEqualityComparer{T}"/> interface, because the <see cref="EqualityComparer{T}"/> class tests for equality using
	/// the <see cref="IEquatable{T}"/>.<see cref="IEquatable{T}.Equals(T)"/> method instead of the <see cref="object"/>.<see
	/// cref="object.Equals(object)"/> method.
	/// </remarks>
	/// <seealso href="http://brendan.enrick.com/blog/linq-your-collections-with-iequalitycomparer-and-lambda-expressions/"/>
	/// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.iequalitycomparer-1"/>
	/// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.equalitycomparer-1"/>
	public class LambdaComparer<T> : EqualityComparer<T>
	{
		public LambdaComparer(Func<T, T, bool> comparer) : this(comparer, _ => 0) { }

		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
		public LambdaComparer(Func<T, T, bool> comparer, Func<T, int> hasher)
		{
			_comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
			_hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
		}

		#region Base Class Member Overrides

		public override bool Equals(T x, T y)
		{
			return _comparer(x, y);
		}

		public override int GetHashCode(T obj)
		{
			return _hasher(obj);
		}

		#endregion

		private readonly Func<T, T, bool> _comparer;
		private readonly Func<T, int> _hasher;
	}
}
