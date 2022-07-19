namespace Encoder.Converters
{
    public interface IConvertible<T>
    {
        /// <summary>
        /// Конвертировать
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sourceEncodingName"></param>
        /// <param name="destinationEncodingName"></param>
        /// <returns></returns>
        ConvertionResult<T> Convert(string text, string sourceEncodingName, string destinationEncodingName);
    }
}
