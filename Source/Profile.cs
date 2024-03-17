using UnityEngine;
using Verse;

namespace LevelUp;

public sealed class Profile : IExposable
{
    internal ActionMaker levelUpActionMaker = new();
    internal ActionMaker levelDownActionMaker = new();
    private LevelingAction? selected;

    internal void Draw(Rect rect)
    {
        Rect leftRect = new(rect) { width = rect.width / 3 };
        Rect rightRect = new(rect) { xMin = leftRect.xMax };

        Widgets.DrawLineVertical(leftRect.xMax, leftRect.y + 24f, rect.height - 24f);

        leftRect.xMax -= 10f;
        DoLeft(leftRect);

        rightRect.xMin += 10f;
        DoRight(rightRect);
    }

    private void DoLeft(Rect rect)
    {
        Rect levelUpListRect = new(rect) { height = rect.height / 2 };
        DoList(levelUpListRect, levelUpActionMaker, I18n.LevelUpActionsLabel, I18n.LevelUpActionHeaderDescription);

        Rect levelDownListRect = new(rect) { yMin = levelUpListRect.yMax };
        DoList(levelDownListRect, levelDownActionMaker, I18n.LevelDownActionsLabel, I18n.LevelDownActionHeaderDescription);
    }

    private void DoList(Rect rect, ActionMaker actionMaker, string header, string headerTooltip)
    {
        Rect labelRect = new(rect) { height = 24f, xMax = rect.xMax - 15f };
        Widgets.Label(labelRect, $"<b>{header}</b>");
        TooltipHandler.TipRegion(labelRect, headerTooltip);
        Widgets.DrawHighlight(labelRect);

        rect.y = labelRect.yMax;
        rect.xMin += 10f;
        actionMaker.Draw(rect, ref selected);
    }

    private void DoRight(Rect rect)
    {
        if (selected is not null)
        {
            DoActionContent(rect, ref selected);
        }
    }

    private static void DoActionContent(Rect rect, ref LevelingAction selectedAction)
    {
        Rect rowRect = new(rect) { height = 24f };

        string actionLabel = selectedAction.actionDef.label;
        Vector2 actionLabelSize = Text.CalcSize(actionLabel);
        actionLabelSize.x += 15f;
        Rect actionLabelRect = new(rowRect) { width = actionLabelSize.x };
        Widgets.Label(actionLabelRect, $"<b>{selectedAction.actionDef.label}</b>");
        TooltipHandler.TipRegion(actionLabelRect, selectedAction.actionDef.description);

        rect.y = rowRect.yMax;
        Widgets.DrawLineHorizontal(rect.x, rect.y, rect.width);

        rect.y += 10f;
        selectedAction.Draw(rect);
    }

    public void ExposeData()
    {
        Scribe_Deep.Look(ref levelUpActionMaker, "levelUpActionMaker");
        Scribe_Deep.Look(ref levelDownActionMaker, "levelDownActionMaker");
    }
}
