# DevLog

## 01/11/2026 — Casa em primeira pessoa e percurso até a loja

Criar ou continuar um save agora entra no quarto em primeira pessoa, usando os mesmos controles, corpo visível e câmera da loja. Novo interior com cama, guarda-roupa, mesa/celular, janela noturna, porta, tapete, pôster e prateleira; materiais de tecido, madeira, reboco, metal e papelão existentes. Móveis e limites impedem atravessamentos. Cama, mesa e guarda-roupa abrem os serviços domésticos; a volta ao quarto devolve controle e atualiza o uniforme no corpo.

E na porta de casa leva à frente do prédio identificado como CASA no bairro. A porta sob DRIPLAB leva à loja, ainda fechada até E no balcão. A saída da loja devolve o jogador à frente da fachada, e a porta do prédio permite voltar para casa. O percurso substitui o botão que pulava diretamente de casa ao trabalho. Portas trocam o ambiente sem adicionar o sistema geral de fades reservado para depois. Pausa, cursor e preservação de campanha permanecem ativos em todos os locais.

Ondulado longo oculta as orelhas e restaura a visibilidade ao mudar de corte. O volume lateral foi ampliado na região do crânio para eliminar a abertura de pele que a orelha escondia. Cada cabelo pode definir os detalhes que precisa, sem obrigar a mesma composição em todos.

Verificação nativa aprovada percorre caminhos dentro do quarto, testa cama/mesa/guarda-roupa, ida e volta pelas quatro portas, abertura manual, chegada do cliente, corpo ao olhar para baixo, pausa, menu, repetição pelo FitFeed e campanha preservada. Criação/continuação dos três saves verificam entrada no quarto. Atendimento completo e 56 combinações de cabelos/coberturas mantidos. Evidências em Captures/HomeCommute e logs home-commute de ../tools; build Windows-HomeCommute sem erros nem avisos. Terceiro pedido de trabalho desde o último envio: commit e push após a conferência final.

## 01/11/2026 — Ondulado longo, coque alto e saída sem deslocar o jogador

Adicionado o sétimo corte, Ondulado longo, com volume contínuo até os ombros, laterais abertas ao redor do rosto, ondas amplas e textura diagonal já usada no cabelo. O coque sem cobertura sobe para o topo posterior, com maior volume, ficando reconhecível de frente. Com bonés/toucas, mantém o encaixe recolhido anterior. Os índices dos seis cabelos antigos permanecem iguais nos saves; seleção e verificações passam a usar a quantidade do catálogo.

A saída do cliente mantém a mesma loja e o mesmo corpo do jogador, substituindo apenas o avatar do cliente pelo look finalizado. Isso elimina a reconstrução que reposicionava o jogador na entrada. O próximo atendimento retoma a posição e direção do balcão usadas na conversa. Testes verificam permanência durante a apresentação, percurso até a porta e início do próximo cliente; a preparação do novo dia continua manual.

Verificações de cabelo ampliadas para 56 combinações de sete cortes, quatro coberturas e dois corpos, mais criação/persistência e vistas nativas de frente/perfil/costas. Build Windows-HairAndExit; evidências em Captures/HairAndExit e logs hair-exit de ../tools. Contagem desde o último envio ao GitHub: 2/3.

## 01/11/2026 — Abertura manual e presença do jogador no balcão

Entrar na loja não inicia mais a chegada. Antes de começar, o jogador pode explorar; E no balcão oferece Abrir a loja. Só então o cliente entra. O estado aberto vale para os próximos atendimentos daquele dia na sessão; um novo dia ou save exige abertura novamente. A chegada e a acomodação do cliente usam a câmera normal em primeira pessoa, permitindo andar/olhar, sem deslocamento cinematográfico nem retorno de câmera.

O corpo do jogador mantém o modelo completo no cenário durante a ida ao provador e a montagem do fit. Cabeça/pescoço ficam ocultos perto da câmera em primeira pessoa e são exibidos quando ela se afasta; roupa, posição e aparência permanecem as do jogador, sem criar outro NPC. A sombra acompanha a vista externa. A revisão de arte dos avatares foi incrementada para reconstruir cópias antigas no Editor.

Testes nativos aprovados: loja fechada sem chegada automática, pausa antes da abertura, câmera estável e movimento livre na entrada, jogador imóvel/visível e com cabeça na câmera externa, chegada ao provador, pausa, cancelamento, saída/FitFeed, próximo cliente, abertura do próximo dia e isolamento dos três saves. Build Windows-ShopOpening sem erros ou avisos. Capturas em Captures/ShopOpening; logs shop-opening em ../tools. Contagem de trabalho desde o último envio: 1/3, sem contar a mensagem duplicada interrompida.

## 01/11/2026 — Fachada na entrada e três saves

Tela inicial separada com fotografia renderizada da fachada noturna em perspectiva e botão Jogar. A tela anterior vira seleção de três espaços: nomes, estado da semana e prévia tridimensional do personagem selecionado. Espaço vazio abre o criador; salvar cria aparência e campanha próprias, e continuar retoma em casa. Retorno ao menu passa pela tela da fachada.

Dados organizados em saves/slot-1 a slot-3. Importação copia a campanha/aparência anteriores para o primeiro espaço, preservando os arquivos de origem e as fotos referenciadas pelo FitFeed. Criar ou cancelar um personagem não altera os outros saves. Slots ilegíveis ficam bloqueados para evitar sobrescrita; backups de campanha e aparência continuam disponíveis. Saves sem slot selecionado não podem gravar o progresso legado.

Verificação nativa cobre migração de semana concluída, criação/cancelamento, três slots independentes, limite de três, troca de personagem/progresso, recuperação de backup e bloqueio de arquivos corrompidos. Também verifica que um save novo recebe o cliente na loja em vez de abrir resumo. Atendimento completo e verificações anteriores preservados. Destino do build: Windows-SaveSlots; logs save-slots em ../tools.

Terceiro pedido de trabalho desde o último envio: preparar commit e push após a verificação final, conforme frequência solicitada.

## 01/11/2026 — Frequentador fixo e clientes separados

Corrigida a interpretação do personagem encostado: ele é um frequentador fixo, com roupa e pose próprias, e permanece na loja durante todo o atendimento. Um segundo personagem entra pela porta, caminha até a frente do balcão e espera indefinidamente até o jogador atendê-lo por trás do balcão com E. Após a conversa, somente esse cliente segue ao provador, troca de roupa e sai. A montagem do look substitui apenas o avatar do cliente; o frequentador é preservado.

A câmera acompanha o cliente contornando o balcão antes de se aproximar do provador. Checagens nativas verificaram os dois papéis durante chegada, espera de quatro segundos sem interação, conversa, caminhada, montagem, saída e próximo pedido. Interagir perto do frequentador não inicia atendimento. Continuidade da câmera, pausa, cancelamento, FitFeed e verificações anteriores aprovados. Build Windows-CounterClients sem erros nem avisos; capturas em Captures/CounterClients. Prévia de câmera validada em 317 quadros, 30 fps, 960×540, sem interface IMGUI.

Contagem de trabalho desde o último envio ao GitHub: 2/3. A mensagem duplicada interrompida não foi contada novamente.

## 01/11/2026 — Atendimento contínuo no interior da loja

A montagem do fit agora acontece fisicamente no provador da loja. O cliente sai da pose apoiada, percorre uma rota com curvas sem parar em cada ponto, aguarda a cortina, entra e vira-se para a câmera antes da interface de roupas. O avatar editável ocupa a posição do cliente, com a sombra mantida e sem duplicação. Giro e zoom continuam disponíveis; o enquadramento mantém espaço para os controles. Ao finalizar, o cliente apresenta o look, caminha para a saída e só depois entra o FitFeed.

Adicionadas aproximações suaves na conversa/provador e retorno gradual à câmera do jogador após a chegada. A pose apoiada ganha entrada e saída graduais. Pés e pernas retornam à posição de descanso ao parar. A câmera e seu campo de visão congelam na pausa, e o cronômetro não consome tempo durante a caminhada.

Cancelar uma sequência ou a montagem descarta o estado parcial da loja, evitando cliente invisível ou preso no provador ao retornar. Checagens nativas aprovadas: continuidade da câmera, ambiente físico da montagem, giro, estoque/pergunta, cronômetro/pausa, cancelamento, saída com look salvo e próximo cliente. Build Windows-ShopFlow sem erros nem avisos. Capturas e prévia de câmera em Captures/ShopFlow; a prévia é renderização da cena, sem a interface IMGUI.

Nova rodada de trabalho desde o último envio ao GitHub: 1/3. Alterações mantidas locais até completar a frequência solicitada.

## 01/11/2026 — Volume e postura natural das mãos

A construção plana anterior foi substituída por uma superfície tridimensional contínua: palma arredondada, quatro dedos próximos com duas falanges curvadas e polegar flexionado. A base é calculada uma vez e reutilizada; cada avatar recebe sua própria malha para a animação. Palmas voltadas ao corpo; contorno preto desligado somente nas mãos para evitar riscos artificiais nas dobras. Revisão dos avatares incrementada para reconstruir cópias antigas no Play.

Build Windows-HandVolume sem erros nem avisos; verificação de malha fechada e orientada, encaixe das mangas/pulsos, caminhada, corpo do jogador e atendimento completo aprovados. Imagens nativas do personagem inteiro e das duas faces de cada mão em Captures/HandVolume.

Rodada 3/3 do versionamento solicitado. Projeto preparado para o repositório THEUZIN5/FitCheck, com README atualizado, créditos das texturas e configurações portáveis. Os manifests publicados excluem a integração MCP local; caches, capturas, builds, recuperação automática e referências de vídeo ficam fora do Git. O preparo do índice ocorre pelo script local ../tools/stage_fitcheck.py.

## 01/11/2026 — Mãos integradas ao avatar

Substituídos palma, dedos e polegar feitos de três elipsoides por uma malha única fechada por mão, com contorno de quatro dedos, polegar integrado, bordas chanfradas e leve curvatura. Palmas viradas para dentro na postura de descanso. A revisão de arte dos moradores foi incrementada para reconstruir os avatares antigos após recompilação durante o Play.

Validação de malha confirma duas faces orientadas por aresta e ausência de triângulos degenerados; os 36 casos existentes de mangas/corpos/movimento reduzido continuam aprovados. Caminhada completa, corpo do player, combinações de roupas e atendimento também passaram na versão Windows-HandFix, compilada sem erros nem avisos. Capturas nativas do personagem inteiro e das duas faces de cada mão em Captures/HandFix.

Contagem para o primeiro commit/envio solicitado: 2/3 pedidos de trabalho. Repositório configurado: THEUZIN5/FitCheck; ainda sem publicação, conforme a frequência combinada.

## 01/11/2026 — Recuperação do cursor e preparação do GitHub

O cursor da exploração volta a ser capturado quando a janela Game recupera foco. Uma liberação temporária do Editor/sistema não altera mais a escolha de liberar o mouse com Tab. A sincronização também ocorre em LateUpdate; o primeiro movimento após recaptura é descartado para evitar saltos da câmera. O mouse permanece disponível fora da Game View, na pausa e nas interfaces de montagem/conversa.

Corrigida a rota vazia de NPC após recompilação durante o Play. Teste no Editor confirmou recuperação de array vazio e RuntimeReady válido, sem novos erros no Console. Build Windows-CursorFix e verificações nativas do atendimento passaram. A recuperação visual da captura após alternar janelas não foi confirmada: a ferramenta de controle da janela expirou ao tentar ativar a Unity.

Git local inicializado nesta pasta, remoto origin configurado para https://github.com/THEUZIN5/FitCheck.git, inicialmente vazio. Nenhum commit/push nesta rodada: frequência solicitada de três pedidos de trabalho, contador atual 1/3. Regras e estado estão em ../AGENTS.md e ../tools/github-cadence.json. O repositório 2D THEUZIN5/drip-lab não será alterado.

## 30/10/2026 — Primeiro ciclo do K7

Implementados diálogo do tutorial e pergunta única opcional, 11 peças iniciais com IDs, seleção por categoria, visualização 3D provisória, confirmação, foto automática em praça provisória, post de resultado e persistência local em JSON/PNG. Pontuação segue pesos 4/3/3; notas abaixo de 5 não retiram Reputação no tutorial.

Arte e interface são provisórias. A produção final de roupas, estampas, animações, áudio e cidade ainda não começou. O protótipo usa IMGUI e não exige Input System; a movimentação futura seguirá o sistema novo previsto no GDD.

Pendência interpretada provisoriamente: maioria neutra nas três peças principais, excluindo acessório. Essa escolha está explicitada no README e precisa de revisão junto à arte.

Teste de execução passou no editor: envio, foto PNG, nota 10/+100 e recuperação do post. Não foi usado o save do jogador. O Console consultado após esse teste retornou zero erros.

Revisão visual da tela inicial feita por captura da Game View. Acrescentada rolagem ao painel de detalhes para comportar pedido e conceitos dos drops simultaneamente. Ainda não foi validada a usabilidade com jogadores nem a versão navegador.

O primeiro build Windows foi gerado sem erros de build, mas falhou ao iniciar a cena por shader removido do player. A correção inclui um material em Resources; o teste fora do editor é necessário para concluir a verificação.

Reteste: build Windows corrigido gerado com zero erros e dois avisos (153,33 MB); inicialização sem repetição da exceção de shader. Smoke check repetido no editor passou após a mudança. Percurso completo por cliques no executável e testes com jogadores permanecem pendentes.

## 30/10/2026 — Primeiro expediente: K7, Bia e Pico

Continuidade da base existente, preservando a cena e o atendimento do tutorial. Pedidos, perguntas, critérios e ordem de Bia/Pico transcritos do GDD. Adicionados histórico consultável, progressão por atendimento, Reputação acumulada com piso zero, marcos pendentes preservados após quedas e fechamento idempotente. Prêmios registrados no fechamento; utilização nas etapas seguintes ainda pendente.

Catálogo agora exibe 21 peças, com 10 bloqueadas e informação dos marcos. As 11 iniciais continuam sendo o inventário deste dia. Salve de expediente separado do save anterior, com importação do post legado e três arquivos de foto. Saída durante montagem recomeça somente o atendimento incompleto, como regra provisória documentada.

Praça, fachada da galeria e quadra diferenciadas com geometria provisória. Cabelo volumoso de Bia e cabeça raspada de Pico distinguem os manequins. Não são assets finais. Home Turf mais saturada para leitura da cor vibrante. Convenções de luminosidade registradas no README; legendas K7/Bia permanecem provisórias.

Validação de lógica passou; teste em Play Mode completou 3 notas 10, Reputação 300, três PNGs recuperados e seis recompensas. Console sem erros na consulta após o teste. Captura da tela inicial inspecionada: texto e controles legíveis. Fluxo completo por cliques, testes com jogadores e navegador ainda pendentes.

Build desta etapa: `Builds/Windows-DayOne/FitCheck.exe` gerado com sucesso (153,35 MB, zero erros e dois avisos). A versão anterior foi preservada em `Builds/Windows`. Este novo executável ainda não passou por percurso completo fora do editor.

## 30/10/2026 — Reconstrução do provador e da apresentação

Após feedback de que visual, controles e atendimento estavam estranhos, esta etapa se concentrou na apresentação, sem adicionar clientes. A lógica do primeiro expediente foi preservada. Personagem substituído por malhas facetadas com proporções próprias, rosto/cabelo e peças volumétricas; mangas, gola, barra, bolsos, cadarços e estampas discretas distinguem as roupas iniciais. Camiseta larga e reta têm larguras diferentes. Gorro e boné têm formatos próprios.

Provador reconstruído com moldura violeta, cortina, arara, banco, tablado e piso de ladrilhos. Luz frontal e ambiente neutras. Interface refeita com hierarquia de texto, catálogo em cartões, miniaturas ilustrativas, seleção e bloqueios; diálogo, confirmação, pausa, resultado e fechamento receberam telas próprias. Arrasto gira e rolagem aproxima; Esc pausa. FitFeed preserva navegação pelo histórico.

Incluída prévia temporária para experimentar sem substituir a campanha concluída. Não corresponde ao sistema de replays em casa. Os assets ainda são uma base geométrica em refinamento, sem animações/áudio, cidade final ou identidade completa de marca.

Na inspeção visual foram corrigidos luz voltada para trás, texto inicial cortado, sobreposição de botões com o personagem e baixo contraste das instruções. A câmera mantém o quadro inteiro; o deslocamento lateral do enquadramento acomoda a interface e evita capturas cortadas. Materiais e malhas de troca são liberados para evitar acumulação por experimentação.

A ferramenta de controle do computador falhou ao iniciar o executável (accessibility window-opened handler did not become ready); percurso por cliques fora do editor permanece pendente. Capturas e verificações de execução foram feitas pela integração da Unity.


Verificação final após os ajustes de enquadramento: regras, smoke check de três clientes e isolamento da prévia passaram novamente; consulta do Console retornou zero erros. Controles por entrada real permanecem pendentes, pois a ferramenta de automação não ficou confiável nesta sessão. Capturas diretas do backbuffer da Unity apresentaram cortes; não tratá-las como teste completo da renderização do executável.

Build final desta etapa: `Builds/Windows-Studio/FitCheck.exe`, 153,36 MB, zero erros e zero avisos. Builds anteriores preservados. Não foi concluída validação por cliques fora do editor.

## 30/10/2026 — Campanha de três dias e progressão completa dos atendimentos

Ampliados os três atendimentos existentes para os nove pedidos do GDD. As seis novas falas, perguntas e legendas foram extraídas do documento original. Implementados os três dias, fechamento independente, sono/avanço em casa e permanência no pós-campanha. A rotina ainda ocorre por menus; caminhada e loja física não foram declaradas prontas.

Casa com mobiliário geométrico, guarda-roupa e celular; quatro uniformes provisórios aplicáveis ao funcionário. Conquistas internas de primeiro expediente, nota perfeita e sintonia, com avisos no canto inferior direito. Sons de interface/publicação/conquista sintetizados originalmente, com opção de silêncio; respiração leve e enquadramento com transição, desligáveis por movimento reduzido.

Todas as 21 peças têm representação geométrica e miniaturas renderizadas. Peças conquistadas passam a ser equipáveis nos dias seguintes, com marcos permanentes e entrega no fechamento. Adicionados visuais distintos de Malu, Seu Jota e Nina, preservando os retornos do elenco. Cenários das fotos incluem praça, azulejos, quadra com mural, salão/casa de shows, terraço com DJ, galeria autoral, fachada decorada e palco/público.

Atendimento de Malu no dia 2 tem 120 segundos, aviso aos 30, pausa, confirmação sem congelar e envio automático ao esgotar. Perda de foco da janela também pausa. Replay funciona a partir do celular em casa, com inventário e pergunta originais, mesma condição de tempo e melhor resultado. Contribuição efetivamente aplicada resolve o caso de penalidade absorvida pelo zero sem inflar ganhos. Marcos do pós-jogo têm entrega equivalente em casa.

Salvamento evoluído para versão 2 no caminho compatível day-one.json, com migração do save anterior, escrita por arquivo temporário, validação antes da troca e backup da versão válida. Galeria com retrospectiva das publicações e notas. Corrigida renderização de várias etapas no mesmo evento da interface; matriz gráfica restaurada ao final. Proporção da câmera reiniciada após miniaturas e fotos para evitar distorção.

Verificações de lógica passaram: nove soluções iniciais, resultados 4/3/3, Reputação máxima 900, 21 peças, três conquistas, marcos após queda, replay sem ganho infinito/roupas futuras, migração e recuperação do backup. Verificação em Play Mode percorreu os nove atendimentos, recuperou nove PNGs e saves, validou pausa/confirmação/esgotamento, arrasto/clique/zoom por eventos simulados e renderizou as 21 peças. Console consultado sem erros.

Convenções e lacunas estão em DECISOES_IMPLEMENTACAO.md. Esta etapa não equivale ao jogo final: seguem pendentes caminhada isométrica, entrada/saída e atuação do funcionário na loja, criação do player, animações com esqueleto, música, falas estilizadas, arte final, marca oficial e versão navegador. Testes por eventos/chamadas não substituem usabilidade com pessoas.

Ajuste adicional de persistência: melhorias de replay geram PNGs com nomes únicos, mantendo a associação da imagem antiga com o backup. Nova tentativa de salvar conserva os bytes pendentes. Miniaturas passaram a isolar somente a categoria do produto, evitando mostrar calças junto à camiseta.

Build Windows desta etapa gerado com sucesso em Builds/Windows-Campaign/FitCheck.exe: 153,41 MB, zero erros e zero avisos. Versões anteriores preservadas. O percurso completo por cliques reais fora do editor e a versão navegador continuam pendentes.

## 31/10/2026 — Passe visual solicitado: PS1, proporções Mii e pintura animada

Trabalho concentrado na direção visual, conforme nova orientação do usuário, antes de ampliar qualquer sistema. Cabeça reconstruída com silhueta arredondada e normais suaves; olhos, brilhos, sobrancelhas, sorrisos e piscadas substituem o rosto rígido anterior. Proporções de cabelo/acessórios e enquadramento ajustados para a cabeça maior.

Criado shader original de pintura, contorno discreto e quantização de cor, sem PBR brilhante. Malhas receberam UVs. Seis atlas originais de quatro quadros (64 × 64 por quadro), em filtro pontual, animam tecido, papel, madeira e reboco. A colagem do provador ganhou violeta/verde/amarelo sem alterar a paleta das roupas. Não foram usados materiais de Storebound ou assets da Nintendo.

Separada renderização do mundo em baixa resolução da interface em resolução nativa, após instabilidade observada nas capturas do filtro inicial. Nenhum tremor de vértice ou distorção de perspectiva foi adicionado ao provador. O movimento reduzido congela a pintura e as piscadas; a pausa permite comparar o filtro de resolução.

Fotos antigas recebem atualização visual por revisão de arte, sem reavaliar pedidos ou aplicar créditos. Imagens anteriores ficam preservadas. A referência e os limites estão em DIRECAO_ARTE.md.

Validação do passe de arte passou: atlas/compilação do shader, quadros de pintura distintos, quadro congelado idêntico e atualização visual das fotos sem alterar os resultados. A campanha foi percorrida novamente com materiais e malhas novos. Durante a revisão, a tentativa de clonar uma fonte dinâmica falhou; foi substituída pelo uso direto da fonte interna com pré-carregamento de caracteres. Captura final ps1-mii-reviewed.png inspecionada com textos e catálogo legíveis. A revisão de fotos é 2, incluindo o enquadramento final para cabelos e acessórios maiores.

Build final do passe visual em Builds/Windows-ArtPass/FitCheck.exe: sucesso, 153,72 MB, zero erros e zero avisos. Versões anteriores preservadas. Esta rodada concentrou-se em PS1, proporções Mii e pintura animada; não ampliou a campanha. Teste interativo completo do novo executável ainda pendente.

## 31/10/2026 — Movimento contínuo e avatar retrô

Após o usuário aprovar as texturas e apontar animação com aparência de FPS baixo, removida a troca rígida a seis quadros por segundo. Os mesmos atlas agora transitam continuamente com smoothstep, 0,4 segundo por transição; quantização ocorre antes da mistura para não recriar degraus. Movimento reduzido e pausa mantêm o congelamento.

Pesquisa de alternativa externa registrada em DIRECAO_ARTE.md. Squigglevision também usa saltos temporais e não foi importado. A correção continua no shader original do projeto.

Usuário esclareceu que deseja referência aos avatares retrô da época dos Miis, preservando a presença das roupas. Cabeça passou a oval com queixo e frente mais plana; olhos pequenos com pupilas, nariz em cunha e boca simples. Removidos brilho de emoji e bochechas; variação de proporções por perfil, mãos menores, corpo estreito e ajustes de cabelo, óculos e acessórios. Pernas sob calças e antebraços sob mangas longas ficam ocultos para evitar atravessamentos. Revisão de fotos 3 preserva resultados e imagens anteriores.

Validação de arte confirmou 50.336 pixels intermediários, ciclo sem salto e congelamento estável. Campanha completa, 21 peças, relógio, replays e salvamento passaram novamente. Captura avatar-retro-final.png inspecionada. A avaliação subjetiva da fluidez no executável fica para teste do usuário.

Build Windows-AvatarMotion concluído: 153,72 MB, zero erros e zero avisos. Executável em Builds/Windows-AvatarMotion/FitCheck.exe. Versões anteriores preservadas.

## 31/10/2026 — Teste jogável dos humanos simples retrô

Usuário autorizou testar no jogo a última proposta visual, após considerar as caricaturas anteriores distantes de YAPYAP/PEAK e pedir referência retrô na própria geometria.

Cabeça reconstruída com planos próprios e bevels, olhos octogonais grandes com pupilas pequenas, nariz em cunha e boca mínima. Pele lisa e imóvel; mãos com polegar, membros e cabelos simplificados. Proporções de cabeça/corpo variam entre os perfis. Camisetas, calças, tênis, mangas e acessórios foram ajustados à nova base. Enquadramento acrescenta espaço aos modelos altos no provador e nas fotos.

Prévia isolada começa com camiseta clara/calça escura e permite alternar compacto/esguio sem trocar o conjunto. Sair da prévia restaura a campanha original e limpa a escolha de proporção. Nenhuma mudança nos critérios, pedidos ou desbloqueios. Fotos antigas da revisão 3 permanecem preservadas durante o experimento; não houve atualização global forçada da galeria.

Validações estáticas e de execução passaram: nove atendimentos, 21 peças/miniaturas, relógio, pausa, replays e salvamento. Arte: 49.381 pixels intermediários, ciclo contínuo e congelamento estável. Comparação de corpo preservada pelo idle; saída da prévia conferida por identidade e conteúdo do progresso. Capturas frontal/lateral e conjunto de manga longa/óculos/boné revisados. A captura alta revelou corte pelo cabeçalho; o enquadramento foi corrigido e a imagem final conferida.

Capturas finais: Captures/retro-human-compact-final.png e Captures/retro-human-tall-final.png. A versão é experimental para avaliação do usuário, sem aprovação definitiva do modelo. Animação de textura do fundo permanece pendente.

Build Windows-RetroHumans concluído: 153,72 MB, zero erros e zero avisos. Versões anteriores preservadas. A execução completa do novo executável fora do Editor permanece para teste do usuário.

## 31/10/2026 — Personagens redondos e reconstrução das peças

Nova orientação do usuário: aproximar bastante de PEAK/YAPYAP, com formas redondas e texturas bem aplicadas. A interpretação angular anterior foi substituída. Implementadas cabeça/olhos elipsoidais, boca e sobrancelhas projetadas no rosto curvo, nariz arredondado, mãos/membros suaves. A expressão/piscada passou a atuar num pivô separado do tamanho da malha do olho, após a revisão revelar escala indevida.

Cabelo agora acompanha a superfície da cabeça e possui linha frontal/comprimento por perfil. Volume é oculto sob boné/gorro. Novos boné e gorro com bordas/copa e aba unida; óculos com aros/hastes curvos. Tênis reconstruídos por seções do calcanhar à ponta, sola compartilhando planta, colarinho e cadarços apoiados na superfície. As três identidades de cor do catálogo foram mantidas.

Criadas superfícies UV próprias em StudioSurfaces.cs e detalhes em StudioDetails.cs. Escala de repetição aproximadamente física; tampas recebem projeção planar; atlas têm margem de meio texel para não atravessar quadros. Quatro atlas estáveis de cabelo, couro, borracha e malha gerados pelo script tools/avatar_textures.py, com importação pontual e sem compressão/mipmaps. Pele usa detalhe suave estático. Iluminação suave opcional do shader usada nos avatares; ambiente mantém as bandas anteriores.

Identificado e corrigido problema de montagem anterior: SetParent preservava transformação mundial, fazendo roupa/cabelo/acessórios herdarem giro e escala incompatíveis após uma troca com avatar girado. Agora componentes novos entram no espaço local correto. Regressão em AvatarRuntimeChecks percorreu 42 trocas em duas proporções já giradas e verificou 247 superfícies com UVs válidos, escala dos olhos e preservação da campanha. Arte e campanha completa passaram novamente. Console sem erros de jogo; avisos do transporte MCP sobre comandos duplicados suprimidos foram observados.

Capturas reais revistas: rounded-human-final.png, rounded-cap-final.png, rounded-beanie-final.png e rounded-shoes-detail.png. Não houve nova geração conceitual de imagem nesta rodada: essas imagens vêm da renderização das malhas no Unity. A animação de pintura do fundo permanece para revisão posterior; a galeria antiga não foi regenerada à força.

Build Windows-RoundedCharacters gerado com sucesso: 153,92 MB, zero erros e zero avisos. Versões anteriores preservadas. Execução completa do executável fora do Editor ainda pendente.

## 31/10/2026 — Cinco correções: esfera, bolinha, expressões, tênis e bonés

Atendidos os cinco pontos explícitos do usuário. A cabeça agora é esfera de diâmetro 1,12, compensando a escala do corpo. O teste radial revelou que a antiga respiração ainda ovalizava a esfera; o esticamento da raiz foi removido. A malha separada Mecha K7, responsável pela bolinha bege, foi removida.

Seis expressões por perfil e por botão na prévia: neutra, curiosa, desconfiada, sonolenta, animada e surpresa. Pupilas mudam forma/tamanho/posição e são projetadas na superfície dos olhos; pálpebras superiores/inferiores são malhas curvas e realizam as piscadas. Bocas e sobrancelhas acompanham a esfera. A prévia abre o catálogo completo para comparar as peças.

StudioSneakers.cs substitui a construção comum recolorida por três formas diferentes: runner alto em camadas com tração, suportes e puxador; skate de cano médio, lona, cadarço cruzado e biqueira de borracha; baixo de couro, perfil estreito e sola fina de borracha. Na revisão, uma sobreposição causou recortes pretos na biqueira de skate; foi substituída por uma região de material da própria malha, sem casca sobreposta.

StudioCaps.cs reconstrói Neighborhood e Independent com abertura inclinada sobre a esfera, seis painéis/costuras, ajuste traseiro e aba integrada. Um usa aba curva, o outro copa estruturada e aba reta. Capturas de frente/perfil conferidas.

Verificações finais passaram: 42 trocas já giradas em duas proporções, raio de todos os vértices da cabeça em espaço mundial, ausência da mecha, 247 superfícies com UVs válidos, três malhas de tênis distintas, seis expressões, nove atendimentos, relógio/pausa, replays e salvamento. Os retornos de timeout do transporte MCP foram conferidos pelos registros de conclusão dos testes; nenhuma operação de teste foi repetida apenas por esse aviso.

Pranchas são renderizações reais das malhas Unity: sphere-expressions-review.png, streetwear-shoes-review.png e caps-construction-review.png. A arte permanece em avaliação do usuário. Galeria anterior preservada durante o teste; animação do fundo não foi expandida nesta rodada.

Build final Windows-SphereStreetwear concluído após a correção da biqueira: 153,94 MB, zero erros e zero avisos. Versões anteriores preservadas.

## 31/10/2026 — Cabelo e junção com bonés/toucas

Usuário gostou das expressões e pediu refino do cabelo e do encaixe com as peças de cabeça. Corrigido o comportamento que apagava todo o cabelo ao equipar cobertura. A nova faixa capilar preserva laterais, costeletas e nuca, começa sob a linha da peça e reduz espessura na parte coberta. O volume sem cobertura tem franja e assimetria integradas à superfície, com linhas diferentes para curto, preso, franja, grisalho e assimétrico. Grisalho passou a região de material da malha, sem volumes colados às têmporas.

Bonés ganharam abertura traseira arqueada acima do ajuste. O cabelo aparece através dela; o coque muda de posição para essa abertura ou para baixo da touca, mantendo união à nuca. Malha do boné recebeu mais segmentos na abertura para suavizar o arco. O perfil sem cabelo permanece sem cabelo.

Novo botão Cabelo na prévia isolada permite comparar os seis cortes com cada cobertura, preservando as demais escolhas. Fora da prévia o corte segue o perfil do cliente. Cabeça esférica, expressões e tênis não foram redesenhados nesta rodada.

Validações finais passaram: 48 combinações de seis cabelos, quatro estados de cobertura e dois corpos. Os 30 encaixes com cabelo e cobertura mantiveram cabelo exposto abaixo da peça, limite de espessura e coque unido. Verificações anteriores também passaram: 42 trocas já giradas, cabeça esférica, 253 superfícies com UV válido, três tênis, seis expressões e nove atendimentos com persistência/relógio/replays.

Capturas reais renderizadas e inspecionadas de frente/trás: hair-fit-short-review.png, hair-fit-tied-review.png e hair-cuts-review.png; captura no provador hair-fit-game-final.png. Não houve geração conceitual de imagem nesta rodada. O resultado continua para avaliação do usuário.

Build Windows-HairFit concluído: 153,94 MB, zero erros e zero avisos. Versões anteriores preservadas.

## 31/10/2026 — Encaixe dos braços e postura natural

Atendido o pedido de encaixar braços nas blusas e reduzir a rigidez da pose. StudioPosture.cs substitui antebraços/mangas separados por superfícies na mesma curva ombro-punho. As mangas entram na blusa e têm barra aberta com espessura e forro, permitindo ver o braço dentro da peça de perfil. A primeira inspeção mostrou quinas nos ombros; as raízes foram aprofundadas na blusa antes da validação final.

Mãos arredondadas com dedos recolhidos, cotovelos dobrados, punhos inclinados, tronco/quadril deslocados, pequenas diferenças entre as pernas e pés abertos compõem a posição de descanso. O idle usa movimento discreto do tronco, sem esticar a cabeça. A cabeça/hair/headwear acompanham a posição por translação, preservando a esfera em ambas as proporções. As peças são reconstruídas a partir das coordenadas de repouso para não acumular deformação nas trocas.

Novo PostureSmokeCheck passou em 36 combinações de nove blusas, dois corpos e duas opções de movimento: centro da manga, folga interna, união mão-punho e pausa estável. Passaram novamente as validações de arte, 42 trocas de avatar/cabeça esférica, 48 combinações de cabelo/coberturas e campanha completa com nove fotos/saves, 21 peças, timer, pausa e replays. Inspecionadas nove vistas nativas e duas capturas do provador.

Build Windows-RelaxedPose concluído: 153,95 MB, zero erros e zero avisos. Versões anteriores preservadas. Este passe não implementa caminhada nem animações completas com esqueleto; o modelo e sua postura seguem para avaliação estética do usuário.

## 31/10/2026 — Revisão das mangas, gorro, identidades e fundo

Reconstruído o torso da blusa com aberturas laterais e mangas ligadas aos pontos dessas aberturas. Menor volume e melhor transição no ombro. A inspeção ampliada encontrou pele aparecendo junto a uma costura; corrigidos a ordenação do contorno e o recorte de pele coberta, deixando apenas a sobreposição necessária sob a manga. A validação verifica também coincidência real dos pontos da junção com a malha da blusa.

Gorro azul ganhou dobra larga mais clara, tecido nervurado, etiqueta e topo folgado inclinado para lado/trás. Ampliada a diferença de tons de pele, cores/cortes de cabelo e expressões iniciais dos seis perfis, mantendo identidade nos clientes que retornam. O botão Cabelo da prévia percorre os estados atualizados.

Substituído o painel de colagem por textura abstrata preta/roxa e shader de deslocamento vertical contínuo. Corrigido o importador antigo que impunha 256 pixels e filtro pontual também à nova textura; AbstractFlow mantém 256x512, filtro bilinear, repetição e ausência de compressão/mipmaps. Velocidade 0,065 alturas/segundo, ciclo de aproximadamente 15,38 segundos. Nenhum vídeo/GIF externo: o efeito em ciclo é renderizado no jogo e compartilha o relógio que respeita pausa e movimento reduzido.

Validações finais passaram: 36 combinações de blusas/corpos/movimento, 42 trocas de avatar, 48 combinações capilares e campanha completa. Nova verificação renderiza o painel isolado e compara deslocamento de 32 pixels para baixo e fechamento do ciclo, ambos com erro médio 0,0000; confirma congelamento estável e opções de acessibilidade. Seis cores de pele, cinco cores de cabelo, seis cortes/estados e seis expressões iniciais verificados. Inspecionadas as capturas de corpo, detalhes, gorro e identidades. O resultado estético segue para o usuário avaliar.

Build Windows-VisualRefinement concluído: 154,35 MB, zero erros e zero avisos. Versões anteriores preservadas. Pacote FitCheck-Windows-VisualRefinement.zip contém a pasta completa para distribuição de teste.

## 31/10/2026 — Erro de envio na prévia e gorro afastado dos olhos

Reproduzida a exceção do usuário: prévia ativa com SB_HEAD_01 selecionado, catálogo completo na interface e inventário inicial da campanha no DayProgress.Add. A liberação visual anterior não havia sido acompanhada por um fluxo próprio de envio.

Submit agora encaminha a prévia para SubmitVisualPreview, validando o inventário da própria prévia e criando avaliação/foto apenas em memória. Não chama Add/Improve, não concede Reputação nem grava fotos/progresso. A tela final oferece Continuar experimentando e Menu, sem navegação pela galeria da campanha. DayProgress.CheckOutfit e os desbloqueios da campanha permanecem restritos. Os verificadores de campanha isolam a sinalização de prévia ao exercitar envios normais.

Novo teste de regressão envia cada uma das 21 peças pela prévia, confirma resultado e inventário, campanha inalterada, zero arquivos criados/alterados e restauração ao voltar ao menu. Verifica também que a mesma peça bloqueada continua recusada na campanha inicial. Teste integrado a Run full campaign runtime check.

Gorro reconstruído com cúpula de 0,35 de altura sobre uma dobra de 0,16, substituindo o topo alto inclinado. A dobra começa em 3,16, acima dos olhos. Ajustada a linha de entrada do cabelo sob a peça. Etiqueta e marca acompanham a curva da dobra em vez de atravessá-la. A verificação capilar agora exige distância vertical mínima de 0,075 entre dobra e olhos nas duas proporções e seis cabelos. Conferidas vistas reais frontal/lateral/traseira e envio da prévia com peças antes bloqueadas.

Build Windows-PreviewBeanieFix concluído: 154,35 MB, zero erros e zero avisos. Versões anteriores preservadas; pacote completo FitCheck-Windows-PreviewBeanieFix.zip.

## 31/10/2026 — Fechamento lateral das pálpebras

O usuário aprovou as expressões e pálpebras, mas identificou uma fresta no perfil. A geometria anterior cobria apenas a metade frontal do olho com uma casca sem espessura, deixando a borda posterior aberta e afastada do globo ocular.

UpdateLid agora constrói uma casca fechada: superfície externa, revestimento interno, continuação em torno da parte traseira do olho e união das duas superfícies na abertura. Reduzido o afastamento da superfície do olho de aproximadamente 8% para poucos milésimos. Mantidos os valores de expressão, coberturas superior/inferior e sincronização de piscada. A fase de fechamento total evita um anel degenerado no polo sem expor o branco do olho.

Novo EyelidSmokeCheck valida 92 malhas ativas em seis expressões, três fases de piscada e dois corpos: cada aresta pertence a duas faces, sem bordas abertas ou triângulos degenerados, com continuação posterior e dimensões limitadas ao olho. Inspecionadas nove vistas de expressão e três fases da piscada em perfil. Arte, campanha completa, prévia com 21 peças, gorro, cabelo e postura passaram novamente. Capturas nativas: eyelid-closed-profile-review.png e eyelid-blink-profile-review.png.

Build Windows-EyelidFix concluído: 154,36 MB, zero erros e zero avisos. Versões anteriores preservadas; pacote completo FitCheck-Windows-EyelidFix.zip.

## 31/10/2026 — Primeiro trecho da cidade

Usuário aprovou manter os personagens como base por enquanto e aceitou iniciar a fachada da DripLab, calçada e parte da praça. Implementada a Praça do Encontro, acessível por Passear pelo bairro no menu, separada dos atendimentos e sem alterar a campanha.

Arquitetura em malhas próprias: quinas chanfradas, fachada em camadas, vitrine com tênis/camiseta, toldos curvos, letreiro pixelado em geometria, etiqueta, azulejos, venezianas, café vizinho, casa azul, cartazes, laje e caixa-d'água. Calçada portuguesa original em atlas estático, canteiro, bancos, palmeiras e árvore. CityPaint combina cor por vértice, luz em faixas e interpolação de pintura; o piso recebe sombra estilizada. Arquitetura agrupada em sete lotes de materiais. Sem importação de assets externos ou instalação de pacotes.

Funcionário usa seu uniforme escolhido. Dois moradores reutilizam cópias independentes dos modelos aprovados. Movimento por WASD relativo à câmera ou clique, com preferência salva, rotas em grade e bloqueios para edifícios, bancos, canteiros, postes, moradores e limites. Câmera isométrica com acompanhamento e zoom. Passos provisórios de pernas/pés preservam tronco e cabeça. Pausa e perda de foco interrompem circulação. Fotos forçam perspectiva temporariamente e restauram o modo anterior.

Inspeção de perto revelou toldo de café baixo e copa atrapalhando a visibilidade; ambos foram elevados. Repetido teste da cidade após os ajustes: cinco percursos concluídos, velocidade diagonal sem ganho, clique correto com imagem retrô, bloqueio de UI, pausa/foco, retorno de câmera e reentrada sem duplicar moradores. Arte, campanha de nove fotos/saves e envio das 21 peças na prévia também passaram. Capturas reais: city-square-final.png, city-driplab-final.png, city-cafe-final.png e city-entry-menu.png.

Limite: primeira fatia externa para avaliação visual e passeio. Não conecta a casa ao trabalho, não implementa interiores, clientes entrando/saindo, rotina dos moradores ou monumento. Os cenários das fotos e a casa continuam anteriores. O percurso de 30–45 segundos do GDD ainda não está pronto.

Ao encerrar a revisão, a conexão MCP parou de responder. A primeira checagem de processos não enxergou o editor, mas o teste de bloqueio do UnityLockfile confirmou que outra instância ainda mantinha o projeto aberto. Acrescentado CitySliceBuild e preparada uma cópia isolada em tools/.city-build-project para empacotar sem disputar essa sessão. A cópia usa apenas os módulos Unity integrados, omitindo ferramentas de editor que não são necessárias ao jogo; o manifesto do projeto original foi preservado. As verificações e capturas já haviam sido feitas no editor interativo.

Build Windows-CitySlice concluído no modo batch: 151,66 MB, zero erros e zero avisos. Conferidos os hashes dos assets/fontes da cópia contra o projeto original, sem diferenças de conteúdo. Resultado copiado para FitCheck/Builds/Windows-CitySlice; relatório Windows-CitySlice-build-report.txt e pacote FitCheck-Windows-CitySlice.zip. Versões anteriores preservadas.

## 31/10/2026 — Passeio em primeira pessoa e revisão gráfica

Atendida a mudança de direção do usuário: passeio em primeira pessoa, inspiração em Anger Foot e vídeo usado somente como base gráfica. Cinco quadros do vídeo local foram decodificados com Blender para examinar formas, cor e textura, sem usar seu conteúdo na produção. Personagens aprovados preservados.

Câmera perspectiva a 2,78 de altura, FOV vertical 68, mouse com limite vertical ±75 e movimento WASD relativo ao olhar. Corpo do player oculto para não atravessar a câmera; restaurado ao sair. Esc pausa/libera cursor, Tab alterna cursor/olhar e clique no mundo retoma a captura. O provador mantém câmera e manipulação anteriores, com FOV restaurado. Sem tremor, corrida, combate ou ações extraídas do vídeo.

Filtro da imagem passou de 360 para até 720 linhas. Granulação urbana reduzida, preservando áreas de cor. Piso em 1024x256 com tiles 256, filtragem bilinear, mipmaps e anisotropia para evitar blocos e cintilação próximos/à distância. UI permanece nítida.

Build em cópia isolada, sem disputar a sessão do editor. Novo modo automatizado -city-proof usa armazenamento separado, não captura o mouse do desktop, executa testes no Player D3D11 e salva vistas reais. O modo batch transforma eventos sintéticos de mouse em Ignore; a verificação passou a exercitar o mesmo manipulador de ponteiro usado pela UI normal, mantendo o teste de Tab e validação de áreas bloqueadas. Na execução final, câmera/limites/cursor/pausa/corpo oculto, cinco percursos, arte, 42 trocas de avatar, 48 combinações capilares, 36 posturas, 92 pálpebras fechadas e envio das 21 peças na prévia passaram. Renderizações nativas 1280x720 inspecionadas.

Build Windows-FirstPerson concluído: 152,61 MB, zero erros e zero avisos. A rotina completa casa–loja e interiores continuam pendentes. Nenhum asset do vídeo foi incluído no jogo.


## 31/10/2026 — Identidade e silhueta do bairro

Paleta conferida no chat INTERFACE: #E30613, preto e branco. Adicionado CitySky, skybox original com céu verde-azulado, horizonte quente e nuvens contínuas. CityIdentity reúne volumes deslocados da loja, cobertura curva costurada, grande etiqueta, terraço do café, vitrines, rádio, bairros em camadas e monumento distante original. Colisões e destinos do primeiro trecho preservados. Bandeiras recebem deformação suave e moradores giram discretamente em postura ambiente. Personagens aprovados e pixelização leve preservados.

Revisão visual do executável corrigiu nuvens excessivamente finas, borda de terraço que parecia flutuar e paredes laterais vazias. Animações respeitam pausa/movimento reduzido. Skybox usa componente local na câmera e restaura o estado ao voltar ao provador. CitySmokeCheck verifica suporte do skybox, avanço/congelamento e restauração, além dos cinco trajetos e controles existentes. Verificação nativa também cobre arte, roupas, cabelo, postura, pálpebras, expressões e envio de looks.

Build isolado Windows-CityIdentity, fontes e assets sincronizados com o projeto original, sem disputar o editor aberto. Capturas em Captures/CityIdentity; relatório e pacote em Builds. Bairro ainda é trecho externo, sem interiores e rotina completa dos moradores.


## 31/10/2026 — Noite, materiais urbanos e atividade do bairro

Atendidos os quatro ajustes do usuário: céu noturno, paleta sem azul dominante, materiais de cidade com detalhe PS1/PS2 e animação visível. Atlas CityNightAtlas gerado com image_gen, importação máxima 512/point/mipmaps; shader urbano mapeia quatro materiais e mantém o piso português existente. Fonte/prompt em Art/CityNight/README.md. RetroPaint ganha parâmetro local de iluminação noturna somente nos materiais copiados dos moradores; provador/personagens originais permanecem com valor padrão zero.

CityResidents guarda geometria de repouso de membros e anima ciclos de caminhada e gesto. Três percursos e quatro moradores; testes exercitam vinte segundos de atividade com checagem de arquitetura, pausa e movimento reduzido. Colisões dinâmicas e parada por proximidade do jogador/outros moradores. CityNight acrescenta carro, luzes estilizadas, janelas/letreiros quentes, cordão de lâmpadas, vento e equalizador animado da rádio. Carro para se o jogador bloqueia a rua.

Executável Windows-CityNight gerado em cópia isolada do projeto; capturas e sequência nativa em Captures/CityNight. Animação é funcional no jogo, não apenas nas capturas. Limites: trecho externo inicial, sem interiores conectados, rotina completa de moradores ou tráfego de sistema aberto.

Acabamento do trânsito após inspeção nativa: carroceria chanfrada, cabine inclinada, vidros separados, rodas circulares com calotas em rotação, faróis e lanternas. Build final Windows-CityNight: 156,65 MB, zero erros e zero avisos; sequência nativa e verificação de atividade concluídas. A prévia MP4 compacta é exportada a 10 fps; o movimento no jogo é atualizado continuamente pelo tempo de cada frame.


## 31/10/2026 — Proporções, caminhada, letras impressas e espaço

Mantida a materialidade retro aprovada; AmbientCG autorizado como fonte futura. Corpo alongado retirado da geometria ativa e da opção de experimentação. HeadLift é zero para todos e BodyProportion usa só a base compacta; cabeça esférica, cabelos e expressões preservados. Check adicional impede o retorno de escala alta/pescoço longo, inclusive quando flags antigos são alternados pelos testes.

Caminhada agora depende da distância real, com desaceleração, giro anterior ao deslocamento, transição de repouso, perna em dois segmentos e pé de apoio estável. Amplitude dos braços reduzida e junção da manga ancorada. Carro ampliado 1,5× em comprimento, 1,42× em altura e 1,45× em largura; colisão e parada ajustadas, rotação das rodas corrigida para o novo raio.

CitySigns/CitySign substituem as letras de geometria por textura de tipografia com desgaste; fonte Impact do sistema, rasterizada com contornos e falhas de tinta. Fonte não é distribuída no build. Letreiros da loja, café, rádio, praça e cartazes cobertos. Menus preservados conforme resposta do usuário.

Disco iniciou com 1.537.609.728 bytes livres. Removidos 33 alvos de builds/pacotes superados, com caminhos absolutos validados e manifesto em tools/cleanup-2026-10-31.json; após a primeira limpeza havia 5.302.644.736 bytes livres. Mantidos todos os Assets/ProjectSettings, GDD, referências/anexos, texturas, ferramentas de execução, cache do projeto aberto, versão noturna funcional e o ZIP EyelidFix como cópia de segurança dos personagens aprovados. Versões antigas removidas deixam de estar disponíveis nos links históricos deste log.

Build Windows-CityPolish em cópia isolada: 159,32 MB, zero erros/avisos. Checks nativos de cidade/campanha, arte, cabelo, expressões, pálpebras, envio, corpo compacto e apoio vertical dos pés passaram. Inspeção de loja/café/carro e sequência de 180 quadros a 30 fps em Captures/CityPolish; vídeo curto usa apenas imagens do executável, sem interpolação.

Após conferir o pacote final, retirada também a duplicata Windows-CityNight da pasta de build isolada, mantendo a versão noturna anterior em FitCheck/Builds. Manifesto total: 34 alvos. Verificados os arquivos preservados, os hashes dos assets/fontes e dos 246 arquivos copiados, e a integridade do ZIP final (68.437.258 bytes). Espaço livre ao final: 5.687.410.688 bytes, aproximadamente 5,30 GiB. Prévia nativa de seis segundos a 30 fps validada com 180 quadros.


## 31/10/2026 — Recuperação de referências do rosto

Corrigido NullReferenceException em UpdatePupil após perda dos caches do rosto, compatível com recompilação de scripts durante Play no editor. Arrays readonly de olhos/malhas/pálpebras não eram serializados. Agora esses campos e listas de ownership são serializados e o rosto recupera referências da hierarquia existente quando necessário, incluindo sobrancelhas, bocas e estado de expressão. As malhas do rosto ficam no proprietário permanente, mesmo se a recuperação ocorrer durante troca de roupa. Caches de postura usam referências serializadas e repouso das pernas persistente, com recuperação de sessões antigas.

Regressão nativa simula perda das referências e testa doze aplicações de expressão em dois caminhos (animação e troca de roupa); recupera as mesmas malhas e verifica ownership. Sequência existente de cidade, arte, roupas, cabelo, postura, pálpebras e envio passou. Este teste simula a perda de caches; a recompilação real do editor aberto não foi controlada, pois a integração não encontrou uma instância acessível. Build Windows-FaceRecovery: 159,32 MB, zero erros e avisos.


## 31/10/2026 — Criação do jogador, cabelos e junções da caminhada

Adicionado editor do jogador com nome (até 24 caracteres), seis tons de pele, seis cortes, seis cores e seis expressões, preview 3D, giro, zoom, salvar e cancelar. Acesso no início e na casa; a primeira ida à casa pede a criação se ainda não existe perfil. Aparência salva em player-appearance.json, separada da campanha, com escrita atômica e backup validado. Clientes mantêm identidades próprias. O uniforme continua sendo escolhido em casa.

StudioView aplica o perfil apenas ao funcionário (customer negativo), sem vincular tom de pele a óculos/bigode dos clientes. Personalização é restaurada no início e na casa, inclusive depois de cancelar. Menus existentes preservados, com painel de criação próprio em preto/branco/vermelho.

Cabelos recebem casca com mais volume, mechas curvas de seção variável e movimento mínimo nas pontas. Sob bonés/gorros, a casca permanece recolhida e não recebe mechas externas novas. Coque e demais modelos anteriores preservados. O limite de volume da checagem sem chapéu foi atualizado para .73; limite sob coberturas continua .577.

Caminhada: região inferior da canela/barra compartilha a transformação do tênis, com transição até o joelho. Braços passam a combinar ombro e antebraço, flexão de cotovelo, balanço menor e ancoragem da manga. Closes stride-0 a stride-5 produzidos para avaliar junções em movimento.

Build inicial identificou arquivo novo fora de Assets; corrigida a localização e repetida a geração. Build final Windows-CharacterCreator: 159,34 MB, zero erros/avisos. Checks de criação/salvamento/cancelamento, recuperação de backup, isolamento da campanha, seis cortes/cores/expressões e sequência anterior de arte/cidade/rosto passaram. Capturas 3D em Captures/CharacterCreator e prévia de movimento em 30 fps. A tentativa de captura automática da interface em execução oculta produziu imagem preta, portanto não é usada como evidência visual da tela; os testes funcionais chamam os mesmos métodos usados por seus botões.


## 31/10/2026 — Cabelo sem mechas tubulares

Removido StudioHairVolume e seus ganchos de animação/registro de mechas nos moradores. HairShell passa a carregar o volume diretamente, com cachos largos, topo enviesado e franja diferenciada. Material recebe leitura de mechas largas e contorno zero, mantendo os materiais retro existentes. Resolução da casca 72×24 com normais e costura suavizadas.

A revisão comparativa mostrou aparência inicialmente lisa demais; localizado uso incorreto de SmoothStep(.78,1,f), que não representa um smoothstep por intervalo em Unity. Corrigido com InverseLerp, mantendo projeção mínima fora do crânio. Teste detectou a falta de margem de um ponto durante o ajuste, corrigida na geometria. Removida também a faixa cinza/branca binária nos lados, preservando a cor escolhida.

HairReview produz dezoito imagens nativas quadradas com proporção correta da câmera. Checks completos de arte, rosto/pálpebras, cabelo/chapéus, campanha e criação executados antes das capturas. Build final Windows-HairRefined; criação e arquivos de aparência mantidos compatíveis. Código e capturas anteriores preservam o histórico; a versão anterior funcional segue disponível.


## 31/10/2026 — Proporções do bairro

CityDistrict substitui os antigos prédios cenográficos com andares/janelas em escala reduzida. Escala baseada na loja aprovada: térreo 4,7, demais pavimentos 4,1, portas 3,96 e esquadrias 2,05×2,1. Volumes com largura 12,4–13,2 e profundidade 10,5; três/cinco andares no fundo e dois/três nas laterais. Espaçamento, lajes, acessos de cobertura e caixas-d'água ajustados. Rua/calçadas cenográficas de apoio adicionadas fora dos limites do passeio.

Café/rádio passam a ter corpo de dois pavimentos com altura 8,9 e janelas superiores em 6,8; terraço/vasos do café e torre da rádio acompanham os deslocamentos. Não alteradas as dimensões da loja DripLab, personagens, câmera ou limites de navegação.

CityScaleReview registra cinco vistas nativas: loja, lateral direita, lateral esquerda, skyline e café. Inspecionados os renders, incluindo a relação entre personagens e esquadrias. Sequência anterior de checks passou, com cinco trajetos e campanha/criação preservadas. Resultados em Captures/CityScale e tools/city-scale-proof.log. Build Windows-CityScale empacotado a partir da cópia isolada.

Build final Windows-CityScale: 159,34 MB, zero erros e avisos; 246 arquivos e ZIP verificados. Retirada a cópia temporária do build isolado e quatro ZIPs duplicados (CityNight, CityPolish, FaceRecovery, CharacterCreator), mantendo suas pastas completas de executáveis. ZIPs atual CityScale, anterior HairRefined e backup EyelidFix preservados. Espaço livre ao final: cerca de 5,24 GiB.


## 31/10/2026 — Sobrancelhas visíveis e moradores em movimento

Atendida a lista de Franja/Assimétrico, cabelo sobre sobrancelhas, sobrancelhas em traço e NPCs parados; aplicada também a preferência por filtro desligado. HairEdge protege a região frontal acima das sobrancelhas; Franja ganha recorte próprio e Assimétrico mantém uma diagonal mais clara. Topo de ambos reduzido. StudioBrows cria volumes afilados e arqueados por expressão, mantendo vínculo à superfície da cabeça.

Nos testes, identificado erro numérico na ponta da nova sobrancelha: sin(pi) em float produzia valor negativo minúsculo, incompatível com potência fracionária. Limitada a base a não negativa. Cópia dos moradores também passa a usar buffers e submeshes independentes, evitando dependência de malhas mutáveis do provador.

NPCs estavam acoplados ao relógio de efeitos: Frame só animava quando movimento reduzido estava desligado. Separados TickResidents/TickTraffic de AnimateAtmosphere. Estado dos moradores, rotas, ownership e referências de efeitos agora serializáveis para recompilação durante Play; fallback recompõe o bairro se referências legadas estiverem vazias, preservando posição/olhar quando livres.

ExpressionLifeReview verifica 216 pontos/rays de visibilidade em 36 pares cabelo/expressão, caminhada em movimento reduzido usando UpdateCity, pausa e recuperação após perda simulada da lista. Capturas nativas e 180 quadros/30 fps em Captures/ExpressionLife. Baseline de cidade/campanha/roupas/rosto/cabelos/criação passou. O teste simula perda de referências; não controla uma recompilação real no editor aberto.

## 31/10/2026 — Linha do cabelo e caminhada do corpo inteiro

Revertida a abertura excessiva da testa: linha frontal dos cabelos desce, sobrancelhas aproximam-se dos olhos e ficam mais finas. Ajustadas em conjunto para manter expressão visível sem aparência de cabelo recuado.

Identificada a causa dos membros rígidos: SmoothStep era usado como se os dois primeiros argumentos fossem limites de entrada; na Unity são os valores de saída. Pesos de ombro e cintura resultavam negativos e a interpolação anulava o movimento. MotionWeight normaliza com InverseLerp antes de SmoothStep. Todos os meshes agora participam da pose: quadril, tronco com contra-rotação, cabeça com vínculo ao pescoço, braços com cotovelo e pernas com IK. Calçados e barra da calça compartilham a transformação do tornozelo; apoio vertical continua verificado. Rig versão 2 força reconstrução de moradores antigos durante recuperação.

BodyMotionReview mede deslocamento dos vértices com raiz fixa, separadamente para cabeça, tronco, braços, pernas e calçados. Valores RMS observados: 0,124 / 0,073 / 0,168 / 0,158 / 0,276. Captura próxima de 180 quadros nativos a 30 fps acompanha um morador; câmera da prova fica fora da faixa dos carros. Filtro continua desligado por padrão. Build Windows-BodyMotion.

## 31/10/2026 — Franja longa e corpo em primeira pessoa

Franja deixa de obedecer à reserva frontal das sobrancelhas: borda com queda central chega a cobri-las, conforme pedido. As outras expressões continuam iguais. Verificação de sobrancelhas mantém proteção dos demais cortes e permite a cobertura intencional da Franja.

Velocidade dos NPCs aumentada em 50% (1,38/1,59 unidades/s), com aceleração proporcional; fase continua derivada da distância para aumentar a cadência junto da velocidade. Jogador passa de 3,05 para 4,4 unidades/s, mantendo normalização da diagonal e colisões.

CopyAvatar compartilha a cópia independente de malhas entre moradores e corpo do jogador. Cópia de primeira pessoa usa roupa/pele escolhidas e rig articulado existente, omitindo rosto/cabelo/cobertura da cabeça para liberar a câmera. Segue a direção do olhar mesmo ao andar de lado. Fase do jogador avança pela distância (ciclo a cada 2,2 unidades), com parada gradual. Corpo é recriado a cada entrada para refletir mudanças da aparência e roupa; suas malhas e materiais anteriores são liberados sem destruir recursos do provador. Recuperação de runtime inclui ausência do corpo.

Limite de olhar para baixo ampliado de 75 para 88 graus. A primeira revisão visual mostrou o pescoço fechando a vista superior: ele também é omitido na cópia, e o corpo fica 0,38 unidade atrás da câmera para expor pernas/calçados à frente do torso. Isso não modifica o modelo no criador ou os NPCs. Capturas nativas em Captures/FirstPersonBody incluem vista a 65/88 graus e durante o passo. Build Windows-FirstPersonBody; filtro desligado por padrão preservado.

## 31/10/2026 — Primeiro interior jogável da DripLab

Usuário pediu avançar apesar das pendências de cabelo/personalização, priorizando o interior da loja antes de conectar a rotina com a rua. Criado ShopInterior: balcão com caixa e sacola, araras/cabides com seis camisetas do catálogo, três prateleiras com nove pares dos três tênis existentes, mesa com peças dobradas, banco, vitrines noturnas e trilhos de iluminação. Provador preto/roxo com cortinas, banco e painel abstrato reaproveitado. Materiais urbanos aprovados mantidos; paleta preto/creme/Vermelho Kaos, roxo reservado ao provador. Atlas de letreiros impresso ampliado com coleções e PROVADOR, sem TextMesh no mundo.

Interior usa CityView em espaço independente, com limites/obstáculos próprios, sem trânsito ou skybox dentro. Acrescentada iluminação de interior ao material urbano, com valor padrão zero para preservar o bairro. Corpo do jogador, velocidade, WASD/mouse, pausa e Tab mantidos. Morador na loja corresponde ao próximo cliente da campanha e aguarda em pose de espera; entrada/saída física dos clientes ainda não implementada.

Menu Visitar a loja e ação Ir trabalhar abrem o interior. E perto do balcão inicia o diálogo do próximo pedido (ou resumo se o dia acabou). E no provador abre a prévia com catálogo completo e campanha isolada. E na porta voltada para a saída retorna ao menu. A ligação física rua–loja permanece para uma etapa posterior, conforme ordem combinada. Não foi implementado sistema de caixa/estoque/compras.

ShopReview percorre corredores, verifica bloqueios, interação com E, pausa, atendimento sem alterar progresso, prévia isolada, corpo visível, recuperação de referências do interior e restauração de bairro/menu/câmera. Sete vistas nativas 1280×720 em Captures/ShopInterior, inspecionadas para piso, escala, roupas/tênis, balcão e provador. Build Windows-ShopInterior.

## 31/10/2026 — Revisão da câmera e dos móveis da loja

Usuário relatou olhar limitado aos lados e com pouca amplitude, além de mobiliário simples. Eixos Mouse X/Y estavam configurados; interação real na Unity anterior produziu inclinação vertical. Revisada a captura: alterações de lock adiadas de OnGUI para Update, cursor não é travado repetidamente, dois frames iniciais ignoram deslocamento da captura e liberação externa permite recapturar pela cena/Tab. Sensibilidade de 2,2 para 2,8. Arrasto com botão direito, quando cursor livre, oferece controle alternativo nos dois eixos e permite WASD durante o arrasto. Dica do interior não cobre mais o botão de retomar olhar. GroundPoint usa a origem real do espaço, inclusive interior.

ShopFurniture adiciona sólidos com cantos arredondados, tampo/rodapé do balcão, frente ripada, gavetas/alças, monitor inclinado e pedestal, teclado, leitor e impressora com recibo. Sacolas têm pregas e alças. Mesa substitui pedestal fechado por pés tubulares e prateleira com caixas; camisetas dobradas têm gola e bordas. Araras ganham bases/rodízios, prateleiras ganham suportes/etiquetas, banco ganha assentos acolchoados e pés inclinados. Cortinas deixam de ser colunas de blocos e viram superfícies com pregas, argolas e barra. Catálogo, personagens, proporções do espaço e colisões anteriores preservados.

CheckShopLookInput testa rotação vertical/horizontal e parada ao soltar botão. Unity batch reescreve Mouse Events sintéticos como Ignore; prova chama o mesmo handler com valores crus, como já feito para clique do bairro. Primeiro teste revelou essa limitação da simulação, corrigida no caminho de teste. ShopReview completo e sete renders nativos passaram. Executável normal substitui Development Build para não iniciar serviços de depuração em rede; não requer mudanças no firewall. Ver Captures/ShopRefined e logs shop-refined. Executável aberto por UI e conferidos menu, entrada e alternância de cursor por Tab; avaliação do dispositivo/sensibilidade do usuário continua necessária.

## 31/10/2026 — Cliente apoiado, braços relaxados e abertura do provador

Cliente da loja deslocado para a parede junto ao balcão. Pose própria com peso na perna esquerda, quadril/ombros inclinados, joelho direito dobrado e sola apoiada na parte baixa da parede. Alvo do tornozelo considera a geometria real do tênis para manter contato sem atravessar a parede. Expressão da loja usa pálpebras assimétricas, olhar lateral e sorriso de canto, preservando as seis opções do criador. Apoio baixo revestido integra a pose à parede sem fazer a cabeça atravessá-la. Interação de atendimento acompanha a nova posição do cliente.

ArmControl compartilhado aproxima cotovelos/punhos do corpo e baixa as mãos. Suportes da mão e articulação dos moradores acompanham os novos pontos; mangas e pele continuam usando a mesma curva. Movimento de braços/cotovelos, tronco, quadril e pernas mantido na caminhada. Versão 3 do rig força atualização de cópias antigas na recuperação.

Cortinas passam a ser duas malhas independentes com tecido e argolas. Começam fechadas e bloqueiam passagem. E inicia abertura de 0,72 s, recolhendo o tecido para os lados. Só após terminar a abertura o fluxo entra na prévia de roupas, isolada da campanha. Entrada repetida não pula a abertura; pausa congela o progresso; sair/cancelar e voltar fecha a cortina. Não foi adicionado fade geral ou sistema de transição entre locais, conforme pedido de deixar essa parte para depois.

Provas nativas verificam contato/elevação do pé, preservação da campanha, abertura antes de vestir, pausa e reset. Baseline passou em 36 combinações de blusa/corpo/movimento reduzido. Movimento local dos braços na caminhada medido em RMS 0,185, sem contar translação da raiz. Seis poses de caminhada, retrato do cliente e 36 frames nativos da cortina em Captures/ShopPose. Câmera externa da demonstração omite a cópia sem cabeça do jogador; visibilidade real de primeira pessoa permanece igual.

## 01/11/2026 — Traço solto na cintura

Removida a peça geométrica Barra costurada da montagem compartilhada das blusas. Era uma faixa reta separada do tecido, visível como traços soltos na cintura. Remoção abrange jogador, clientes, moradores e peças expostas. Atualizado o build Windows-ShopPose; evidências em tools/waist-clean-build.log e tools/waist-clean-proof.log.

## 01/11/2026 — Materiais do ambientCG

Usuário autorizou buscar mais texturas no ambientCG. Consultada API oficial e licença CC0; selecionados Wood051, Fabric030, Metal032, Concrete034, PaintedPlaster017, Bricks074, Cardboard001 e Asphalt012. Baixados pacotes 1K-JPG apenas temporariamente, extraídos somente os mapas Color e removidos ZIPs. Fontes JPEG originais em Resources/Surfaces; API, procedência, licença, tamanho e SHA-256 registrados em Art/AmbientCG. Não importados mapas normais, altura, roughness ou pacotes de alta resolução.

AmbientSurfaceImporter configura 256 px (512 para tecido), repetição, mipmaps, filtro bilinear/anisotropia e compressão. CityPaint aceita superfícies individuais além dos atlas antigos; luminância normalizada, contraste e tintas por papel preservam Vermelho Kaos, creme, preto e roxo. Madeira em tampos/cabides/móveis; tecido em cortinas/assentos/roupas; metal nas ferragens internas; concreto no piso; reboco nas paredes internas; tijolos/papelão/asfalto no ambiente. Fachadas de reboco e calçada portuguesa externas mantêm suas bases anteriores. Normalizada a seleção de material das ferragens para metal, evitando veios de madeira em suportes.

UVs dos tampos deixam de repetir a imagem por triângulo: projeção contínua por plano mantém os veios alinhados em superfícies arredondadas. Piso interno substitui o padrão de calçada por placas de concreto com juntas. RetroPaint recebe detalhe estático de tecido somente nas peças de roupa; pele, cabelo, olhos e expressões continuam com seus materiais anteriores. Sem filtro global adicional e sem novo efeito de animação de textura.

AmbientSurfaces.Validate exige as oito fontes, resolução limitada, mipmaps e repetição. Prova completa de loja, controles, campanha, cortina, apoio na parede, corpo e caminhada passou; renders nativos de loja, móveis, rua e asfalto inspecionados. Build Windows-SurfacePass; logs tools/surface-pass-build.log e tools/surface-pass-proof.log. Cópia funcional Windows-ShopPose preservada.
