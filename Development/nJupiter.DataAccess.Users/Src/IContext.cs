#region Copyright & License
/*
	Copyright (c) 2005-2011 nJupiter

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
	THE SOFTWARE.
*/
#endregion

using System;

namespace nJupiter.DataAccess.Users {
	public interface IContext {
		string Name { get; }
	}

	[Serializable]
	public class Context : IContext {

		private static readonly IContext defaultContext = new Context();
		public static IContext DefaultContext { get { return defaultContext; } }

		private readonly string name;

		private Context() {
			this.name = string.Empty;
		}

		public Context(string contextName) {
			if(string.IsNullOrEmpty(contextName)) {
				throw new ArgumentException("contextName can not be empty.", "contextName");
			}
			this.name = contextName;
		}

		public string Name { get { return this.name; } }

		public override int GetHashCode() {
			return this.Name.ToLowerInvariant().GetHashCode();
		}

		public override bool Equals(object obj) {
			IContext context = obj as IContext;
			if(context == null) {
				return false;
			}
			return string.Equals(this.Name, context.Name, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}