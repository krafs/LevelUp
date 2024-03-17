using RimWorld;
using System.Globalization;
using System.Text;
using UnityEngine;
using Verse;

namespace LevelUp;

public abstract class LevelingAction_Text : LevelingAction
{
    private static readonly StringBuilder stringBuilder = new();
    internal string text = string.Empty;

    internal static string ResolveText(LevelingInfo levelingInfo, string text)
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
        TextAnchor curAnchor = Text.Anchor;
        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(exampleTextRect, ResolveText(pawnLabel, skillLevel, skillLabel, text));
        Text.Anchor = curAnchor;

        rowRect.y = rowRect.yMax + 5f;
        Rect textEntryRect = new(rowRect) { y = exampleTextRect.yMax, height = rowRect.height * 3 };
        text = Widgets.TextArea(textEntryRect, text);

        rowRect.y = textEntryRect.yMax + 5f;
        Rect guideRect = new(rowRect) { height = 24f * 11 };
        Rect innerGuideRect = guideRect.GetInnerRect();
        Widgets.DrawMenuSection(guideRect);
        Rect guideRowRect = new(innerGuideRect) { height = 24f };
        Rect label2Rect = new(guideRowRect) { x = guideRowRect.x + (guideRowRect.width / 2) };
        Widgets.Label(guideRowRect, $"<b>{I18n.InjectableValuesLabel}</b>");

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
        Widgets.Label(guideRowRect, $"<b>{I18n.FormattingLabel}</b>");

        guideRowRect.y = guideRowRect.yMax;
        label2Rect.y = label2Rect.yMax;
        Text.CurFontStyle.richText = false;
        Widgets.Label(guideRowRect, $"<b>{I18n.FormattingBoldLabel}</b>");
        Text.CurFontStyle.richText = true;
        Widgets.Label(label2Rect, $"<b>{I18n.FormattingBoldLabel}</b>");

        guideRowRect.y = guideRowRect.yMax;
        label2Rect.y = label2Rect.yMax;
        Text.CurFontStyle.richText = false;
        Widgets.Label(guideRowRect, $"<i>{I18n.FormattingItalicLabel}</i>");
        Text.CurFontStyle.richText = true;
        Widgets.Label(label2Rect, $"<i>{I18n.FormattingItalicLabel}</i>");

        guideRowRect.y = guideRowRect.yMax;
        label2Rect.y = label2Rect.yMax;
        Text.CurFontStyle.richText = false;
        Widgets.Label(guideRowRect, $"<color=green>{I18n.FormattingColorLabel}</color>");
        Text.CurFontStyle.richText = true;
        Widgets.Label(label2Rect, $"<color=green>{I18n.FormattingColorLabel}</color>");

        guideRowRect.y = guideRowRect.yMax;
        label2Rect.y = label2Rect.yMax;
        Text.CurFontStyle.richText = false;
        Widgets.Label(guideRowRect, $"<color=#F0CC1E>{I18n.FormattingColorLabel}</color>");
        Text.CurFontStyle.richText = true;
        Widgets.Label(label2Rect, $"<color=#F0CC1E>{I18n.FormattingColorLabel}</color>");

        return new Rect(rect) { yMax = guideRect.yMax + 15f };
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref text, "text", string.Empty);
    }
}
