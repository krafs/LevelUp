using RimWorld;
using UnityEngine;
using Verse;

namespace LevelUp;

public sealed class OverheadMessageAction : TextAction
{
    private bool historical;

    internal override void Execute(LevelingInfo levelingInfo)
    {
        Pawn pawn = levelingInfo.Pawn;
        if (pawn.Map is null)
        {
            return;
        }

        string resolvedText = ResolveText(levelingInfo, text);
        MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, resolvedText);

        if (historical)
        {
            Message message = new(resolvedText, MessageTypeDefOf.SilentInput, pawn);
            Find.Archive.Add(message);
        }
    }

    internal override void Draw(Rect rect)
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
