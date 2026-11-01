# Materiais do bairro noturno

Geração: ferramenta integrada image_gen, modo padrão; não foi usada API/CLI externa. Atlas original produzido para este projeto.

Arquivo usado no jogo: Assets/Resources/CityNightAtlas.png
Cópia original: Art/CityNight/material-atlas-original.png

Importação Unity com resolução máxima 512, filtro point e mipmaps. O shader usa amostras em blocos de 128 por material e margens internas para evitar misturar quadrantes. Reboco, tijolo, metal e asfalto usam regiões independentes; não substituem os materiais dos personagens. O atlas é estático. Vento e atividade do bairro usam um relógio contínuo separado, respeitando pausa e movimento reduzido.

## Prompt final

Use case: stylized-concept. Create ONE production game texture atlas, square 1024x1024, laid out as an exact 2-column by 2-row grid of four equally sized seamless material swatches touching edge to edge, no gutters, no borders, no text. This is a flat albedo texture sheet for a vibrant but gritty PS1/early PS2 Brazilian streetwear neighbourhood at night; NOT a render of objects, NOT a scene, NO perspective, no lighting baked as gradients. Top left quadrant: warm ivory weathered plaster, convincing patchy worn limewash, visible rough aggregate, irregular cracks, small missing paint revealing gray-beige cement. Top right quadrant: exposed warm terracotta brickwork, staggered courses, dirty offwhite mortar, chipped edges and individually varied bricks. Bottom left quadrant: charcoal painted metal shutter, horizontal thin corrugation, worn red paint scratches and chips, subtle warm rust patches, raised edges. Bottom right quadrant: neutral gray gritty asphalt, small irregular gravel and worn aggregate, subtle tar repairs. Strong readable medium-size painted surface details and pixel clusters, hand painted late-1990s videogame texture style, limited earthy palette, deliberately low resolution detail, crisp shapes. No cyan, blue, teal or green cast; no words, no symbols, no labels, no decorative scene framing. Each quadrant must fill its exact quarter completely, with roughly uniform average brightness in each swatch and tiling-compatible edges. The resulting bitmap will be directly mapped as four distinct surface materials in a Unity game.

## Prévia de movimento

Captures/CityNight/night-city-motion.mp4: 90 quadros nativos, 640×360, exportação compacta a 10 fps. O executável atualiza animações a cada frame; a taxa do arquivo de prévia não define a taxa do jogo.
