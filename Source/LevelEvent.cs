using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LevelUp
{
    class LevelEvent
    {
        public enum LevelEventType
        {
            LevelUp,
            LevelDown
        }

        public LevelEvent(Pawn pawn, SkillRecord skillRecord, LevelEventType levelType)
        {
            CurrentPawn = pawn;
            SkillRecord = skillRecord;
            LevelType = levelType;
        }

        Pawn CurrentPawn { get; set; }
        SkillRecord SkillRecord { get; set; }
        public LevelEventType LevelType { get; set; }
        public static Queue<LevelEvent> LevelEventQueue { get; } = new Queue<LevelEvent>();
        public static Dictionary<Pawn, Dictionary<SkillDef, int>> LevelRecord { get; } = new Dictionary<Pawn, Dictionary<SkillDef, int>>();

        public void NotifyLevelUp()
        {
            // Do text message on screen on level up.
            if (Settings.allowLevelUpTextMessage)
            {
                Color color = Color.yellow;
                string pawn = CurrentPawn.LabelShortCap.Bold();
                string skill = SkillRecord.def.skillLabel.Italic();
                string level = SkillRecord.levelInt.ToString().Bold().Colored(color);
                string label = "LevelUpLabel".Translate(pawn, level, skill);

                LookTargets lookTargets = new LookTargets(CurrentPawn);
                Messages.Message(label, lookTargets, MessageTypeDefOf.SilentInput);
            }

            if (Settings.allowLevelUpLetter)
            {
                string pawn = CurrentPawn.LabelShortCap;
                string skill = SkillRecord.def.skillLabel;
                string level = SkillRecord.levelInt.ToString();
                string labelShort = "LevelUpLabelShort".Translate(pawn);
                string label = "LevelUpLabel".Translate(pawn, level, skill);
                LookTargets lookTargets = new LookTargets(CurrentPawn);

                Find.LetterStack.ReceiveLetter(labelShort, label, LetterDefOf.PositiveEvent, lookTargets);
            }

            // Do sound effect on level up.
            if (Settings.allowLevelUpSoundEffect)
                DefHandler.LevelUp.PlayOneShot(SoundInfo.InMap(new TargetInfo(CurrentPawn)));

            // Do animation on pawn levelling up.
            if (!Settings.allowLevelUpAnimation)
                return;

            // Animation motes. Using three similar ones on top of each other to achieve stretch effect.
            Vector3 pawnPosition = CurrentPawn.Drawer.DrawPos;
            Map map = CurrentPawn.Map;

            if (!pawnPosition.ToIntVec3().ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
                return;

            MoteThrown innerMote = MakeMote(DefHandler.Mote_LevelUpBeamInner, CurrentPawn);
            innerMote.SetVelocity(0, 0.6f);
            GenSpawn.Spawn(innerMote, pawnPosition.ToIntVec3(), map);

            MoteThrown middleMote1 = MakeMote(DefHandler.Mote_LevelUpBeamMiddle, CurrentPawn);
            middleMote1.SetVelocity(0, 0.3f);
            GenSpawn.Spawn(middleMote1, pawnPosition.ToIntVec3(), map);

            MoteThrown middleMote2 = MakeMote(DefHandler.Mote_LevelUpBeamMiddle, CurrentPawn);
            middleMote2.SetVelocity(0, 0.5f);
            GenSpawn.Spawn(middleMote2, pawnPosition.ToIntVec3(), map);

            MoteThrown outerMote1 = MakeMote(DefHandler.Mote_LevelUpBeamOuter, CurrentPawn);
            outerMote1.SetVelocity(0, 0.15f);
            GenSpawn.Spawn(outerMote1, pawnPosition.ToIntVec3(), map);

            MoteThrown outerMote2 = MakeMote(DefHandler.Mote_LevelUpBeamOuter, CurrentPawn);
            outerMote2.SetVelocity(0, 0.25f);
            GenSpawn.Spawn(outerMote2, pawnPosition.ToIntVec3(), map);
        }

        public void NotifyLevelDown()
        {
            // Do text message on screen on level down.
            if (Settings.allowLevelDownTextMessage)
            {
                Color color = Color.red;
                string pawn = CurrentPawn.LabelShortCap.Bold();
                string skill = SkillRecord.def.skillLabel.Italic();
                string level = SkillRecord.levelInt.ToString().Bold().Colored(color);
                string label = "LevelDownLabel".Translate(pawn, level, skill);

                LookTargets lookTargets = new LookTargets(CurrentPawn);
                Messages.Message(label, lookTargets, MessageTypeDefOf.SilentInput);
            }

            if (Settings.allowLevelDownLetter)
            {
                string pawn = CurrentPawn.LabelShortCap;
                string skill = SkillRecord.def.skillLabel;
                string level = SkillRecord.levelInt.ToString();
                string labelShort = "LevelDownLabelShort".Translate(pawn);
                string label = "LevelDownLabel".Translate(pawn, level, skill);
                LookTargets lookTargets = new LookTargets(CurrentPawn);

                Find.LetterStack.ReceiveLetter(labelShort, label, LetterDefOf.NegativeEvent, lookTargets);
            }

            // Do sound effect on level down.
            if (Settings.allowLevelDownSoundEffect)
                DefHandler.LevelDown.PlayOneShot(SoundInfo.InMap(new TargetInfo(CurrentPawn)));

            // Do animation on pawn levelling down.
            if (!Settings.allowLevelDownAnimation)
                return;

            // Animation motes.
            Vector3 pawnPosition = CurrentPawn.Drawer.DrawPos;
            Map map = CurrentPawn.Map;

            if (!pawnPosition.ToIntVec3().ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
                return;

            MoteThrown topMote = (MoteThrown)ThingMaker.MakeThing(DefHandler.Mote_LevelDownBeamTop);
            topMote.Attach(new TargetInfo(CurrentPawn));
            topMote.Scale = 1.5f;
            topMote.exactPosition = CurrentPawn.Drawer.DrawPos;
            topMote.SetVelocity(180, 0.1f);
            GenSpawn.Spawn(topMote, pawnPosition.ToIntVec3(), map);

            MoteThrown bottomMote = (MoteThrown)ThingMaker.MakeThing(DefHandler.Mote_LevelDownBeamBottom);
            bottomMote.Attach(new TargetInfo(CurrentPawn));
            bottomMote.Scale = 1.5f;
            bottomMote.exactPosition = CurrentPawn.Drawer.DrawPos;
            bottomMote.SetVelocity(180, 0.1f);
            GenSpawn.Spawn(bottomMote, pawnPosition.ToIntVec3(), map);
        }

        // Make basic setup of level up motes.
        MoteThrown MakeMote(ThingDef def, Pawn pawn)
        {
            MoteThrown mote = (MoteThrown)ThingMaker.MakeThing(def);
            mote.Attach(new TargetInfo(pawn));
            mote.Scale = 1.5f;
            mote.exactPosition = pawn.Drawer.DrawPos;
            return mote;
        }
    }
}