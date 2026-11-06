# User-supplied sneaker collection — 2026-11-05

Active editable source: SuppliedSneakerCollection.blend, with the optimized meshes and packed textures. The original input folder was removed after the Blue sneaker 2.0 replacement. manifest.json retains the input filenames, hashes, triangle counts, orientation and target dimensions.

Runtime mapping preserves catalog IDs, drops, criteria and unlocks:

- AH_SHOES_01: Blue sneaker 2.0, with its DripLab tongue lettering preserved.
- SB_SHOES_01: Fire sneaker.
- CG_SHOES_01: Green sneaker.

Each supplied model had approximately 501,000 triangles. Optimized copies use up to 2,500 triangles per foot, one material and one renderer per foot, and a 512-pixel base-color atlas. Civilian footwear remains separate.

For future edits, start from the optimized Blender collection and export the selected shoe at the origin. The preparation script is retained for imports when raw sources are available. The Unity bake preserves unit/axis transforms on single-mesh FBX roots. Keep the Unity-baked JSON beside each runtime FBX/PNG in Assets/Resources/Garments/Streetwear.

Native review: -city-proof -sneaker-v2-review; Captures/SuppliedSneakers/collection-shoes.png. All 63 mixed fits, seven profiles, thumbnails, first-person body/movement, eight loading transitions, home/shop service and three save slots passed. The original custom collection remains in ../SneakerCollection for recovery.
