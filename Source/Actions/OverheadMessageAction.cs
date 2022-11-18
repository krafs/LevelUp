using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace LevelUp;

[Serializable]
public class OverheadMessageAction : TextAction
{
    private bool historical;

    internal override void Execute(LevelingInfo levelingInfo)
    {
        var pawn = levelingInfo.Pawn;
        if (pawn.Map is null)
        {
            return;
        }
        var resolvedText = ResolveText(levelingInfo, Text);
        MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, resolvedText);

        if (historical)
        {
            var message = new Message(resolvedText, MessageTypeDefOf.SilentInput, pawn);
            Find.Archive.Add(message);
        }
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
