# Registro de testes — protótipo K7

Plataforma: Unity Editor 6000.6.7f1, Windows. Data: 30/10/2026.

| ID | Procedimento executado | Resultado observado |
|---|---|---|
| T01 | Menu Validate first customer após último ajuste de UI | FITCHECK_CHECKS_PASSED emitido |
| T02 | Em Play Mode, Run runtime smoke check | Marcador FITCHECK_RUNTIME_PASSED emitido |
| T03 | T02 equipa Last Stop + Late Shift + Sidewalk sem acessório e envia | Nota 10, +100 de Reputação |
| T04 | T02 captura PNG, serializa JSON, relê e reabre post | PNG de 480 px recuperado; teste isolado do save do jogador |
| T05 | Consulta do Console após T02 | Zero entradas de erro |
| T06 | Captura de tela inicial | Imagem inspecionada; personagem e controles visíveis |
| T07 | Repetir smoke check após mudança de material e rolagem | FITCHECK_RUNTIME_PASSED; Console sem erros |
| T08 | Gerar build Windows de desenvolvimento corrigido | Build succeeded, zero erros e dois avisos; 153,33 MB |
| T09 | Iniciar build corrigido fora do editor e examinar log de inicialização | Inicialização concluída; erro de shader não reapareceu. Fluxo por cliques no executável ainda não validado |

T02–T04 verificam chamadas de lógica em execução; não simulam todos os cliques do jogador. Não equivalem a teste completo de usabilidade, de build Windows ou de navegador.

## Pendências

- Compilação após adicionar rolagem: concluída, Console sem erros; revalidar os cliques manualmente.
- Testar manualmente as duas perguntas, continuar sem pergunta, abertura simultânea de pedido/drops, confirmação/cancelamento e pausa.
- Testar build Windows e persistência após encerrar o processo.
- Testar armazenamento web quando o módulo Web Build Support estiver disponível.
- Conferir clareza de cor/modelagem após substituir os modelos provisórios.

Nenhum bug de gameplay foi marcado como resolvido sem reprodução e reteste. A rolagem é ajuste preventivo de layout.

## Bugs encontrados

| ID | Plataforma e reprodução | Observado | Correção e estado |
|---|---|---|---|
| B01 | Primeiro build Windows; iniciar executável | ArgumentNullException no material: Shader.Find("Standard") retornava null, impedindo criar o cenário | Material explícito em Resources. Build corrigido iniciado novamente: erro não reapareceu no log. Resolvido nesse reteste |

## 30/10/2026 — Ampliação ao primeiro expediente

| ID | Procedimento | Resultado observado |
|---|---|---|
| T10 | Validate first customer com validação do dia | Passou: soluções iniciais Bia/Pico, retirada de pontos por boné/bermuda/cor, piso zero e marco preservado após perda |
| T11 | Fechar dia duas vezes e tentar quarto atendimento na validação | Seis prêmios sem duplicação; quarto atendimento rejeitado |
| T12 | Smoke check em Play Mode com três clientes | FITCHECK_RUNTIME_PASSED: três notas 10, total 300, três fotos PNG de 480 px recuperadas e seis recompensas |
| T13 | Serializar/recuperar fechamento e validar contribuições | Passou no teste de lógica e no teste em execução |
| T14 | Consultar Console após T12 | Nenhuma entrada de erro retornada |
| T15 | Captura da tela inicial com post legado | Menu oferece continuar expediente e consultar K7; texto e controles visíveis |

Pendentes: migração com reinício real do processo, saída no meio do provador, todas as perguntas/cliques, resolução pequena, erros de armazenamento, execução Windows completa e navegador. T10–T13 são testes de lógica e chamadas diretas em execução; não simulam cliques.

| ID | Procedimento | Resultado observado |
|---|---|---|
| T16 | Gerar build Windows do primeiro expediente | Build succeeded, zero erros, dois avisos, 153,35 MB; executável em Builds/Windows-DayOne. Ainda não validado por cliques fora do editor |

## 30/10/2026 — Nova apresentação do estúdio

| ID | Procedimento | Resultado observado |
|---|---|---|
| T17 | Validação de regras após separar visual/interface | Passou; critérios e progressão preservados |
| T18 | Smoke check em Play Mode com as novas malhas | Três fotos PNG recuperadas, notas e fechamento verificados |
| T19 | Iniciar prévia temporária e voltar ao início por chamadas de execução | Estado serializado da campanha idêntico antes/depois |
| T20 | Capturas da nova apresentação | Inspecionados personagem, luz, texto, catálogo e instruções; corrigidos enquadramento e sobreposição |
| T21 | Tentativa de abrir executável pela ferramenta de controle do computador | Ferramenta falhou: accessibility window-opened handler did not become ready. Não conta como teste do executável |

Arrasto, rolagem, Esc, confirmação e navegação foram implementados, mas o percurso completo com entrada real de mouse/teclado ainda precisa de validação. Esta versão não está marcada como arte final.

| T22 | Repetição das verificações após ajustes finais | Regras, fotos, serialização e isolamento da prévia passaram; Console sem erros |
| T23 | Build Windows-Studio final | Sucesso, zero erros e zero avisos; 153,36 MB. Execução interativa ainda pendente |

## 30/10/2026 — Campanha de nove atendimentos

| ID | Procedimento | Resultado observado |
|---|---|---|
| T24 | Validate full campaign | Nove soluções iniciais e todas as combinações do catálogo inicial retornaram notas permitidas pelo modelo 4/3/3 |
| T25 | Três dias perfeitos, fechamento repetido e progressão | 900 de Reputação, 21 peças disponíveis, três conquistas; sem duplicação de recompensas |
| T26 | Replay após penalidade absorvida pelo zero | Melhorar para 10 deu +100, sem crédito artificial de +130; repetição igual não deu novo ganho |
| T27 | Replay com peça desbloqueada após atendimento original | Rejeitado; inventário original preservado |
| T28 | Migração de save versão 1 | Convertido para versão 2, com 11 peças originais registradas |
| T29 | Corromper arquivo de teste após gerar backup e recuperar | Backup válido carregado e regravado; teste isolado em pasta temporária |
| T30 | Run full campaign runtime check em Play Mode | Nove fotos PNG de 480 px e nove saves recuperados; campanha concluída |
| T31 | Simular pausa, confirmação e expiração do relógio da Malu | Pausa congelou; confirmação consumiu tempo; zero enviou o visual atual |
| T32 | Simular eventos de arrasto, clique e rolagem no manipulador real | Arrasto girou sem abrir pedido; clique abriu pedido; zoom respeitou limite; pausa bloqueou arrasto |
| T33 | Construir/renderizar todas as roupas e miniaturas | 21 peças aplicadas; 21 miniaturas normais e 21 bloqueadas disponíveis |
| T34 | Consulta do Console após verificação em execução | Zero erros retornados |

T30–T33 usam chamadas e eventos simulados dentro do editor. Não equivalem a percurso completo com mouse/teclado reais nem a teste com jogadores. A versão navegador permanece sem validação.

| T35 | Melhorar replay e carregar o backup anterior | Novo resultado usa outro PNG; backup continua apontando para a imagem antiga, e ambas existem |
| T36 | Revisão de capturas após renderização completa do quadro | Casa, provador com miniaturas reais e retrospectiva das nove publicações inspecionados; conteúdo legível e cenários diferenciados |

| T37 | Gerar build Windows-Campaign | Sucesso: 153,41 MB, zero erros e zero avisos. Percurso interativo fora do editor ainda pendente |

## 31/10/2026 — Validação do passe de arte

| ID | Procedimento | Resultado observado |
|---|---|---|
| T38 | Validate retro art | Shader suportado e sem erros; seis atlas de quatro quadros em filtro pontual, sem mipmaps/compressão |
| T39 | Comparar quadros de pintura renderizados | Mais de 70 mil pixels diferentes no ensaio inicial; variação limitada, sem mudança de cor média relevante no tecido |
| T40 | Renderizar duas vezes o quadro congelado | Imagens idênticas no teste; modo estático estável |
| T41 | Atualizar foto de uma revisão anterior em pasta isolada | Nota, Reputação, pergunta e quantidade de posts preservadas; nova foto criada e imagem anterior mantida no backup |
| T42 | Repetir validações da campanha após materiais/malhas | Nove pedidos, catálogo, relógio, replays e salvamento passaram novamente |
| T43 | Revisão da composição e leitura da interface | Mundo pixelado separado da camada nativa; captura com nomes, categorias, cadeados e confirmação legíveis |
| B02 | Tentativa de clonar fonte dinâmica | Unity informou clonagem não suportada e renderizou texto incorretamente; corrigido para recurso interno direto, caracteres pré-carregados e estados de cor restaurados. Nova sessão sem erros de fonte |

Validação e revisão em Unity Editor. O percurso completo por cliques reais no novo executável e os testes com jogadores ainda estão pendentes.

| T44 | Gerar build Windows-ArtPass | Sucesso: 153,72 MB, zero erros e zero avisos; versão anterior preservada |

## 31/10/2026 — Revisão de movimento e avatar

| ID | Procedimento | Resultado observado |
|---|---|---|
| T45 | Renderizar fases 0, 0,5 e 1 da pintura | 50.336 pixels intermediários distintos dos extremos; eliminada troca rígida |
| T46 | Comparar fase 3,99 com fase 0 | Diferença média abaixo de 0,05 por canal; ciclo sem salto |
| T47 | Repetir quadro congelado e migração de foto | Estabilidade e preservação do progresso passaram, revisão de foto 3 |
| T48 | Repetir campanha após mudanças de proporção | Nove atendimentos, 21 peças, relógio, pausa, replays e salvamento passaram |
| T49 | Inspecionar capturas frontal e lateral | Boca visível, rosto oval, proporções por perfil e óculos reposicionados; capturas na pasta Captures |

A percepção da fluidez e a aprovação da nova direção de avatar dependem da avaliação do usuário no executável.

| T50 | Gerar Windows-AvatarMotion | Sucesso: 153,72 MB, zero erros e zero avisos |

## 31/10/2026 — Humanos retrô em jogo

| ID | Procedimento | Resultado observado |
|---|---|---|
| T51 | Validação estática de arte/campanha | Shader/atlas suportados; nove pedidos e catálogo completos |
| T52 | Repetir arte e campanha em execução com a nova geometria | Nove fotos/saves, 21 peças, relógio/pausa, replays e preservação dos resultados passaram |
| T53 | Alternar proporção e executar idle | Corpo esguio mantém escala de teste, sem voltar à escala padrão |
| T54 | Sair da prévia de proporções | Mesmo objeto e conteúdo da campanha original restaurados; escolhas de proporção limpas |
| T55 | Inspeção visual de frente/lado e roupa com acessórios | Novos olhos/cabeça visíveis, roupas aplicadas; cabelo e acessórios reajustados |
| T56 | Conferir enquadramento alto após correção | Cabeça inteira visível abaixo do cabeçalho na captura final |

A sensação do novo modelo em interação contínua no executável será avaliada pelo usuário. A inspeção e os testes de execução foram feitos no Editor.

| T57 | Gerar Windows-RetroHumans | Sucesso: 153,72 MB, zero erros e zero avisos |

## 31/10/2026 — Redondos, UVs e encaixe das peças

| ID | Procedimento | Resultado observado |
|---|---|---|
| T58 | Validar 6 atlas animados e 4 estáveis | Importação correta e tiles estáticos idênticos para cabelo/couro/borracha/malha |
| T59 | 42 trocas já giradas em compacto/esguio | Raiz das roupas e peças manteve orientação/escala local correta |
| T60 | Validar UVs de cabeça/cabelo/tênis/sola/boné/gorro | 247 superfícies com UVs finitos, completos e não colapsados |
| T61 | Verificar olhos após expressão/idle | Dimensões proporcionais à cabeça preservadas; corrigido tamanho do olho sendo substituído pela escala de expressão |
| T62 | Repetir arte e campanha | 47.822 pixels intermediários, ciclo/freeze estáveis; nove atendimentos, 21 peças, timer, pausa e replays passaram |
| T63 | Inspeção renderizada de cabelo, boné/óculos, gorro e tênis | Capturas reais frontal/lateral e detalhe dos tênis inspecionadas; peças acompanharam giro após troca |

Verificações no Unity Editor. Avaliação estética da aproximação desejada e uso contínuo do novo executável ficam para o usuário.

| T64 | Gerar Windows-RoundedCharacters | Sucesso: 153,92 MB, zero erros e zero avisos |

## 31/10/2026 — Esfera e cinco correções

| ID | Procedimento | Resultado observado |
|---|---|---|
| T65 | Medir distância dos vértices ao centro em espaço mundial | Cabeça mantém raio 0,56 em 42 trocas giradas com duas proporções; retirado o esticamento do idle |
| T66 | Conferir mecha separada do K7 | Ausente; bolinha bege removida |
| T67 | Percorrer seis expressões | Pupilas, pálpebras e uma boca ativa por pose; prancha renderizada inspecionada |
| T68 | Comparar vértices das três malhas de tênis | Construções distintas, além das cores; runner, skate médio e baixo de couro |
| T69 | Conferir dois bonés em duas vistas | Abertura inclinada, seis painéis, fecho, aba curva/reta renderizados sem corte na prancha final |
| T70 | Repetir validações completas após biqueira integrada | 247 superfícies com UVs válidos; nove fotos/saves, 21 peças, timer, pausa e replays passaram |

Revisão e verificações realizadas no Editor. Uso contínuo do executável e aprovação estética ainda dependem do usuário.

| T71 | Gerar Windows-SphereStreetwear após correção final | Sucesso: 153,94 MB, zero erros e zero avisos |

## 31/10/2026 — Encaixe cabelo/coberturas

| ID | Procedimento | Resultado observado |
|---|---|---|
| T72 | Percorrer seis cortes, quatro coberturas e dois corpos | 48 combinações concluídas |
| T73 | Conferir cabelo sob cada boné/touca | 30 encaixes com cabelo presente e exposto abaixo da peça; área coberta com espessura recolhida |
| T74 | Conferir raiz e coque | Superfície fora da esfera, dentro dos limites; coque manteve união à nuca com e sem cobertura |
| T75 | Renderizar comparações de frente e trás | Laterais/nuca, abertura traseira e coque visíveis; capturas inspecionadas |
| T76 | Repetir validações gerais | Cabeça esférica, 253 superfícies com UV válido, tênis, expressões, nove atendimentos, relógio e replays passaram |

Inspeção e testes no Editor. A avaliação estética final e interação contínua no executável ficam para o usuário.

| T77 | Gerar Windows-HairFit | Sucesso: 153,94 MB, zero erros e zero avisos |

## 31/10/2026 — Braços, mangas e postura

| ID | Procedimento | Resultado observado |
|---|---|---|
| T78 | Percorrer nove blusas, dois corpos e movimento normal/reduzido | 36 combinações; centros de braço/manga alinhados e folga positiva na abertura |
| T79 | Conferir punhos e pausa em cada combinação | Mãos unidas à ponta dos braços; tronco manteve posição e rotação ao pausar |
| T80 | Renderizar frente/perfil/costas de três tipos de blusa | Nove vistas inspecionadas; corrigidas quinas no início das mangas; barra aberta e braço interno visíveis no perfil |
| T81 | Repetir validações de arte/avatar/cabelo | Ciclo contínuo estável; 42 trocas, esfera de raio 0,56, 253 superfícies com UV válido, seis expressões e 48 combinações capilares passaram |
| T82 | Repetir campanha no Editor | Nove fotos/saves, 21 peças renderizadas, três dias, timer/pausa/expiração e replay sem crédito duplicado passaram |
| T83 | Gerar Windows-RelaxedPose | Sucesso: 153,95 MB, zero erros e zero avisos |

Capturas: relaxed-short-game.png, relaxed-long-game.png e relaxed-posture-review.png. Testes e inspeção no Editor; não foi feita uma sessão prolongada no executável. A avaliação estética fica para o usuário.

## 31/10/2026 — Mangas, gorro, identidades e fluxo abstrato

| ID | Procedimento | Resultado observado |
|---|---|---|
| T84 | Conferir as junções das mangas em 36 combinações | Pontos do ombro coincidem com a abertura da blusa, braços centrados, folga e punhos preservados |
| T85 | Inspecionar nove vistas e nove detalhes das blusas | Corrigido vazamento de pele na costura; regular, larga e comprida renderizadas em frente/perfil/costas |
| T86 | Comparar os seis perfis sem cobertura | Seis tons de pele, cinco cores de cabelo, seis cortes/estados e seis expressões iniciais distintas |
| T87 | Conferir gorro em frente/perfil/costas | Dobra de 0,21, etiqueta e topo folgado deslocado; mantido encaixe com cabelo |
| T88 | Renderizar o painel em dois tempos com avanço conhecido | Movimento real de 32 pixels para baixo; erro médio por canal 0,0000 |
| T89 | Comparar início/fim do ciclo e opções de movimento | Emenda com erro médio 0,0000; quadro estável, relógio congelado na pausa e movimento reduzido |
| T90 | Repetir verificações completas após correção final | 42 trocas, esfera, 253 UVs, seis expressões, 48 combinações capilares e campanha de nove fotos/saves, 21 peças, timer e replay passaram |
| T91 | Gerar Windows-VisualRefinement | Sucesso: 154,35 MB, zero erros e zero avisos |

Capturas nativas: sleeves-integrated-review.png, sleeves-junction-detail.png, characters-identity-review.png, soft-beanie-review.png e refinement-final-game.png. Testes no Editor; uso prolongado do executável e aprovação estética ficam para o usuário.

## 31/10/2026 — Regressão de envio da prévia e distância do gorro

| ID | Procedimento | Resultado observado |
|---|---|---|
| T92 | Reproduzir o estado relatado com prévia e SB_HEAD_01 | Confirmado conflito entre catálogo completo da prévia e DayProgress.Add restrito à campanha |
| T93 | Enviar cada uma das 21 peças na prévia | Avaliação/foto em memória; nenhum erro de peça indisponível, nenhuma alteração de progresso ou arquivo |
| T94 | Voltar ao menu e validar bloqueio normal | Campanha original restaurada; SB_HEAD_01 continua bloqueado em inventário inicial normal |
| T95 | Medir dobra do gorro contra o topo dos olhos | Distância mínima de 0,075 mantida nos seis cabelos e duas proporções; cúpula compacta e etiqueta curva inspecionadas |
| T96 | Repetir verificações de arte e campanha | Esfera, UVs, 48 combinações capilares, 36 posturas, fluxo abstrato, nove atendimentos/fotos/saves, timer e replays passaram |
| T97 | Gerar Windows-PreviewBeanieFix | Sucesso: 154,35 MB, zero erros e zero avisos |

Capturas: preview-unlocked-submit-fixed.png, beanie-clearance-review.png e beanie-clearance-final-game.png. Testes e inspeção no Editor; avaliação estética e uso prolongado do executável ficam para o usuário.

## 31/10/2026 — Fresta das pálpebras em perfil

| ID | Procedimento | Resultado observado |
|---|---|---|
| T98 | Validar fechamento de 92 malhas de pálpebra | Seis expressões, três fases de piscada e dois corpos; todas as arestas com duas faces, sem borda aberta ou triângulo degenerado |
| T99 | Conferir expressões em três ângulos | Nove vistas reais inspecionadas, incluindo perfil a 85 e 105 graus; lateral com fechamento contínuo |
| T100 | Conferir piscada de perfil | Aberto, meio fechado e fechado renderizados; branco do olho coberto ao fechar, sem abertura lateral |
| T101 | Repetir verificações completas | Cabeça esférica, UVs, gorro, cabelo, postura, fundo, campanha e envio das 21 peças na prévia passaram; zero erros no Console |
| T102 | Gerar Windows-EyelidFix | Sucesso: 154,36 MB, zero erros e zero avisos |

Capturas: eyelid-closed-profile-review.png e eyelid-blink-profile-review.png. Inspeção e testes no Editor; avaliação estética e uso prolongado do executável ficam para o usuário.

## 31/10/2026 — Praça/comércio caminhável

| ID | Procedimento | Resultado observado |
|---|---|---|
| T103 | Construir bairro e validar materiais | Shader suportado, piso 256x64, sete lotes de arquitetura e dois moradores; malhas/materiais presentes |
| T104 | Simular cinco percursos pela praça, café e loja | Destinos alcançados, sem atravessar obstáculos ou sair dos limites |
| T105 | Simular movimento reto/diagonal e pausa | Velocidade 3,05 unidades/s sem ganho diagonal; posição/rotação congeladas na pausa |
| T106 | Projetar destino e enviar evento de clique | Clique coincide com piso da imagem retrô; evento gera percurso; UI não comanda movimento |
| T107 | Perder foco, sair e reentrar | Pausa ao perder foco; câmera/posição restauradas ao sair; sem duplicar moradores |
| T108 | Trocar roupas após construir moradores | Cópias de malhas/materiais preservadas; moradores bloqueiam passagem; campanha inalterada |
| T109 | Conferir capturas reais e escala | Vista geral, café, fachada e menu inspecionados; elevados toldo/copa para melhorar folga e visibilidade |
| T110 | Repetir arte e campanha após integração | Esfera, 92 pálpebras fechadas, cabelo, postura, fundo, nove atendimentos/fotos/saves e prévia com 21 peças passaram |
| T111 | Gerar Windows-CitySlice numa cópia isolada | Sucesso: 151,66 MB, zero erros e zero avisos; nenhuma diferença de conteúdo nos assets/fontes copiados |

Capturas: city-square-final.png, city-driplab-final.png, city-cafe-final.png e city-entry-menu.png. Entradas simuladas no Editor, não uma sessão contínua de teclado/mouse no executável. Rotina principal ainda por menus; percurso da casa, interiores e avaliação estética ficam para próximas etapas.

## 31/10/2026 — Primeira pessoa / gráfico menos pixelizado

| ID | Procedimento | Resultado observado |
|---|---|---|
| T112 | Conferir cinco quadros do vídeo | Estudo restrito a linguagem de formas, cor, textura e legibilidade; conteúdo não incorporado |
| T113 | Executar verificação no Player com D3D11 | Cinco percursos, velocidade/diagonal, câmera a 2,78, FOV 68, limites ±75, Tab/cursor, pausa/foco e retorno ao provador passaram |
| T114 | Conferir corpo e dados | Funcionário oculto em primeira pessoa, visível ao sair; moradores e campanha preservados |
| T115 | Conferir suavidade e materiais | Limite 720 linhas, piso 1024x256 com mipmaps e menor força de granulação urbana |
| T116 | Renderizar vistas reais na versão executável | Loja, café, personagem e piso em 1280x720 inspecionados |
| T117 | Repetir regressões no Player | Arte, 42 trocas/UVs/esfera, 48 encaixes capilares, 36 posturas, 92 pálpebras, fundo e prévia com 21 peças passaram |
| T118 | Gerar Windows-FirstPerson | Sucesso: 152,61 MB, zero erros e zero avisos |

Automação executada em sessão separada com -city-proof, sem alterar a campanha nem capturar o cursor do desktop. São verificações de motor/handlers e renderização real; uma sessão contínua com mouse físico e a avaliação estética permanecem para o usuário. Capturas e checks.txt em Captures/FirstPerson. As capturas 3D não incluem a UI sobreposta.


### T119–T123 — Céu e identidade urbana

- T119: skybox presente, material correto na câmera e shader suportado no executável D3D11.
- T120: tempo do céu avança normalmente; pausa e movimento reduzido congelam a animação. Bandeiras e postura ambiente usam o mesmo relógio.
- T121: saída do bairro restaura clear flags, material e estado de habilitação do skybox do provador; nova entrada não duplica moradores.
- T122: cinco trajetos anteriores, bloqueios de edifícios/canteiros/moradores, velocidade diagonal, olhar, cursor, foco e campanha preservados após revisão da geometria.
- T123: imagens nativas de loja, café, personagem, piso e skyline inspecionadas; ajustes de terraço e nuvens conferidos no jogo. Arte, cabelos, roupas, pálpebras e envio experimental seguem cobertos pela sequência de checks do executável. Evidência: Captures/CityIdentity/checks.txt e tools/city-identity-proof.log.


### T124–T127 — Bairro noturno em atividade

- T124: shader noturno e atlas compilam em D3D11; sequência anterior de controles, campanha, roupas, cabelos, pálpebras e envio experimental preservada. Quatro moradores sem duplicação ao reentrar.
- T125: vinte segundos de atividade em passos de 0,05s; morador da caminhada percorre mais de três unidades; moradores permanecem fora da arquitetura; buffers de animação sem NaN/Infinity.
- T126: pausa (delta zero) e movimento reduzido congelam relógio e posição de morador; navegação do jogador continua coberta pelos cinco trajetos anteriores.
- T127: noventa quadros nativos a 640×360 para avaliar caminhada, balanço de membros, vento e rádio. Imagens nativas em 1280×720 para loja, café, personagem, piso e trânsito. Evidência: Captures/CityNight e tools/city-night-proof.log. A captura não equivale a um teste manual de todos os encontros possíveis entre jogador e NPCs.


### T128–T131 — Compacto, apoio, escala e letreiros

- T128: todas as trocas de roupa, incluindo flags legados de comparação, mantêm escala (.98,.94,1), pescoço com altura local até .321 e cabeça esférica. Loops antigos que mencionam duas proporções/corpos no texto do log agora exercitam dois estados legados sobre a mesma base compacta.
- T129: caminhada simulada por vinte segundos, sem atravessar arquitetura e sem valores inválidos; quando o pé está na fase de apoio e a transição terminou, sua altura global permanece a até .025 da referência. Pausa e movimento reduzido congelam a atividade.
- T130: texturas de letreiros com mipmaps e shader suportado no executável; inspeção visual de loja, café, praça e cartazes. Letreiros preservam leitura à noite; menus sem alteração.
- T131: carro ampliado conferido no executável com colisão e parada correspondentes; sequências anteriores de cinco trajetos e controles continuam passando. Capturas estáticas 1280×720 e vídeo nativo 640×360, 180 quadros/30 fps.


### T132 — Recuperação do rosto após perda de caches

ValidateFaceReloadRecovery limpa deliberadamente referências de pupilas/olhos/sobrancelhas/pálpebras/bocas e coberturas, recupera os objetos da hierarquia e percorre seis expressões em dois caminhos de entrada. Exige as mesmas malhas de pupilas, geometria preenchida e nenhum asset facial na lista descartável das roupas. Repouso das pernas recuperado de caches persistentes. Evidência: tools/face-recovery-proof.log, FITCHECK_FACE_RECOVERY_PASSED (12 casos), mais sequência completa de checks nativos.


### T133–T136 — Aparência do jogador e novos cabelos

- T133: criar/editar/salvar/recarregar aparência, com nome e valores esperados; cancelar preserva o jogador e o arquivo salvo. Campanha permanece com JSON idêntico. Evidência: FITCHECK_CREATOR_PASSED, tools/creator-proof.log.
- T134: seis cortes, cores e expressões aplicados no jogador, com cabeça/base preservadas. Arquivo inválido recuperado pelo backup e regravado. Testes usam apenas o caminho isolado de prova.
- T135: checks anteriores de cabelo sob chapéus, coque unido, pálpebras/rosto, roupas e envio passaram. Cabelos sem chapéu aceitam o novo volume; sob chapéus os limites permanecem anteriores.
- T136: sequência de caminhada e apoio dos pés anterior permanece passando. Inspecionados closes 3D de perna/calçado e braços, além do preview de cabelo. Vídeo de 180 quadros/30 fps em Captures/CharacterCreator. Captura de GUI oculta ficou preta e não valida a disposição visual dos controles; save/cancel são verificados por seus handlers.


### T137 — Revisão de cabelos em três ângulos

Seis cortes renderizados de frente, perfil e costas (18 imagens nativas 640×640). Checagem impede retorno de objetos Mecha volumétrica. Checks existentes cobrem 48 combinações de cabelos/coberturas, união do coque, folga do gorro, superfície fora da cabeça e criação/salvamento/cancelamento. Limite externo sem chapéu .77 para acomodar o volume esculpido; sob coberturas permanece .577. Evidência: Captures/HairRefined/checks.txt e tools/hair-refined-proof.log.


### T138 — Escala dos edifícios

Baseline completo de cidade (cinco trajetos, bloqueios, câmera/cursor/foco e retorno), arte, rosto/pálpebras, cabelos/coberturas e criação/salvamento passou após a revisão do bairro. Cinco renders nativos 1280×720 inspecionados para proporções de janelas/portas/andares e junções do terraço do café. Loja aprovada preservada; câmera e NPCs não foram escalados para compensar o cenário. Evidência: Captures/CityScale/checks.txt e FITCHECK_CITY_SCALE_REVIEW_PASSED no log. Prédios exteriores permanecem fora da área de circulação atual.


### T139–T142 — Cabelo, sobrancelhas, NPCs e filtro

- T139: 36 combinações (seis cortes × seis expressões), com seis amostras das sobrancelhas por combinação e teste de interseção contra a casca do cabelo. Exige sobrancelha com geometria fechada e nenhum cabelo bloqueando as amostras frontais.
- T140: doze segundos em movimento reduzido pelo fluxo UpdateCity/Frame; morador percorre mais de três unidades e relógio decorativo permanece congelado. Pausa explícita mantém a posição.
- T141: perda deliberada da lista de moradores; UpdateCity recupera quatro moradores e preserva posição/olhar. Estado serializável para próximos reloads, com recuperação legada. Foco na cidade não pausa a simulação; entrada depende do foco.
- T142: filtro default desligado, mantendo opção e altura de 720 linhas se reativado. Provas nativas usam preferências isoladas de escrita. Evidência: FITCHECK_EXPRESSION_LIFE_PASSED, tools/expression-life-proof.log e Captures/ExpressionLife. Build sem erros/avisos.

### T143 — Corpo inteiro na caminhada

Comparação de duas fases com a raiz do morador fixa: exige movimento local em cabeça/tronco (RMS mínimo .003) e braços/pernas/calçados (.025). Evita aprovar apenas translação do personagem ou animação isolada dos tênis. CheckResidents verifica geometria finita, apoio vertical dos pés e circulação durante 180 quadros próximos. Repetidos os checks anteriores de campanha, criação, cabelo/coberturas, visibilidade das sobrancelhas (216 amostras), movimento reduzido, pausa e recuperação. Evidências em Captures/BodyMotion e tools/body-motion-proof.log; vídeo composto apenas dos renders nativos, sem interpolação de poses.

### T144 — Velocidade e corpo em primeira pessoa

CitySmokeCheck exige deslocamento de 4,4 unidades em um segundo, diagonal normalizada, pausa sem deslocamento e limites do olhar -75/88 graus. CheckPlayerBody exige geometria do corpo dentro da vista ao olhar para baixo e impede inclusão de peças da cabeça. Entrada/saída repetidas continuam verificando materiais e campanha preservada. Capturas mostram corpo parado, durante o passo e vistas a 65/88 graus. Franja pode cobrir sobrancelhas intencionalmente; os demais cortes continuam com verificação de visibilidade. Mantidos os testes de corpo inteiro, apoio dos pés, 48 pares cabelo/cobertura, criação, salvamento e recuperação de runtime. Evidências: Captures/FirstPersonBody e tools/first-person-body-proof.log.

### T145 — Interior da loja

Prova nativa visita araras, prateleiras, balcão, provador e retorno à entrada usando a navegação real; verifica bloqueios da mesa/balcão/prateleiras/limites e skybox desabilitado. Evento E inicia diálogo e é consumido; pausa impede interação. Experimentação do provador usa a campanha isolada, com peças trocadas antes de sair; o JSON da campanha é comparado antes/depois. Corpo do jogador continua visível ao olhar para baixo. Perda simulada da lista de moradores recupera o interior, posição e direção. Saída retorna ao menu e o bairro continua independente. Baseline de cidade/câmera, roupas, criação, pálpebras, campanha e envio de prévia também executado. Evidência: Captures/ShopInterior/checks.txt, sete renders nativos e tools/shop-interior-proof.log. Cliques reais e avaliação estética do interior ainda cabem ao teste do usuário.

### T146 — Olhar e mobiliário revisados

Handler do arrasto com botão direito recebe movimento vertical, exige variação de inclinação sem rotação horizontal, depois movimento horizontal com amplitude mínima e parada após soltar botão. Mouse Events sintéticos ficam Ignore em batch; valores crus entram no handler real de OnGUI, sem afirmar que o teste valida o dispositivo físico. Evidência FITCHECK_SHOP_LOOK_PASSED. Repetida T145 com novos objetos, vistas nativas inspecionadas e build normal sem erros/avisos. UI do executável aberta via controle do computador, entrada por botão e alternância Tab verificadas. Conforto/captura do mouse no computador do usuário dependem de nova avaliação. Evidências em Captures/ShopRefined e tools/shop-refined-proof.log.

### T147–T149 — Apoio, braços e cortina

- T147: ValidateShopLean exige contato da extremidade do tênis com a parede em tolerância de 0,015 e pé direito pelo menos 0,25 acima do pé de apoio. Geometria finita e posição sem atravessar arquitetura verificadas durante 36 frames. Primeiro ajuste estava baixo (diferença 0,225); tornozelo elevado e prova repetida. Retrato nativo inspecionado.
- T148: postura e encaixe de mangas/mãos passam nas 36 combinações anteriores; ValidateFullBodyMotion exige deslocamento local de cabeça/tronco/braços/pernas/calçados, com raiz fixa. Seis poses reais de caminhada registradas. Braços continuam articulados após baixar a pose de repouso.
- T149: provador começa fechado e bloqueia a passagem; E mantém Stage.City durante abertura. Pausa preserva o progresso e impede entrada. Ao terminar, passagem fica livre e FinishShopFittingEntryIfReady entra em Dressing com campanha isolada. Retorno à loja exige progresso zero. Resto da prova T145 permanece passando. Imagens fechada/intermediária/aberta e vídeo composto de 36 renders nativos a 30 fps, sem interpolação externa. Evidências: Captures/ShopPose/checks.txt e tools/shop-pose-proof.log.

### T150 — Importação dos materiais ambientCG

Oito mapas Color carregados via Resources, com largura/altura até 512, mipmaps e WrapMode.Repeat; FITCHECK_AMBIENT_SURFACES_PASSED. Configure/PrepareAll aplica limites de importação antes do build. SHA-256 dos originais registrado em sources.json. Renderizações inspecionadas para veios contínuos de madeira, contraste da trama, escala de tijolos/concreto, cor de embalagens e paleta noturna. Prova completa de loja e bairro, pálpebras, roupas, criação, postagem de prévia, apoio e abertura da cortina repetida. Evidências: Captures/SurfacePass/checks.txt, surface-street.png, surface-asphalt.png e tools/surface-pass-proof.log.
