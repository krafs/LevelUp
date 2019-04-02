using Harmony;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LevelUp
{
    class LevelUp
    {
        LevelUp(Pawn pawn, SkillRecord skillRecord)
        {
            CurrentPawn = pawn;
            SkillRecord = skillRecord;
        }

        Pawn CurrentPawn { get; set; }
        SkillRecord SkillRecord { get; set; }
        static Queue<LevelUp> LevelUpQueue { get; } = new Queue<LevelUp>();
        static Dictionary<Pawn, Dictionary<SkillDef, int>> PastLevelUps { get; } = new Dictionary<Pawn, Dictionary<SkillDef, int>>();

        // Puts message on screen for this level up.
        void Notify_LevelUp()
        {
            Color color = Color.yellow;
            string pawn = CurrentPawn.LabelShortCap.Bold();
            string skill = SkillRecord.def.skillLabel.Italic();
            string level = SkillRecord.levelInt.ToString().Bold().Colored(color);
            string label = "LevelUpLabel".Translate(pawn, level, skill);

            LookTargets lookTargets = new LookTargets(CurrentPawn);
            Messages.Message(label, lookTargets, MessageTypeDefOf.SilentInput);

            SoundDef sound = DefHandler.LevelUp;
            sound.PlayOneShot(SoundInfo.InMap(new TargetInfo(CurrentPawn)));

            // Motes. Using three similar ones on top of each other.
            Vector3 pawnPosition = CurrentPawn.Drawer.DrawPos;
            Map map = CurrentPawn.Map;

            if (!pawnPosition.ToIntVec3().ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
                return;

            MoteThrown innerMote = MakeMote(DefHandler.Mote_LevelBeamInner, CurrentPawn);
            innerMote.SetVelocity(0, 1f);
            GenSpawn.Spawn(innerMote, pawnPosition.ToIntVec3(), map);

            MoteThrown middleMote = MakeMote(DefHandler.Mote_LevelBeamMiddle, CurrentPawn);
            middleMote.SetVelocity(0, 0.5f);
            GenSpawn.Spawn(middleMote, pawnPosition.ToIntVec3(), map);

            MoteThrown outerMote = MakeMote(DefHandler.Mote_LevelBeamOuter, CurrentPawn);
            outerMote.SetVelocity(0, 0.1f);
            GenSpawn.Spawn(outerMote, pawnPosition.ToIntVec3(), map);
        }

        // Make basic setup of motes the same.
        MoteThrown MakeMote(ThingDef def, Pawn pawn)
        {
            MoteThrown mote = (MoteThrown)ThingMaker.MakeThing(def);
            mote.Attach(new TargetInfo(pawn));
            mote.Scale = 1.5f;
            mote.exactPosition = pawn.Drawer.DrawPos;
            return mote;
        }

        // Checks for new level ups for drawing on screen.
        internal static void LevelUpOnGUI()
        {
            if (LevelUpQueue.Count > 0)
                LevelUpQueue.Dequeue().Notify_LevelUp();
        }

        // Harmony.
        internal static void ApplyPatches(HarmonyInstance harmony)
        {
            MethodInfo original = AccessTools.Method(typeof(SkillRecord), "Learn", new[] { typeof(float), typeof(bool) });
            HarmonyMethod prefix = new HarmonyMethod(AccessTools.Method(typeof(LevelUp), "Prefix"));
            HarmonyMethod postfix = new HarmonyMethod(AccessTools.Method(typeof(LevelUp), "Postfix"));
            harmony.Patch(original, prefix, postfix);
        }

        // Prefix passes the value of skill level before level increase.
        static void Prefix(out int __state, int ___levelInt)
        {
            __state = ___levelInt;
        }

        // If original method increased skill level, add it to queue for level up notifications.
        static void Postfix(int __state, int ___levelInt, Pawn ___pawn, SkillRecord __instance)
        {
            if (___pawn.Faction == null)
                return;

            if (!___pawn.Faction.IsPlayer)
                return;

            if (__state >= ___levelInt)
                return;

            if (!PastLevelUps.ContainsKey(___pawn))
                PastLevelUps.Add(___pawn, new Dictionary<SkillDef, int>());

            if (PastLevelUps[___pawn].ContainsKey(__instance.def))
                if (PastLevelUps[___pawn][__instance.def] >= ___levelInt)
                    return;

            PastLevelUps[___pawn].Add(__instance.def, ___levelInt);
            LevelUpQueue.Enqueue(new LevelUp(___pawn, __instance));
        }
    }
}