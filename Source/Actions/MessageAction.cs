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
        var resolvedText = ResolveText(levelingInfo, Text);
        var message = new Message(resolvedText, MessageTypeDefOf.SilentInput, levelingInfo.Pawn);

        Messages.Message(message, historical);
    }

    public override void Draw(Rect rect)
    {
        rect.yMin = DrawTextBuilder(rect).yMax;

        var rowRect = new Rect(rect) { height = 24f };
        var historicalLabel = I18n.HistoricalLabel;
        TooltipHandler.TipRegion(rowRect, I18n.HistoricalDescription);
        Widgets.CheckboxLabeled(rowRect, historicalLabel, ref historical, placeCheckboxNearText: true);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref historical, "historical");
    }
}
