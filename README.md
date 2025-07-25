# CardRenderer for Rogue Tower

## CardRenderer is a mod that enables dynamic display and rendering of cards in Rogue Tower.
It provides tools to create, customize, and arrange cards into menus or tree structures, with automatic resizing of panels to fit the layout.

## Installation

    Download the CardRenderer DLL.

    Place the .dll file into your BepInEx/plugins folder.

    Launch the game.

Note: This is a library mod intended for use by other mods to render cards, menus, and upgrade trees.

## Features

    Card Rendering
    Dynamically create and display upgrade cards in-game.

    Tree Structures
    Display cards in hierarchical tree layouts (horizontal or vertical).

    Panel Resizing
    Automatically resize panels to fit the number and arrangement of cards.

    Customizable UI
    Adjust card colors, slice variants, and text colors with ease.

## API Reference

The mod exposes a public API through CardRendererAPI:

### Menus & Panels

// Create a new menu
GameObject newMenu = CardRendererAPI.NewMenu("ParentPath", "NewMenuName", "PrefabAspectName");

// Create a new panel inside a menu
GameObject newPanel = CardRendererAPI.NewPanel(
    "ParentPath", "PanelName", new Vector2(0, 0), false, "LargeUI9SliceBlue"
);

### Cards

// Create a single card display
GameObject card = CardRendererAPI.CreateCardDisplay(
    cardDisplayData, "ParentPath", "SliceVariant", "PrefabAspect", Color.white, Color.black
);

// Adjust position of an existing card
CardRendererAPI.ConfigureCardPosition(card, localPosition: new Vector3(0, 0, 0));

// Change card slice or color
CardRendererAPI.SwapCardSlice("Parent/CardPath", "SliceVariant", Color.blue, Color.white);

### Card Trees

// Display a horizontal card tree
CardRendererAPI.DisplayCardTree(cardTreeRoots, "ParentPath");
// Resize a panel to fit all its children
CardRendererAPI.ResizeToFitChildren(createdPanel);

// Display a vertical card tree
CardRendererAPI.DisplayCardTreeVertical(cardTreeRoots, "ParentPath");
// Resize a panel to fit all its children
CardRendererAPI.ResizeToFitChildrenVertical(createdVerticalPanel);

### Utility

// Wait for card/menu aspects to be loaded
yield return CardRendererAPI.WaitForCardAspect("DefaultHorizontalCard");
yield return CardRendererAPI.WaitForMenuAspect("DefaultMenu");
