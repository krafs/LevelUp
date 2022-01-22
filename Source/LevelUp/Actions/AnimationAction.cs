﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace LevelUp
{
    [Serializable]
    public class AnimationAction : LevelingAction
    {
        private static readonly List<IAnimation> animations;
        private IAnimation animation = null!;

        public static List<IAnimation> Animations => animations;

        public IAnimation Animation
        {
            get => animation;
            set
            {
                animation = value;
                Prepare();
            }
        }

        static AnimationAction()
        {
            animations = new List<IAnimation>();
            animations.AddRange(MoteAnimation.Animations);

#if v1_3
            animations.AddRange(FleckAnimation.Animations);
#endif
        }

        public AnimationAction()
        {
            animation = animations.RandomElement();
            Prepare();
        }

        public override void Prepare()
        {
            animation.Prepare();
        }

        public override void Execute(LevelingInfo levelingInfo)
        {
            animation.Execute(levelingInfo);
        }

        public override void Draw(Rect rect)
        {
            Rect rowRect = new Rect(rect) { height = 24f };
            Rect buttonRect = new Rect(rowRect) { width = rowRect.width / 2 };
            if (Widgets.ButtonText(buttonRect, animation.Label))
            {
                List<FloatMenuOption> options = animations
                    .Select(x => new FloatMenuOption(x.Label, () => Select(x)))
                    .ToList();

                Find.WindowStack.Add(new FloatMenu(options));
            }

            Rect imageRect = new Rect(rect.x, buttonRect.yMax + 10f, rect.width / 2, rect.width / 2);
            Widgets.DrawMenuSection(imageRect);
            animation.DrawGraphic(imageRect);
        }

        private void Select(IAnimation newAnimation)
        {
            animation = newAnimation;
            Prepare();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref animation, "animation");
        }
    }
}