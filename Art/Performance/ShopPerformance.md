# Shop gameplay CPU optimization — 2026-11-04

The complaint was low FPS inside the shop. Previous pose-only and shader readback benchmarks did not include the complete gameplay Update. Added a native `-city-proof -shop-gameplay-performance` capture: normal engine Update/render/GUI, 30 warm-up frames then 180 samples each, standing, scripted walking, and walking looking down. Pixel filter off, garment finishes unchanged, 1440×900, 60 FPS target, same machine. JSON reports are adjacent.

| Scenario | Before Update median | After Update median |
|---|---:|---:|
| Standing | 8.81 ms | 2.43 ms |
| Walking forward | 9.10 ms | 2.42 ms |
| Walking looking down | 9.02 ms | 4.60 ms |

Full frame medians were capped at 16.67 ms in both builds on this machine. These are CPU Update savings, not a claim of a 73% FPS increase or a reproduction of every Editor stall. Rendering, other engine work and GUI remain included in the frame duration, but outside the timed Update segments. The walking input is a short oscillating probe; collision remains active.

Changes:
- Blend affine skin matrices using their twelve direct fields, avoiding repeated Matrix4x4 indexer dispatch. Geometry, UVs, normals, sleeves and animation equations remain unchanged.
- Advance player movement, pose clocks and root every frame. Defer invisible first-person body skinning until the camera pitches down past a conservative 20 degrees; restore the current pose immediately before it can enter view. External follow cameras always request the complete current body pose.
- Keep visible NPCs and the player seen from outside at full pose frequency. No resolution, texture or material reduction.

Validation: native gameplay probe, immediate look-down/external pose recovery, 475,191 optimized/reference vertex poses (maximum position difference 3.63e-7, normal difference 1.46e-7). Full garment/service/save regression is recorded in tools/shop-performance-regression.log and Captures/ShopPerformance/checks.txt. Existing source assets and saved progress are preserved.
