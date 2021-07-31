using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace LevelUp
{
    public class DebugButton : GameComponent
    {
        public DebugButton(Game _)
        { }

        public override void GameComponentOnGUI()
        {
            if (Find.CurrentMap is not Map map)
            {
                return;
            }

            Rect rowRect = new Rect(0, 40f, 120f, 24f);
            Widgets.Label(rowRect, "Level Up Testing Mode".Bolded());

            Pawn pawn = map
                    .mapPawns
                    .FreeColonists
                    .First();

            rowRect.y = rowRect.yMax;
            if (Widgets.ButtonText(rowRect, $"Level up {pawn.LabelShortCap}"))
            {
                SkillRecord skillRecord = pawn
                    .skills
                    .skills
                    .First(x => !x.TotallyDisabled && x.levelInt < SkillRecord.MaxLevel);

                skillRecord.Learn(skillRecord.XpRequiredForLevelUp + 500f, direct: true);
            }

            rowRect.y = rowRect.yMax;
            if (Widgets.ButtonText(rowRect, $"Level down {pawn.LabelShortCap}"))
            {
                SkillRecord skillRecord = pawn
                    .skills
                    .skills
                    .First(x => !x.TotallyDisabled && x.levelInt >= SkillRecord.MinLevel);

                skillRecord.Learn(-skillRecord.xpSinceLastLevel - 1100f, direct: true);
            }
        }
    }
}