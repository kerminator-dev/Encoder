using System;
using System.Collections.Generic;
using System.Text;

namespace Encoder.Converters
{
    public class EncodingConverter : IConvertible<string>
    {
        /// <summary>
        /// Конвертироватьь
        /// </summary>
        /// <param name="sourceText">Исходный текст/строка</param>
        /// <param name="sourceEncodingName">Кодировка исходного текста</param>
        /// <param name="destinationEncodingName">Конечная ожидаемая кодировка</param>
        /// <returns>Результат выполнения конвертации</returns>
        public ConvertionResult<string> Convert(string sourceText, string sourceEncodingName, string destinationEncodingName)
        {
            try
            {
                Encoding sourceEncoding = Encoding.GetEncoding(sourceEncodingName);
                Encoding destinationEncoding = Encoding.GetEncoding(destinationEncodingName);
                byte[] destinationBytes = destinationEncoding.GetBytes(sourceText);
                byte[] sourceBytes = Encoding.Convert(destinationEncoding, sourceEncoding, destinationBytes);
                string result = destinationEncoding.GetString(sourceBytes);

                return new ConvertionResult<string>
                (
                    result: result,
                    isSuccess: true,
                    exceptions: new List<Exception>()
                );
            }
            catch (Exception ex)
            {
                return new ConvertionResult<string>
                (
                    result: string.Empty,
                    isSuccess: false,
                    exceptions: new List<Exception>() { ex }
                );
            }
        }
    }
}
