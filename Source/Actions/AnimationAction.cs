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
    private AnimationDefExtension defExtension = null!;

    public FleckDef FleckDef
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
                .Where(x => x.HasModExtension<AnimationDefExtension>())
                .RandomElement();
        Prepare();
    }

    public override void Execute(LevelingInfo levelingInfo)
    {
        Pawn pawn = levelingInfo.Pawn;
        Map map = pawn.Map;
        if (map != Find.CurrentMap)
        {
            return;
        }

        FleckCreationData fleckData = FleckMaker.GetDataStatic(pawn.DrawPos, map, fleckDef);
        fleckData.exactScale = defExtension.ExactScale;
        fleckData.scale = defExtension.Scale;
        fleckData.rotation = defExtension.Rotation;
        fleckData.rotationRate = defExtension.RotationRate;
        fleckData.solidTimeOverride = defExtension.SolidTimeOverride;
        fleckData.airTimeLeft = defExtension.AirTimeLeft;
        fleckData.targetSize = defExtension.TargetSize;
        fleckData.velocity = defExtension.Velocity;
        fleckData.velocityAngle = defExtension.VelocityAngle;
        fleckData.velocitySpeed = defExtension.VelocitySpeed;
        fleckData.instanceColor = defExtension.InstanceColor;
        fleckData.link = new FleckAttachLink(pawn);

        map.flecks.CreateFleck(fleckData);
    }

    public override void Prepare()
    {
        defExtension = fleckDef.GetModExtension<AnimationDefExtension>();

        graphic = fleckDef.graphicData.Graphic is Graphic_Single graphicSingle
            ? graphicSingle
            : throw new InvalidOperationException("Null graphic on FleckDef.");

        texture = ContentFinder<Texture2D>.Get(fleckDef.graphicData.texPath);
    }

    public override void Draw(Rect rect)
    {
        Rect rowRect = new Rect(rect) { height = 24f };
        Rect buttonRect = new Rect(rowRect) { width = rowRect.width / 2 };
        if (CustomWidgets.ButtonText(buttonRect, fleckDef.LabelCap))
        {
            List<FloatMenuOption> options = DefDatabase<FleckDef>.AllDefs
                .Where(x => x.HasModExtension<AnimationDefExtension>())
                .Select(x => new FloatMenuOption(fleckDef.LabelCap, () => this.fleckDef = x))
                .ToList();

            Find.WindowStack.Add(new FloatMenu(options));
        }

        Rect imageRect = new Rect(rect.x, buttonRect.yMax + 10f, rect.width / 2, rect.width / 2);
        Widgets.DrawMenuSection(imageRect);
        DrawGraphic(imageRect);
    }

    public void DrawGraphic(Rect rect)
    {
        Widgets.DrawTextureFitted(
            outerRect: rect,
            tex: texture,
            scale: 1f,
            texProportions: new Vector2(texture.width, texture.height),
            texCoords: new Rect(0, 0, 1, 1),
            defExtension.Rotation,
            graphic.MatSingle);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Defs.Look(ref fleckDef, "fleckDef");
    }
}
