# Level Up 
![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/krafs/levelup?label=Latest)
![GitHub all releases](https://img.shields.io/github/downloads/krafs/levelup/total?label=GitHub%20Downloads)
![Steam Downloads](https://img.shields.io/steam/downloads/1701592470?label=Steam%20Downloads)

Level Up is a RimWorld mod that notifies you when colonists level up.

## Installation

Subscribe to the mod on [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=1701592470) or download the latest release [here](https://github.com/krafs/LevelUp/releases/latest) and unzip in your Mods-folder.

Start the game and enable the mod in the Mod menu. Place it anywhere below **Core** and **Harmony**[^1] in the load order.

## Compatibility
Level Up is compatible with Rimworld 1.4.

Known incompatible mods:
* Static Quality Plus
* Ducks' Insane Skills

## Usage
You will automatically get notified whenever a colonist level up.
Advanced configuration is available in the Mod settings.

## FAQ
#### Why do colonists level up and then almost instantly down again?
This is caused by a mechanic in vanilla RimWorld known as *skill decay*. Skills at lvl 10 and higher lose xp. If xp goes low enough, the skill levels down. The rate of skill decay increases with every level - It is slow at lvl 10, and fast at lvl 20.
This can lead to two problems:

- A colonist on level 10 or higher in a skill she almost never uses will lead to that skill almost always ending up at the very top of lvl 9, because skill decay stops as soon as a skill goes below lvl 10. This means that only a tiny bit of xp is needed to level her up to lvl 10 again, which then goes down to 9 again, and so on.

- A colonist with a high level skill she uses almost all the time will likewise lead to that skill jumping up and down between e.g. lvl 19 and 20.

Level Up has a cooldown between actions to make this feel less irritating, but it does not in any way prevent or slow down skill decay.

## Contributing
Pull requests are welcome, for both code and translations. 
Please open an issue first to discuss what you would like to change.

[^1]: Harmony: ([Github](https://github.com/pardeike/HarmonyRimWorld) / [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077))
