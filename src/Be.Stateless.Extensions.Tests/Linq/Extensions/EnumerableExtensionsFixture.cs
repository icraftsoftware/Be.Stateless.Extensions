#region Copyright & License
// Copyright © 2012 - 2019 François Chabot
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
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using Xunit;

namespace Be.Stateless.Linq.Extensions
{
	public class EnumerableExtensionsFixture
	{
		[Fact]
		[SuppressMessage("ReSharper", "CoVariantArrayConversion")]
		public void Distinct()
		{
			var source = new[] {
				new Tuple<int, int>(1, 2),
				new Tuple<int, int>(2, 2),
				new Tuple<int, int>(3, 2),
				new Tuple<int, int>(4, 2)
			};

			// equality on Tuple.Item1
			source.Distinct((t1, t2) => t1.Item1 == t2.Item1).Should().BeEquivalentTo(source);
			// equality on Tuple.Item2
			source.Distinct((t1, t2) => t1.Item2 == t2.Item2).Should().BeEquivalentTo(new Tuple<int, int>(1, 2));
		}

		[Fact]
		[SuppressMessage("ReSharper", "CoVariantArrayConversion")]
		public void Except()
		{
			var first = new[] {
				new Tuple<int, int>(1, 2),
				new Tuple<int, int>(2, 3),
				new Tuple<int, int>(3, 4),
				new Tuple<int, int>(4, 5)
			};
			var second = new[] {
				new Tuple<int, int>(1, 1),
				new Tuple<int, int>(2, 1),
				new Tuple<int, int>(3, 1),
				new Tuple<int, int>(4, 1)
			};

			// equality on Tuple.Item1
			first.Except(second, (t1, t2) => t1.Item1 == t2.Item1).Should().BeEmpty();
			// equality on Tuple.Item2
			first.Except(second, (t1, t2) => t1.Item2 == t2.Item2).Should().BeEquivalentTo(first);
		}

		[Fact]
		public void ForEach()
		{
			var source = new[] {
				new Tuple<int, int>(1, 4),
				new Tuple<int, int>(2, 5),
				new Tuple<int, int>(3, 6)
			};
			var actionMock = new Mock<Action<Tuple<int, int>>>();

			source.ForEach(actionMock.Object);

			actionMock.Verify(a => a.Invoke(source[0]));
			actionMock.Verify(a => a.Invoke(source[1]));
			actionMock.Verify(a => a.Invoke(source[2]));
		}

		[Fact]
		public void ForEachIncorporatesElementIndex()
		{
			var source = new[] {
				new Tuple<int, int>(1, 4),
				new Tuple<int, int>(2, 5),
				new Tuple<int, int>(3, 6)
			};
			var actionMock = new Mock<Action<int, Tuple<int, int>>>();

			source.ForEach(actionMock.Object);

			actionMock.Verify(a => a.Invoke(0, source[0]));
			actionMock.Verify(a => a.Invoke(1, source[1]));
			actionMock.Verify(a => a.Invoke(2, source[2]));
		}
	}
}
