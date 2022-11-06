﻿using System.Linq;
using UnityEngine;
using Verse;

namespace LevelUp.Debug;

public class DebugOverlay : GameComponent
{
    private Pawn? pawn;
    public DebugOverlay(Game _)
    {
    }

    public override void FinalizeInit()
    {
        pawn = Find.CurrentMap.mapPawns.FreeColonists.First();
    }

    public override void GameComponentOnGUI()
    {
        var rect = new Rect(0, 0, 150, 24);
        rect.y = rect.yMax;

        Widgets.Label(rect, pawn.Name.ToStringShort);

        rect.y = rect.yMax;
        if (Widgets.ButtonText(rect, "Add gene"))
        {
            var gene = DefDatabase<GeneDef>.AllDefsListForReading.First(x => x.defName == "AptitudeRemarkable_Construction");
            pawn.genes.AddGene(gene, false);
        }

        rect.y = rect.yMax;
        if (Widgets.ButtonText(rect, "Remove gene"))
        {
            var geneDef = DefDatabase<GeneDef>.AllDefsListForReading.First(x => x.defName == "AptitudeRemarkable_Construction");
            var gene = pawn.genes.GetGene(geneDef);
            pawn.genes.RemoveGene(gene);
        }
    }
}