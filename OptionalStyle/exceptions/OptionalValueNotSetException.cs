using System;
using System.Runtime.Serialization;

namespace OptionalStyle.exceptions
{
    public class OptionalValueNotSetException : Exception
    {
        public OptionalValueNotSetException()
        {
        }

        public OptionalValueNotSetException(string message) : base(message)
        {
        }

        public OptionalValueNotSetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OptionalValueNotSetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}