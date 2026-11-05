# Opposite frontage and speech contrast — 2026-11-04

The previously deferred street frontage and customer-bubble hover fix were authorized on 2026-11-04.

Added two north-facing commercial modules across the avenue from DripLab: SIDE B record shop and a bicycle repair workshop, with upper-floor housing, display records/tools/wheels, awnings, a bench, bicycle and two planters. Reuses city geometry batches, texture atlas and sign atlas. No new animated actors, texture files, Update loops or real-time lights. City renderer count remains 411. Existing first-person player optimization is retained.

Text labels explicitly apply their requested ink to all eight GUIStyle states; speech remains dark on its paper background while hovered, focused or active. Button rendering remains independent. The final native speech crop is pixel-identical with/without hover: 2,172 dark text pixels and 62,121 paper pixels in the inspected area.

Native checks: new shops have collision; pedestrian routes reach both fronts and the passage between the buildings. Home–store walking remains 31.50 s outward and 31.10 s return. Existing districts, residents, pause, complete shop service and three independent saves passed. Visual captures in Captures/StreetFrontage; log tools/street-frontage-final-proof.log. The final visual capture waits until speech opening animation is complete.

Future task recorded in AGENTS.md: loading themes/screens on Play and entering/leaving home/shop. Not implemented in this pass.
