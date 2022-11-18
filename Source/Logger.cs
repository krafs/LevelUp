using System.Diagnostics;
using Verse;

namespace LevelUp;

internal static class Logger
{
    [Conditional("DEBUG")]
    internal static void Debug(string message)
    {
        Log.Message(message);
    }
}
