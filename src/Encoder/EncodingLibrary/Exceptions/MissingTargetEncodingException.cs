using System;

namespace EncodingLibrary.Exceptions
{
    public class MissingTargetEncodingException : Exception
    {
        public MissingTargetEncodingException(string message) : base(message)
        {

        }
    }
}
