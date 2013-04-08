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
			var dochandle = TidyLibrary.Create();
			try {
				Assert.AreNotEqual(IntPtr.Zero,dochandle);
			}
			finally {
				TidyLibrary.Release(dochandle);
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
			var dochandle = TidyLibrary.Create();
			try
			{
				TidyLibrary.SetOption(dochandle,TidyOptionId.TidyAltText, "text");
				Assert.AreEqual("text",TidyLibrary.GetOption<string>(dochandle,TidyOptionId.TidyAltText),"string");
				TidyLibrary.SetOption(dochandle, TidyOptionId.TidyBodyOnly, true);
				Assert.AreEqual(true, TidyLibrary.GetOption<bool>(dochandle, TidyOptionId.TidyBodyOnly), "bool true");
				TidyLibrary.SetOption(dochandle, TidyOptionId.TidyBodyOnly, false);
				Assert.AreEqual(false, TidyLibrary.GetOption<bool>(dochandle, TidyOptionId.TidyBodyOnly), "bool false");
				Assert.AreEqual(TidyLibrary.Constants.DEFAULT_INDENT_SPACES, TidyLibrary.GetOption<int>(dochandle, TidyOptionId.TidyIndentSpaces), "int");
				TidyLibrary.SetOption(dochandle, TidyOptionId.TidyIndentSpaces, 16);
				Assert.AreEqual(16, TidyLibrary.GetOption<int>(dochandle, TidyOptionId.TidyIndentSpaces), "int");
			}
			finally
			{
				TidyLibrary.Release(dochandle);
			}
		}

	}
}