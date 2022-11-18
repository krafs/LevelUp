namespace LevelUp;

internal static class StringFormatExtensions
{
    internal static string Bolded(this string str)
    {
        return $"<b>{str}</b>";
    }

    internal static string Italicized(this string str)
    {
        return $"<i>{str}</i>";
    }
}
