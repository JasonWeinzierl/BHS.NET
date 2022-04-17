namespace BHS.Domain
{
    [Serializable]
    public class BadRequestException : Exception
    {
        public const string BaseMessage = "Bad Request: ";

        public BadRequestException(string message) : base(BaseMessage + message) { }
        public BadRequestException(string message, Exception inner) : base(BaseMessage + message, inner) { }
        protected BadRequestException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
