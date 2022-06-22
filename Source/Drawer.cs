using UnityEngine;

namespace LevelUp;

public sealed class Drawer : IDrawer
{
    public static IDrawer Empty { get; } = new Drawer();

    private Drawer()
    { }

    public void Draw(Rect rect)
    { }
}
