# User-supplied sneaker collection — 2026-11-05

Active editable source: SuppliedSneakerCollection.blend. Preparation script: prepare_supplied.py (Blender 5.0). Original FBX files and textures remain unchanged in the workspace refs roupas/modelos tenis directory, outside Assets. manifest.json records filenames, source hashes, triangle counts, orientation and target dimensions.

Runtime mapping preserves catalog IDs, drops, criteria and unlocks:

- AH_SHOES_01: Blue sneaker.
- SB_SHOES_01: Fire sneaker.
- CG_SHOES_01: Green sneaker.

Each supplied model had approximately 501,000 triangles. Optimized copies have 2,500 / 2,500 / 2,496 triangles per foot, one material and one renderer per foot, and a 512-pixel base-color atlas. Original high-resolution textures and models are not imported into Unity. No extra normal-map or reflection pass was added. Civilian footwear remains separate.

Reproduce by running Blender --background --python-exit-code 1 --python prepare_supplied.py, then FitCheck.Editor.StreetwearBake.BakeShoesAndBuild in Unity. The Unity bake is required: single-mesh FBX roots carry unit/axis transforms, which the baker preserves. Keep the resulting canonical Unity-baked JSON beside each runtime FBX/PNG in Assets/Resources/Garments/Streetwear.

Native review: -city-proof -sneaker-v2-review; Captures/SuppliedSneakers/collection-shoes.png. All 63 mixed fits, seven profiles, thumbnails, first-person body/movement, eight loading transitions, home/shop service and three save slots passed. The original custom collection remains in ../SneakerCollection for recovery.
