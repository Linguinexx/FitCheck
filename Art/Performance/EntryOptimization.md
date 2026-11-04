# Otimização de entrada e animação — 04/11/2026

Removido o custo de construir/copiar um avatar inteiro para cada roupa ou tênis da loja. Araras usam modelos estáticos da peça pedida, combinados e reutilizados. A coleção Blender mantém as mesmas posições, normais, UVs, triângulos e cores; vértices só são unidos quando todos esses atributos coincidem.

As peças têm moldes imutáveis em cache, com materiais próprios por aparência para evitar que a cor de um NPC altere outro. Roupas exibidas nas araras têm materiais independentes. A cópia dos moradores usa matrizes preparadas por objeto. Cabeça, cabelo, olhos e acessórios rígidos se movem pelo transform; só as pálpebras mudam de forma durante a piscada. Bocas mantêm a animação existente.

A deformação de braços e pernas usa pesos constantes e matrizes reutilizadas, mantendo a mesma equação do movimento anterior. A atualização repetida do corpo do jogador entre movimento e câmera foi removida; as piscadas continuam com relógio independente. Moradores próximos (16 m), ou no cone visível até 50 m, mantêm atualização completa. Mais longe, a pose atualiza a 10 Hz; fora da câmera, a 4 Hz. Rotas, posição, velocidade e relógios continuam avançando em todo quadro. O frequentador e os clientes da loja sempre atualizam integralmente.

## Medições reproduzíveis

Unity 6000.6.7f1, player Windows x64, Direct3D 11, GTX 1650. Mesma máquina e roteiro antes/depois, processo novo para cada medição. `-city-proof -entry-performance` mede seis entradas e 30 chamadas a `CityView.Frame` por ambiente. `cold` é a primeira construção do ambiente; `return` é o retorno à instância existente.

| Ambiente | Entrada em ms: antes → depois | Atualização da cidade em ms: antes → depois |
| --- | ---: | ---: |
| home-cold | 212.0 → 147.9 | 33.02 → 0.01 |
| street-cold | 998.4 → 723.7 | 300.86 → 11.87 |
| shop-cold | 681.1 → 193.7 | 66.86 → 4.17 |
| home-return | 105.6 → 21.0 | 33.45 → 0.02 |
| street-return | 103.9 → 23.5 | 299.64 → 14.97 |
| shop-return | 107.5 → 22.0 | 66.91 → 4.25 |

Estes são tempos de trechos de CPU, não uma medição de FPS completo. O loop inclui moradores e câmera, com o jogador parado; renderização, movimento do jogador, driver, Editor e pressão de memória do Windows afetam o desempenho percebido. A primeira construção da cidade ainda é síncrona e custa cerca de 0,75 s nesta máquina; portanto, a troca inicial não é instantânea.

Na loja, renderers passaram de 1.447 para 178 e vértices de 694.985 para 189.524, removendo as partes ocultas dos avatares das araras. Materiais carregados após visitar os três ambientes passaram de 2.648 para 717. Memória alocada reportada pela Unity na primeira loja caiu de cerca de 343 MB para 265 MB (não inclui toda a memória do sistema ou da GPU).

## Verificação

`-city-proof -entry-regression` valida:

- 17 peças Blender, todas as miniaturas, 63 combinações em sete perfis, encaixe de manga/mão e calçados.
- 1.616.949 posições de vértices contra a equação de animação anterior, com erro máximo inferior a 0,000001 unidade; normais também equivalentes.
- Caminhada de cabeça, tronco, braços, pernas e sapatos; piscadas dos moradores e jogador; corpo visível ao olhar para baixo.
- Nove peças nas araras, afastadas do provador; atendimento completo, espera no balcão, câmera do provador, saída e FitFeed.
- Três saves isolados, recuperação de backup e proteção do progresso legado.

Compilação final sem erros ou avisos. Relatórios locais: `Captures/EntryBaseline/entry-performance.json`, `Captures/EntryOptimized/entry-performance.json`, `Captures/EntryOptimized/checks.txt`. Esses arquivos, builds e caches não são versionados. Roupas, texturas, fontes e saves não foram removidos pela otimização.
