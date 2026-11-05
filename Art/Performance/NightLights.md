# Night lighting, facade corners and loading motion — 2026-11-04

Both DripLab planters now align with the two facade corners at local X .05 and 14.95, Z 2.2; vegetation and collision move together. The right planter clears the palm/post at X 17.8. Green foliage remains independent of architecture tint.

The neighborhood has 26 registered light sources: warm street posts and residential entry, amber shop lighting, Kaos red signage spill, violet record/music venue accents, green workshop and cool gallery/court accents. A 256x160 RGBA field (160 KiB) is generated once and sampled in world coordinates by city surfaces, windows, skin and clothing. Moon direction matches the sky crescent and adds cooler directional shading. Interiors retain their previous light direction and color; street lighting is disabled there.

Soft depth-tested additive halos for all sources share one 104-vertex mesh/renderer. LED housings share existing static batches. No real-time Unity Light/shadow passes, per-frame light selection, bloom render targets or new downloaded texture maps. Existing texture quantization, animated sky and reduced-motion behavior are preserved.

Same-executable render/readback probe at 1280x720: steady off sample 1.2002 ms, on samples 1.1859/1.1513 ms; initial off sample 13.3634 ms includes cold setup and is not representative. Differences are within run noise. This probe measures camera rendering and synchronous one-pixel readback, not complete gameplay FPS; it does not guarantee identical performance in the Editor or other hardware.

Loading lasts at least 2.00 seconds total (1.82 cover + .18 reveal), with extra time only when preparation needs it. Diagonals move downward at 130 logical px/s and loop seamlessly; logo/dots remain unchanged. Reduced motion freezes all decorative motion.

Navigation review helpers now tick pedestrians/traffic while walking rather than freezing dynamic blockers. Planner cells/line interiors keep 4 cm extra static clearance to avoid exact-boundary corner stalls at the plaza planter. Manual movement collision and speed stay unchanged.

Native review: -city-proof -night-lighting-review; captures in Captures/NightLights. Source/halo/color budget, moved planter collision, traffic wheels/tunnels, home/shop isolation, descending loading offsets, all eight loading transitions and complete home/service/save regression are checked. See tools/night-lights-corner-proof.log.
