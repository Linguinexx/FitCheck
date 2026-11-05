# Supplied sneakers and lateral loading — 2026-11-05

User-supplied Blue, Fire and Green FBXs were optimized in Blender and integrated into the existing three shoe catalog entries. Raw inputs are unchanged and remain outside Unity. Active editable source: Art/Garments/SuppliedSneakers/SuppliedSneakerCollection.blend.

Native measured geometry: 2,500 / 2,500 / 2,496 triangles per shoe, two single-material renderers per pair, 512 atlases. The supplied sources were approximately 501k triangles each. Compared with the previous custom shoes, the new versions use more triangles but reduce renderers from six to two per pair; this is a bounded asset budget, not a full-game FPS guarantee.

The FBX baker now retains unit/axis correction on a single-mesh root. Runtime dimensions and toe direction passed validation. Resident artwork revision was advanced to invalidate cached earlier footwear.

Loading restores the previous continuous diagonal bands moving sideways at 24 logical pixels/second. Two-second duration, logo animation, reversible dots and reduced-motion behavior remain.

Unity 6000.6.7f1 build passed (tools/supplied-sneakers-final-build.log). Native proof exited 0 (tools/supplied-sneakers-proof.log): 17 garment assets; 45,966 total triangles; 63 fits across seven profiles; first-person body/movement; speech-tail bounds; all eight loadings; home commute; full shop service; three independent save slots. Screenshots were inspected in Captures/SuppliedSneakers.
