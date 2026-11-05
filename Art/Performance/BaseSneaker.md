# Tênis padrão dos civis — 05/11/2026

O Base_sneaker.fbx fornecido pelo usuário tinha 501.384 triângulos. A cópia preparada no Blender tem 1.200 por pé, textura 512 e um material/renderer por pé. O original e as texturas de referência não foram alterados; ficam fora de Assets. O runtime mantém modelo e templates em cache, reutilizando o binding existente de pés. A compactação evita recombinar um pé já formado por um único renderer.

ApplyCivilian usa exclusivamente CIV_SHOES_BASE; os três IDs dos tênis da loja permanecem independentes. A revisão dos trajes de chegada dos clientes e dos trajes comuns do player permanece pendente, conforme PROXIMOS_PASSOS.md.

Validação nativa em Captures/BaseSneaker: nove visuais civis, escala/UV/textura, dois renderers por par, movimento de cabeça/tronco/braços/pernas/calçados e piscadas. A mesma revisão inclui 63 fits da coleção, corpo em primeira pessoa, loadings, percurso casa/loja, atendimento e isolamento dos saves. Resultado completo em tools/base-sneaker-proof.log no workspace.

O orçamento de geometria/material evita importar a malha original densa; não representa uma medição de FPS do jogo completo.
