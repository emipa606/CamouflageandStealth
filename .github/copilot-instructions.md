# GitHub Copilot Instructions for RimWorld Modding: Camouflage and Stealth (Continued)

## Mod Overview and Purpose

**Mod Name:** Camouflage and Stealth (Continued)

**Purpose:** 
The Camouflage and Stealth (CAS) framework allows other mod authors to enhance apparel and equipment with passive and active camouflage capabilities, and improve observation sight capabilities. This technology grants users the ability to blend into backgrounds via camouflage and enhances sight and stealth potential in the game, thereby enhancing the tactical experience in RimWorld.

## Key Features and Systems

1. **Passive Camouflage:** 
   - Provides pattern-based or color-coded camouflage helpful in blending with surroundings.
   - Apparel-based passive camouflage is weighted by the wearer’s natural skin-covered body-part coverage, so small items contribute proportionally less than large garments across different races and body layouts.

2. **Active Camouflage:** 
   - Utilizes advanced technologies to disguise wearers, focusing on stealth and tactical advantages.

3. **Observation Enhancements:** 
   - Improves sight capabilities through advanced optics, providing potential counters to camouflage.

4. **Framework Utilization:** 
   - Designed for other mod developers to integrate CAS into their mods through API and XML patches.

5. **Visibility Cache Behavior:**
   - Camo visibility checks cache hidden and visible results with asymmetric lifetimes so hidden states expire faster than visible states.

## Coding Patterns and Conventions

- **Naming Conventions:** 
  - CamelCase for method names and PascalCase for class names are used consistently.
  
- **Organization:**
  - Source files are categorized within the project directory, for example:
    - `CompCamo` for camouflage-related functionalities.
  
- **File Summary:**
  - C# source files are detailed with class and member documentation where possible.
  
- **Consistent Use of Regions:**
  - Maintain organization within files with regions to encapsulate related functions.

## XML Integration

XML files are primarily used for patching biome and terrain data to integrate custom camouflage settings. Key XML patches are included for:
- `Advanced Biomes`
- `Biomes Vanilla`
- `Nature's Pretty Sweet`
- `Realistic Planets`

Include precise tags and syntax as per RimWorld’s XML schema to ensure compatibility and stability.

## Harmony Patching

**Dependency:** brrainz.harmony

- **Patch with Caution:** 
  - When using Harmony to patch methods, ensure that patches are non-destructive and compatible with other mods that might patch the same methods.
  
- **Documentation:** 
  - Document your Harmony patches within the codebase to allow for easier troubleshooting and updates.

## Suggestions for Copilot

- **Autocomplete Suggestions:**
  - Implement Copilot suggestions for repetitive pattern selections and simple function definitions.

- **Automation:**
  - Utilize Copilot to automate unit tests for new features as they integrate with CAS functionalities.

- **Code Optimization:**
  - Let Copilot suggest optimizations particularly in performance-heavy areas like stealth detection and rendering functions.

- **Error-Handling:** 
  - Use Copilot to suggest robust error-handling routines, especially crucial during mod load/unload sequences.

- **XML Patch Utility:**
  - Copilot can assist in generating XML patch templates and error-checking snippets for easier mod integration.

This detailed guide provides a comprehensive starting point for utilizing GitHub Copilot in the development and enhancement of the Camouflage and Stealth (Continued) mod for RimWorld.

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

