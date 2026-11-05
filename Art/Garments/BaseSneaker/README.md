# Tênis cotidiano fornecido pelo usuário — 05/11/2026

Fonte original, preservada: refs roupas/modelos tenis/Base sneaker/Base_sneaker.fbx e pasta Base_sneaker.fbm, fora de Assets. manifest.json registra SHA256, dimensões e contagens. Cópia editável otimizada: BaseSneaker.blend.

O modelo foi reduzido de 501.384 para 2.400 triângulos por pé, com basecolor reduzida em uma cópia 512 × 512. Um material e um renderer por pé. Sem importar o normal map de alta resolução. Silhueta cotidiana cinza/preta com sola clara, independente dos três tênis do catálogo DripLab. Os civis usam a malha/material em cache e mantêm o binding existente dos pés.

Reprodução: Blender --background --python-exit-code 1 --python prepare_base_sneaker.py. Requer os originais no workspace. O script emite FBX e JSON com posição/normal em coordenadas Unity e UVs por canto da malha; o runtime lê o JSON. Exportações em Assets/Resources/Garments/Civilian/CIV_SHOES_BASE. Evitar rebake com o gerador da coleção da loja.

Revisão inicial: -city-proof -sneaker-v2-review, saída Captures/BaseSneaker. Inclui catálogo da loja, atendimento e saves. Após feedback visual, a versão atual preserva proporções com escala uniforme e usa 2.400 triângulos. Revisão focada -city-proof -base-sneaker-review, saída Captures/BaseSneakerFit, valida proporções, orçamento/escala/textura, nove visuais civis, dois renderers por par e caminhada completa/piscadas. Resultado: tools/base-sneaker-fit-proof.log no workspace, saída 0.
