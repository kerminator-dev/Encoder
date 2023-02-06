using System;
using System.Collections.Generic;
using System.Text;

namespace EncodingLibrary.Converters
{
    public class EncodingConverter : IConvertible<string>
    {
        protected readonly string _sourceText;
        protected readonly string _sourceEncodingName;
        protected readonly string _destinationEncodingName;

        public EncodingConverter(string sourceText, string sourceEncodingName, string destinationEncodingName)
        {
            if (string.IsNullOrEmpty(sourceEncodingName))
                throw new Exception("Должна быть выбрана исходная кодировка текста!");

            if (string.IsNullOrEmpty(destinationEncodingName))
                throw new Exception("Должна быть выбрана конечная кодировка текста!");

            if (string.IsNullOrEmpty(sourceText))
                throw new Exception("Исходный текст должен иметь содержимое!");

            _sourceText = sourceText;
            _sourceEncodingName = sourceEncodingName;
            _destinationEncodingName = destinationEncodingName;
        }


        /// <summary>
        /// Конвертировать
        /// </summary>
        /// <returns>Результат выполнения конвертации</returns>
        public string Convert()
        {
            // Смена кодировки
            Encoding sourceEncoding = Encoding.GetEncoding(_sourceEncodingName);
            Encoding destinationEncoding = Encoding.GetEncoding(_destinationEncodingName);
            byte[] destinationBytes = destinationEncoding.GetBytes(_sourceText);
            byte[] sourceBytes = Encoding.Convert(destinationEncoding, sourceEncoding, destinationBytes);
                
            return destinationEncoding.GetString(sourceBytes);
        }
    }
}
