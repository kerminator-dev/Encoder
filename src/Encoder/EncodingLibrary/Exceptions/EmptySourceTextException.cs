using System;

namespace EncodingLibrary.Exceptions
{
    public class EmptySourceTextException : Exception
    {
        public EmptySourceTextException(string message = "Исходный текст должен иметь содержимое!") : base(message)
        {

        }
    }
}
