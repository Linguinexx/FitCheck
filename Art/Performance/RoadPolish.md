# Car wheels, loading duration and storefront foliage — 2026-11-04

Replaced the cylinder-plus-cross hubcaps with a shared wheel mesh: 20-segment tire with beveled shoulder/tread, recessed five-spoke alloy rim, center hub and five bolts. 1,520 vertices are shared by all four wheels. Two existing city materials are shared too; one renderer per wheel replaces three (eight fewer renderer components, eight wheel material submissions instead of twelve). No new texture, shader, light or per-frame geometry generation. Tire radius .39 brings the contact patch closer to the ground; existing traffic rotation and collision/tunnels remain unchanged.

Storefront planters and cafe terrace foliage previously inherited the architecture's neutral Teal constant. They now use a dedicated green leaf color. Other architecture colors/materials are preserved.

Loading minimum cover increased from .60 to 1.20 seconds, followed by the existing .18-second reveal (about 1.4 seconds for a ready destination). Actual longer preparation is still awaited. Logo, diagonal treatment and reversible dots are unchanged.

Native road-polish review passed: shared mesh/material counts and bounded profile, four wheels, rotation while moving and no rotation when stopped, green vertex colors in both DripLab facade planters, pavement/vehicle-only tunnels, eight loading routes and minimum time, reduced motion, complete home/shop service and three independent saves. Captures inspected at Captures/RoadPolish; native log tools/road-polish-proof.log.
