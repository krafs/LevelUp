using System;
using Verse;

namespace LevelUp
{
    public static class ProfileInitializer
    {
        public static void InitializeProfile(Profile profile)
        {
            // Level up
            foreach (ActionDef actionDef in DefDatabase<ActionDef>.AllDefs)
            {
                if (!profile.LevelUpActionMaker.Actions.Any(x => x.ActionDef == actionDef))
                {
                    if (Activator.CreateInstance(actionDef.ActionClass) is not LevelingAction action)
                    {
                        throw new InvalidOperationException("action is null");
                    }
                    action.ActionDef = actionDef;
                    profile.LevelUpActionMaker.Actions.Add(action);

                    if (action is SoundAction soundAction)
                    {
                        soundAction.Active = true;
                        soundAction.SoundDef = DefDatabase<SoundDef>.GetNamed("LevelUpSound_Ding");
                    }
                    else if (action is MessageAction messageAction)
                    {
                        messageAction.Active = true;
                        messageAction.Text = I18n.DefaultLevelUpMessage;
                    }
                    else if (action is OverheadMessageAction overheadMessageAction)
                    {
                        overheadMessageAction.Text = I18n.DefaultLevelUpOverheadMessage;
                    }
                    else if (action is AnimationAction animationAction)
                    {
                        animationAction.Active = true;
#if v1_3
                        animationAction.Animation = new FleckAnimation
                        {
                            FleckDef = DefDatabase<FleckDef>.GetNamed("LevelUpAnimation_Radiance")
                        };
#else
                        animationAction.Animation = new MoteAnimation
                        {
                            MoteDef = DefDatabase<ThingDef>.GetNamed("LevelUpAnimation_Radiance")
                        };
#endif
                    }
                }
            }

            profile.LevelUpActionMaker.Actions.SortBy(x => x.ActionDef.LabelCap.RawText);

            // Level down
            foreach (ActionDef actionDef in DefDatabase<ActionDef>.AllDefs)
            {
                if (!profile.LevelDownActionMaker.Actions.Any(x => x.ActionDef == actionDef))
                {
                    if (Activator.CreateInstance(actionDef.ActionClass) is not LevelingAction action)
                    {
                        throw new InvalidOperationException("action is null");
                    }
                    action.ActionDef = actionDef;
                    profile.LevelDownActionMaker.Actions.Add(action);

                    if (action is SoundAction soundAction)
                    {
                        soundAction.SoundDef = DefDatabase<SoundDef>.GetNamed("LevelUpSound_Negative");
                    }
                    else if (action is MessageAction messageAction)
                    {
                        messageAction.Text = I18n.DefaultLevelDownMessage;
                    }
                    else if (action is OverheadMessageAction overheadMessageAction)
                    {
                        overheadMessageAction.Text = I18n.DefaultLevelDownOverheadMessage;
                    }
                    else if (action is AnimationAction animationAction)
                    {
#if v1_3
                        animationAction.Animation = new FleckAnimation
                        {
                            FleckDef = DefDatabase<FleckDef>.GetNamed("LevelUpAnimation_Drain")
                        };
#else
                        animationAction.Animation = new MoteAnimation
                        {
                            MoteDef = DefDatabase<ThingDef>.GetNamed("LevelUpAnimation_Drain")
                        };
#endif
                    }
                }
            }

            profile.LevelDownActionMaker.Actions.SortBy(x => x.ActionDef.LabelCap.RawText);
        }
    }
}