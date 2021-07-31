using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LevelUp
{
    [Serializable]
    public class ActionMaker : IExposable
    {
        private List<LevelingAction> actions = new List<LevelingAction>();
        private Vector2 scrollPosition;
        private readonly List<LevelingAction> preparedActions = new List<LevelingAction>();
        private readonly List<LevelingAction> preparedCooldownActions = new List<LevelingAction>();
        public List<LevelingAction> Actions => actions;

        public void Prepare()
        {
            preparedActions.Clear();
            preparedCooldownActions.Clear();

            foreach (LevelingAction action in actions)
            {
                if (!action.Active)
                {
                    continue;
                }

                if (action.Cooldown)
                {
                    preparedCooldownActions.Add(action);
                }
                else
                {
                    preparedActions.Add(action);
                }

                action.Prepare();
            }
        }

        public void ExecuteActions(LevelingInfo levelingInfo)
        {
            if (!levelingInfo.Pawn.IsFreeColonist)
            {
                return;
            }

            for (int i = 0; i < preparedActions.Count; i++)
            {
                preparedActions[i].Execute(levelingInfo);
            }

            bool cooldownPassed = PawnSkillTimerCache.EnoughTimeHasPassed(levelingInfo);
            if (!cooldownPassed)
            {
                return;
            }

            for (int i = 0; i < preparedCooldownActions.Count; i++)
            {
                preparedCooldownActions[i].Execute(levelingInfo);
            }
        }

        public void Draw(Rect rect, ref IDrawer selectedAction)
        {
            float listHeight = actions.Count * 24f;
            Rect viewRect = new Rect(rect) { xMax = rect.xMax - 15f, height = listHeight };
            Widgets.BeginScrollView(rect, ref scrollPosition, viewRect);
            Rect rowRect = new Rect(viewRect) { height = 24f };
            for (int i = 0; i < actions.Count; i++)
            {
                LevelingAction action = actions[i];
                Rect checkboxRect = new Rect(rowRect) { xMin = rowRect.xMax - rowRect.height, width = rowRect.height };
                bool isActive = action.Active;
                Widgets.Checkbox(checkboxRect.x, checkboxRect.y, ref isActive);
                action.Active = isActive;

                Rect labelRect = new Rect(rowRect) { xMax = checkboxRect.xMin };
                Widgets.Label(labelRect, action.ActionDef.label);

                if (Widgets.ButtonInvisible(labelRect))
                {
                    SoundDefOf.Click.PlayOneShotOnCamera();
                    if (selectedAction == action)
                    {
                        selectedAction = Drawer.Empty;
                    }
                    else
                    {
                        selectedAction = action;
                    }
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
}