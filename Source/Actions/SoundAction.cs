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
    private const float MinVolume = 0f;
    private const float MaxVolume = 1.5f;
    private static readonly FloatRange volumeRange = new(MinVolume, MaxVolume);

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
        soundDef = DefDatabase<SoundDef>.AllDefs
                .Where(x => x.HasModExtension<SoundDefExtension>())
                .RandomElement();
    }

    internal override void Prepare()
    {
        base.Prepare();
    }

    internal override void Execute(LevelingInfo levelingInfo)
    {
        SoundInfo soundInfo = SoundInfo.OnCamera();
        soundInfo.volumeFactor = volume;
        soundDef.PlayOneShot(soundInfo);
    }

    public override void Draw(Rect rect)
    {
        Rect rowRect = new(rect) { height = 24f };

        Rect dropDownRect = new(rowRect) { width = rect.width / 2 };

        if (Widgets.ButtonText(dropDownRect, soundDef.LabelCap))
        {
            List<FloatMenuOption> options = DefDatabase<SoundDef>.AllDefs
                .Where(x => x.HasModExtension<SoundDefExtension>())
                .Select(x => new FloatMenuOption(x.LabelCap, () => soundDef = x))
                .ToList();

            Find.WindowStack.Add(new FloatMenu(options));
        }
        Rect slideControlRect = new Rect(rowRect) { xMin = dropDownRect.xMax }.ContractedBy(5f, 0f);
        Rect iconRect = new(slideControlRect) { x = dropDownRect.xMax + 5f, width = slideControlRect.height };
        if (Widgets.ButtonImageFitted(iconRect, TexButton.Play))
        {
            Execute(default);
        }

        Rect sliderRect = new(slideControlRect) { xMin = iconRect.xMax + 5f, yMin = slideControlRect.yMin + 8f };
        Widgets.HorizontalSlider(sliderRect, ref volume, volumeRange);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Defs.Look(ref soundDef, "soundDef");
        Scribe_Values.Look(ref volume, "volume", 0.5f);
    }
}
