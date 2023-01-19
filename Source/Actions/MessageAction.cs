using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace LevelUp;

[Serializable]
public class MessageAction : TextAction
{
    private bool historical;

    internal override void Execute(LevelingInfo levelingInfo)
    {
        string resolvedText = ResolveText(levelingInfo, Text);
        Message message = new(resolvedText, MessageTypeDefOf.SilentInput, levelingInfo.Pawn);

        Messages.Message(message, historical);
    }

    public override void Draw(Rect rect)
    {
        rect.yMin = DrawTextBuilder(rect).yMax;

        Rect rowRect = new(rect) { height = 24f };
        string historicalLabel = I18n.HistoricalLabel;
        TooltipHandler.TipRegion(rowRect, I18n.HistoricalDescription);
        Widgets.CheckboxLabeled(rowRect, historicalLabel, ref historical, placeCheckboxNearText: true);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref historical, "historical");
    }
}
