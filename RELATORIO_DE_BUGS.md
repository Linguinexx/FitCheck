# Relatório de bugs do Fit Check

Atualizado em 13/11/2026. Consolidação dos incidentes identificáveis na documentação local, do protótipo às entregas Windows/Web BairroVivo de 13/11/2026. Casos sem registro suficiente ficam de fora até haver um relato. Os IDs B01 e B02 foram preservados de TESTES.md.

## Bugs do jogo

**Retestado** indica que a correção foi seguida de teste, captura ou inspeção documentada. **Reteste pendente** indica que a verificação específica ainda falta. Em **Correção registrada**, a alteração consta no histórico, mas o registro específico não permite afirmar reteste.

| ID | Área e sintoma | Correção ou mudança observada | Estado e referência |
|---|---|---|---|
| B01 | Primeiro Windows falhava ao montar a cena com shader Standard ausente. | Material explícito em Resources preserva o shader no player. | Retestado na inicialização Windows. TESTES, T08–T09. |
| B02 | Clonagem da fonte dinâmica gerava texto incorreto. | Uso direto do recurso de fonte, caracteres pré-carregados e estados de cor restaurados. | Retestado no Editor. TESTES, revisão da apresentação. |
| B03 | Iluminação voltada para trás, texto cortado, controles sobre o personagem e contraste baixo. | Luz, enquadramento, espaçamento e contraste ajustados. | Retestado por captura. DEVLOG, Reconstrução do provador. |
| B04 | Respiração deformava a cabeça esférica. | Removido esticamento da raiz. | Retestado por raio mundial. DEVLOG, Cinco correções. |
| B05 | Mecha separada aparecia como bolinha bege no K7. | Malha separada removida. | Retestado. DEVLOG, Cinco correções. |
| B06 | Sobreposição na biqueira do tênis de skate gerava recortes pretos. | Biqueira integrada como região de material. | Retestado visualmente na versão daquela coleção. DEVLOG, Cinco correções. |
| B07 | Boné ou touca apagava todo o cabelo. | Laterais, costeletas e nuca preservadas com volumes próprios sob a peça. | Retestado em combinações de cabelo e cobertura. DEVLOG, Cabelo e junção. |
| B08 | Raízes de mangas formavam quinas e deixavam pele vazar na costura. | Junção e recorte da pele revistos. | Retestado em vistas de detalhe e casos de postura. TESTES, T80/T85. |
| B09 | Importador forçava resolução e filtro inadequados ao fundo animado. | Regras específicas de resolução, repetição e filtro para AbstractFlow. | Retestado por comparação de pixels e ciclo. DEVLOG, Revisão das mangas. |
| B10 | Enviar peça liberada na prévia falhava por consultar o inventário da campanha. | Envio da prévia usa inventário próprio e resultado em memória. | Retestado com 21 peças e comparação de arquivos. DEVLOG, Erro de envio na prévia. |
| B11 | Gorro se aproximava dos olhos e etiqueta atravessava a dobra. | Dobra elevada e etiqueta ajustada à curvatura. | Retestado com distância mínima e vistas frontal/lateral/traseira. DEVLOG, Erro de envio na prévia. |
| B12 | Pálpebra deixava fresta lateral e piscada das cópias deformava a calota ou ocultava pupilas. | Casca fechada e reconstrução esférica por fase, com expressão preservada. | Retestado em perfis e atores. DEVLOG, Fechamento lateral e PrintStyle. |
| B13 | Escala de expressão substituía a proporção do olho. | Dimensão relativa à cabeça preservada. | Retestado. TESTES, T61. |
| B14 | Recompilação durante Play podia causar NullReferenceException em UpdatePupil. | Referências faciais persistidas e recuperadas na hierarquia. | Retestado com perda de caches simulada. Recompilação real desse caso ainda precisa de reteste. DEVLOG, Recuperação de referências do rosto. |
| B15 | Cabelo ficava liso demais, com faixa binária de cor ou aparência de peça colada. | Curva de transição corrigida, margem fora do crânio e ondulado contínuo. | Retestado visualmente e em combinações com cobertura. DEVLOG, Cabelo sem mechas tubulares e Residencial AP 03. |
| B16 | Sobrancelha podia ficar inválida na ponta por potência fracionária de valor negativo. | Base da potência limitada a valores não negativos. | Correção registrada. DEVLOG, Sobrancelhas visíveis. |
| B17 | Faixa de costura aparecia como traços soltos na cintura. | Peça geométrica separada removida da montagem compartilhada. | Retestado em Windows-ShopPose. DEVLOG, Traço solto na cintura. |
| B18 | Mouse podia ficar solto ao voltar para a Game View ou saltar ao recapturar. | Estado de Tab preservado e primeiro delta descartado na recaptura. | Reteste manual de alternância entre janelas pendente. DEVLOG, Recuperação do cursor. |
| B19 | Rota de NPC ficava vazia após recompilar durante Play. | Reconstrução da rota e do estado RuntimeReady. | Retestado no Editor. DEVLOG, Recuperação do cursor. |
| B20 | Cancelar atendimento podia deixar cliente invisível ou preso no provador. | Estado parcial descartado ao cancelar. | Retestado no fluxo nativo. DEVLOG, Atendimento contínuo. |
| B21 | Frequentador fixo era tratado como cliente e participava da troca. | Cliente e frequentador separados, com rota e interação próprias. | Retestado até a saída e o próximo pedido. DEVLOG, Frequentador fixo. |
| B22 | Saída do cliente deslocava o jogador para a entrada da loja. | Ambiente e corpo do jogador mantidos na posição atual. | Retestado até o próximo atendimento. DEVLOG, Ondulado longo e saída. |
| B23 | Corpo completo do jogador sumia na câmera que acompanha o cliente. | Cabeça e pescoço ocultos apenas perto da câmera em primeira pessoa. | Retestado em câmera externa e atendimento. DEVLOG, Abertura manual. |
| B24 | Provador vazio abria prévia de roupa sem cliente. | E alterna somente a cortina quando vazia. | Retestado com campanha preservada. TESTES, GifGallery. |
| B25 | Cliente podia ser reaproveitado entre campanhas diferentes com o mesmo índice. | Reconstrução da loja considera campanha e dia. | Retestado na prova NormalFlow. DEVLOG, NormalFlow. |
| B26 | Acesso de casa usava a fachada RADIO. | Porta própria no residencial AP 03. | Retestado na ida e volta. DEVLOG, Residencial AP 03. |
| B27 | Quitinete de estúdio, cenário de foto ou cenário de seleção apareciam no bairro. | Isolamento dos renderizadores e restauração de visibilidade por ambiente. | Retestado em exploração e retorno ao menu. DEVLOG, Bairro conectado e Cidade distribuída. |
| B28 | Calçada fragmentada e faixa de asfalto atravessavam áreas de passeio e bancos. | Piso contínuo com UV global e remoção da superfície sobreposta. | Retestado nas provas MapPolish e NormalFlow. DEVLOG, Correções do mapa. |
| B29 | Fim da avenida tinha bloqueios visuais atrás do portal e vegetação atravessava muros. | Túneis funcionais para veículos, limites do jogador e vegetação reposicionada. | Retestado em rota e colisão. DEVLOG, Correções do mapa. |
| B30 | AP 03 tinha fechamento e acabamento incompletos na traseira e no vestíbulo. | Fechamento, janelas, assoalho e acabamento da entrada. | Retestado em capturas e percurso. DEVLOG, Correções do mapa. |
| B31 | Pés e postes de araras não coincidiam e a última ficava junto à cortina. | Posições compartilhadas e folga superior a 1 m. | Retestado na prova MapPolish. TESTES, MapPolish. |
| B32 | Observador da galeria aparecia sem corpo e havia porta residual visível abaixo. | Avatar inteiro apoiado na sacada e base da fachada fechada. | Retestado na revisão GifGallery. DEVLOG, GifGallery. |
| B33 | Balões ambientais vazavam para interiores, ou tinham ponta solta em contextos sem âncora. | Exibição limitada ao bairro e ponta mantida apenas no diálogo do balcão. | Retestado em interiores e por captura. DEVLOG, Quadros fornecidos e Tênis Blender. |
| B34 | Hover alterava a cor do texto do balão, prejudicando contraste. | Tinta aplicada aos oito estados do estilo. | Retestado por comparação de pixels. Art/Performance/StreetFrontage.md. |
| B35 | Jeans baggy apresentava frestas entre cintura e pernas. | Deformação contínua por vértice. | Retestado em postura, caminhada e vista para baixo. TESTES, BaggyDenim. |
| B36 | UV de estampas/painéis chegava com orientação incorreta em partes da coleção. | Orientação corrigida no Blender ou na conversão, conforme a peça. | Retestado no lookbook. TESTES, Streetwear. |
| B37 | Exportação FBX de tênis fornecidos não preservava eixos/unidades. | Bake corrigido com malha na raiz. | Retestado na revisão nativa dos tênis. DEVLOG, Modelos fornecidos. |
| B38 | Tênis civil foi alargado e achatado pela escala independente. | Escala uniforme e redução revisada para 2.400 triângulos por pé. | Retestado em proporção, nove visuais civis e caminhada. DEVLOG, Correção das proporções. |
| B39 | Preparação de ambientes e deformações de roupa sobrecarregavam a CPU. | Cache de malhas, matrizes reutilizadas, menor frequência à distância e atualização do corpo quando visível. | Retestado por medições da mesma máquina. Cobertura em outras máquinas pendente. Art/Performance/EntryOptimization.md e ShopPerformance.md. |
| B40 | Clientes chegavam com roupa de catálogo antes de montar o fit. | Trajes cotidianos próprios e tênis civil até o provador. | Retestado no Teste 01. DEVLOG, Clientes com roupa cotidiana. |

| B41 | Web inicial perdia componentes de primitivas no stripping e acusava erro de BoxCollider. | Componentes preservados explicitamente em link.xml. | Retestado na Web01. Art/Releases/Web01/VALIDACAO.md. |
| B42 | Captura do ponteiro no navegador exigia clique extra ou falhava sem foco. | Solicitação direta na ativação do navegador, verificação da captura real e recuperação de recusa. | Retestado em iframe Web02 e BairroVivo, incluindo recaptura em um clique. |
| B43 | Uso de Esc para pausa conflitou com a reserva do navegador e a saída de tela cheia. | Regra atual usa P e botões. Ponte observa liberação do ponteiro sem interceptar teclado. | Retestado em iframe Web08 e BairroVivo. Teclado físico e outras hospedagens seguem na cobertura manual. |
| B44 | Materiais do catálogo eram identificados internamente como Civilian. | Identificação dos materiais do catálogo corrigida, mantendo roupas de chegada civis. | Retestado no Test08 com verificação de troca do cliente preservada. Art/Releases/Test08/VALIDACAO.md. |
| B45 | Espelho do quarto deixava de funcionar do lado distante por limite de seis unidades/plano de corte. | Limite removido e plano distante calculado a partir do quarto. | Retestado em cinco posições e restauração da pose. Art/Reviews/HomeDecor/README.md. |
| B46 | Joinha apresentava segmentos de retorno duplicados nos dedos. | Núcleo de punho e uma almofada contínua por dedo, polegar revisto. | Retestado no revisor de emotes e capturas ThumbAnd67. Art/Reviews/Emotes/README.md. |
| B47 | Orelhas dos gatos tinham encaixe inadequado na cabeça. | Base estreita embutida, pivô na base e faces laterais fechadas. | ROOM_EAR_PASSED em quatro tipos/seis cores. Art/Reviews/RoomAndEars/README.md. |
| B48 | Catálogo/porta-retrato ficavam sem apoio adequado e affordances da mesa precisavam acompanhar a nova posição. | Apoio no tampo e separação de celular, catálogo e foto. | ROOM_EAR_PASSED e entrega BairroVivo. Art/Reviews/RoomAndEars/README.md. |

## Incidentes de produção e teste

| ID | Ocorrência | Tratamento |
|---|---|---|
| E01 | MCP indisponível durante inicialização e timeouts durante provas. | Conexão local verificada e resultados conferidos em logs. Timeouts isolados não indicam falha do jogo. |
| E02 | Automação de janelas não conseguiu abrir o executável ou recuperar foco em algumas sessões. Capturas ocultas também saíram pretas ou cortadas. | Usadas provas e capturas nativas válidas. Reteste manual de foco segue em B18. |
| E03 | Arquivo novo ficou fora de Assets e quebrou compilação do criador. | Arquivo movido para Assets e build repetida com sucesso. |
| E04 | Disco abaixo de 256 MB impediu uma compilação. | Espaço liberado com cópias antigas já recuperáveis em ZIP. Compilação seguinte passou. |
| E05 | Provas antigas falharam por coordenadas fixas de araras, reconstrução redundante do avatar ou eventos sintéticos ignorados em batch. | Atualizadas expectativas e preparação dos testes. Provas finais passaram. |

## Atualização dos incidentes de produção — 06 a 13/11

| ID | Ocorrência | Tratamento e limite |
|---|---|---|
| E06 | Primeira tentativa Web ficou sem espaço temporário. | Paralelismo de Emscripten/Binaryen limitado a dois processos. Build final passou. Web01/VALIDACAO.md. |
| E07 | Indexador UnityEditor.Search/SearchDatabase emitiu exceção/aviso durante revisões isoladas. | Registrado como incidente do Editor. Revisores FittingRoom, SocialWeek e RoomAndEars passaram. Resolução do indexador continua pendente. |
| E08 | Três OGG existentes avisaram duração truncada na primeira importação, e houve variável sem uso. | Variável corrigida e build incremental final sem avisos. Originais preservados. Conferir duração/escuta dos áudios na revisão manual. CompanionsAndMirrors/VALIDACAO.md. |
| E09 | Trailer exigiu correções de enquadramento e timestamps na revisão editorial. | Render final de 5.400 quadros aprovado no registro da produção. Áudio medido, ainda sem escuta direta documentada. Captures/TrailerCompleto/edit. |

## Retestes e próximos relatos

- B14 ainda pede recompilação real durante Play, além da simulação de perda de caches. B18 ainda pede alternância manual de foco/janelas na Game View. A recuperação Web de B42 foi verificada em iframe, sem encerrar automaticamente o caso do Editor.
- Executar a semana completa por cliques na Web e em outros equipamentos/navegadores. BairroVivo verificou criação, pet, casa, P/Retomar, recaptura e reabertura do save em Edge/Chromium local. Esse percurso delimita a evidência disponível.
- Retestar interrupções de atendimento, replays com inventários históricos, backup, exclusão/cancelamento por espaço e erros de gravação. Usar armazenamento isolado.
- Conferir combinações de roupas em repouso, caminhada, espelhos e câmeras externas, com atenção a corpo/cabelo/acessórios. Verificar desempenho e legibilidade em resoluções menores.
- Escutar os arquivos finais do jogo e do trailer em equipamento real. Rever duração dos OGG, volume, sincronização, repetição, pausa e mute.
- Verificar a integração final no itch.io após publicação manual. Iframe local não cobre todas as condições da página hospedada.
- Novos relatos devem indicar build/plataforma, passos, impacto, frequência e evidência. Cenários preventivos e melhorias ficam com seus próprios estados.

## Pedidos futuros e mudanças já entregues

Casa personalizável, companheiros, espelhos, emotes, comentários do FitFeed, encontros pós-atendimento no bairro, áudio adicional e foto coletiva já têm implementação e revisões registradas. Sua validação por jogadores e os refinamentos derivados do feedback seguem abertos. A organização do quarto foi atualizada em 13/11.

A avaliação de IA para outras roupas permanece como possibilidade a discutir. O fluxo de integração conserva originais, cópia otimizada e teste reversível de todo o catálogo. A publicação da entrega e da mídia segue manual.

Pedidos e estados por etapa permanecem em [PROXIMOS_PASSOS.md](PROXIMOS_PASSOS.md). O [GDD atual](GDD.md) e o [diário de bordo](DEVLOG.md) registram a versão consolidada. Créditos de áudio em [CREDITS.md](Art/Audio/CREDITS.md).

## Como registrar o próximo bug

Usar o próximo ID B49 e incluir data, build, plataforma, resolução, área/dia/cliente, passos de reprodução, comportamento esperado e observado, frequência, impacto e evidência. Anotar o save por número ou usar dados de teste. Registrar a versão da correção e o resultado do reteste antes de fechar. O [modelo da entrega atual](Art/Releases/BairroVivo/RELATO-DE-BUG.txt) pode ser entregue ao grupo.

Fontes principais do levantamento. [Histórico de desenvolvimento](DEVLOG.md), [registro de testes](TESTES.md), [validação Teste 01](Art/Releases/Test01/VALIDACAO.md) e [validação do teaser](Art/Video/TeaserAtual/VALIDACAO.md).

Atualização de 13/11 baseada nos registros citados. As revisões anteriores foram preservadas. Não foram executados novos testes do jogo para esta consolidação documental.
