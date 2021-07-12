using System.Globalization;
using RimWorld;
using Verse;
using Verse.Sound;

namespace LevelUp
{
    public class LevelEventMaker
    {
        private readonly PawnSkillTimerCache _pawnSkillTimerCache;
        private readonly Settings _modSettings;
        private readonly string _levelUpMessage;
        private readonly string _levelDownMessage;
        private readonly SoundDef _soundLevelUpDef = DefDatabase<SoundDef>.GetNamed("LevelUpDing");
        private readonly SoundDef _soundLevelDownDef = DefDatabase<SoundDef>.GetNamed("LevelDownDrop");
        private readonly ThingDef _moteLevelUpDef = DefDatabase<ThingDef>.GetNamed("Mote_LevelUp");
        private readonly ThingDef _moteLevelDownDef = DefDatabase<ThingDef>.GetNamed("Mote_LevelDown");

        public LevelEventMaker(PawnSkillTimerCache pawnSkillTimerCache, Settings modSettings)
        {
            _pawnSkillTimerCache = pawnSkillTimerCache;
            _modSettings = modSettings;
            _levelUpMessage = "Krafs.LevelUp.LevelUpMessage".TranslateSimple().Replace("{0}", "{0}".Colorize(ColoredText.NameColor));
            _levelDownMessage = "Krafs.LevelUp.LevelDownMessage".TranslateSimple().Replace("{0}", "{0}".Colorize(ColoredText.NameColor));
        }

        public void OnLevelUp(SkillRecord skillRecord, Pawn pawn)
        {
            if (!_modSettings.DoLevelUp)
            {
                return;
            }

            if (!pawn.IsFreeColonist)
            {
                return;
            }

            if (!_pawnSkillTimerCache.EnoughTimeHasPassed(pawn, skillRecord.def))
            {
                return;
            }

            if (_modSettings.DoLevelUpSound)
            {
                DoSound(_soundLevelUpDef);
            }

            if (_modSettings.DoLevelUpMessage)
            {
                DoMessage(_levelUpMessage, pawn, skillRecord);
            }

            if (_modSettings.DoLevelUpAnimation)
            {
                DoAnimation(pawn, _moteLevelUpDef, 1.0f, 100f);
            }
        }

        public void OnLevelDown(SkillRecord skillRecord, Pawn pawn)
        {
            if (!_modSettings.DoLevelDown)
            {
                return;
            }

            if (!pawn.IsFreeColonist)
            {
                return;
            }

            if (!_pawnSkillTimerCache.EnoughTimeHasPassed(pawn, skillRecord.def))
            {
                return;
            }

            if (_modSettings.DoLevelDownSound)
            {
                DoSound(_soundLevelDownDef);
            }

            if (_modSettings.DoLevelDownMessage)
            {
                DoMessage(_levelDownMessage, pawn, skillRecord);
            }

            if (_modSettings.DoLevelDownAnimation)
            {
                DoAnimation(pawn, _moteLevelDownDef, 8.0f, -100f);
            }
        }

        private static void DoMessage(string messageTemplate, Pawn pawn, SkillRecord skillRecord)
        {
            string text = string.Format(CultureInfo.InvariantCulture, messageTemplate, pawn.LabelShortCap, skillRecord.Level, skillRecord.def.LabelCap.Resolve());
            var message = new Message(text, MessageTypeDefOf.SilentInput, new LookTargets(pawn));
            Messages.Message(message, false);
        }

        private static void DoSound(SoundDef soundDef)
        {
            soundDef.PlayOneShotOnCamera(null);
        }

        private static void DoAnimation(Pawn pawn, ThingDef moteDef, float scale, float rotationRate)
        {
            if (pawn.Map == Find.CurrentMap)
            {
                var mote = ThingMaker.MakeThing(moteDef) as Mote;
                mote.Scale = scale;
                mote.rotationRate = rotationRate;
                UnityEngine.Vector3 position = pawn.DrawPos;
                mote.Attach(pawn);
                mote.exactPosition = position;

                GenSpawn.Spawn(mote, position.ToIntVec3(), Find.CurrentMap);
            }
        }
    }
}