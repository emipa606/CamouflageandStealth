# GitHub Copilot Instructions for Camouflage and Stealth (Continued) Mod

## Mod Overview and Purpose

**Camouflage and Stealth (Continued)** is an update to the original Camouflage and Stealth (CAS) mod for RimWorld. It provides a framework for mod authors to implement camouflage and stealth features in apparel, both passive and active. The mod allows for the customization of camouflage patterns, terrain, and biome-specific camouflage. Additionally, it introduces observation sight improvements that can enhance the functionality of optical aids and gun optics on the battlefield.

## Key Features and Systems

### Camouflage Types
- **Passive Camouflage**: Traditional camouflage that uses patterns or coloration to merge the wearer into their environment.
- **Active Camouflage**: Advanced technological camouflage that uses other methods to disguise the wearer, providing enhanced stealth capabilities.

### Observation Sight Improvements
- Modifies sight capabilities of equipment to a limited extent.
- Enables more functional gun optics and optical aids on the battlefield.
- Acts as a counter to camouflage use.

### XML Integration
- The mod uses XML files to patch various RimWorld mods that support CAS, allowing for terrain and biome-specific camouflage.

### Harmony Patching
- Utilizes Harmony patching to integrate with RimWorld's codebase, enabling the mod's functionality without modifying the original game files.

### Expansion Mods
- Several expansion mods are available for CAS, such as Power Armour, Cataphract Armour, Recon Armour, and Flak Armour.

## Coding Patterns and Conventions

### C# Files
- **CamoDefGet.cs**: Contains methods for retrieving camouflage definitions based on biome and terrain.
- **CamoPresetColour.cs**: Defines color presets for camouflage patterns.
- **ActiveCamoApparel.cs**: Manages active camouflage functionality, including energy consumption and toggling.
- **CamoDrawTools.cs**: Handles the rendering of camouflage overlays.
- **Camo_Setup.cs**: Sets up the mod and its components.

### XML Files
- **Patch_*.xml**: XML patch files for various mods that support CAS, enabling terrain and biome-specific camouflage.

## Suggestions for Copilot

When using GitHub Copilot to assist with the Camouflage and Stealth (Continued) mod, consider the following guidelines:

### XML Integration
- When adding new XML patch files for terrain or biome-specific camouflage, ensure that the patches are correctly formatted and target the appropriate RimWorld mods.
- Use the existing patch files as a reference for structure and syntax.

### Harmony Patching
- When creating new Harmony patches, ensure that the patches are correctly targeted and that they do not interfere with the original game code.
- Use the existing patches as a reference for structure and syntax.

### C# Code
- When adding new C# code, follow the existing coding patterns and conventions.
- Ensure that new code is well-documented and follows the same naming conventions as the existing code.
- Use the existing classes and methods as a reference for structure and syntax.

### Testing
- After making changes, thoroughly test the mod to ensure that it functions correctly with the rest of the RimWorld modding ecosystem.
- Pay special attention to compatibility with other mods, especially those that use their own line of sight and detection mechanics (e.g., CE / CE:FT).

## Conclusion

The Camouflage and Stealth (Continued) mod is a powerful tool for mod authors to implement camouflage and stealth features in RimWorld. By following the coding patterns and conventions outlined in this guide, and by utilizing GitHub Copilot effectively, you can contribute to the continued development and improvement of this mod.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

