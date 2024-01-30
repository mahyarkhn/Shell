using System;

namespace Shell.Core.Exceptions
{
    [Serializable]
    public class ShellCommandAlreadyExistsException : Exception
    {
        public ShellCommandAlreadyExistsException() { }
        public ShellCommandAlreadyExistsException(string message) : base(message) { }
        public ShellCommandAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected ShellCommandAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}