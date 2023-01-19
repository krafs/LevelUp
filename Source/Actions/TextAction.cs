using System;
using System.Globalization;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace LevelUp;

[Serializable]
public abstract class TextAction : LevelingAction
{
    private static readonly StringBuilder stringBuilder = new();
    private string text = string.Empty;
    public string Text { get => text; set => text = value; }

    internal override void Prepare()
    {
        base.Prepare();
        text ??= string.Empty;
    }

    protected static string ResolveText(LevelingInfo levelingInfo, string text)
    {
        return ResolveText(levelingInfo.Pawn.LabelShortCap, levelingInfo.SkillRecord.Level, levelingInfo.SkillRecord.def.LabelCap, text);
    }

    private static string ResolveText(string pawnLabel, int skillLevel, string skillLabel, string text)
    {
        return stringBuilder
            .Clear()
            .Append(text)
            .Replace("{PAWN}", pawnLabel)
            .Replace("{LEVEL}", skillLevel.ToString(CultureInfo.InvariantCulture))
            .Replace("{SKILL}", skillLabel)
            .ToString();
    }

    protected Rect DrawTextBuilder(Rect rect)
    {
        string pawnLabel = "Levelyn";
        int skillLevel = 18;
        TaggedString skillLabel = SkillDefOf.Intellectual.LabelCap;

        Rect rowRect = new(rect) { height = 24f };

        Rect exampleTextRect = new(rowRect) { height = rowRect.height * 2 };
        TextAnchor curAnchor = Verse.Text.Anchor;
        Verse.Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(exampleTextRect, ResolveText(pawnLabel, skillLevel, skillLabel, Text));
        Verse.Text.Anchor = curAnchor;

        rowRect.y = rowRect.yMax + 5f;
        Rect textEntryRect = new(rowRect) { y = exampleTextRect.yMax, height = rowRect.height * 3 };
        text = Widgets.TextArea(textEntryRect, Text);

        rowRect.y = textEntryRect.yMax + 5f;
        Rect guideRect = new(rowRect) { height = 24f * 11 };
        Rect innerGuideRect = guideRect.GetInnerRect();
        Widgets.DrawMenuSection(guideRect);
        Rect guideRowRect = new(innerGuideRect) { height = 24f };
        Rect label2Rect = new(guideRowRect) { x = guideRowRect.x + (guideRowRect.width / 2) };
        Widgets.Label(guideRowRect, I18n.InjectableValuesLabel.Bold());

        guideRowRect.y = guideRowRect.yMax;
        label2Rect.y = label2Rect.yMax;
        Widgets.Label(guideRowRect, "{PAWN}");
        Widgets.Label(label2Rect, pawnLabel);

        guideRowRect.y = guideRowRect.yMax;
        label2Rect.y = label2Rect.yMax;
        Widgets.Label(guideRowRect, "{LEVEL}");
        Widgets.Label(label2Rect, skillLevel.ToString(CultureInfo.InvariantCulture));

        guideRowRect.y = guideRowRect.yMax;
        label2Rect.y = label2Rect.yMax;
        Widgets.Label(guideRowRect, "{SKILL}");
        Widgets.Label(label2Rect, skillLabel);

        guideRowRect.y = guideRowRect.yMax + 5f;
        label2Rect.y = label2Rect.yMax + 5f;
        Widgets.Label(guideRowRect, I18n.FormattingLabel.Bold());

        guideRowRect.y = guideRowRect.yMax;
        label2Rect.y = label2Rect.yMax;
        Verse.Text.CurFontStyle.richText = false;
        Widgets.Label(guideRowRect, I18n.FormattingBoldLabel.Bold());
        Verse.Text.CurFontStyle.richText = true;
        Widgets.Label(label2Rect, I18n.FormattingBoldLabel.Bold());

        guideRowRect.y = guideRowRect.yMax;
        label2Rect.y = label2Rect.yMax;
        Verse.Text.CurFontStyle.richText = false;
        Widgets.Label(guideRowRect, I18n.FormattingItalicLabel.Italic());
        Verse.Text.CurFontStyle.richText = true;
        Widgets.Label(label2Rect, I18n.FormattingItalicLabel.Italic());

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
