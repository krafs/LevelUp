using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LevelUp;

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
        var leftRect = new Rect(rect) { width = rect.width / 3 };
        var rightRect = new Rect(rect) { xMin = leftRect.xMax };

        Widgets.DrawLineVertical(leftRect.xMax, leftRect.y + 24f, rect.height - 24f);

        leftRect.xMax -= 10f;
        DoLeft(leftRect);

        rightRect.xMin += 10f;
        DoRight(rightRect);
    }

    private void DoLeft(Rect rect)
    {
        var generalRect = new Rect(rect) { height = 24f, xMax = rect.xMax - 15f };
        DoGeneralRect(generalRect);

        Widgets.DrawLineHorizontal(generalRect.x, generalRect.yMax + 5f, generalRect.width);
        var levelList = new Rect(rect) { yMin = generalRect.yMax + 10f };

        var levelUpListRect = new Rect(levelList) { height = levelList.height / 2 };
        DoList(levelUpListRect, LevelUpActionMaker, I18n.LevelUpActionsLabel, I18n.LevelUpActionHeaderDescription);

        var levelDownListRect = new Rect(levelList) { yMin = levelUpListRect.yMax };
        DoList(levelDownListRect, LevelDownActionMaker, I18n.LevelDownActionsLabel, I18n.LevelDownActionHeaderDescription);
    }

    private void DoList(Rect rect, ActionMaker actionMaker, string header, string headerTooltip)
    {
        var labelRect = new Rect(rect) { height = 24f, xMax = rect.xMax - 15f };
        Widgets.Label(labelRect, header.Bold());
        TooltipHandler.TipRegion(labelRect, headerTooltip);
        Widgets.DrawHighlight(labelRect);

        rect.y = labelRect.yMax;
        rect.xMin += 10f;
        actionMaker.Draw(rect, ref selected);
    }

    private void DoGeneralRect(Rect rect)
    {
        var generalLabel = I18n.GeneralSettingsLabel;
        Widgets.Label(rect, generalLabel.Bold());
        Widgets.DrawLightHighlight(rect);
        if (Widgets.ButtonInvisible(rect))
        {
            SoundDefOf.Click.PlayOneShotOnCamera();
            selected = selected == GeneralSettingsContent ? Drawer.Empty : GeneralSettingsContent;
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
        var rowRect = new Rect(rect) { height = 24f };
        var cooldownCheckbox = new Rect(rowRect) { xMin = rowRect.xMax - rowRect.height };
        var cooldownLabel = I18n.CooldownLabel;
        var cooldownLabelSize = Text.CalcSize(cooldownLabel);
        var cooldownLabelRect = new Rect(cooldownCheckbox.xMin - cooldownLabelSize.x, rowRect.y, cooldownLabelSize.x, rowRect.height);
        Widgets.Label(cooldownLabelRect, cooldownLabel);

        TooltipHandler.TipRegion(cooldownLabelRect, I18n.CooldownOnActionDescription);
        var hasCooldown = selectedAction.Cooldown;
        Widgets.Checkbox(cooldownCheckbox.x, cooldownCheckbox.y, ref hasCooldown);
        selectedAction.Cooldown = hasCooldown;

        var actionLabel = selectedAction.ActionDef.label;
        var actionLabelSize = Text.CalcSize(actionLabel);
        actionLabelSize.x += 15f;
        var actionLabelRect = new Rect(rowRect) { width = actionLabelSize.x };
        Widgets.Label(actionLabelRect, selectedAction.ActionDef.label.Bold());
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
