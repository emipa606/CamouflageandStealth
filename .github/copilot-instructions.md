# CoPilot Instructions for RimWorld Camo Mod

## Mod Overview and Purpose

This mod introduces advanced camouflage mechanics to RimWorld, enhancing both the visual and tactical aspects of the game. Players can equip colonists with camouflage apparel that affects visibility to enemies, introduces new stealth-based strategies, and provides expanded gameplay opportunities.

## Key Features and Systems

1. **Camouflage Apparel**: Includes new types of apparel with camouflage capabilities, integrating into the existing apparel system.
2. **Active Camouflage Mechanism**: Allows players to toggle active camouflage on certain apparel, impacting visibility mechanics in-game.
3. **AI and Observer Utilities**: Implements utilities to determine how AI perceives camouflaged entities, and adjusts their behavior accordingly.
4. **Camouflage Presets**: Offers pre-configured camouflage patterns for ease of use and personalization.
5. **Multiplayer Support**: Ensures the mod functions seamlessly in both single and multiplayer settings.

## Coding Patterns and Conventions

- **Pascal Casing**: Used for class names and public methods (e.g., `ToggleActiveCamo`).
- **Camel Casing**: Used for private variables and parameters (e.g., `flag` in `ToggleActiveCamo` method).
- **Encapsulation**: Utilized consistently to manage state and behavior within classes such as `ActiveCamoApparel` and `CardboardBox`.
- **Modular Design**: Separate classes are used for distinct features, promoting maintainability and scalability (e.g., see `CamoDrawTools` and `CamoUtility`).

## XML Integration

The mod relies on XML for defining the new apparel and integrations:

- **Defs Folder**: Contains XML files that define new camouflage apparel and presets.
- **DefModExtension**: Used in classes such as `CompCamoDefs` to extend regular functionality with camouflage-related properties.
  
When adding new items or properties:
- Ensure that XML elements are correctly defined with attributes matching the properties of the classes they instantiate.
- Use `<Defs>` to wrap item definition files, and `<ThingDef>` to define new items or features.

## Harmony Patching

A core component of this mod is utilizing Harmony for runtime method patching which allows for:

- **Method Prefixes/Suffixes**: To adjust game logic before or after existing methods run.
- **Replacement**: For completely redirecting method calls.
- **Maintaining Compatibility**: Ensuring other mods can coexist by not modifying original code excessively.

To add a Harmony patch:
- Use the `HarmonyPatching` class to manage and organize patch methods with the `[HarmonyPatch]` attribute.
- Define the target method using attributes like `[HarmonyPatch(typeof(TargetType), "MethodName")]`.

## Suggestions for Copilot

- **Suggest Patterns**: For tasks such as toggling camouflage states or updating observer perceptions.
- **Automate XML Suggestions**: Based on class attributes and field definitions in code files, suggest appropriate XML element configurations.
- **Method Descriptions**: Help auto-document methods like those in `HarmonyPatching` as they are implemented.

When utilizing GitHub Copilot, consider:
- Drafting consistent method signatures and comments to guide the AI in generating relevant code suggestions.
- Leveraging class-level documentation to inform Copilot of intended functionalities, particularly in utility classes like `CamoUtility` and `Observer_Setup`.

This guide should support new developers in contributing to the mod and ensure consistency across future updates and feature additions.
