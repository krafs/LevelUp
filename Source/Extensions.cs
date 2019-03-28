using UnityEngine;

namespace LevelUp
{
    internal static class Extensions
    {
        internal static string Italic(this string self)
        {
            return "<i>" + self + "</i>";
        }

        internal static string Bold(this string self)
        {
            return "<b>" + self + "</b>";
        }

        internal static string Colored(this string self, Color inColor)
        {
            Color32 color = inColor;
            string hexColor = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return "<b><color=#" + hexColor + ">" + self + "</color></b>";
        }
    }
}