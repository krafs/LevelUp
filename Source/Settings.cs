using System;
using System.Collections.Generic;
using Verse;

namespace LevelUp;

public sealed class Settings : ModSettings
{
    internal static readonly Dictionary<int, int> timerCache = [];
    internal static Profile profile = new();
    private int settingsVersion;

    public Settings()
    {
        if (Scribe.mode == LoadSaveMode.Inactive)
        {
            InitializeProfile(profile);
        }
    }

    public override void ExposeData()
    {
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            settingsVersion = 1;
        }

        Scribe_Values.Look(ref settingsVersion, "settingsVersion", forceSave: true);
        if (Scribe.mode == LoadSaveMode.LoadingVars)
        {
            if (settingsVersion == 0)
            {
                Log.Warning("[Level Up!] Detected incompatible mod settings. Resetting to default settings.");
                profile = new();
            }
            else
            {
                Scribe_Deep.Look(ref profile, "profile");
            }

            InitializeProfile(profile);
            return;
        }


        Scribe_Deep.Look(ref profile, "profile");
    }

    private static void InitializeProfile(Profile profile)
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

                if (action is LevelingAction_Sound soundAction)
                {
                    soundAction.active = true;
                    soundAction.SoundDef = DefOfs.Ding;
                }
                else if (action is LevelingAction_Message messageAction)
                {
                    messageAction.active = true;
                    messageAction.text = I18n.DefaultLevelUpMessage;
                }
                else if (action is LevelingAction_OverheadMessage overheadMessageAction)
                {
                    overheadMessageAction.text = I18n.DefaultLevelUpOverheadMessage;
                }
                else if (action is LevelingAction_Animation animationAction)
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

                if (action is LevelingAction_Sound soundAction)
                {
                    soundAction.SoundDef = DefOfs.Negative;
                }
                else if (action is LevelingAction_Message messageAction)
                {
                    messageAction.text = I18n.DefaultLevelDownMessage;
                }
                else if (action is LevelingAction_OverheadMessage overheadMessageAction)
                {
                    overheadMessageAction.text = I18n.DefaultLevelDownOverheadMessage;
                }
                else if (action is LevelingAction_Animation animationAction)
                {
                    animationAction.FleckDef = DefOfs.Drain;
                }
            }
        }

        profile.levelDownActionMaker.actions.SortBy(x => x.actionDef.LabelCap.RawText);
    }
}
