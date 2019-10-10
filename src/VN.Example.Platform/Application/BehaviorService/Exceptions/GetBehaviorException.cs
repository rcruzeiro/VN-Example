using System;

namespace VN.Example.Platform.Application.BehaviorService.Exceptions
{
    public class GetBehaviorException : Exception
    {
        public GetBehaviorException(string message)
            : base(message)
        { }

        public GetBehaviorException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
