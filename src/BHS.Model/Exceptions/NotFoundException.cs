using System;

namespace BHS.Model.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public const string BaseMessage = "Not Found: ";

        public NotFoundException(string message) : base(BaseMessage + message) { }
        public NotFoundException(string message, Exception inner) : base(BaseMessage + message, inner) { }
        protected NotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
