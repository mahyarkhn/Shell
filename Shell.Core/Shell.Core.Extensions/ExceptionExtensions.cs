using Shell.Core.Helpers;
using System;

namespace Shell.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static void PrintShellCommandNotFoundException(this Exception exception)
        {
            Utils.PrintError(string.Format("Command ^3\"{0}\" ^15was ^4not found\n", exception.Data["name"]));
        }
        public static void PrintShellCommandAlreadyExistsException(this Exception exception)
        {
            Utils.PrintError(string.Format("Command ^3\"{0}\" ^15already exists, could not be registered again\n", exception.Data["name"]));
        }
    }
}