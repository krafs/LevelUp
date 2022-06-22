# For modders

Level Up supports other mods adding sounds and animations to choose from. It can be done entirely with xml. Below is an overview of what can be done, and how to do it. You can also go [here](SampleMods/) to look at complete mod samples that can be downloaded and tweaked to your preference.

## Sounds
In short, you add a sound to Level Up by adding LevelUp.SoundDefExtension to a SoundDef's mod extensions. SoundDef is a vanilla def, and there are many different options that can be tweaked for different behaviors. This is a minimal SoundDef needed to work properly with Level Up:
```xml
<SoundDef>
	<defName>MyLevelUpSoundDef</defName>
	<label>My sound</label>
	<modExtensions>
		<li Class="LevelUp.SoundDefExtension" />
	</modExtensions>
	<subSounds>
		<li>
			<onCamera>True</onCamera>
			<sustainLoop>False</sustainLoop>
			<grains>
				<li Class="AudioGrain_Clip">
					<clipPath>MyLevelUpSounds/MySound</clipPath>
				</li>
			</grains>
		</li>
	</subSounds>
</SoundDef>
```

In the above case, a sound file is expected to be at RootFolder/Sounds/MyLevelUpSounds/MySound.ogg. RimWorld supports other sound formats too, like .wav and .mp3.

## Animations
An animation is added to Level Up by making a FleckDef with a LevelUp.AnimationDefExtension.
```xml
<FleckDef ParentName="FleckBase_Thrown">
	<defName>MyLevelUpAnimationDef</defName>
	<label>My animation</label>
	<modExtensions>
		<li Class="LevelUp.AnimationDefExtension" />
	</modExtensions>
	<graphicData>
	  <texPath>MyLevelUpTextures/MyTexture</texPath>
	  <shaderType>MoteGlow</shaderType>
	</graphicData>
	<altitudeLayer>MoteOverhead</altitudeLayer>
	<fadeInTime>1</fadeInTime>
	<fadeOutTime>1</fadeOutTime>
</FleckDef>
```
The above expects there to be an image file at RootFolder/Textures/MyLevelUpTextures/MyTexture.png. This particular FleckDef will result in an animation that fades in your texture on the pawn for 1 second, and then fades it out for 1 second.

The FleckDef and LevelUp.AnimationDefExtension can be additionally configured for various effects, such as growing, moving and rotating textures. For available values on the FleckDef itself, I recommend browsing the vanilla FleckDefs.
For the custom LevelUp.AnimationDefExtension, below is a complete specification on what values are available, and their default values. 
The values are taken from vanilla RimWorld, and just exposes them in xml. 
```xml
<modExtensions>
	<li Class="LevelUp.AnimationDefExtension">
		<scale>1.0<scale>
		<rotation>0.0<rotation>
		<rotationRate>0.0<rotationRate>
		<exactScale>(1.0, 1.0, 1.0)<exactScale>
		<instanceColor>(1.0, 1.0, 1.0)<instanceColor>
		<velocity>(0.0, 0.0, 0.0)<velocity>
		<velocityAngle>0.0<velocityAngle>
		<velocitySpeed>0.0<velocitySpeed>
		<solidTimeOverride>-1.0<solidTimeOverride>
		<airTimeLeft>999999.0<airTimeLeft>
		<targetSize>0.0<targetSize>
	<li>
</modExtensions>
```