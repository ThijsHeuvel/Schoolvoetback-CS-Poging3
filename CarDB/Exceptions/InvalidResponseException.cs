﻿using System.Runtime.Serialization;

namespace CarDB.Exceptions
{
    [Serializable]
    internal class InvalidResponseException : Exception
    {
        public InvalidResponseException()
        {
        }

        public InvalidResponseException(string? message) : base(message)
        {
        }

        public InvalidResponseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}