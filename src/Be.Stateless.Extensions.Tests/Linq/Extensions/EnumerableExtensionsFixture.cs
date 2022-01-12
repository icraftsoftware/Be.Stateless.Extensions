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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;
using static FluentAssertions.FluentActions;

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
			source.Distinct((t1, t2) => t1.Item2 == t2.Item2).Should().BeEquivalentTo(new[] { new Tuple<int, int>(1, 2) });
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

		[Fact]
		public void Single()
		{
			Invoking(
					() => Enumerable.Empty<int>().Single(
						() => new InvalidOperationException("Custom lazy message for no element."),
						() => new InvalidOperationException("Custom lazy message for more than one element.")))
				.Should().Throw<InvalidOperationException>()
				.WithMessage("Custom lazy message for no element.");

			Invoking(
					() => new[] { 1, 2, 3 }.Single(
						() => new InvalidOperationException("Custom lazy message for no element."),
						() => new InvalidOperationException("Custom lazy message for more than one element.")))
				.Should().Throw<InvalidOperationException>()
				.WithMessage("Custom lazy message for more than one element.");
		}

		[Fact]
		public void SingleOrDefault()
		{
			Invoking(() => new[] { 1, 2, 3 }.SingleOrDefault(() => new InvalidOperationException("Custom lazy message for more than one element.")))
				.Should().Throw<InvalidOperationException>()
				.WithMessage("Custom lazy message for more than one element.");
		}

		[Fact]
		public void SingleOrDefaultWithPredicate()
		{
			Invoking(() => new[] { 1, 2, 3 }.SingleOrDefault(i => i % 2 == 1, () => new InvalidOperationException("More than one odd number.")))
				.Should().Throw<InvalidOperationException>()
				.WithMessage("More than one odd number.");
		}

		[Fact]
		public void SingleWithPredicate()
		{
			Invoking(
					() => Enumerable.Empty<int>().Single(
						i => i % 2 == 1,
						() => new InvalidOperationException("No odd number."),
						() => new InvalidOperationException("More than one odd number.")))
				.Should().Throw<InvalidOperationException>()
				.WithMessage("No odd number.");

			Invoking(
					() => new[] { 1, 2, 3 }.Single(
						i => i % 2 == 1,
						() => new InvalidOperationException("No odd number."),
						() => new InvalidOperationException("More than one odd number.")))
				.Should().Throw<InvalidOperationException>()
				.WithMessage("More than one odd number.");
		}
	}
}
