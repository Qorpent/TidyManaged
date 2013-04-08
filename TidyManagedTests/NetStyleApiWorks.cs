using System;
using NUnit.Framework;
using TidyManaged.Interop;

namespace TidyManagedTests {
	/// <summary>
	/// </summary>
	[TestFixture(Description = "cover-propose test to .net-style api wrapper")]
	public class NetStyleApiWorks {
		[Test]
		public void CreateAndRelaseDocumentTest() {
			var dochandle = TidyLibrary.Document.Create();
			try {
				Assert.AreNotEqual(IntPtr.Zero,dochandle);
			}
			finally {
				TidyLibrary.Document.Release(dochandle);
			}
		}

		[Test]
		public void ReleaseDateTest() {
			var releasedate = TidyLibrary.GetReleaseDate();
			Assert.Greater(releasedate,new DateTime(2008,1,1));
			Assert.Less(releasedate, DateTime.Now);
		}

		[Test]
		public void SetAndGetOptionsInGenericMode() {
			var dochandle = TidyLibrary.Document.Create();
			try
			{
				TidyLibrary.Document.SetOption(dochandle,TidyOptionId.TidyAltText, "text");
				Assert.AreEqual("text",TidyLibrary.Document.GetOption<string>(dochandle,TidyOptionId.TidyAltText),"string");
				TidyLibrary.Document.SetOption(dochandle, TidyOptionId.TidyBodyOnly, true);
				Assert.AreEqual(true, TidyLibrary.Document.GetOption<bool>(dochandle, TidyOptionId.TidyBodyOnly), "bool true");
				TidyLibrary.Document.SetOption(dochandle, TidyOptionId.TidyBodyOnly, false);
				Assert.AreEqual(false, TidyLibrary.Document.GetOption<bool>(dochandle, TidyOptionId.TidyBodyOnly), "bool false");
				Assert.AreEqual(TidyLibrary.Constants.DEFAULT_INDENT_SPACES, TidyLibrary.Document.GetOption<int>(dochandle, TidyOptionId.TidyIndentSpaces), "int");
				TidyLibrary.Document.SetOption(dochandle, TidyOptionId.TidyIndentSpaces, 16);
				Assert.AreEqual(16, TidyLibrary.Document.GetOption<int>(dochandle, TidyOptionId.TidyIndentSpaces), "int");
			}
			finally
			{
				TidyLibrary.Document.Release(dochandle);
			}
		}

	}
}