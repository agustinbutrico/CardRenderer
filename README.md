# TexturesLib.UI for Rogue Tower

TexturesLib.UI is a mod designed to provide UI-related textures that can be easily referenced by other Rogue Tower mods. It serves as a centralized library of UI sprite textures, ensuring consistency and simplifying UI customization or extension for mod developers.

## Installation

    Download the TexturesLib.UI DLL.

    Place the .dll file into your BepInEx/plugins folder.

    Launch the game.

Note: This is a library mod. It doesn't add UI changes on its own, but rather supplies textures that other mods can use.

## Features

    UI Sprite Library
    Adds referenceable UI sprite textures that can be accessed in any game scene.

    Modder-Friendly
    Designed for use by UI-based mods, making it easier to build or extend game menus, panels, or HUD elements.

    Consistent Visuals
    Provides shared resources to ensure a uniform UI style across multiple mods.

## Usage

Mod developers can access UI sprites by using TexturesLib.Shared.SpriteHelper.FindSpriteByName.
Example (C#):

using TexturesLib.Shared;
using UnityEngine;

Sprite myUISprite = SpriteHelper.FindSpriteByName("UI9SliceBrown");

This will search all loaded resources for the sprite with the given name (or the name + _TexturesLib) and cache it for future calls.