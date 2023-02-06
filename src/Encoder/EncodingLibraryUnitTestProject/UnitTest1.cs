using EncodingLibrary.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EncodingLibraryUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UTF8_To_Windows1251()
        {
            var text = "Текст";
            var expected = "РўРµРєСЃС‚";
            var actual = text.ChangeEncoding("utf-8", "windows-1251");

            Assert.AreEqual(expected, actual);
        }
    }
}
