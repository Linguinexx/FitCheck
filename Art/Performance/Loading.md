# Branded loading — 2026-11-04

Requested by the user for Play and home/shop entry/exit. Current treatment (restored 2026-11-05): black background with continuous low-opacity Kaos diagonal stripes sliding sideways at 24 logical pixels/second, repeating every 400 pixels; official red logo reuses the menu pulse/float function and texture; Loading below it has reversible 0,1,2,3,2,1 dots at 0.14 s intervals. Reduced motion freezes the logo, stripes and dots. No new texture, shader, audio clip or external dependency.

A coroutine displays the cover for two frames before executing destination preparation. FirstCustomer gameplay Update and interaction/pause controls remain blocked while covered; repeated travel requests are ignored. Destination preparation is currently synchronous on the main thread under this cover, not asynchronous scene streaming. Short loads have a 1.82 s minimum readable cover and 0.18 s reveal (2.00 s total); longer preparation takes as long as required without displaying a fabricated percentage. Cursor state is restored for the destination, including first-person capture.

Native review flag: -city-proof -loading-review. Covers Play → three saves, empty slot → creator, saved creator → first-person home, home exit, shop entry, shop exit, home entry with reduced motion, existing save → home; duplicate requests/input gating; complete prior home/service/save checks. Screen captures in Captures/Loading, log tools/loading-release-proof.log.

Latest native validation: Captures/SuppliedSneakers, tools/supplied-sneakers-proof.log. All eight loading transitions and home/service/save regression passed. Supplied sneaker integration is documented in SuppliedSneakers.md.
