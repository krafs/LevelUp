namespace LevelUp;

public static class StringFormatExtensions
{
    public static string Bolded(this string str)
    {
        return $"<b>{str}</b>";
    }

    public static string Italicized(this string str)
    {
        return $"<i>{str}</i>";
    }
}
