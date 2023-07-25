using EncodingLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncodingLibrary.Converters
{
    public class DefaultEncodingConverter : IEncodingConverter
    {
        public string Convert(string text, string sourceEncodingName, string destinationEncodingName)
        {
            var exceptions = new List<Exception>();

            if (string.IsNullOrEmpty(sourceEncodingName))
                exceptions.Add(new MissingSourceEncodingException("Должна быть указана исходная кодировка текста!"));

            if (string.IsNullOrEmpty(destinationEncodingName))
                exceptions.Add(new MissingTargetEncodingException("Должна быть указана конечная кодировка текста!"));

            if (sourceEncodingName == destinationEncodingName)
                exceptions.Add(new InvalidEncodingException("Исходная кодировка должна отличаться от конечной кодировки!"));

            if (string.IsNullOrEmpty(text))
                exceptions.Add(new EmptySourceTextException("Исходный текст должен иметь содержимое!"));

            if (exceptions.Any())
                throw new AggregateException(exceptions);

            Encoding sourceEncoding = Encoding.GetEncoding(sourceEncodingName);
            Encoding destinationEncoding = Encoding.GetEncoding(destinationEncodingName);
            byte[] destinationBytes = destinationEncoding.GetBytes(text);
            byte[] sourceBytes = Encoding.Convert(destinationEncoding, sourceEncoding, destinationBytes);
                
            return destinationEncoding.GetString(sourceBytes);
        }
    }
}
