using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace LevelUp
{
    [Serializable]
    public class OverheadMessageAction : TextAction
    {
        private bool historical;

        public override void Execute(LevelingInfo levelingInfo)
        {
            Pawn pawn = levelingInfo.Pawn;
            if (pawn.Map is null)
            {
                return;
            }
            string resolvedText = ResolveText(levelingInfo, Text);
            MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, resolvedText);

            if (historical)
            {
                Message message = new Message(resolvedText, MessageTypeDefOf.SilentInput, pawn);
                Find.Archive.Add(message);
            }
        }

        public override void Draw(Rect rect)
        {
            rect.yMin = DrawTextBuilder(rect).yMax;

            Rect rowRect = new Rect(rect) { height = 24f };
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
}