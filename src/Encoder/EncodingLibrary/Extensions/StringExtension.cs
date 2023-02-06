using EncodingLibrary.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncodingLibrary.Extensions
{
    public static class StringExtension
    {
        public static string ChangeEncoding(this string sourceText, string sourceEncodingName, string destinationEncodingName)
        {
            return new EncodingConverter
            (
                sourceText: sourceText,
                sourceEncodingName: sourceEncodingName,
                destinationEncodingName: destinationEncodingName
            ).Convert();
        }
    }
}
