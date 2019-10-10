using System;

namespace VN.Example.Platform.Application.BehaviorService.Exceptions
{
    public class CreateBehaviorException : Exception
    {
        public CreateBehaviorException(string message)
            : base(message)
        { }

        public CreateBehaviorException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
