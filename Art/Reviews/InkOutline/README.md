# Prévia de contorno — 17/11/2026

Experimento aprovado para comparação, ainda desligado por padrão. Não foi adotado no jogo publicado nem substituiu as builds Windows/Web. Saves e progressão não são modificados.

## Experimentar na Unity

Entre no Play e use Tools → Fit Check → Prévia de contorno → Ativar no Play. Para comparar, escolha Restaurar original. Ao sair do Play ou recarregar scripts, a prévia restaura os shaders. Trocas de cabelo/roupa são acompanhadas no editor, com verificação a cada 0,5 s somente enquanto a prévia estiver ativa.

Apenas avatar, moradores/clientes, corpo/mãos do jogador e pets recebem o efeito. Paredes, rua, UI, cenário e textura do espelho mantêm seus shaders. Pupilas, bocas e overlays faciais são excluídos. O reflexo real desenha o mesmo personagem com contorno.

## Técnica e orçamento

Wrappers de shaders reutilizam os passes originais de pintura, preservando texturas, cores e acabamentos. Casca invertida em espaço de tela: 1,15 px de referência a 720p, limite físico 0,012 m e redução entre 12 e 28 m. Proporção acompanha a resolução do espelho. Sem duplicar malhas, renderizadores, materiais, luzes ou texturas. Há custo de desenho adicional: roupa/pet de um passe ganham outro; RetroPaint substitui seu passe de borda existente.

Nenhum shader de prévia é atribuído em gameplay normal. O único ajuste no shader base CatFur é nomear seu passe existente PAINT para reutilização. Materiais clonados durante o experimento também voltam ao shader original.

## Validação

Compilação nativa sem erros/avisos. INK_OUTLINE_PREVIEW_PASSED: padrão desligado, seleção dos atores, sem duplicação de modelos/materiais, material clonado restaurado, cinco situações (provador, loja, pet, primeira pessoa e espelho real), progresso isolado e restauração ao final. Comparação de pixels: original e restaurado idênticos nas cinco capturas; imagens com contorno mudaram apenas cerca de 0,3–0,5% do quadro. Evidências em Captures/InkOutline e pixel-checks.json.

Amostra de renderização: GTX 1650, D3D11, alvo 1152×720, espera por readback de 1 pixel, aquecimento e 24 pares em ordem alternada AB/BA. Medianas registradas em render-cost.json. A amostra curta sofre variação de clocks, filas e sincronização; NÃO estabelece ganho/perda de FPS ou custo confiável durante o jogo. Não usar a mediana menor do modo com contorno como alegação de otimização. Web/GLSL e desempenho em gameplay prolongado continuam para a etapa de adoção, se o visual for aprovado.

Fontes: InkOutlinePreview.cs, InkOutlineChecks.cs, Editor/InkOutlineReview.cs e Resources/InkPreview*.shader. Execução isolada: InkOutlineReview.Build e -city-proof -ink-outline-review -proof-output <pasta>. Comparativos PNG e imagens completas preservados; builds públicas do mural mantidas.
