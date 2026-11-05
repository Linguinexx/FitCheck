# Sneakers, diagonal loading and contextual speech — 2026-11-05

Three Blender-authored catalog sneakers replace the previous shoe geometry while preserving IDs, criteria and unlocks. Authoring source, construction notes and geometry budgets are in Art/Garments/SneakerCollection. Total shoe triangles fell from 4,916 to 3,178; three 512 atlases replace the previous flat shoe materials. Pairs retain six renderer components with one material each. Civilian basics remain independent.

The loading background now has finite brighter strokes moving along the stripe axis, with a dim continuous diagonal underneath. This provides a directional cue; uniform infinite stripes can look as if they move sideways even when translated down. Minimum total duration remains two seconds, and reduced motion freezes the pattern.

Contextual ambient bubbles and fitting/departure captions omit the loose tail. Counter dialogue keeps the connected left tail. Bubble shadow follows the entry opacity, and zero-opacity frames draw nothing. Native screenshot comparison found zero changed pixels outside the contextual bubble/shadow bounds.

The longer route review exposed a pedestrian touching the player's rectangular blocker while outside the previous 1.0-unit avoidance circle. Civilian standoff is now 1.16 units, covering the .78 x .73 half-extents; scripted shop customers retain their existing behavior. Manual movement speeds and controls are unchanged.

Native checks passed: 63 fits, all thumbnails, shoe/profile geometry and texture budgets, player looking down and walking, contextual bubble pixel bounds, eight loading transitions, complete home/shop service and save isolation. Log: tools/sneakers-v2-final-proof.log. Preview: Captures/SneakersV2/collection-shoes.png.
