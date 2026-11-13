# Fit Check by DripLab — Game Design Document

Versão 2.0 • 13/11/2026 • TCC DripLab • Português brasileiro.

Este documento consolida as decisões aprovadas e a implementação registrada até as entregas Windows/Web de 13/11/2026. O conceito de 30/10 foi preservado em [GDD histórico](Art/Documentation/GDD_2026-10-30.md). As regras abaixo prevalecem sobre metas, nomes, câmeras e quantidades daquela versão.

## 1. Visão

Fit Check é um jogo 3D de interpretação de pedidos e composição de visuais streetwear. O jogador trabalha na DripLab durante uma semana, conversa com clientes, escolhe peças e acompanha as fotos dos resultados no FitFeed. A expressão pessoal, a música e a vida do bairro ligam a atividade à marca.

O público é jovem, interessado em moda urbana, games e cultura digital. A cidade brasileira fictícia integra a rotina, os encontros e a ambientação. A avaliação decorre das pistas e dos atributos cadastrados nas roupas. Várias combinações podem atender ao mesmo pedido.

## 2. Escopo entregue

| Elemento | Estado em 13/11/2026 |
|---|---|
| Engine | Unity 6000.6.7f1 |
| Plataformas | Windows x64 e WebGL/WebAssembly |
| Campanha | Sete dias, três atendimentos por dia, 21 encontros |
| Clientes distintos | 13 perfis presentes na campanha |
| Catálogo | 32 peças nos três drops, 11 iniciais |
| Slots de roupa | Cima, baixo, calçado e acessório |
| Saves | Até três espaços independentes |
| Rotina | Casa, bairro e loja em primeira pessoa, com portas nos dois sentidos |
| Resultados | Fotos automáticas, critérios, Reputação, comentários e replays no FitFeed |
| Conteúdo doméstico | Aparência, uniformes, companheiro, decoração, espelho e lembrança final |
| Distribuição atual | Builds/FitCheck-Windows.zip e Builds/FitCheck-Web.zip |

A build pública deve permanecer abaixo de 200 MB. A entrega atual mede 77.454.290 bytes no ZIP Windows e 139.514.362 extraídos. A Web mede 52.269.080 bytes no ZIP e 52.379.149 extraídos. A publicação no itch.io continua manual.

## 3. Entrada, personagem e persistência

Fachada noturna animada → Jogar → seleção de até três saves → criar ou continuar → casa. Cada espaço mantém aparência humana, companheiro, campanha, decoração e fotos. O criador oferece bases de aparência, pele, cabelo, cores, expressão, nome e companheiro. O guarda-roupa permite editar a aparência e escolher uniformes com prévia do corpo inteiro.

Os dados são locais. A migração conserva o legado no primeiro espaço. Criar outro personagem preserva os espaços existentes. Excluir exige confirmar o número e o nome do espaço, limita a remoção à pasta confirmada e impede reimportação automática do legado. Cancelar preserva os dados. Arquivos inválidos e backups recebem validação antes de uso.

Windows e navegador têm armazenamentos separados. O navegador usa IndexedDB. Limpar os dados do site pode remover os saves locais. Os verificadores utilizam pastas ou perfis isolados.

## 4. Rotina e controles

O quarto fica no residencial AP 03, na Rua das Lajes. A porta conduz ao vestíbulo e ao bairro. A DripLab é alcançada fisicamente em cerca de 30–45 segundos caminhando. As medições da entrega atual foram 31,87 s na ida e 31,18 s na volta. RADIO é outro estabelecimento.

WASD move em primeira pessoa, mouse orienta a câmera, Shift corre e E interage com o alvo próximo. Caminhada usa 4,4 unidades/s e corrida 7,2, com diagonal normalizada. Tab controla a liberação do cursor. No provador, arrastar gira e a roda aproxima. Sensibilidade e demais controles estão nas configurações.

Windows usa Esc na pausa. Na Web, P pausa e retoma, e os botões controlam Voltar/Cancelar. Esc fica reservado ao navegador e não executa comandos do jogo. Um clique recupera a captura do ponteiro quando necessário. A ajuda deve mostrar a plataforma atual.

Os loadings de Jogar e das entradas/saídas cobrem a preparação do destino. Duração mínima total de dois segundos, incluindo revelação de 0,18 s. Logo vermelha, pontos reversíveis e diagonais contínuas movem-se lateralmente. Comandos ficam bloqueados durante a transição. Movimento reduzido conserva a identidade estática e aguarda o destino quando o preparo demora mais.

## 5. Casa e loja

A cama abre o descanso e a passagem de dia. O guarda-roupa abre aparência/uniforme. O celular na mesa abre o FitFeed. O catálogo na mesa abre a decoração em F. A mesa separa celular, catálogo, luminária e porta-retrato. A estante tem livros, vinil e caixa de som.

A loja começa fechada. O jogador abre no balcão quando desejar. Clientes entram um por vez, caminham até a frente do balcão e esperam a interação. O jogador conserva câmera e movimento livres na chegada. O frequentador apoiado na parede tem presença própria e permanece fora da fila de atendimento.

Após a conversa, o cliente vai ao provador. A câmera acompanha o percurso e a cortina abre antes da montagem. O jogador continua visível inteiro no balcão nas vistas externas. Cabeça e pescoço são ocultados apenas junto à câmera de primeira pessoa. Com o provador vazio, E abre ou fecha a cortina.

## 6. Atendimento

1. Receber o cliente no balcão e ler o pedido.
2. Escolher uma de duas perguntas opcionais ou prosseguir sem perguntar.
3. Acompanhar a entrada no provador e experimentar as peças.
4. Consultar o pedido e os conceitos dos drops, girar e aproximar a câmera.
5. Finalizar e confirmar o conjunto. Nos desafios especiais, cumprir os requisitos adicionais comunicados.
6. Ver o cliente sair com o fit escolhido e acompanhar a publicação.
7. Conferir foto, critérios, nota e Reputação. Prosseguir até o fechamento do dia.

Clientes chegam com roupas cotidianas e calçado civil, preservando rosto, cabelo e expressão. O catálogo DripLab entra na montagem. Cima, baixo e calçado permanecem obrigatórios. Acessório pode ser removido. Óculos, bonés e outras peças do slot usam o mesmo catálogo, com seus atributos próprios.

## 7. Regras de avaliação e Reputação

Três critérios binários, com pesos 4, 3 e 3. As notas possíveis são 0, 3, 4, 6, 7 e 10. As justificativas aparecem no resultado. Atributos de cor, luminosidade, neutralidade, modelagem, estampa e drop devem corresponder à leitura visual das peças.

O tutorial considera maioria neutra entre cima, baixo e calçado, com pelo menos duas peças neutras. Estampa discreta vale para todas as peças vestidas. Essa é a convenção implementada em Rules. O cadastro do catálogo é a referência para cada classificação.

Nota 10 concede +100, 7 concede +60 e 6 concede +30. Abaixo de 5 aplica −30, com piso zero. O tutorial protege contra perda. Notas baixas permitem continuar a campanha. O máximo bruto de 21 notas 10 é 2.100 RP.

Marcos em 100, 200, 300, 450, 600, 750, 1.050 e 1.400 RP. Marcos alcançados permanecem registrados e as entregas chegam no fechamento. Quantidade e peças por marco seguem o catálogo abaixo. Peças conquistadas não voltam a bloquear.

Replays em casa conservam inventário e condições históricos. Só a melhora do melhor resultado gera ajuste de Reputação, considerando a contribuição efetiva anterior. Prévia e opção reversível de teste permitem avaliar todas as roupas, sem reescrever inventários históricos ou recompensas. O catálogo de teste começa desligado nas entregas.

Uniformes são uma progressão separada. Incluem as conquistas Primeiro Expediente, Visual Perfeito e Sintonia com os Clientes, além de Amigo do Fubá, Figurinha do Bairro, DJ de Garagem e Fantasma do Provador. O traje Modo Fubá depende de oito carinhos no Fubá do bairro. As condições e os contadores persistidos ficam em CampaignProgress.cs.

## 8. Campanha, elenco e desafios

| Dia | Clientes em ordem |
|---|---|
| 1 | Kaio, Lia, Breno “Pico” |
| 2 | Júlia, Kaio, Seu Jota |
| 3 | Lia, Júlia, Nina Faísca |
| 4 | Dandara, Ravi, Iara Ponteiro |
| 5 | Téo, Joana, Dante Remendo |
| 6 | Cora Semente, Dandara, Kaio |
| 7 | Ravi, Joana, Nina Faísca |

Os nomes antigos K7/Bia/Malu em IDs e documentos históricos foram preservados quando necessários à leitura dos saves. As falas, perguntas, legendas e critérios atuais estão integralmente em DayOne.cs, CampaignContent.cs, WeekExpansion.cs e SpecialCustomers.cs.

Júlia no primeiro atendimento do dia 2 tem 30 segundos. Iara no terceiro atendimento do dia 4 tem 45. O relógio começa na montagem, congela na pausa, continua na confirmação e avisa nos dez segundos finais. Tempo esgotado envia o conjunto atual. Os demais encontros seguem sem cronômetro próprio.

Iara pede SIDE B, cima discreta e contraste. Dante pede calça, cima larga e COMMON GROUND. Cora pede calça, cima clara e maioria neutra. Eles substituem repetições mantendo 21 encontros, IDs e avaliação compatíveis com históricos.

Há três requisitos especiais nos atendimentos ao vivo. Ravi no dia 4 mantém sua camiseta de chegada. Téo no dia 5 exige cima, baixo e calçado AFTER HOURS. Joana no dia 7 pede o calçado usado por Ravi no atendimento anterior. O jogador monta manualmente o fit e recebe explicação se o envio estiver fora da proposta. Replays e prévias mantêm as regras históricas de pontuação.

## 9. Catálogo atual

AFTER HOURS combina vida depois do expediente e luz urbana, com apresentação bordô. COMMON GROUND trata de praça, quadra e pertencimento, com apresentação verde. SIDE B reúne música independente e criação autoral, com apresentação violeta. O provador permite consultar categorias e drops. Itens bloqueados indicam o marco necessário.

Os originais fornecidos ficam em refs roupas/modelos tenis, fora de Assets. A integração usa cópias otimizadas, texturas e malhas ligadas à animação, verificadas em repouso e movimento. IDs, semântica e desbloqueios acompanham a substituição visual. Fontes Blender e referências permanecem preservadas.

| ID | Peça | Drop | Categoria | Reputação |
|---|---|---|---|---|
| AH_TOP_01 | Last Stop | AFTER HOURS | Cima | Inicial |
| CG_TOP_01 | Local Team | COMMON GROUND | Cima | Inicial |
| SB_TOP_01 | Demo Tape | SIDE B | Cima | Inicial |
| AH_BOTTOM_01 | Late Shift | AFTER HOURS | Baixo | Inicial |
| CG_BOTTOM_01 | Open Court | COMMON GROUND | Baixo | Inicial |
| CG_BOTTOM_02 | Home Turf | COMMON GROUND | Baixo | Inicial |
| SB_BOTTOM_01 | Backstage | SIDE B | Baixo | Inicial |
| SB_SHOES_01 | Garage Session | SIDE B | Calçado | Inicial |
| AH_SHOES_01 | Sidewalk | AFTER HOURS | Calçado | Inicial |
| AH_HEAD_01 | After Dark | AFTER HOURS | Acessório | Inicial |
| CG_HEAD_01 | Neighborhood | COMMON GROUND | Acessório | Inicial |
| AH_TOP_02 | Night Route | AFTER HOURS | Cima | 100 |
| SB_BOTTOM_02 | Remix | SIDE B | Baixo | 100 |
| CG_TOP_02 | Court Lines | COMMON GROUND | Cima | 200 |
| CG_SHOES_01 | Daily Steps | COMMON GROUND | Calçado | 200 |
| AH_TOP_03 | Closing Time | AFTER HOURS | Cima | 300 |
| SB_HEAD_01 | Independent | SIDE B | Acessório | 300 |
| SB_TOP_02 | Cut & Paste | SIDE B | Cima | 450 |
| AH_BOTTOM_02 | City Glow | AFTER HOURS | Baixo | 450 |
| CG_TOP_03 | Meeting Point | COMMON GROUND | Cima | 600 |
| SB_TOP_03 | Unreleased | SIDE B | Cima | 600 |
| CG_TOP_04 | Pista Aberta | COMMON GROUND | Cima | 750 |
| AH_BOTTOM_03 | Cargo das Lajes | AFTER HOURS | Baixo | 750 |
| AH_SHOES_02 | Slide Nuvem | AFTER HOURS | Calçado | 750 |
| SB_TOP_04 | Fita Local | SIDE B | Cima | 1050 |
| CG_BOTTOM_03 | Bermuda de Pista | COMMON GROUND | Baixo | 1050 |
| AH_TOP_04 | Luz de Poste | AFTER HOURS | Cima | 1400 |
| SB_HEAD_02 | Aba de Tijolo | SIDE B | Acessório | 1400 |
| AH_SHOES_03 | Clog Brasa | AFTER HOURS | Calçado | 1050 |
| CG_BOTTOM_04 | Jeans Rosa | COMMON GROUND | Baixo | 750 |
| SB_HEAD_03 | Óculos Prisma | SIDE B | Acessório | 1050 |
| CG_BOTTOM_05 | Bermuda Comando | COMMON GROUND | Baixo | 1050 |

## 10. FitFeed e memória final

O cliente publica sua foto automática com o conjunto enviado, ocasião, legenda, avaliação e Reputação. O histórico pertence ao save ativo. Fotos antigas continuam legíveis após revisões do mapa e das roupas. Comentários locais usam retratos de personagens e respostas adequadas ao desempenho. Fundo preto com estrelas cinza diagonais acompanha a identidade da página do jogo, com cartões e barra lateral legíveis.

Clientes atendidos aparecem no bairro no local da foto, com o look salvo e permanência durante o restante do dia. O jogador pode encontrá-los mesmo com a loja aberta. No dia seguinte deixam o local. Encontros registrados alimentam a conquista Figurinha do Bairro.

Ao concluir a semana, uma foto coletiva de 960×540 reúne o jogador e os 13 perfis de clientes, usando o último look de cada um. week-group.png fica no espaço do save. O porta-retrato na mesa permite consultar a lembrança e gerar a foto para saves já concluídos. Recriar ou excluir um save descarta a imagem da sessão anterior.

## 11. Companheiros e vida no bairro

Criar ou editar aparência inclui espécie, tipo, pelagem e nome do companheiro. Gatos têm SRD, Persa, Siamês e Sphynx. Cães têm vira-lata, Shiba e Dachshund. Há seis cores por espécie, com gato branco na opção antes bicolor. Branco e siamês têm olhos azuis. O mesmo companheiro descansa em casa e na loja, com respiração, carinho em E, som e movimentos de olhos, orelhas e patas. Dados antigos recebem gato padrão.

Fubá é o gato laranja fixo próximo à DripLab. Apenas os carinhos nele contam para sua conquista, mesmo que outro pet se chame Fubá. Sons e movimentos respeitam pausa, mute e intervalo.

Emotes de exploração usam 1 joinha, 2 palmas, 3 aceno, 4 “67” e 5 Absolute Cinema. Zero encerra e vírgula aciona um gesto secreto. Caminhar ou interagir cancela o gesto. Mãos e braços acompanham a visão e aparecem corretamente nos espelhos.

Moradores visíveis a até sete metros reagem aos emotes, com intervalo de oito segundos e verificação de arquitetura entre eles e o jogador. Objetos acompanham as mãos e as rotas são retomadas. Cartazes mudam em sete estados, com preparativos no bairro ao longo da semana.

## 12. Decoração, espelhos e cenário

F na loja fechada abre paredes, piso, cortina, quadro, plantas e as duas cores independentes do fundo animado. F no catálogo doméstico abre paredes, piso, têxteis, artes, planta e moldura do espelho. A câmera enquadra suavemente o item alterado. Movimento reduzido faz a troca direta. Vistas manuais continuam disponíveis.

Salvar grava shop-style.json ou home-style.json no save ativo. Cancelar restaura a prévia. Falha de gravação conserva o rascunho e mostra mensagem. Dados antigos recebem defaults válidos. As molduras do quarto são Original, Paint 95 e Fit Player. O reflexo central permanece real.

Quarto e loja têm espelhos de corpo inteiro, com textura 128×352 e atualização máxima de 15 Hz, apenas de frente e no enquadramento. A renderização restaura a cabeça e os braços após usar a pose externa. Pausa e ambiente ativo controlam a atualização.

O bairro tem calçadas contínuas, túneis de trânsito, perímetro fechado, galeria/quadra ao norte e AO VIVO ao sul. Barbearia, lavanderia e consertos preenchem o trecho mercearia–oficina. Estúdio SOM/LADO B e pátio de rima ligam SIDE B a AO VIVO. Vegetação tem tronco/canteiro e circulação preservada. Cenários de seleção e foto ficam isolados durante a exploração.

## 13. Direção visual, interface e áudio

Humanos estilizados e expressivos, formas compactas, texturas de baixa resolução e ambientação noturna brasileira. A direção evoluiu para referências PS1/PS2 e filtro retrô opcional. Todos os atores mantêm piscadas esféricas e expressão, com ciclos próprios e pausa. A iluminação de personagem recebe calor e recorte vermelho, preservando leitura do vestuário.

Logo oficial Selo Kaos, lettering gestual, estrela e pequenos chifres. Vermelho Kaos #E30613, carvão e papel organizam a interface. Archivo Black identifica títulos e Space Grotesk textos e campos. Botões são arredondados, com relevo e sombra, clareamento neutro no hover e contorno somente na seleção. Crédito A game by DripLab.

Música do menu/quarto veio do pacote AUDIOS. Bairro, loja e conquista incluem material Pixabay registrado em créditos. Há efeitos de roupas, interface, trânsito, quadra, oficina, barbearia, lavanderia, mercearia, animais e emotes. Miado de Fubá vem do material recebido. Latido e palmas têm síntese original documentada. Reprodução respeita canais, volume, pausa, mute, posição e intervalos. Procedência em Art/Audio/CREDITS.md e manifestos.

## 14. Produção, entregas e validação

Windows usa player x64. Web usa WebGL 2/WebAssembly, IL2CPP e template com ponte de captura de mouse. ZIP Web tem index.html na raiz. A identificação do aplicativo permanece compatível com saves locais. Publicação de jogo e mídia requer ação manual no itch.io.

A entrega BairroVivo registrou compilações sem erros/avisos. Windows passou TEST_BUILD_01_PASSED, nome histórico da suíte consolidada. Edge/Chromium em iframe local verificou criação, Sphynx, entrada em casa, P/Retomar, recaptura em um clique e reabertura do save após recarregar. Essas evidências descrevem os percursos executados. Cobertura manual completa da semana na Web, outros navegadores e equipamentos segue no relatório de bugs.

Trailer completo de 180 s, 1920×1080/30 FPS, e pacote de 12 capturas/seis GIFs foram entregues em Captures/TrailerCompleto/edit. A lembrança final fica em extras com spoiler. O vídeo registra 5.400 quadros e áudio medido em −14 LUFS e pico verdadeiro −3,5 dBFS. A validação técnica não incluiu escuta direta. Fontes editáveis e créditos acompanham a produção local.

## 15. Pendências e referências

Concentrar novos relatos em RELATORIO_DE_BUGS.md e acompanhar retestes manuais, foco/ponteiro, erros de armazenamento, mistura de peças em movimento, desempenho por equipamento e campanha completa no navegador. A publicação manual e o feedback dos jogadores continuam como próximas verificações da entrega.

O diário de bordo registra a evolução por data em [DEVLOG.md](DEVLOG.md). Bugs e limites de verificação estão em [RELATORIO_DE_BUGS.md](RELATORIO_DE_BUGS.md), [TESTES.md](TESTES.md) e [validação BairroVivo](Art/Releases/BairroVivo/VALIDACAO.md). Os roteiros de revisões estão em Art/Reviews e a [lista de próximos passos](PROXIMOS_PASSOS.md) preserva os pedidos e seus estados por etapa.

Esta atualização é documental, baseada nas fontes, regras persistentes e relatórios existentes. Nenhum teste de gameplay foi repetido para editar este GDD. A versão de 30/10 permanece como registro da concepção e dos primeiros roteiros.
