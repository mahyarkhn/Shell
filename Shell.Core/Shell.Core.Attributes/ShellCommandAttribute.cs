using System;

namespace Shell.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ShellCommandAttribute : Attribute
    {
        public ShellCommandAttribute()
        {

        }

        public ShellCommandAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public string Aliases { get; set; }
    }
}