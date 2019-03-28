using Harmony;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LevelUp
{
    class LevelUp
    {
        LevelUp(Pawn pawn, SkillRecord skillRecord)
        {
            Pawn = pawn;
            SkillRecord = skillRecord;
        }

        Pawn Pawn { get; set; }
        SkillRecord SkillRecord { get; set; }
        static List<LevelUp> AllLevelUps { get; } = new List<LevelUp>();

        // Puts message on screen for this level up.
        void Notify_LevelUp()
        {
            Color color = Color.white;
            string pawn = Pawn.Label.Colored(color);
            string skill = SkillRecord.def.skillLabel.Italic();
            string level = SkillRecord.levelInt.ToString().Bold();
            string label = "LevelUpLabel".Translate(pawn, level, skill);

            LookTargets lookTargets = new LookTargets(Pawn);
            Messages.Message(label, lookTargets, MessageTypeDefOf.SilentInput);

            SoundDef sound = DefHandler.LevelUp;
            sound.PlayOneShot(SoundInfo.InMap(new TargetInfo(Pawn)));

            // Mote
            Vector3 loc = Pawn.Drawer.DrawPos;
            Map map = Pawn.Map;
            if (!loc.ToIntVec3().ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
                return;

            Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Heart, null);

            mote.Scale = 1.5f;
            mote.exactPosition = loc;
            GenSpawn.Spawn(mote, loc.ToIntVec3(), map);
        }

        // Checks for new level ups for drawing on screen.
        internal static void LevelUpOnGUI()
        {
            if (AllLevelUps.NullOrEmpty())
                return;

            LevelUp levelUp = AllLevelUps.First();
            levelUp.Notify_LevelUp();
            AllLevelUps.Remove(levelUp);
        }

        internal static void Clear()
        {
            AllLevelUps.Clear();
        }

        // Harmony.
        // Patching two methods with the same pre- and postfixes, as both can make a skill increase in level.
        internal static void ApplyPatches(HarmonyInstance harmony)
        {
            var original1 = AccessTools.Method(typeof(SkillRecord), "Learn", new[] { typeof(float), typeof(bool) });
            var original2 = AccessTools.Method(typeof(SkillRecord), "set_Level");
            var prefix = new HarmonyMethod(AccessTools.Method(typeof(LevelUp), "Prefix"));
            var postfix = new HarmonyMethod(AccessTools.Method(typeof(LevelUp), "Postfix"));

            harmony.Patch(original1, prefix, postfix);
            harmony.Patch(original2, prefix, postfix);
        }

        // Prefix passes the value of skill level before level increase.
        static void Prefix(out int __state, int ___levelInt)
        {
            __state = ___levelInt;
        }

        // If original method increased skill level, add it to queue for level up notifications.
        static void Postfix(int __state, int ___levelInt, Pawn ___pawn, SkillRecord __instance)
        {
            if (__state < ___levelInt)
                AllLevelUps.Add(new LevelUp(___pawn, __instance));
        }
    }
}