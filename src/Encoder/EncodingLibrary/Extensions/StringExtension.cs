using EncodingLibrary.Converters;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncodingLibrary.Extensions
{
    public static class StringExtension
    {
        public static string ChangeEncoding(this string text, string sourceEncodingName, string destinationEncodingName)
        {
            return EncodingConverter.Convert
            (
                text: text,
                sourceEncodingName: sourceEncodingName,
                destinationEncodingName: destinationEncodingName
            );
        }

        public static Encoding DetectEncoding(this string text)
        {
            Encoding encoding = null;

            foreach (EncodingInfo ei in Encoding.GetEncodings())
            {
                Encoding e = ei.GetEncoding();

                if (text.IsMatchEncoding(e))
                {
                    encoding = e;
                    break;
                }
            }

            return encoding;
        }

        public static IEnumerable<Encoding> DetectEncodings(this string text)
        {
            var encodings = new List<Encoding>();

            foreach (EncodingInfo ei in Encoding.GetEncodings())
            {
                Encoding e = ei.GetEncoding();

                if (text.IsMatchEncoding(e))
                {
                    encodings.Add(e);
                }
            }

            return encodings;
        }

        public static bool IsMatchEncoding(this string text, Encoding encoding)
        {
            return encoding.GetString(encoding.GetBytes(text)) == text;
        }
    }
}
