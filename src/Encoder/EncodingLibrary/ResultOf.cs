using System;
using System.Collections.Generic;

namespace EncodingLibrary
{
    /// <summary>
    /// Результат выполнения
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ResutOf<T>
    {
        /// <summary>
        /// Результат выполнения
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Успешность выполнения
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Список полученных в ходе конвертации исключений
        /// </summary>
        public IReadOnlyList<Exception> Exceptions { get; }

        /// <summary>
        /// Есть ли исключения
        /// </summary>
        public bool HasExceptions => Exceptions.Count > 0;

        public ResutOf(T result, bool isSuccess, IReadOnlyList<Exception> exceptions)
        {
            Result = result;
            IsSuccess = isSuccess;
            Exceptions = exceptions;
        }
    }
}
