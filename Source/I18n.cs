using Verse;

namespace LevelUp;
internal static class I18n
{
    private const string TranslatePrefix = "LevelUp.";
    internal static string LevelUpActionsLabel { get; } = "LevelUpActionsLabel".TranslateInternal();
    internal static string LevelDownActionsLabel { get; } = "LevelDownActionsLabel".TranslateInternal();
    internal static string LevelUpActionHeaderDescription { get; } = "LevelUpActionHeaderDescription".TranslateInternal();
    internal static string LevelDownActionHeaderDescription { get; } = "LevelDownActionHeaderDescription".TranslateInternal();
    internal static string CooldownLabel { get; } = "CooldownLabel".TranslateInternal();
    internal static string CooldownEntryDescription { get; } = "CooldownEntryDescription".TranslateInternal();
    internal static string CooldownOnActionDescription { get; } = "CooldownOnActionDescription".TranslateInternal();
    internal static string GeneralSettingsLabel { get; } = "GeneralSettingsLabel".TranslateInternal();
    internal static string HistoricalLabel { get; } = "HistoricalLabel".TranslateInternal();
    internal static string HistoricalDescription { get; } = "HistoricalDescription".TranslateInternal();
    internal static string InjectableValuesLabel { get; } = "InjectableValuesLabel".TranslateInternal();
    internal static string FormattingLabel { get; } = "FormattingLabel".TranslateInternal();
    internal static string FormattingBoldLabel { get; } = "FormattingBoldLabel".TranslateInternal();
    internal static string FormattingItalicLabel { get; } = "FormattingItalicLabel".TranslateInternal();
    internal static string FormattingColorLabel { get; } = "FormattingColorLabel".TranslateInternal();
    internal static string DefaultLevelUpMessage { get; } = "DefaultLevelUpMessage".TranslateInternal();
    internal static string DefaultLevelDownMessage { get; } = "DefaultLevelDownMessage".TranslateInternal();
    internal static string DefaultLevelUpOverheadMessage { get; } = "DefaultLevelUpOverheadMessage".TranslateInternal();
    internal static string DefaultLevelDownOverheadMessage { get; } = "DefaultLevelDownOverheadMessage".TranslateInternal();

    private static string TranslateInternal(this string key)
    {
        return $"{TranslatePrefix}{key}".Translate().RawText;
    }
}
