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
using FluentAssertions;
using Moq;
using Xunit;

namespace Be.Stateless.Extensions
{
	public class StringExtensionsFixture
	{
		[Fact]
		public void IfNotNullOrEmptyDoesNotInvokeActionDelegate()
		{
			var @string = string.Empty;
			var mock = new Mock<Action<string>>();

			@string.IfNotNullOrEmpty(mock.Object);

			mock.Verify(a => a.Invoke(@string), Times.Never);
		}

		[Fact]
		public void IfNotNullOrEmptyDoesNotInvokeFunctionDelegate()
		{
			var @string = string.Empty;
			var mock = new Mock<Func<string, bool>>();
			mock.Setup(f => f.Invoke(@string)).Returns(true);

			@string.IfNotNullOrEmpty(mock.Object).Should().BeFalse();

			mock.Verify(a => a.Invoke(@string), Times.Never);
		}

		[Fact]
		public void IfNotNullOrEmptyInvokesActionDelegate()
		{
			var @string = "anything";
			var mock = new Mock<Action<string>>();

			@string.IfNotNullOrEmpty(mock.Object);

			mock.Verify(a => a.Invoke(@string), Times.Once);
		}

		[Fact]
		public void IfNotNullOrEmptyInvokesFunctionDelegate()
		{
			var @string = "anything";
			var mock = new Mock<Func<string, bool>>();
			mock.Setup(f => f.Invoke(@string)).Returns(true);

			@string.IfNotNullOrEmpty(mock.Object).Should().BeTrue();

			mock.Verify(a => a.Invoke(@string), Times.Once);
		}

		[Fact]
		public void IfNotNullOrWhiteSpaceDoesNotInvokeActionDelegate()
		{
			var @string = "   ";
			var mock = new Mock<Action<string>>();

			@string.IfNotNullOrWhiteSpace(mock.Object);

			mock.Verify(a => a.Invoke(@string), Times.Never);
		}

		[Fact]
		public void IfNotNullOrWhiteSpaceDoesNotInvokeFunctionDelegate()
		{
			var @string = "   ";
			var mock = new Mock<Func<string, bool>>();
			mock.Setup(f => f.Invoke(@string)).Returns(true);

			@string.IfNotNullOrWhiteSpace(mock.Object).Should().BeFalse();

			mock.Verify(a => a.Invoke(@string), Times.Never);
		}

		[Fact]
		public void IfNotNullOrWhiteSpaceInvokesActionDelegate()
		{
			var @string = "anything";
			var mock = new Mock<Action<string>>();

			@string.IfNotNullOrWhiteSpace(mock.Object);

			mock.Verify(a => a.Invoke(@string), Times.Once);
		}

		[Fact]
		public void IfNotNullOrWhiteSpaceInvokesFunctionDelegate()
		{
			var @string = "anything";
			var mock = new Mock<Func<string, bool>>();
			mock.Setup(f => f.Invoke(@string)).Returns(true);

			@string.IfNotNullOrWhiteSpace(mock.Object).Should().BeTrue();

			mock.Verify(a => a.Invoke(@string), Times.Once);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(@"\name.txt")]
		[InlineData(@"/name.txt")]
		public void IsValidFileNameReturnsFalse(string @string)
		{
			@string.IsValidFileName().Should().BeFalse();
		}

		[Theory]
		[InlineData("name")]
		[InlineData("name.txt")]
		[InlineData("name-0.txt")]
		[InlineData("name_0.txt")]
		public void IsValidFileNameReturnsTrue(string @string)
		{
			@string.IsValidFileName().Should().BeTrue();
		}

		[Theory]
		[InlineData("Exclusive", Range.Exclusive)]
		[InlineData("EXCLUSIVE", Range.Exclusive)]
		public void ParseEnumLabel(string value, Enum expected)
		{
			value.Parse<Range>().Should().Be(expected);
		}

		[Fact]
		public void ParseEnumThrowsWhenLabelIsUnknown()
		{
			Action act = () => "Unknown".Parse<Range>();
			act.Should().Throw<ArgumentException>().WithMessage("Requested value 'Unknown' was not found.");
		}

		[Theory]
		[InlineData(null, 3, "")]
		[InlineData(null, -3, "")]
		[InlineData("", 3, "")]
		[InlineData("", -3, "")]
		[InlineData("123456", 3, "123")]
		[InlineData("123456", -3, "456")]
		[InlineData("123456", 9, "123456")]
		[InlineData("123456", -9, "123456")]
		public void SubstringEx(string @string, int length, string expected)
		{
			@string.SubstringEx(length).Should().Be(expected);
		}
	}
}
