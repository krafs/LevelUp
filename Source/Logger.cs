using System.Diagnostics;
using Verse;

namespace LevelUp;

public static class Logger
{
    [Conditional("DEBUG")]
    public static void Debug(string message)
    {
        Log.Message(message);
    }
}
