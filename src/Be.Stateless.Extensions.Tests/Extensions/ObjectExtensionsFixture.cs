﻿#region Copyright & License

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
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using Xunit;

namespace Be.Stateless.Extensions
{
	public class ObjectExtensionsFixture
	{
		[Fact]
		[SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
		public void IfNotNullDoesNotInvokeActionDelegate()
		{
			object @object = null;
			var actionMock = new Mock<Action<object>>();

			@object.IfNotNull(actionMock.Object);

			actionMock.Verify(a => a.Invoke(@object), Times.Never);
		}

		[Fact]
		[SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
		public void IfNotNullDoesNotInvokeFunctionDelegate()
		{
			object @object = null;
			var actionMock = new Mock<Func<object, bool>>();
			actionMock.Setup(f => f.Invoke(@object)).Returns(true);

			@object.IfNotNull(actionMock.Object).Should().BeFalse();

			actionMock.Verify(f => f.Invoke(@object), Times.Never);
		}

		[Fact]
		public void IfNotNullInvokesActionDelegate()
		{
			var @object = new object();
			var actionMock = new Mock<Action<object>>();

			@object.IfNotNull(actionMock.Object);

			actionMock.Verify(a => a.Invoke(@object), Times.Once);
		}

		[Fact]
		public void IfNotNullInvokesFunctionDelegate()
		{
			var @object = new object();
			var actionMock = new Mock<Func<object, bool>>();
			actionMock.Setup(f => f.Invoke(@object)).Returns(true);

			@object.IfNotNull(actionMock.Object).Should().BeTrue();

			actionMock.Verify(f => f.Invoke(@object), Times.Once);
		}
	}
}
