using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace LevelUp
{
    [Serializable]
    public class MoteAnimation : IAnimation
    {
        private static readonly List<ThingDef> allMotes;

        private ThingDef moteDef = null!;
        private Graphic_Single graphic = null!;
        private Texture2D texture = null!;
        private AnimationDefExtension animationExtension = null!;

        public static IEnumerable<IAnimation> Animations => allMotes.Select(x => new MoteAnimation(x));

        public ThingDef MoteDef
        {
            get => moteDef;
            set
            {
                moteDef = value;
                Prepare();
            }
        }

        static MoteAnimation()
        {
            allMotes = DefDatabase<ThingDef>
               .AllDefs.Where(x => x.HasModExtension<AnimationDefExtension>()).ToList();
        }

        public MoteAnimation()
        {
            moteDef = allMotes.RandomElement();
            Prepare();
        }

        private MoteAnimation(ThingDef def)
        {
            moteDef = def;
            Prepare();
        }

        public string Label => moteDef.LabelCap;

        public void Prepare()
        {
            animationExtension = moteDef.GetModExtension<AnimationDefExtension>();

            if (moteDef.graphic is Graphic_Single graphicSingle)
            {
                graphic = graphicSingle;
            }
            else
            {
                throw new InvalidOperationException("Null graphic on MoteDef.");
            }

            texture = ContentFinder<Texture2D>.Get(moteDef.graphicData.texPath);
        }

        public void Execute(LevelingInfo levelingInfo)
        {
            Pawn pawn = levelingInfo.Pawn;
            Map pawnMap = pawn.Map;

            if (pawnMap != Find.CurrentMap)
            {
                return;
            }

            if (ThingMaker.MakeThing(moteDef) is not MoteThrown mote)
            {
                throw new InvalidOperationException("mote is null");
            }

            mote.exactScale = animationExtension.ExactScale;
            mote.Scale = animationExtension.Scale;
            mote.exactRotation = animationExtension.Rotation;
            mote.rotationRate = animationExtension.RotationRate;
            mote.solidTimeOverride = animationExtension.SolidTimeOverride ?? -1f;
            mote.airTimeLeft = animationExtension.AirTimeLeft ?? 999999f;
            mote.SetVelocity(animationExtension.VelocityAngle, animationExtension.VelocitySpeed);
            if (animationExtension.Velocity.HasValue)
            {
                mote.Velocity = animationExtension.Velocity.Value;
            }
            mote.instanceColor = animationExtension.InstanceColor;
            Vector3 position = pawn.DrawPos;
            mote.exactPosition = position;
            mote.Attach(pawn);

            GenSpawn.Spawn(mote, position.ToIntVec3(), pawnMap);
        }

        public void DrawGraphic(Rect rect)
        {
            Widgets.DrawTextureFitted(
                outerRect: rect,
                tex: texture,
                scale: 1f,
                texProportions: new Vector2(texture.width, texture.height),
                texCoords: new Rect(0, 0, 1, 1),
                animationExtension.Rotation,
                graphic.MatSingle);
        }

        public void ExposeData()
        {
            Scribe_Defs.Look(ref moteDef, "moteDef");
        }
    }
}