using System;

namespace Shell.Core.Exceptions
{
    [Serializable]
    public class ShellCommandNotFoundException : Exception
    {
        public ShellCommandNotFoundException() { }
        public ShellCommandNotFoundException(string message) : base(message) { }
        public ShellCommandNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ShellCommandNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}