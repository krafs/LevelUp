using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LevelUp;

public sealed class ActionMaker : IExposable
{
    internal List<LevelingAction> actions = [];
    private Vector2 scrollPosition;

    internal void ExecuteActions(LevelingInfo levelingInfo)
    {
        foreach (LevelingAction action in actions)
        {
            if (action.active)
            {
                action.Execute(levelingInfo);
            }
        }
    }

    internal void Draw(Rect rect, ref LevelingAction? selectedAction)
    {
        float listHeight = actions.Count * 24f;
        Rect viewRect = new(rect) { xMax = rect.xMax - 15f, height = listHeight };
        Widgets.BeginScrollView(rect, ref scrollPosition, viewRect);
        Rect rowRect = new(viewRect) { height = 24f };
        for (int i = 0; i < actions.Count; i++)
        {
            LevelingAction action = actions[i];
            Rect checkboxRect = new(rowRect) { xMin = rowRect.xMax - rowRect.height, width = rowRect.height };
            bool isActive = action.active;
            Widgets.Checkbox(checkboxRect.x, checkboxRect.y, ref isActive);
            action.active = isActive;

            Rect labelRect = new(rowRect) { xMax = checkboxRect.xMin };
            Widgets.Label(labelRect, action.actionDef.label);

            if (Widgets.ButtonInvisible(labelRect))
            {
                SoundDefOf.Click.PlayOneShotOnCamera();
                selectedAction = selectedAction == action ? null : action;
            }

            if (selectedAction == action)
            {
                Widgets.DrawHighlightSelected(rowRect);
            }
            else if (Mouse.IsOver(rowRect))
            {
                Widgets.DrawHighlight(rowRect);
            }
            else if (i % 2 != 0)
            {
                Widgets.DrawLightHighlight(rowRect);
            }

            rowRect.y = rowRect.yMax;
        }
        Widgets.EndScrollView();
    }

    public void ExposeData()
    {
        Scribe_Collections.Look(ref actions, "actions", LookMode.Deep);
    }
}
