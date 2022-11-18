using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace LevelUp;

public class AnimationAction : LevelingAction
{
    private FleckDef fleckDef;
    private Graphic_Single graphic = null!;
    private Texture2D texture = null!;
    private FleckDefExtension defExtension = null!;

    internal FleckDef FleckDef
    {
        get => fleckDef;
        set
        {
            fleckDef = value;
            Prepare();
        }
    }

    public AnimationAction()
    {
        fleckDef = DefDatabase<FleckDef>.AllDefs
                .Where(x => x.HasModExtension<FleckDefExtension>())
                .RandomElement();
        Prepare();
    }

    internal override void Execute(LevelingInfo levelingInfo)
    {
        Pawn pawn = levelingInfo.Pawn;
        Map map = pawn.Map;
        if (map != Find.CurrentMap)
        {
            return;
        }

        FleckCreationData fleckData = FleckMaker.GetDataStatic(pawn.DrawPos, map, fleckDef);
        fleckData.exactScale = defExtension.exactScale;
        fleckData.scale = defExtension.scale;
        fleckData.rotation = defExtension.rotation;
        fleckData.rotationRate = defExtension.rotationRate;
        fleckData.solidTimeOverride = defExtension.solidTimeOverride;
        fleckData.airTimeLeft = defExtension.airTimeLeft;
        fleckData.targetSize = defExtension.targetSize;
        fleckData.velocity = defExtension.velocity;
        fleckData.velocityAngle = defExtension.velocityAngle;
        fleckData.velocitySpeed = defExtension.velocitySpeed;
        fleckData.instanceColor = defExtension.instanceColor;
        fleckData.link = new FleckAttachLink(pawn);

        map.flecks.CreateFleck(fleckData);
    }

    internal override void Prepare()
    {
        defExtension = fleckDef.GetModExtension<FleckDefExtension>();

        graphic = fleckDef.graphicData.Graphic is Graphic_Single graphicSingle
            ? graphicSingle
            : throw new InvalidOperationException("Null graphic on FleckDef.");

        texture = ContentFinder<Texture2D>.Get(fleckDef.graphicData.texPath);
    }

    public override void Draw(Rect rect)
    {
        Rect rowRect = new Rect(rect) { height = 24f };
        Rect buttonRect = new Rect(rowRect) { width = rowRect.width / 2 };
        if (Widgets.ButtonText(buttonRect, fleckDef.LabelCap))
        {
            List<FloatMenuOption> options = DefDatabase<FleckDef>.AllDefs
                .Where(x => x.HasModExtension<FleckDefExtension>())
                .Select(x => new FloatMenuOption(fleckDef.LabelCap, () => this.fleckDef = x))
                .ToList();

            Find.WindowStack.Add(new FloatMenu(options));
        }

        Rect imageRect = new Rect(rect.x, buttonRect.yMax + 10f, rect.width / 2, rect.width / 2);
        Widgets.DrawMenuSection(imageRect);
        DrawGraphic(imageRect);
    }

    private void DrawGraphic(Rect rect)
    {
        Widgets.DrawTextureFitted(
            outerRect: rect,
            tex: texture,
            scale: 1f,
            texProportions: new Vector2(texture.width, texture.height),
            texCoords: new Rect(0, 0, 1, 1),
            defExtension.rotation,
            graphic.MatSingle);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Defs.Look(ref fleckDef, "fleckDef");
    }
}
