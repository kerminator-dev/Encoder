namespace EncodingLibrary.Converters
{
    public interface IEncodingConverter
    {
        string Convert(string text, string sourceEncodingName, string destinationEncodingName);
    }
}
