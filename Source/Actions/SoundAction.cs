using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LevelUp;

[Serializable]
public class SoundAction : LevelingAction
{
    private static readonly List<SoundDef> allSounds = DefDatabase<SoundDef>
            .AllDefs.Where(x => x.HasModExtension<SoundDefExtension>()).ToList();

    private SoundDef soundDef = null!;
    private float volume = 0.5f;

    public SoundDef SoundDef
    {
        get => soundDef;
        set
        {
            soundDef = value;
            Prepare();
        }
    }

    public SoundAction()
    {
        soundDef = allSounds.RandomElement();
    }

    public override void Prepare()
    {
        base.Prepare();
    }

    public override void Execute(LevelingInfo levelingInfo)
    {
        var soundInfo = SoundInfo.OnCamera();
        soundInfo.volumeFactor = volume;
        soundDef.PlayOneShot(soundInfo);
    }

    public override void Draw(Rect rect)
    {
        var rowRect = new Rect(rect) { height = 24f };
        var soundDefRect = new Rect(rowRect) { width = rect.width / 2 };
        if (CustomWidgets.ButtonText(soundDefRect, soundDef.LabelCap))
        {
            var options = allSounds
                .Select(x => new FloatMenuOption(x.LabelCap, () => soundDef = x))
                .ToList();

            Find.WindowStack.Add(new FloatMenu(options));
        }

        rowRect.y = rowRect.yMax + 5f;
        var sliderRect = new Rect(rowRect) { width = rect.width / 2 };
        DrawVolumeSlider(sliderRect, ref volume);

        rowRect.y = rowRect.yMax + 5f;
        var playButtonRect = new Rect(rowRect) { height = rowRect.height * 2, width = rowRect.height * 2 };
        if (Widgets.ButtonImage(playButtonRect, GenTextures.PlayButton) && SoundDef != null)
        {
            Execute(default);
        }
    }

    private static void DrawVolumeSlider(Rect rect, ref float volume)
    {
        const float Min = 0f;
        const float Max = 1.5f;

        Texture2D image;
        if (volume > Max * 0.8)
        {
            image = GenTextures.SpeakerFull;
        }
        else if (volume > Max * 0.4)
        {
            image = GenTextures.SpeakerMid;
        }
        else
        {
            image = volume > Min ? GenTextures.SpeakerLow : GenTextures.SpeakerMute;
        }

        var imageRect = new Rect(rect) { width = rect.height };
        Widgets.DrawTextureFitted(imageRect, image, 1f);
        var sliderRect = new Rect(rect) { xMin = imageRect.xMax, yMin = rect.y };
        volume = Widgets.HorizontalSlider(sliderRect, volume, Min, Max, middleAlignment: true);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Defs.Look(ref soundDef, "soundDef");
        Scribe_Values.Look(ref volume, "volume", 0.5f);
    }
}
