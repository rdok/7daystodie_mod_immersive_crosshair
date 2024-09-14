## Changelog

#### 1.8.1 - 14 Aug 2024
- fix: Missing bow crosshair event.

#### 1.8.0 - 11 Aug 2024

- feat: Mod can now be customized using `ModSettings.xml` for users that are unable to use [Gears](https://www.nexusmods.com/7daystodie/mods/4017) & [Quartz](https://www.nexusmods.com/7daystodie/mods/2409/).
- feat: Tool setting: off, static, dynamic
- feat: Melee setting: off, static, dynamic
- feat: Bow setting: off, static
- feat: Ranged Weapons setting: off, static
- feat: Use game's native collider for detecting the precise interactable distance.

#### 1.7.0 - 25 Aug 2024

- feat: Optional Gears setting to enable crosshair when holding any melee weapons.

#### 1.6.0 - 25 Aug 2024

- feat: Optional Gears setting to enable crosshair when holding any tools or hands.

#### 1.5.0 - 17 Aug 2024

- feat: Add support for in game mod settings manager: [Gears](https://www.nexusmods.com/7daystodie/mods/4017)
    - In game option to enable crosshair when holding a bow with no sights
    - Translate to all in game supported languages

#### 1.4.0 - 11 Aug 2024

- feat: Show the crosshair having the interact prompt even when holding weapons.

#### 1.3.1 - 10 Aug 2024

- fix: Shows the crosshair when holding any resource based tool instead of non-ranged weapons.
    - Apparently there are ranged repair tools such as nail guns
    - Many thanks to [MrSamuelAdams1992](https://next.nexusmods.com/profile/MrSamuelAdams1992/about-me?gameId=1059)
      for [reporting this issue](https://www.nexusmods.com/7daystodie/mods/5601?tab=posts&jump_to_comment=142699761)

#### 1.3.0 - 08 Aug 2024

- feat: shows crosshair when in third person view and holding a ranged weapon.
    - Many thanks to [Khajits](https://www.nexusmods.com/7daystodie/users/37992605) for identifying this issue.
- fix: shows crosshair holding a knife.
    - Many thanks to [MrSamuelAdams1992 ](https://www.nexusmods.com/7daystodie/users/78780238) for identifying this
      issue.
- feat: increased the interactable items distance by 4.6% as per popular request.

#### 1.2.0 - 04 Aug 2024

- [Hides crosshair when holding any ranged weapons.](https://www.nexusmods.com/7daystodie/articles/813)
- fix: enable crosshair having bare hands, or knuckles like tools.

#### 1.1.1 - 04 Aug 2024

- fix: Removes debug logs from production release

#### 1.1.0 - 03 Aug 2024

- feat: Activates the crosshair when the player is close to an object and points at it with a tool like an axe, hammer,
  wrench, or shovel.
- feat: Hides crosshair when holding a melee weapon.

#### 1.0.0 - 02 Aug 2024

- feat: Activates the crosshair only when close enough to interact with an object.
- feat: Preserves the default behavior for ranged weapons.
