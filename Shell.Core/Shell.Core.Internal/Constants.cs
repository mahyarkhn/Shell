namespace Shell.Core.Internal
{
    internal sealed class Constants
    {
        // escape code sign
        internal const char SmartLog_EscapeSign = '\\';
        // forecolor sign
        internal const char SmartLog_ForeColorSign = '^';
        // escape codes
        internal static readonly char[] SmartLog_EscapeCodes =
        {
            'r',
            'n',
            'a',
            't',
            'v'
        };
    }
}