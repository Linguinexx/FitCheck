# Tênis padrão dos civis — 05/11/2026

O Base_sneaker.fbx fornecido pelo usuário tinha 501.384 triângulos. A cópia preparada no Blender tem 2.400 por pé, textura 512 e um material/renderer por pé. O original e as texturas de referência não foram alterados; ficam fora de Assets. O runtime mantém modelo e templates em cache, reutilizando o binding existente de pés. A compactação evita recombinar um pé já formado por um único renderer.

ApplyCivilian usa exclusivamente CIV_SHOES_BASE; os três IDs dos tênis da loja permanecem independentes. A revisão dos trajes de chegada dos clientes e dos trajes comuns do player permanece pendente, conforme PROXIMOS_PASSOS.md.

Validação nativa em Captures/BaseSneaker: nove visuais civis, escala/UV/textura, dois renderers por par, movimento de cabeça/tronco/braços/pernas/calçados e piscadas. A mesma revisão inclui 63 fits da coleção, corpo em primeira pessoa, loadings, percurso casa/loja, atendimento e isolamento dos saves. Resultado completo em tools/base-sneaker-proof.log no workspace.

O orçamento de geometria/material evita importar a malha original densa; não representa uma medição de FPS do jogo completo.

Correção após feedback visual: a primeira conversão alargava o modelo em cerca de 20% e achatava em cerca de 16% relativamente ao comprimento original. A versão revisada usa escala uniforme, mantendo a relação largura/comprimento 0,40146 e altura/comprimento 0,47678. A redução de 1.200 foi suavizada para 2.400 triângulos para preservar cadarços e contorno da sola; permanecem uma textura 512 e um material por pé. Revisão focada: -base-sneaker-review, Captures/BaseSneakerFit e tools/base-sneaker-fit-proof.log.
