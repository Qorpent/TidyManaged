// Copyright (c) 2009 Mark Beaton
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Runtime.InteropServices;

namespace TidyManaged.Interop {
	/// <summary>
	/// Represents direct low-level tidy API
	/// </summary>
	public  static partial class TidyLibrary {
		/// <summary>
		/// Shallow tidy functions representation
		/// </summary>
		public static class Native {
			/// <summary>
			/// Initializes tidy document
			/// </summary>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern IntPtr tidyCreate();

			/// <summary>
			/// Releases tidy document
			/// </summary>
			/// <param name="tdoc"></param>
			[DllImport("libtidy.dll")]
			public static extern void tidyRelease(IntPtr tdoc);

			/// <summary>
			/// Returns tidy library release date
			/// </summary>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern IntPtr tidyReleaseDate();

			/// <summary>
			/// Get current Option value as a string
			/// </summary>
			/// <param name="tdoc"></param>
			/// <param name="optId"></param>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern IntPtr tidyOptGetValue(IntPtr tdoc, TidyOptionId optId);

			/// <summary>
			/// Set Option value as a string
			/// </summary>
			/// <param name="tdoc"></param>
			/// <param name="optId"></param>
			/// <param name="val"></param>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern bool tidyOptSetValue(IntPtr tdoc, TidyOptionId optId, string val);
			/// <summary>
			/// Get current Option value as int
			/// </summary>
			/// <param name="tdoc"></param>
			/// <param name="optId"></param>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern uint tidyOptGetInt(IntPtr tdoc, TidyOptionId optId);
			/// <summary>
			/// Set option value as int
			/// </summary>
			/// <param name="tdoc"></param>
			/// <param name="optId"></param>
			/// <param name="val"></param>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern bool tidyOptSetInt(IntPtr tdoc, TidyOptionId optId, uint val);

			/// <summary>
			/// Get option value as bool
			/// </summary>
			/// <param name="tdoc"></param>
			/// <param name="optId"></param>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern bool tidyOptGetBool(IntPtr tdoc, TidyOptionId optId);

			/// <summary>
			/// Set option value as bool
			/// </summary>
			/// <param name="tdoc"></param>
			/// <param name="optId"></param>
			/// <param name="val"></param>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern bool tidyOptSetBool(IntPtr tdoc, TidyOptionId optId, bool val);

			/// <summary>
			/// Parse markup in given generic input source
			/// </summary>
			/// <param name="tdoc"></param>
			/// <param name="source"></param>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern int tidyParseSource(IntPtr tdoc, ref TidyInputSource source);

			/// <summary>
			/// Execute configured cleanup and repair operations on parsed markup
			/// </summary>
			/// <param name="tdoc"></param>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern int tidyCleanAndRepair(IntPtr tdoc);

			/// <summary>
			/// Saves result to file
			/// </summary>
			/// <param name="tdoc"></param>
			/// <param name="filname"></param>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern int tidySaveFile(IntPtr tdoc, string filname);

			/// <summary>
			/// Saves result to string buffer
			/// </summary>
			/// <param name="tdoc"></param>
			/// <param name="buffer"></param>
			/// <param name="buflen"></param>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern int tidySaveString(IntPtr tdoc, IntPtr buffer, ref uint buflen);

			/// <summary>
			/// Save to given generic output sink
			/// </summary>
			/// <param name="tdoc"></param>
			/// <param name="sink"></param>
			/// <returns></returns>
			[DllImport("libtidy.dll")]
			public static extern int tidySaveSink(IntPtr tdoc, ref TidyOutputSink sink);
		}
	}
}