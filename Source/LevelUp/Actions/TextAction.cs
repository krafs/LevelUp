using System;
using System.Globalization;
using System.Text;
using UnityEngine;
using Verse;

namespace LevelUp
{
    [Serializable]
    public abstract class TextAction : LevelingAction
    {
        private static readonly StringBuilder stringBuilder = new StringBuilder();
        private string text = string.Empty;
        public string Text { get => text; set => text = value; }

        public override void Prepare()
        {
            base.Prepare();
            text ??= string.Empty;
        }

        protected static string ResolveText(LevelingInfo levelingInfo, string text)
        {
            return stringBuilder
                .Clear()
                .Append(text)
                .Replace("{PAWN}", levelingInfo.Pawn.LabelShortCap)
                .Replace("{LEVEL}", levelingInfo.SkillRecord.levelInt.ToString(CultureInfo.InvariantCulture))
                .Replace("{SKILL}", levelingInfo.SkillRecord.def.LabelCap)
                .ToString();
        }

        protected Rect DrawTextBuilder(Rect rect)
        {
            Rect rowRect = new Rect(rect) { height = 24f };

            Rect exampleTextRect = new Rect(rowRect) { height = rowRect.height * 2 };
            TextAnchor curAnchor = Verse.Text.Anchor;
            Verse.Text.Anchor = TextAnchor.MiddleCenter;
            LevelingInfo levelingInfo = GenLevelingInfo.Fake;
            Widgets.Label(exampleTextRect, ResolveText(levelingInfo, Text));
            Verse.Text.Anchor = curAnchor;

            rowRect.y = rowRect.yMax + 5f;
            Rect textEntryRect = new Rect(rowRect) { y = exampleTextRect.yMax, height = rowRect.height * 3 };
            text = Widgets.TextArea(textEntryRect, Text);

            rowRect.y = textEntryRect.yMax + 5f;
            Rect guideRect = new Rect(rowRect) { height = 24f * 11 };
            Rect innerGuideRect = guideRect.GetInnerRect();
            Widgets.DrawMenuSection(guideRect);
            Rect guideRowRect = new Rect(innerGuideRect) { height = 24f };
            Rect label2Rect = new Rect(guideRowRect) { x = guideRowRect.x + (guideRowRect.width / 2) };
            Widgets.Label(guideRowRect, I18n.InjectableValuesLabel.Bolded());

            guideRowRect.y = guideRowRect.yMax;
            label2Rect.y = label2Rect.yMax;
            Widgets.Label(guideRowRect, "{PAWN}");
            Widgets.Label(label2Rect, levelingInfo.Pawn.LabelShortCap);

            guideRowRect.y = guideRowRect.yMax;
            label2Rect.y = label2Rect.yMax;
            Widgets.Label(guideRowRect, "{LEVEL}");
            Widgets.Label(label2Rect, levelingInfo.SkillRecord.levelInt.ToString(CultureInfo.InvariantCulture));

            guideRowRect.y = guideRowRect.yMax;
            label2Rect.y = label2Rect.yMax;
            Widgets.Label(guideRowRect, "{SKILL}");
            Widgets.Label(label2Rect, levelingInfo.SkillRecord.def.LabelCap);

            guideRowRect.y = guideRowRect.yMax + 5f;
            label2Rect.y = label2Rect.yMax + 5f;
            Widgets.Label(guideRowRect, I18n.FormattingLabel.Bolded());

            guideRowRect.y = guideRowRect.yMax;
            label2Rect.y = label2Rect.yMax;
            Verse.Text.CurFontStyle.richText = false;
            Widgets.Label(guideRowRect, I18n.FormattingBoldLabel.Bolded());
            Verse.Text.CurFontStyle.richText = true;
            Widgets.Label(label2Rect, I18n.FormattingBoldLabel);

            guideRowRect.y = guideRowRect.yMax;
            label2Rect.y = label2Rect.yMax;
            Verse.Text.CurFontStyle.richText = false;
            Widgets.Label(guideRowRect, I18n.FormattingItalicLabel.Italicized());
            Verse.Text.CurFontStyle.richText = true;
            Widgets.Label(label2Rect, I18n.FormattingItalicLabel);

            guideRowRect.y = guideRowRect.yMax;
            label2Rect.y = label2Rect.yMax;
            Verse.Text.CurFontStyle.richText = false;
            Widgets.Label(guideRowRect, $"<color=green>{I18n.FormattingColorLabel}</color>");
            Verse.Text.CurFontStyle.richText = true;
            Widgets.Label(label2Rect, $"<color=green>{I18n.FormattingColorLabel}</color>");

            guideRowRect.y = guideRowRect.yMax;
            label2Rect.y = label2Rect.yMax;
            Verse.Text.CurFontStyle.richText = false;
            Widgets.Label(guideRowRect, $"<color=#F0CC1E>{I18n.FormattingColorLabel}</color>");
            Verse.Text.CurFontStyle.richText = true;
            Widgets.Label(label2Rect, $"<color=#F0CC1E>{I18n.FormattingColorLabel}</color>");

            return new Rect(rect) { yMax = guideRect.yMax + 15f };
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref text, "text", string.Empty);
        }
    }
}