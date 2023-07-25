using System;

namespace EncodingLibrary.Exceptions
{
    public class InvalidEncodingException : Exception
    {
        public InvalidEncodingException(string message) : base(message)
        {

        }
    }
}
