using System;
using RimWorld;
using Verse;

namespace LevelUp
{
    [DefOf]
    public static class DefHandler
    {
        static DefHandler()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DefHandler));
        }

        public static SoundDef LevelUp;
    }
}