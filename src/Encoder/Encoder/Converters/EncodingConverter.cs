using System;
using System.Collections.Generic;
using System.Text;

namespace Encoder.Converters
{
    public class EncodingConverter : IConvertible<string>
    {
        /// <summary>
        /// Конвертировать
        /// </summary>
        /// <param name="sourceText">Исходный текст/строка</param>
        /// <param name="sourceEncodingName">Кодировка исходного текста</param>
        /// <param name="destinationEncodingName">Конечная ожидаемая кодировка</param>
        /// <returns>Результат выполнения конвертации</returns>
        public ConvertionResult<string> Convert(string sourceText, string sourceEncodingName, string destinationEncodingName)
        {
            try
            {
                // Обработка исключений
                var handledExceptions = new List<Exception>();

                if (string.IsNullOrEmpty(sourceEncodingName))
                    handledExceptions.Add(new Exception("Должна быть выбрана исходная кодировка текста!"));

                if (string.IsNullOrEmpty(destinationEncodingName))
                    handledExceptions.Add(new Exception("Должна быть выбрана конечная кодировка текста!"));

                if (string.IsNullOrEmpty(sourceText))
                    handledExceptions.Add(new Exception("Исходный текст должен иметь содержимое!"));
                
                if (handledExceptions.Count > 0)
                {
                    throw new AggregateException(handledExceptions);
                }

                // Смена кодировки
                Encoding sourceEncoding = Encoding.GetEncoding(sourceEncodingName);
                Encoding destinationEncoding = Encoding.GetEncoding(destinationEncodingName);
                byte[] destinationBytes = destinationEncoding.GetBytes(sourceText);
                byte[] sourceBytes = Encoding.Convert(destinationEncoding, sourceEncoding, destinationBytes);
                string result = destinationEncoding.GetString(sourceBytes);

                return new ConvertionResult<string>
                (
                    result: result,
                    isSuccess: true,
                    exceptions: default
                );
            }
            catch (AggregateException ex)
            {
                return new ConvertionResult<string>
                (
                    result: string.Empty,
                    isSuccess: false,
                    exceptions: ex.InnerExceptions
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
