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
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

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

		/// <summary>
		/// Parses document from string content
		/// </summary>
		/// <param name="tdoc"></param>
		/// <param name="content"></param>
		public static void Parse(IntPtr tdoc, string content)
		{
			if (IntPtr.Zero == tdoc)
			{
				throw new ArgumentException("cannot parse to  non-defined document");
			}
			if (String.IsNullOrWhiteSpace(content))
			{
				throw new ArgumentException("cannot parse non-defined content");
			}
			var currentInputEncoding = GetOption<uint>(tdoc, TidyOptionId.TidyInCharEncoding);
			try
			{
				SetOption(tdoc, TidyOptionId.TidyInCharEncoding, (uint)EncodingType.Utf8);
				var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
				Parse(tdoc, stream);
			}
			finally
			{
				SetOption<uint>(tdoc, TidyOptionId.TidyInCharEncoding, currentInputEncoding);
			}
		}

		/// <summary>
		/// Retrieves result as string
		/// </summary>
		/// <param name="tdoc"></param>
		/// <returns></returns>
		public static string Save(IntPtr tdoc)
		{
			if (IntPtr.Zero == tdoc)
			{
				throw new ArgumentException("cannot get result from non-defined document");
			}
			var currentEncoding = GetOption<uint>(tdoc, TidyOptionId.TidyOutCharEncoding);
			var currentBOM = GetOption<int>(tdoc, TidyOptionId.TidyOutputBOM);
			try
			{
				SetOption(tdoc, TidyOptionId.TidyOutCharEncoding, (uint)EncodingType.Utf8);
				SetOption(tdoc, TidyOptionId.TidyOutputBOM, (int)AutoBool.No);
				uint len = 0;
				//most safe and direct way to get len of buffer
				Native.tidySaveString(tdoc, IntPtr.Zero, ref len);
				var buffer = new byte[len];
				var pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
				try
				{
					SetOption(tdoc, TidyOptionId.TidyOutCharEncoding, (uint)EncodingType.Utf8);
					IntPtr pointer = pinnedArray.AddrOfPinnedObject();
					Native.tidySaveString(tdoc, pointer, ref len);
				}
				finally
				{
					pinnedArray.Free();
				}
				return Encoding.UTF8.GetString(buffer);
			}
			finally
			{
				SetOption(tdoc, TidyOptionId.TidyOutCharEncoding, currentEncoding);
				SetOption(tdoc, TidyOptionId.TidyOutputBOM, currentBOM);
			}
		}

		/// <summary>
		/// Saves result data to given file name
		/// </summary>
		/// <param name="tdoc"></param>
		/// <param name="filename"></param>
		public static void Save(IntPtr tdoc, string filename)
		{
			if (IntPtr.Zero == tdoc)
			{
				throw new ArgumentException("cannot save  non-defined document");
			}
			if (string.IsNullOrWhiteSpace(filename))
			{
				throw new ArgumentException("cannot save to non-defined filename");
			}
			var result = Native.tidySaveFile(tdoc, filename);
			if (1 != result)
			{
				throw new IOException("an exception during save to file " + result);
			}
		}


		public static void Save(IntPtr tdoc, Stream stream) {
			if (IntPtr.Zero == tdoc)
			{
				throw new ArgumentException("cannot save  non-defined document");
			}
			if (null==stream)
			{
				throw new ArgumentException("cannot save to non-defined stream");
			}
			OutputSink sink = new OutputSink(stream);
			var result = Native.tidySaveSink(tdoc, ref sink.TidyOutputSink);
			if (1 != result) {
				throw new IOException("an exception during save to stream " + result);
			}

		}

		/// <summary>
		/// Parses document from string content
		/// </summary>
		/// <param name="tdoc"></param>
		/// <param name="filename"></param>
		public static void ParseFile(IntPtr tdoc, string filename)
		{
			if (IntPtr.Zero == tdoc)
			{
				throw new ArgumentException("cannot parse to  non-defined document");
			}
			if (String.IsNullOrWhiteSpace(filename))
			{
				throw new ArgumentException("cannot parse non-defined filename");
			}
			var fullfilename = Path.GetFullPath(filename);
			if (!File.Exists(fullfilename))
			{
				throw new ArgumentException("cannot find file " + filename);
			}
			using (var stream = new FileStream(fullfilename, FileMode.Open))
			{
				Parse(tdoc, stream);
			}

		}
		/// <summary>
		/// Parses document from stream
		/// </summary>
		/// <param name="tdoc"></param>
		/// <param name="stream"></param>
		public static void Parse(IntPtr tdoc, Stream stream)
		{
			if (IntPtr.Zero == tdoc)
			{
				throw new ArgumentException("cannot parse to  non-defined document");
			}
			if (null == stream)
			{
				throw new ArgumentException("cannot parse null stream");
			}
			
				var ins = new InputSource(stream);
				var result = Native.tidyParseSource(tdoc, ref ins.TidyInputSource);
				if (1 != result)
				{
					throw new Exception("some error in parse " + result);
				}
			
		}

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
		/// Executes main work of tidy
		/// </summary>
		/// <param name="tdoc"></param>
		public static void CleanAndRepare(IntPtr tdoc) {
			if (IntPtr.Zero == tdoc)
			{
				throw new ArgumentException("cannot clear  non-defined document");
			}
			var result = Native.tidyCleanAndRepair(tdoc);
			if (1 != result)
			{
				throw new Exception("some error in parse " + result);
			}
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
}