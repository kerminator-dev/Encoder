namespace Encoder.Models
{
    internal class FileInfo
    {
        public string Filename { get; }
        public string Content { get; }
        public string Encoding { get; }

        public FileInfo(string filename, string content, string encoding)
        {
            Filename = filename;
            Content = content;
            Encoding = encoding;
        }
    }
}
