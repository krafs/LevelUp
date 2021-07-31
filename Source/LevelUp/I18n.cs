using Verse;

namespace LevelUp
{
    public static class I18n
    {
        public const string TranslatePrefix = "LevelUp.";

        public static readonly string LevelUpActionsLabel = "LevelUpActionsLabel".TranslateInternal();
        public static readonly string LevelDownActionsLabel = "LevelDownActionsLabel".TranslateInternal();

        public static readonly string LevelUpActionHeaderDescription = "LevelUpActionHeaderDescription".TranslateInternal();
        public static readonly string LevelDownActionHeaderDescription = "LevelDownActionHeaderDescription".TranslateInternal();

        public static readonly string CooldownLabel = "CooldownLabel".TranslateInternal();
        public static readonly string CooldownEntryDescription = "CooldownEntryDescription".TranslateInternal();
        public static readonly string CooldownOnActionDescription = "CooldownOnActionDescription".TranslateInternal();

        public static readonly string GeneralSettingsLabel = "GeneralSettingsLabel".TranslateInternal();
        public static readonly string HistoricalLabel = "HistoricalLabel".TranslateInternal();
        public static readonly string HistoricalDescription = "HistoricalDescription".TranslateInternal();

        public static readonly string InjectableValuesLabel = "InjectableValuesLabel".TranslateInternal();
        public static readonly string FormattingLabel = "FormattingLabel".TranslateInternal();
        public static readonly string FormattingBoldLabel = "FormattingBoldLabel".TranslateInternal();
        public static readonly string FormattingItalicLabel = "FormattingItalicLabel".TranslateInternal();
        public static readonly string FormattingColorLabel = "FormattingColorLabel".TranslateInternal();

        public static readonly string DefaultLevelUpMessage = "DefaultLevelUpMessage".TranslateInternal();
        public static readonly string DefaultLevelDownMessage = "DefaultLevelDownMessage".TranslateInternal();
        public static readonly string DefaultLevelUpOverheadMessage = "DefaultLevelUpOverheadMessage".TranslateInternal();
        public static readonly string DefaultLevelDownOverheadMessage = "DefaultLevelDownOverheadMessage".TranslateInternal();

        private static string TranslateInternal(this string key)
        {
            return $"{TranslatePrefix}{key}".Translate().RawText;
        }
    }
}