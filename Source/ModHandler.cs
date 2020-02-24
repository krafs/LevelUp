using UnityEngine;
using Verse;

namespace LevelUp
{
    public class ModHandler : Mod
    {
        public ModHandler(ModContentPack content) : base(content)
        { }

        private Settings Settings => this.GetSettings<Settings>();

        public override void DoSettingsWindowContents(Rect rect)
        {
            var doLevelUp = this.Settings.DoLevelUp;
            var doLevelDown = this.Settings.DoLevelDown;

            var list = new Listing_Standard
            {
                ColumnWidth = 120f
            };
            list.Begin(rect);
            list.CheckboxLabeled("Krafs.LevelUp.LevelUpLabel".Translate(), ref this.Settings.DoLevelUp);
            list.CheckboxLabeled("Krafs.LevelUp.LevelDownLabel".Translate(), ref this.Settings.DoLevelDown);
            list.End();

            if (doLevelUp != this.Settings.DoLevelUp || doLevelDown != this.Settings.DoLevelDown)
            {
                SkillRecordLearnPatch.UpdatePatch();
            }
        }

        public override string SettingsCategory() => "LevelUp";
    }
}