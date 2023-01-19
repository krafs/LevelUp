namespace LevelUp;

internal static class StringFormatExtensions
{
    internal static string Bold(this string str)
    {
        return $"<b>{str}</b>";
    }

    internal static string Italic(this string str)
    {
        return $"<i>{str}</i>";
    }
}
