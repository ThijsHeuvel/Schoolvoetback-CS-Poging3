using System.Runtime.Serialization;

namespace CarDB.Exceptions
{
    [Serializable]
    internal class InvalidDataStateException : Exception
    {
        public InvalidDataStateException()
        {
        }

        public InvalidDataStateException(string? message) : base(message)
        {
        }

        public InvalidDataStateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidDataStateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}