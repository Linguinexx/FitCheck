# Acabamento das roupas e vestuário civil — 04/11/2026

As roupas da loja mantêm os 17 modelos Blender, a baggy anterior, IDs, drops, critérios e desbloqueios. Nenhum triângulo foi adicionado aos modelos do catálogo. Quatorze mapas UV de 512px foram refeitos a partir da receita original, com ruído mais discreto, trama, sombras largas de tecido e pesponto da barra. As letras agora vêm da Archivo Black já licenciada no projeto, em vez do alfabeto mínimo 5×7. As mangas usam acabamento de algodão coerente com o corpo; superfícies impressas têm trama leve do Fabric030 já embarcado. Borrachas e peças metálicas mantêm materiais sólidos. O arquivo Blender da coleção recebeu as quatorze texturas atualizadas, empacotadas.

Os moradores e o observador da sacada usam um guarda-roupa separado do catálogo: camiseta lisa, polo, moletom, camisa com bolsos e camiseta listrada, com calças/bermudas comuns e tênis casuais sem marca. Cores variam por morador. Não é uma recoloração das peças DripLab: a geometria e as texturas vêm do caminho de vestuário civil. As quatro texturas básicas têm 128/256px, mipmaps e compressão; são compartilhadas, sem geração por quadro. Frequentador da loja, cliente do atendimento e uniforme mantêm suas funções e roupas próprias.

Detalhes de gola/botões, calças e cada par de partes do calçado são combinados preservando materiais e grupos de movimento. O corpo, cabelo, mãos e rosto dos personagens são os anteriores ao protótipo de esqueleto do Blender; aquele protótipo continua separado do jogo. A borda final das mangas agora acompanha exatamente o tronco, fechando a abertura que aparecia ao levantar o braço e durante a caminhada.

## Verificação

Compilação Windows Unity 6000.6.7f1 sem erros/avisos. `-city-proof -wardrobe-finish-review` valida oito civis sem texturas/materiais do catálogo ou identificação da loja, orçamento de vértices, costuras de mangas em duas fases da passada, 63 fits em sete perfis, 17 miniaturas, araras, atendimento completo e saves separados. `-entry-regression` também passou, incluindo equivalência da deformação em cache contra a equação de referência, movimento de cabeça/tronco/braços/pernas/calçados e piscadas esféricas. Os testes não gravam nos saves do jogador.

## Comparação de desempenho

Mesma máquina, player Windows/D3D11, executável anterior EntryOptimized versus WardrobeFinish; `-city-proof -entry-performance`, 30 chamadas por ambiente, jogador parado.

| Ambiente | Entrada antes → depois (ms) | Atualização antes → depois (ms) |
| --- | ---: | ---: |
| home-cold | 82.2 → 128.3 | 0.01 → 0.01 |
| street-cold | 577.0 → 632.8 | 11.48 → 3.77 |
| shop-cold | 114.4 → 139.7 | 4.19 → 4.14 |
| home-return | 20.3 → 26.3 | 0.01 → 0.01 |
| street-return | 21.4 → 24.5 | 14.93 → 4.26 |
| shop-return | 21.5 → 25.9 | 4.13 → 4.15 |

Na cidade, renderers caíram de 449 para 411 e vértices de 1.125.311 para 1.089.420. A loja mantém 178 renderers e 189.524 vértices. O custo de atualizar a cidade caiu; a atualização da loja permaneceu perto de 4,1 ms. As primeiras entradas tiveram variação para cima de dezenas de milissegundos nesta rodada, inclusive na casa, e ainda são construções síncronas. São tempos de trechos de CPU e orçamento de geometria, não uma garantia de FPS completo ou de tempo de carregamento idêntico em todo computador. Não foram aumentadas resoluções nem adicionados cálculos de roupa por quadro.

Relatórios locais: `Captures/WardrobeBaseline/entry-performance.json`, `Captures/WardrobeFinish/entry-performance.json` e logs `tools/wardrobe-*-proof.log`. Receitas portáveis de UV e tecidos ficam em `Art/Garments/StreetwearCollection/paint_uv_finish.py` e `paint_basic_fabrics.py`; máscaras de letras derivam da TTF já incluída. Créditos da fonte e do tecido continuam em Art/Fonts e Art/AmbientCG.
