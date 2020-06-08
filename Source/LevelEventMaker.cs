using RimWorld;
using Verse;
using Verse.Sound;

namespace LevelUp
{
    public class LevelEventMaker
    {
        private readonly PawnSkillTimerCache pawnSkillTimerCache;
        private readonly Settings modSettings;
        private readonly string levelUpMessage;
        private readonly string levelDownMessage;
        private readonly SoundDef soundLevelUpDef = DefDatabase<SoundDef>.GetNamed("LevelUpDing");
        private readonly SoundDef soundLevelDownDef = DefDatabase<SoundDef>.GetNamed("LevelDownDrop");
        private readonly ThingDef moteLevelUpDef = DefDatabase<ThingDef>.GetNamed("Mote_LevelUp");
        private readonly ThingDef moteLevelDownDef = DefDatabase<ThingDef>.GetNamed("Mote_LevelDown");

        public LevelEventMaker(PawnSkillTimerCache pawnSkillTimerCache, Settings modSettings)
        {
            this.pawnSkillTimerCache = pawnSkillTimerCache;
            this.modSettings = modSettings;
            this.levelUpMessage = "Krafs.LevelUp.LevelUpMessage".TranslateSimple().Replace("{0}", "{0}".Colorize(ColoredText.NameColor));
            this.levelDownMessage = "Krafs.LevelUp.LevelDownMessage".TranslateSimple().Replace("{0}", "{0}".Colorize(ColoredText.NameColor));
        }

        public void OnLevelUp(SkillRecord skillRecord, Pawn pawn)
        {
            if (!this.modSettings.DoLevelUp)
            {
                return;
            }

            if (!pawn.IsFreeColonist)
            {
                return;
            }

            if (!this.pawnSkillTimerCache.EnoughTimeHasPassed(pawn, skillRecord.def))
            {
                return;
            }

            if (this.modSettings.DoLevelUpSound)
            {
                DoSound(soundLevelUpDef);
            }

            if (this.modSettings.DoLevelUpMessage)
            {
                DoMessage(this.levelUpMessage, pawn, skillRecord);
            }

            if (this.modSettings.DoLevelUpAnimation)
            {
                DoAnimation(pawn, this.moteLevelUpDef, 1.0f, 100f);
            }
        }

        public void OnLevelDown(SkillRecord skillRecord, Pawn pawn)
        {
            if (!this.modSettings.DoLevelDown)
            {
                return;
            }

            if (!pawn.IsFreeColonist)
            {
                return;
            }

            if (!this.pawnSkillTimerCache.EnoughTimeHasPassed(pawn, skillRecord.def))
            {
                return;
            }

            if (this.modSettings.DoLevelDownSound)
            {
                DoSound(soundLevelDownDef);
            }

            if (this.modSettings.DoLevelDownMessage)
            {
                DoMessage(this.levelDownMessage, pawn, skillRecord);
            }

            if (this.modSettings.DoLevelDownAnimation)
            {
                DoAnimation(pawn, this.moteLevelDownDef, 8.0f, -100f);
            }
        }

        private void DoMessage(string messageTemplate, Pawn pawn, SkillRecord skillRecord)
        {
            var text = string.Format(messageTemplate, pawn.LabelShortCap, skillRecord.Level.ToString(), skillRecord.def.LabelCap.Resolve());
            var message = new Message(text, MessageTypeDefOf.SilentInput, new LookTargets(pawn));
            Messages.Message(message, false);
        }

        private void DoSound(SoundDef soundDef)
        {
            soundDef.PlayOneShotOnCamera(null);
        }

        private void DoAnimation(Pawn pawn, ThingDef moteDef, float scale, float rotationRate)
        {
            if (pawn.Map == Find.CurrentMap)
            {
                var mote = ThingMaker.MakeThing(moteDef) as Mote;
                mote.Scale = scale;
                mote.rotationRate = rotationRate;
                var position = pawn.DrawPos;
                mote.Attach(pawn);
                mote.exactPosition = position;

                GenSpawn.Spawn(mote, position.ToIntVec3(), Find.CurrentMap);
            }
        }
    }
}