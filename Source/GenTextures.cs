using UnityEngine;
using Verse;

namespace LevelUp;

[StaticConstructorOnStartup]
public static class GenTextures
{
    public static Texture2D PlayButton { get; } = ContentFinder<Texture2D>.Get("LevelUp/UI/PlayButton");
    public static Texture2D SpeakerMute { get; } = ContentFinder<Texture2D>.Get("LevelUp/UI/SpeakerMute");
    public static Texture2D SpeakerLow { get; } = ContentFinder<Texture2D>.Get("LevelUp/UI/SpeakerLow");
    public static Texture2D SpeakerMid { get; } = ContentFinder<Texture2D>.Get("LevelUp/UI/SpeakerMid");
    public static Texture2D SpeakerFull { get; } = ContentFinder<Texture2D>.Get("LevelUp/UI/SpeakerFull");
}
