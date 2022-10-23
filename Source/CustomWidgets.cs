using UnityEngine;
using Verse;
using Verse.Sound;

namespace LevelUp;

[StaticConstructorOnStartup]
internal static class CustomWidgets
{
    private static readonly Texture2D buttonBGAtlasMouseover = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGMouseover");

    // Essentially a stable re-implementation of 1.3 Widgets.ButtonText.
    // The method has different signatures in 1.3 and 1.4, and having a custom version here avoids having to
    // re-compile the mod against multiple versions of the game.
    internal static bool ButtonText(Rect rect, string label)
    {
        var anchor = Text.Anchor;
        var color = GUI.color;

        var atlas = Widgets.ButtonBGAtlas;
        if (Mouse.IsOver(rect))
        {
            atlas = buttonBGAtlasMouseover;
            if (Input.GetMouseButton(0))
            {
                atlas = Widgets.ButtonBGAtlasClick;
            }
        }
        Widgets.DrawAtlas(rect, atlas);
        MouseoverSounds.DoRegion(rect);

        Text.Anchor = TextAnchor.MiddleCenter;

        var wordWrap = Text.WordWrap;
        if (rect.height < Text.LineHeight * 2f)
        {
            Text.WordWrap = false;
        }
        Widgets.Label(rect, label);
        Text.Anchor = anchor;
        GUI.color = color;
        Text.WordWrap = wordWrap;

        var result = Widgets.ButtonInvisible(rect, false) ? Widgets.DraggableResult.Pressed : Widgets.DraggableResult.Idle;

        return result is Widgets.DraggableResult.Pressed or Widgets.DraggableResult.DraggedThenPressed;
    }
}
