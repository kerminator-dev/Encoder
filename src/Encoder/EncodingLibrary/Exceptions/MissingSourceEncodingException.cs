using System;

namespace EncodingLibrary.Exceptions
{
    public class MissingSourceEncodingException : Exception
    {
        public MissingSourceEncodingException(string message) : base(message)
        {

        }
    }
}
