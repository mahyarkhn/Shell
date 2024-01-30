using System;

namespace Shell.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ShellListenerAttribute : Attribute
    {
        public ShellListenerAttribute(string type)
        {
            Type = type;
        }

        public string Type { get; private set; }
    }
}
