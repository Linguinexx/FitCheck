# Relatório de bugs do Fit Check

Atualizado em 05/11/2026. Consolidação dos incidentes identificáveis na documentação local, do protótipo à entrega Teste 01. Casos sem registro suficiente ficam de fora até haver um relato. Os IDs B01 e B02 foram preservados de TESTES.md.

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

## Incidentes de produção e teste

| ID | Ocorrência | Tratamento |
|---|---|---|
| E01 | MCP indisponível durante inicialização e timeouts durante provas. | Conexão local verificada e resultados conferidos em logs. Timeouts isolados não indicam falha do jogo. |
| E02 | Automação de janelas não conseguiu abrir o executável ou recuperar foco em algumas sessões. Capturas ocultas também saíram pretas ou cortadas. | Usadas provas e capturas nativas válidas. Reteste manual de foco segue em B18. |
| E03 | Arquivo novo ficou fora de Assets e quebrou compilação do criador. | Arquivo movido para Assets e build repetida com sucesso. |
| E04 | Disco abaixo de 256 MB impediu uma compilação. | Espaço liberado com cópias antigas já recuperáveis em ZIP. Compilação seguinte passou. |
| E05 | Provas antigas falharam por coordenadas fixas de araras, reconstrução redundante do avatar ou eventos sintéticos ignorados em batch. | Atualizadas expectativas e preparação dos testes. Provas finais passaram. |

## Retestes e próximos relatos

- Teste 01 em outros computadores. Priorizar percurso por cliques reais, resoluções menores, sensibilidade e recaptura de mouse após Alt+Tab. Verificar interrupções nas oito transições, além de desempenho ao entrar e durante a exploração.
- Saves e atendimento. Conferir reinício do processo, recuperação de backup e arquivos ilegíveis, cancelamento e exclusão do espaço escolhido. Repetir perguntas, pausa, saída no meio do provador e passagem de dia em dados isolados.
- Recompilação durante Play. Reproduzir a perda de referências do rosto na Unity, separando o resultado real da simulação de B14.
- Web. Retestar persistência, fotos e desempenho quando houver build com Web Build Support. Registros Windows não encerram essa cobertura.
- Aguardar feedback do grupo. Os itens acima são cenários de reteste. Cada nova falha observada entra com ID novo e estado próprio.

## Melhorias futuras já anotadas

- Trajes comuns do jogador, sem peças DripLab nessa proposta, integrados à personalização existente.
- Decoração do quarto, com o estilo atual e atenção ao desempenho.
- Avaliação do FitFeed inspirada no Instagram, adiada pelo usuário.
- Cliente aparece no local da foto assim que o atendimento termina, com o look do post do save ativo. Permanece pelo restante do dia, inclusive com a loja aberta, e sai no avanço de dia.
- Áudio final após recebimento e conferência de fontes/licenças, preservando o miado de Fubá.
- Avaliar IA para modelagem de outras roupas. A produção atual em Blender segue como referência da coleção.

A lista de trabalho continua em [PROXIMOS_PASSOS.md](PROXIMOS_PASSOS.md). O plano de áudio está em [PLANO_AUDIO_FITCHECK.md](Art/Audio/PLANO_AUDIO_FITCHECK.md).

## Como registrar o próximo bug

Usar o próximo ID B41 e incluir data, build, plataforma, resolução, área/dia/cliente, passos de reprodução, comportamento esperado e observado, frequência, impacto e evidência. Anotar o save por número ou usar dados de teste. Registrar a versão da correção e o resultado do reteste antes de fechar. O [modelo do Teste 01](Art/Releases/Test01/RELATO-DE-BUG.txt) pode ser entregue ao grupo.

Fontes principais do levantamento. [Histórico de desenvolvimento](DEVLOG.md), [registro de testes](TESTES.md), [validação Teste 01](Art/Releases/Test01/VALIDACAO.md) e [validação do teaser](Art/Video/TeaserAtual/VALIDACAO.md).
