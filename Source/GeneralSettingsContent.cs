using System;
using UnityEngine;
using Verse;

namespace LevelUp;

[Serializable]
public class GeneralSettingsContent : IExposable, IDrawer
{
    private int cooldownSeconds = 20;
    private string? cooldownEditBuffer;

    public int CooldownSeconds => cooldownSeconds;

    public void Draw(Rect rect)
    {
        Rect rowRect = new(rect) { height = 24f };
        string cooldownLabel = I18n.CooldownLabel;
        Vector2 cooldownSize = Text.CalcSize(cooldownLabel);
        Rect cooldownLabelRect = new(rowRect) { width = cooldownSize.x };
        Widgets.Label(cooldownLabelRect, cooldownLabel);
        TooltipHandler.TipRegion(cooldownLabelRect, I18n.CooldownEntryDescription);
        Rect cooldownEntryRect = new(rowRect) { x = cooldownLabelRect.xMax + 5f, width = 180f };
        Widgets.IntEntry(cooldownEntryRect, ref cooldownSeconds, ref cooldownEditBuffer);
    }

    public void ExposeData()
    {
        Scribe_Values.Look(ref cooldownSeconds, "cooldownSeconds");
    }
}
