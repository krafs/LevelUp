namespace LevelUp
{
    internal static class Extensions
    {
        public static string Italic(this string self)
        {
            return "<i>" + self + "</i>";
        }

        public static string Bold(this string self)
        {
            return "<b>" + self + "</b>";
        }
    }
}