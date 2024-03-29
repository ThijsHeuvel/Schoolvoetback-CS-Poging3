﻿using System.Runtime.Serialization;

namespace CarDB.Exceptions
{
    [Serializable]
    internal class InvalidInputException : Exception
    {
        public InvalidInputException()
        {
        }

        public InvalidInputException(string? message) : base(message)
        {
        }

        public InvalidInputException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidInputException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}