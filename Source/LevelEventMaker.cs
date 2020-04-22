using RimWorld;
using Verse;

namespace LevelUp
{
    public class LevelEventMaker
    {
        private readonly PawnSkillTimerCache pawnSkillTimerCache;
        private string levelDownMessage;
        private string levelUpMessage;
        private MessageTypeDef messageLevelDownDef;

        private MessageTypeDef messageLevelUpDef;

        private ThingDef moteLevelDownDef;

        private ThingDef moteLevelUpDef;

        public LevelEventMaker(PawnSkillTimerCache pawnSkillTimerCache)
        {
            this.pawnSkillTimerCache = pawnSkillTimerCache;
        }

        private string LevelDownMessage => levelDownMessage ??= PrepareLevelMessage("Krafs.LevelUp.LevelDownMessage");
        private string LevelUpMessage => levelUpMessage ??= PrepareLevelMessage("Krafs.LevelUp.LevelUpMessage");
        private MessageTypeDef MessageLevelDownDef => messageLevelDownDef ??= DefDatabase<MessageTypeDef>.GetNamed("MessageLevelDown");
        private MessageTypeDef MessageLevelUpDef => messageLevelUpDef ??= DefDatabase<MessageTypeDef>.GetNamed("MessageLevelUp");
        private ThingDef MoteLevelDownDef => moteLevelDownDef ??= DefDatabase<ThingDef>.GetNamed("Mote_LevelDown");
        private ThingDef MoteLevelUpDef => moteLevelUpDef ??= DefDatabase<ThingDef>.GetNamed("Mote_LevelUp");

        public void OnLevelChange(string baseMessage, MessageTypeDef messageType, SkillRecord skillRecord, Pawn pawn, int level, ThingDef moteDef, float scale, float rotationRate)
        {
            if (pawn.IsFreeColonist && this.pawnSkillTimerCache.EnoughTimeHasPassed(pawn, skillRecord.def))
            {
                var text = string.Format(baseMessage, pawn.LabelShortCap, level, skillRecord.def.LabelCap);
                var message = new Message(text, messageType, new LookTargets(pawn));
                Messages.Message(message, false);

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

        public void OnLevelDown(SkillRecord skillRecord, Pawn pawn, int level)
        {
            OnLevelChange(LevelDownMessage, MessageLevelDownDef, skillRecord, pawn, level, MoteLevelDownDef, 8.0f, -100f);
        }

        public void OnLevelUp(SkillRecord skillRecord, Pawn pawn, int level)
        {
            OnLevelChange(LevelUpMessage, MessageLevelUpDef, skillRecord, pawn, level, MoteLevelUpDef, 1.0f, 100f);
        }

        private string PrepareLevelMessage(string key)
        {
            return key.TranslateSimple()
            .Replace("{0}", "{0}".Colorize(ColoredText.NameColor));
        }
    }
}
