namespace EncodingLibrary.Converters
{
    public interface IConvertible<T>
    {
        /// <summary>
        /// Конвертировать
        /// </summary>
        /// <returns></returns>
        T Convert();
    }
}
