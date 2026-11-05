# Sneaker collection — 2026-11-05

Previous hand-authored version, preserved for recovery. Runtime shoes now use the optimized user-supplied models documented in ../SuppliedSneakers/README.md. Running this older generator would replace the current runtime exports.

Editable source: SneakerCollection.blend. Generator: model_sneakers.py, executed with Blender 5.0 in background mode. It creates a separate scene and does not modify the character source.

- AH_SHOES_01 / Sidewalk: cream canvas, denim toe/heel and sewn star; interpreted from the two court/star references.
- SB_SHOES_01 / Garage Session: dark skate silhouette, raised padded collar, broad orange ribbon laces and rubber bumper; interpreted from 00-e.jpg.
- CG_SHOES_01 / Daily Steps: green platform midsole, round upper and three heatmap side overlays; interpreted from the LaFrance reference.

All three retain their catalog IDs, names, criteria and unlock rules. Civilian shoes remain separate unbranded basics.

Geometry: 1,118 / 1,114 / 946 triangles per shoe (3,178 total, previously 4,916). Each has one generated 512 textile/material atlas, with point sampling and mipmaps in Unity. Upper, sole and laces each share one cached material, giving six single-material renderers per pair. No extra shader pass, reflection capture or high resolution texture.

Reproduction: run Blender --background --python-exit-code 1 --python model_sneakers.py. This writes FBX/PNG and a draft JSON snapshot. In Unity, run FitCheck.Editor.StreetwearBake.BakeShoesAndBuild to bake the three runtime JSON files through the actual Unity FBX transforms/UVs/normals. Other collection pieces are not rebaked. Keep the canonical Unity-baked JSON beside each FBX.

References are the four user-supplied files in refs roupas/refs tenis. Product photos were viewed for construction/color/silhouette; they are not embedded or shipped as textures. All atlas pixels and modeled meshes here were authored for this project. The Blender file embeds its generated atlas images.

Native QA: -city-proof -sneaker-v2-review. Three shoe closeups, 63 mixed fits across seven body profiles, moving player feet, thumbnails, bounded triangle/renderer/texture budgets, context bubble exterior pixel check, eight loadings, home/shop customer flow and three independent saves passed. Captures/SneakersV2/collection-shoes.png is the in-game preview.
