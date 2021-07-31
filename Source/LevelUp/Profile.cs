using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LevelUp
{
    [Serializable]
    public class Profile : IExposable
    {
        private ActionMaker levelUpActionMaker;
        private ActionMaker levelDownActionMaker;
        private GeneralSettingsContent generalSettingsContent;
        private IDrawer selected = Drawer.Empty;

        public ActionMaker LevelUpActionMaker => levelUpActionMaker;
        public ActionMaker LevelDownActionMaker => levelDownActionMaker;
        public GeneralSettingsContent GeneralSettingsContent => generalSettingsContent;

        public Profile()
        {
            levelUpActionMaker = new ActionMaker();
            levelDownActionMaker = new ActionMaker();
            generalSettingsContent = new GeneralSettingsContent();
        }

        public void Prepare()
        {
            levelUpActionMaker.Prepare();
            levelDownActionMaker.Prepare();
        }

        public void Draw(Rect rect)
        {
            Rect leftRect = new Rect(rect) { width = rect.width / 3 };
            Rect rightRect = new Rect(rect) { xMin = leftRect.xMax };

            Widgets.DrawLineVertical(leftRect.xMax, leftRect.y + 24f, rect.height - 24f);

            leftRect.xMax -= 10f;
            DoLeft(leftRect);

            rightRect.xMin += 10f;
            DoRight(rightRect);
        }

        private void DoLeft(Rect rect)
        {
            Rect generalRect = new Rect(rect) { height = 24f, xMax = rect.xMax - 15f };
            DoGeneralRect(generalRect);

            Widgets.DrawLineHorizontal(generalRect.x, generalRect.yMax + 5f, generalRect.width);
            Rect levelList = new Rect(rect) { yMin = generalRect.yMax + 10f };

            Rect levelUpListRect = new Rect(levelList) { height = levelList.height / 2 };
            DoList(levelUpListRect, LevelUpActionMaker, I18n.LevelUpActionsLabel, I18n.LevelUpActionHeaderDescription);

            Rect levelDownListRect = new Rect(levelList) { yMin = levelUpListRect.yMax };
            DoList(levelDownListRect, LevelDownActionMaker, I18n.LevelDownActionsLabel, I18n.LevelDownActionHeaderDescription);
        }

        private void DoList(Rect rect, ActionMaker actionMaker, string header, string headerTooltip)
        {
            Rect labelRect = new Rect(rect) { height = 24f, xMax = rect.xMax - 15f };
            Widgets.Label(labelRect, header.Bolded());
            TooltipHandler.TipRegion(labelRect, headerTooltip);
            Widgets.DrawHighlight(labelRect);

            rect.y = labelRect.yMax;
            rect.xMin += 10f;
            actionMaker.Draw(rect, ref selected);
        }

        private void DoGeneralRect(Rect rect)
        {
            string generalLabel = I18n.GeneralSettingsLabel;
            Widgets.Label(rect, generalLabel.Bolded());
            Widgets.DrawLightHighlight(rect);
            if (Widgets.ButtonInvisible(rect))
            {
                SoundDefOf.Click.PlayOneShotOnCamera();
                if (selected == GeneralSettingsContent)
                {
                    selected = Drawer.Empty;
                }
                else
                {
                    selected = GeneralSettingsContent;
                }
            }

            if (selected == GeneralSettingsContent)
            {
                Widgets.DrawHighlightSelected(rect);
            }
            else if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
            }
        }

        private void DoRight(Rect rect)
        {
            if (selected is LevelingAction selectedAction)
            {
                DoActionContent(rect, ref selectedAction);
            }
            else
            {
                selected.Draw(rect);
            }
        }

        private static void DoActionContent(Rect rect, ref LevelingAction selectedAction)
        {
            Rect rowRect = new Rect(rect) { height = 24f };
            Rect cooldownCheckbox = new Rect(rowRect) { xMin = rowRect.xMax - rowRect.height };
            string cooldownLabel = I18n.CooldownLabel;
            Vector2 cooldownLabelSize = Text.CalcSize(cooldownLabel);
            Rect cooldownLabelRect = new Rect(cooldownCheckbox.xMin - cooldownLabelSize.x, rowRect.y, cooldownLabelSize.x, rowRect.height);
            Widgets.Label(cooldownLabelRect, cooldownLabel);

            TooltipHandler.TipRegion(cooldownLabelRect, I18n.CooldownOnActionDescription);
            bool hasCooldown = selectedAction.Cooldown;
            Widgets.Checkbox(cooldownCheckbox.x, cooldownCheckbox.y, ref hasCooldown);
            selectedAction.Cooldown = hasCooldown;

            string actionLabel = selectedAction.ActionDef.label;
            Vector2 actionLabelSize = Text.CalcSize(actionLabel);
            actionLabelSize.x += 15f;
            Rect actionLabelRect = new Rect(rowRect) { width = actionLabelSize.x };
            Widgets.Label(actionLabelRect, selectedAction.ActionDef.label.Bolded());
            TooltipHandler.TipRegion(actionLabelRect, selectedAction.ActionDef.description);

            rect.y = rowRect.yMax;
            Widgets.DrawLineHorizontal(rect.x, rect.y, rect.width);

            rect.y += 10f;
            selectedAction.Draw(rect);
        }

        public void ExposeData()
        {
            Scribe_Deep.Look(ref levelUpActionMaker, "levelUpActionMaker");
            Scribe_Deep.Look(ref levelDownActionMaker, "levelDownActionMaker");
            Scribe_Deep.Look(ref generalSettingsContent, "generalSettingsContent");
        }
    }
}