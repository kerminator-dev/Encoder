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

        [TestMethod]
        public void Windows1251_To_UTF8()
        {
            var text = "РўРµРєСЃС‚";
            var expected = "Текст";
            var actual = text.ChangeEncoding("windows-1251", "utf-8");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UTF8_To_KOI8R()
        {
            var text = "Текст";
            var expected = "п╒п╣п╨я│я┌";
            var actual = text.ChangeEncoding("utf-8", "koi8-r");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void KOI8R_To_UTF8()
        {
            var text = "п╒п╣п╨я│я┌";
            var expected = "Текст";
            var actual = text.ChangeEncoding("koi8-r", "utf-8");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ISO_8859_5_To_UTF8()
        {
            var text = "Текст";
            var expected = "�����";
            var actual = text.ChangeEncoding("iso-8859-5", "utf-8");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UTF8_To_ISO88591()
        {
            var text = "Текст";
            var expected = "Ð¢ÐµÐºÑÑ";
            var actual = text.ChangeEncoding("utf-8", "iso-8859-1");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UTF8_To_ShiftJIS()
        {
            var text = "Текст";
            var expected = "ﾐ｢ﾐｵﾐｺﾑ・・";
            var actual = text.ChangeEncoding("utf-8", "shift_jis");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UTF8_To_GB2312()
        {
            var text = "Текст";
            var expected = "孝械泻褋褌";  // Chinese characters for 'text' 
            var actual = text.ChangeEncoding("utf-8", "gb2312");

            Assert.AreEqual(expected, actual);  // Chinese characters for 'text' 
        }

        [TestMethod]
        public void UTF8_To_IBM861()
        {
            var text = "Текст";
            var expected = "╨ó╨╡╨║╤ü╤é";  // Chinese characters for 'text' 
            var actual = text.ChangeEncoding("utf-8", "ibm861");

            Assert.AreEqual(expected, actual);  // Chinese characters for 'text' 
        }
    }
}
