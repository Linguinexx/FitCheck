# Provador — revisão do interior

Aplicado no provador físico da loja: piso mineral em escala contínua, tapete com acabamento Kaos, paredes claras texturizadas, ripado com veios verticais, moldura metálica do GIF, banco estofado com pés e travessas conectados, ganchos, pequena prateleira e luminária de acabamento quente. Cortina com trama de tecido e bainhas mais escuras; a personalização conserva essas bainhas ao trocar a cor.

O painel abstrato e suas duas cores continuam personalizáveis. A nova decoração usa as texturas já presentes no projeto (Concrete034, Fabric084, Wood051 e Carpet016), sem importar mapas novos, alterar roupas ou acrescentar luzes em tempo real. Limites de interação e obstáculo do banco preservados.

## Verificação

Renderização Unity 6000.6.7f1/D3D11 em projeto isolado, com bootstrap da campanha desativado: sem leitura/escrita nos saves do jogador. Conferidos interior, enquadramento do personagem e parede dos ganchos; cortina fechada bloqueia a entrada e aberta libera a passagem. Trocas de cor do tecido e do GIF verificadas. Prévias locais sem filtro de pixelização em `Captures/FittingRoom/Final`.

| Cena completa da loja | Antes | Depois |
|---|---:|---:|
| Triângulos de MeshFilters | 162.559 | 164.889 |
| Grupos estáticos por material | 14 | 15 |
| Renderers, incluindo atores | 131 | 132 |

Diferença: 2.330 triângulos (+1,43%) e um grupo estático adicional. Isso mede geometria, não FPS; não houve teste de desempenho em hardware do jogador. Os mapas reutilizados mantêm as configurações de importação existentes. A execução isolada registrou uma exceção do indexador `UnityEditor.Search` também presente na captura anterior; não houve erro de compilação dos scripts ou falha da revisão do provador.

Alterações disponíveis nas fontes da Unity. Nenhuma nova build Windows/web foi gerada neste pedido; ZIPs anteriores permanecem como estavam.
