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
using FluentAssertions;
using Xunit;

namespace Be.Stateless.Linq
{
	public class LambdaComparerFixture
	{
		[Fact]
		public void EqualityWithCustomComparison()
		{
			var sut = new LambdaComparer<Tuple<int, int>>((t1, t2) => t1.Item1 == t2.Item1, t => t.GetHashCode());
			sut.Equals(new(1, 2), new(1, 3)).Should().BeTrue();
		}

		[Fact]
		public void EqualityWithDefaultComparison()
		{
			var sut = EqualityComparer<Tuple<int, int>>.Default;
			sut.Equals(new(1, 2), new(1, 3)).Should().BeFalse();
		}

		[Fact]
		public void HashCodeWithCustomComputation()
		{
			var sut = new LambdaComparer<Tuple<int, int>>((t1, t2) => t1.Item1 == t2.Item1, t => t.GetHashCode());
			sut.GetHashCode(new(1, 2)).Should().Be(EqualityComparer<Tuple<int, int>>.Default.GetHashCode(new(1, 2)));
		}

		[Fact]
		public void HashCodeWithDefaultComputation()
		{
			var sut = new LambdaComparer<Tuple<int, int>>((t1, t2) => t1.Item1 == t2.Item1);
			sut.GetHashCode(new(1, 2)).Should().Be(0);
		}
	}
}
