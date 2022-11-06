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
        var rowRect = new Rect(rect) { height = 24f };
        var cooldownLabel = I18n.CooldownLabel;
        var cooldownSize = Text.CalcSize(cooldownLabel);
        var cooldownLabelRect = new Rect(rowRect) { width = cooldownSize.x };
        Widgets.Label(cooldownLabelRect, cooldownLabel);
        TooltipHandler.TipRegion(cooldownLabelRect, I18n.CooldownEntryDescription);
        var cooldownEntryRect = new Rect(rowRect) { x = cooldownLabelRect.xMax + 5f, width = 180f };
        Widgets.IntEntry(cooldownEntryRect, ref cooldownSeconds, ref cooldownEditBuffer);
    }

    public void ExposeData()
    {
        Scribe_Values.Look(ref cooldownSeconds, "cooldownSeconds");
    }
}
