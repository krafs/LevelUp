using System;
using Verse;

namespace LevelUp;

internal static class ProfileInitializer
{
    internal static void InitializeProfile(Profile profile)
    {
        // Level up
        foreach (ActionDef actionDef in DefDatabase<ActionDef>.AllDefs)
        {
            if (!profile.levelUpActionMaker.actions.Any(x => x.actionDef == actionDef))
            {
                if (Activator.CreateInstance(actionDef.actionClass) is not LevelingAction action)
                {
                    throw new InvalidOperationException("action is null");
                }
                action.actionDef = actionDef;
                profile.levelUpActionMaker.actions.Add(action);

                if (action is SoundAction soundAction)
                {
                    soundAction.active = true;
                    soundAction.SoundDef = DefOfs.Ding;
                }
                else if (action is MessageAction messageAction)
                {
                    messageAction.active = true;
                    messageAction.text = I18n.DefaultLevelUpMessage;
                }
                else if (action is OverheadMessageAction overheadMessageAction)
                {
                    overheadMessageAction.text = I18n.DefaultLevelUpOverheadMessage;
                }
                else if (action is AnimationAction animationAction)
                {
                    animationAction.active = true;
                    animationAction.FleckDef = DefOfs.Radiance;
                }
            }
        }

        profile.levelUpActionMaker.actions.SortBy(x => x.actionDef.LabelCap.RawText);

        // Level down
        foreach (ActionDef actionDef in DefDatabase<ActionDef>.AllDefs)
        {
            if (!profile.levelDownActionMaker.actions.Any(x => x.actionDef == actionDef))
            {
                if (Activator.CreateInstance(actionDef.actionClass) is not LevelingAction action)
                {
                    throw new InvalidOperationException("action is null");
                }
                action.actionDef = actionDef;
                profile.levelDownActionMaker.actions.Add(action);

                if (action is SoundAction soundAction)
                {
                    soundAction.SoundDef = DefOfs.Negative;
                }
                else if (action is MessageAction messageAction)
                {
                    messageAction.text = I18n.DefaultLevelDownMessage;
                }
                else if (action is OverheadMessageAction overheadMessageAction)
                {
                    overheadMessageAction.text = I18n.DefaultLevelDownOverheadMessage;
                }
                else if (action is AnimationAction animationAction)
                {
                    animationAction.FleckDef = DefOfs.Drain;
                }
            }
        }

        profile.levelDownActionMaker.actions.SortBy(x => x.actionDef.LabelCap.RawText);
    }
}
