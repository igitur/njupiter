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

namespace nJupiter.Web.UI.Controls {

	[Obsolete("Try using System.Web.UI.HtmlControls.HtmlGenericControl together with nJupiter.Web.UI.ControlAdapters.HtmlGenericAdapter (or ASP.NET 4)")]
	public class WebParagraph : WebGenericControl {
		
		public WebParagraph() {
			this.TagName = HtmlTag.P;
		}

#pragma warning disable 168
		// ReSharper disable UnusedParameter.Local
		//
		//This is needed for functioning (part of the contract of being an HtmlControl)
		//Should always be a p
		public WebParagraph(string tag) : this(){}
		// ReSharper restore UnusedParameter.Local
#pragma warning restore 168

	}
}
