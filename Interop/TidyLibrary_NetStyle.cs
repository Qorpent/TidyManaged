#region LICENSE

// Copyright (c) 2013 Comdiv (Qorpent Team) Fagim Sadykov
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

#endregion

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace TidyManaged.Interop {
	/// <summary>
	/// </summary>
	partial class TidyLibrary {
		/// <summary>
		/// Contains some found constants
		/// </summary>
		public static class Constants {
			/// <summary>
			/// Default value of <see cref="TidyOptionId.TidyIndentSpaces"/>
			/// </summary>
			public const uint DEFAULT_INDENT_SPACES = 2;
		}
		
		/// <summary>
		/// Wraps document-related functions
		/// </summary>
		public static class Document {
			/// <summary>
			/// Initializes tidy document
			/// </summary>
			/// <returns></returns>
			public static IntPtr Create() {
				return Native.tidyCreate();
			}

			/// <summary>
			/// Releases tidy document
			/// </summary>
			/// <param name="tdoc"></param>		
			public static void Release(IntPtr tdoc) {
				if (IntPtr.Zero == tdoc) {
					return;
				}
				Native.tidyRelease(tdoc);
			}
			/// <summary>
			/// Generic get accessor to document options
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="tdoc"></param>
			/// <param name="optionId"></param>
			/// <returns></returns>
			/// <exception cref="NotSupportedException"></exception>
			public static T GetOption<T>(IntPtr tdoc, TidyOptionId optionId) {
				if (IntPtr.Zero == tdoc)
				{
					throw new ArgumentException("cannot get options from non-defined document");
				}
				if (typeof (string) == typeof (T)) {
					return (T)(object)Marshal.PtrToStringAnsi(Native.tidyOptGetValue(tdoc, optionId));
				}
				if (typeof(int) == typeof(T) || typeof(uint)== typeof(T)||typeof(long)==typeof(T))
				{
					return (T)Convert.ChangeType(Native.tidyOptGetInt(tdoc, optionId),typeof(T));
				}
				if (typeof (bool) == typeof (T)) {
					return (T) (object) Native.tidyOptGetBool(tdoc, optionId);
				}
				throw new NotSupportedException("options of type "+typeof(T).FullName+" are not supported by tidy");
			}
			/// <summary>
			/// Generic set accessor to document options
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="tdoc"></param>
			/// <param name="optionId"></param>
			/// <returns></returns>		
			public static void SetOption<T>(IntPtr tdoc, TidyOptionId optionId, T value) {
				if (IntPtr.Zero == tdoc) {
					throw new ArgumentException("cannot set options on non-defined document"); 
				}
				if (typeof(string) == typeof(T)) {
					Native.tidyOptSetValue(tdoc, optionId, (string)(object) value);
					
				}else if (typeof(int) == typeof(T) || typeof(uint) == typeof(T) || typeof(long) == typeof(T)) {
					uint tidyCompliantIntValue = Convert.ToUInt32(value);
					Native.tidyOptSetInt(tdoc,optionId,tidyCompliantIntValue);
				}else if (typeof (bool) == typeof (T)) {
					Native.tidyOptSetBool(tdoc, optionId, (bool) (object) value);
				}
				else {
					throw new NotSupportedException("options of type " + typeof (T).FullName + " are not supported by tidy");
				}
			}

		}

		/// <summary>
		/// Returns tidy library release date
		/// </summary>
		/// <returns></returns>
		public static DateTime GetReleaseDate() {
			var result = DateTime.MinValue;
			var release = Marshal.PtrToStringAnsi(Native.tidyReleaseDate());
			if (release != null)
			{
				var tokens = release.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				if (tokens.Length >= 3)
				{
					DateTime.TryParseExact(tokens[0] + " " + tokens[1] + " " + tokens[2], "d MMMM yyyy",
										   CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
				}
			}
			return result;
		}
	}
}