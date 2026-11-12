# Pendências para a próxima sessão — 05/11/2026

## Feedback após concluir o jogo — 07/11/2026

Registro solicitado para uma próxima etapa.

- Corrigido em 10/11/2026: clientes recorrentes podem ser cumprimentados no bairro depois de cada novo atendimento. Cada encontro só aceita um cumprimento e o traje Figurinha do Bairro continua contando pessoas distintas.
- Campanha ampliada em 10/11/2026 para sete dias e 21 atendimentos. Saves encerrados no dia 3 continuam pelo mesmo espaço, a partir da cama em casa.
- Depois da expansão para sete dias, avaliar aumentar o número de clientes por dia. Quantidade ainda a definir.
- Os dias 4 a 7 trazem Dandara, Ravi, Joana e Téo, pedidos inéditos de clientes recorrentes e novas peças nos marcos de 750, 1050 e 1400 de Reputação. Os quatro retratos fornecidos pelo usuário estão associados aos nomes no FitFeed.
- Paleta do criador ampliada para 12 cores. Chocolate, cobre, platinado, rosa choque, azul elétrico e turquesa entraram depois das seis cores originais, preservando os IDs dos saves. Joana usa rosa choque e Téo usa cobre.
- Primeira etapa de cabelos implementada em 07/11/2026: Black Power atribuído à Nina e disponível no criador do jogador; Crespo curto incluído como segunda opção. IDs anteriores preservados. Encaixe específico com bonés, gorros, óculos, fones e capuz espectral. Continuação visual depende do feedback.
- Bases de aparência masculina/feminina disponíveis no criador em 07/11/2026, com seleção persistente por save, todos os cabelos e trajes acessíveis e cabeça esférica. Primeira versão feminina com sobrancelhas arqueadas, boca mais delicada, cintura discreta e postura ajustada. Lia, Julia e Nina usam essa base; moradores alternam entre as duas. Revisar com o usuário após as prévias.

Anotações solicitadas pelo usuário ao encerrar a sessão. Registrar agora; retomar no próximo pedido, sem implementar nesta sessão.

- Concluído em 05/11/2026: clientes chegam com roupas cotidianas variadas e tênis padrão, mantendo seus próprios rostos/cabelos/expressões. Peças DripLab aparecem na montagem do fit e permanecem na saída. Frequentador fixo continua separado.
- FitFeed implementado como uma timeline de fotos em ordem recente. Cada publicação abre a avaliação com nota, pontos por critério e Reputação. A mesa do quarto abre a timeline e permite refazer um visual nas condições originais.
- Implementado: clientes atendidos aparecem no bairro com o look salvo no FitFeed do save ativo. Ficam no ponto correspondente à foto durante o restante do dia, inclusive com a loja aberta, e saem no dia seguinte. As duas fotos de terraço usam o pátio musical e o palco como pontos equivalentes no mapa explorável.
- Refinamento do atendimento: seis segundos de espera na loja após a despedida anterior. Clientes do FitFeed têm poses de descanso variadas e se viram para o jogador próximo. O provador conserva as peças de chegada por categoria até a seleção manual. Ordem dos tênis da coleção: Garage Session, Sidewalk e Daily Steps.
- Concluído em 05/11/2026: espaço entre mercearia e oficina preenchido com fachadas de barbearia, lavanderia e consertos, vitrines, toldos, letreiros em textura e vegetação. Passagens e trajeto casa–loja preservados.
- Concluído em 05/11/2026: trecho entre SIDE B e AO VIVO preenchido com fachada de estúdio SOM/LADO B, caixote de discos e passagem para pátio de rima com palco, mural, banco, árvore e bicicleta. Passagem e trajeto casa–loja verificados; materiais/texturas existentes reutilizados.
- Concluído em 05/11/2026: tênis básico fornecido pelo usuário, otimizado para 2.400 triângulos por pé e aplicado aos NPCs civis. Mantém-se separado dos tênis do catálogo da loja. Fonte: Art/Garments/BaseSneaker/BaseSneaker.blend.
- Melhorar os trajes comuns do jogador: ele também não deve usar DripLab nessa proposta. Conciliar com a personalização existente ao retomar.
- Decoração do quarto revisada com parede azul-petróleo, roupa de cama em camadas, tapete estampado, luz quente perto da mesa e da cama, luz fria da janela e canto de vinil. Móveis interativos e Fubá continuam acessíveis.
- O frequentador fixo da loja oferece ajuda de gameplay ao olhar para ele de perto e apertar E. O diálogo cobre expediente, clientes, provador, FitFeed e avanço do dia.
- Áudio: lista detalhada de coleta/produção em Art/Audio/PLANO_AUDIO_FITCHECK.md. Outro integrante vai reunir os arquivos em audio recebidos, na raiz do workspace. Aguardar entrega, conferir fontes/licenças e qualidade antes da integração; preservar o miado do Fubá já existente. Prioridades, durações, quantidades e reutilizações estão no documento.
- Avaliar o uso de IA de modelagem para outras roupas além dos tênis. É uma possibilidade a discutir, não uma decisão de substituir toda a coleção.
- Concluído em 05/11/2026: primeira entrega organizada de teste para Windows 64 bits, Teste 01. Executável em Builds/Windows-Test01/FitCheck.exe e ZIP FitCheck-Windows-Test01.zip, com instruções, roteiro e modelo de relato. Validação nativa de casa/bairro/loja, atendimento, roupas, loadings e saves passou. Aguardar feedback do grupo; pendências de conteúdo acima continuam abertas.

O [relatório de bugs](RELATORIO_DE_BUGS.md) centraliza as falhas históricas, os retestes pendentes e o registro dos próximos relatos do Teste 01.

Manter o estilo gráfico atual e priorizar desempenho. Não descartar fontes Blender, referências ou saves; manter apenas build atual e uma anterior recuperável.

## Modelos fornecidos — 10/11/2026

- Clog Brasa, Jeans Rosa e Óculos Prisma foram acrescentados ao catálogo, preservando as peças anteriores. Fontes editáveis e preparação em Art/Garments/SuppliedWardrobe. Originais preservados em refs roupas/modelos tenis.
- Cópias de jogo com 2.199 triângulos por clog, 5.654 na baggy rosa e 1.800 nos óculos. Texturas de 512 px. A calça usa deformação independente das pernas, joelhos, tornozelos e cintura. O pé do clog tem encaixe próprio. A conversão inicial da calça fornecida foi substituída em 11/11 pelo modelo da baggy azul.
- Catálogo liberado para avaliação de combinações. Configurações → Catálogo de teste alterna entre acesso completo e progressão por Reputação. Preservar recompensas e inventários históricos. Novos marcos normais de Jeans Rosa em 750 RP e Clog Brasa/Óculos Prisma em 1050 RP.
- Conferência na Unity aprovada com as duas bases de aparência e os nove cabelos, a calça com os cinco calçados do catálogo, quatro fases da caminhada e vínculo do pé ao clog. Retratos carregados em até 512 px. Campanha, migração e reversão do catálogo de teste verificadas. Prévias em Captures/SuppliedWardrobe.

## Próxima sessão — feedback dos modelos e novas etapas

Retomada em 11/11/2026 para artes, calça rosa e óculos. Sons, tutorial e build continuam na lista das próximas etapas.

- Calçados aprovados pelo usuário.
- Concluído em 11/11: calça rosa usa a mesma geometria, UVs, bolsos e costuras da baggy jeans azul, com textura denim rosa. Conferência confirmou igualdade da malha e movimento nas duas pernas, com os cinco calçados.
- Concluído em 11/11: óculos com rotação e escala uniforme do modelo original, retirando o alargamento de lentes e a deformação das hastes. Posição orientada pelo encaixe dos óculos de grau existentes; cobertura parcial dos olhos permitida. Conferidos com nove cabelos e duas bases de aparência. A avaliação visual do usuário continua sendo a referência para refinamentos.
- Adicionar mais sons ao jogo, conferindo os arquivos disponíveis e as pendências do plano de áudio.
- Concluído em 11/11: dez novas imagens de artes quadros incorporadas ao bairro, em cartazes e um outdoor com suporte/refletores. As 26 imagens fornecidas estão disponíveis na personalização da loja, com índices de saves anteriores preservados. Conferidos proporção, gravação/leitura das escolhas, retorno da prévia e percurso casa–loja. Fontes e hashes em Art/UserArtwork/sources.json. As quatro fotos de perfil seguem integradas.
- Criar um tutorial para novos saves, acompanhando o início e ensinando o fluxo do jogo. Definir apresentação e etapas ao implementar, mantendo a progressão separada por espaço de save.
- Preparar uma nova build de teste após os ajustes, com limite de 200 MB solicitado pelo usuário. Medir o tamanho da pasta distribuída e do ZIP final e registrar ambos na entrega.

## Refinamentos de quadros, óculos e atmosfera — 11/11/2026

- As 26 opções de quadro receberam nomes curtos. A largura dos rótulos foi conferida na fonte Space Grotesk Bold, tamanho 17, respeitando os 112 px internos de cada botão.
- Miniatura dos Óculos Prisma enquadrada pelos limites da armação, em uma imagem horizontal de 128 × 64. A versão bloqueada acompanha as mesmas dimensões. Armação ocupa 111 × 29 px na conferência de captura.
- Teste solicitado: ao equipar os Óculos Prisma, ocultar olhos, pupilas, cílios e pálpebras atrás das lentes. Remover a peça restaura os olhos e suas piscadas. Sobrancelhas e boca permanecem expressivas. As cópias de NPCs seguem essa visibilidade, incluindo a exclusão de pálpebras soltas. Verificados nove cabelos e duas bases de aparência. Aguardar avaliação visual do usuário.
- Neblina leve por distância no bairro, aumentando suavemente entre 16 e 106 unidades, em cinza discreto. Aplicada a superfícies, letreiros, artes e personagens externos pelos shaders existentes. Conferência de compilação e exclusão dos interiores aprovada. Prévias em Captures/GlassesFogReview.

## Revisão posterior — bermuda militar, óculos e sons

- Neblina de teste retirada a pedido do usuário, restaurando os shaders anteriores.
- Óculos Prisma aproximados 0,085 unidade do rosto, mantendo o teste com olhos ocultos. A armação e as lentes continuam nas proporções do modelo original.
- Nova Bermuda Comando CG_BOTTOM_05, COMMON GROUND, com 6.844 triângulos, camuflagem de sarja 512, corte largo abaixo do joelho, cós com relevo e estampa 07 assentada na malha. Fonte em Art/Garments/MilitaryShorts/MilitaryShorts.blend. Desbloqueio normal em 1050 RP. Ao detectar expansão do catálogo, o modo de teste libera todas as 32 peças uma vez e continua podendo ser desligado nas Configurações.
- Dezoito efeitos adicionais integrados do ZIP em audio recebidos, com cópias mono de 22.050 Hz. Roupas, interface, campainha, FitFeed, drops, trânsito, quadra, oficina e barbearia. Originais preservados e mapeamento em Art/Audio/ADDITIONAL_SOUNDS.json.
- Conferência visual e de caminhada em Captures/MilitaryAudioReview. Tutorial de novos saves e nova build de até 200 MB seguem pendentes.
- Limpeza concluída da cópia temporária tools/.city-build-project após as verificações, removendo aproximadamente 459 MB. Os hashes das entregas Teste 07 e do ZIP anterior Teste 06 foram preservados. Fontes, referências, arquivos recebidos e dados do jogador mantidos. Relatório em tools/cleanup-validation-20261111.json. A cópia técnica pode ser recriada a partir de Assets, Packages e ProjectSettings do projeto para novas verificações.

## Acabamento das roupas, pé e intro — 11/11/2026

- Pé do slide de nuvem remodelado com contorno arredondado e cinco dedos unidos à base, em uma malha por pé. Verificados quatro momentos da caminhada e o vínculo rígido dos dedos ao pé.
- Revisadas as texturas das 22 peças de blusas/casacos e shorts/calças do catálogo, incluindo as partes procedurais, trajes do jogador e roupas civis. Tecidos diferenciados, trama fotográfica e dobras suaves; estampas e paleta preservadas. Texturas continuam em até 512 px, sem novo passe de renderização. Fontes e mapas anteriores preservados em Art/Garments/ClothFinish.
- Fundo preto da intro desenhado nas dimensões reais da tela antes de restaurar a escala da interface. Posição, proporção e animação da logo mantidas.
- Prévias e verificações em Captures/ClothingPolish. A nova build distribuível e o tutorial permanecem pendentes.
- Removidas as cópias e o executável temporários da verificação, recuperando cerca de 656 MiB. As entregas Teste 07 e Teste 06 foram preservadas. Captura automática da intro limitada pela ausência de Repaint na janela oculta; conferir a apresentação visual no Play normal.

## Provador organizado por drop — 11/11/2026

- Seleção alterna entre **Por peça** e **Por drop**, sem trocar o fit. Por drop reúne todas as categorias; seleção equipa somente a categoria da peça. As peças originais e a opção Sem acessório continuam disponíveis.
- Três drops formalizados em DropCatalog: After Hours (11 peças, noite/camadas/couro), Common Ground (11, quadra/jeans/utilitários) e Side B (10, música/estampas autorais). Mantidos os nomes e vínculos de todas as peças, IDs, critérios e marcos para preservar avaliações e inventários históricos. Distribuição completa em Art/Garments/DROPS.md.
- Cartões em carvão/papel com seleção Vermelho Kaos, relevo, etiqueta do drop e estado de desbloqueio. Prateleiras com identidades próprias: luzes noturnas, marcações de quadra e sulcos de vinil. Miniaturas reenquadradas pelos limites de transparência, mantendo as texturas e proporções existentes.
- Etiquetas distinguem bloqueio por RP, entrega no fechamento e peça indisponível no replay. As recompensas dos oito marcos são derivadas diretamente do catálogo para não divergir das etiquetas. Modo de teste reversível e saves mantidos.
- Conferências em Captures/DropShelfReview e Tools/Fit Check/Check clothing drops. Nova build distribuível e tutorial seguem pendentes.

## Teste 08 — cores, otimização e entrega — 11/11/2026

- Por drop usa fundo destacado na prateleira inteira e nos cartões: bordô After Hours, verde Common Ground e violeta Side B. Textos claros, identidade dos drops e seleção Kaos preservados.
- Filtros, guia, disponibilidade, etiquetas e medição dos nomes reutilizados. Cache atualiza na troca de inventário, campanha, entregas, marcos, teste ou replay. Selecionar uma peça já equipada não reconstrói o avatar. Mil consultas estáveis sem alocação; sem reduzir resolução de texturas ou alterar a distribuição da interface.
- Corrigida a identificação interna de materiais civis/catalogados encontrada na validação de atendimento. Chegada civil e troca de roupa no provador verificadas.
- Build Windows Teste 08 entregue em Builds/Windows-Test08 e Builds/FitCheck-Windows-Test08.zip. Pasta 138,7 MB; ZIP 76,6 MB. Compressão LZ4HC, tela cheia/configurações preservadas, relato de bugs e roteiro atualizados. Fontes da entrega e validação em Art/Releases/Test08.
- Suítes de campanha, atendimento, loadings, saves e provador passaram. Loja em cerca de 60 FPS nos testes nativos, incluindo caminhada olhando para baixo; dados completos em Art/Releases/Test08/VALIDACAO.md.
- Limpeza de aproximadamente 652 MiB; apenas Teste 08 atual e Teste 07 arquivado como versão anterior. Cache da Unity aberta, referências, fontes e saves preservados.
- Nova build solicitada concluída. Tutorial de novos saves continua pendente.

## Web Teste 01 — 11/11/2026

- Versão de navegador em Builds/Web-Test01 e Builds/FitCheck-Web-Test01.zip, aproximadamente 50,9 MB de pasta e 50,7 MB de ZIP. Index na raiz para hospedagem HTML5; servidor local e instruções incluídos. Entrega Windows Teste 08 preservada.
- Compilação IL2CPP, primitivas preservadas no stripping, gravação compatível com IndexedDB e sincronização persistente automática. Captura do mouse por clique; recusa de permissão não derruba o player. Interface e fluxo mantidos.
- Verificados menu, criador, continuidade do save após reabrir o navegador, casa, movimento, pausa, interação com Fubá e porta para o bairro. Não foi repetida a campanha completa na web. Detalhes em Art/Releases/Web01/VALIDACAO.md.
- Hospedagem pública ainda não realizada. Os saves web são separados dos saves Windows. Tutorial de novos saves continua pendente.

## Correções da versão web — Teste 02

- Mouse mantém captura durante loadings entre ambientes e retorna com um clique em Retomar, usando a ativação direta do navegador. Câmera consulta o pointer lock real. Menus e Tab liberam o cursor; uma recusa de permissão permanece recuperável.
- Sair removido somente do menu inicial web para evitar que Application.Quit pare o player dentro do itch.io. Navegação por teclado acompanha os dois botões restantes. Windows preservado.
- Nova entrega em Builds/FitCheck-Web-Test02.zip, com relato de bugs e instruções. Testes em iframe local passaram para criação/continuidade, quarto, bairro, três ciclos de pause/Retomar, Tab e porta sem perda de captura. Detalhes em Art/Releases/Web02/VALIDACAO.md. A substituição no itch.io deve ser feita pelo usuário; não houve publicação automática.
- Comparação de fontes para os títulos da página em Art/Marketing/Itch/titulos-comparacao-v1.png: Bungee, Bowlby One SC e Passion One Black. Tema publicado e fontes do jogo preservados.

## Tela de controles — Web Teste 03

- Configurações → Controles no título e na pausa, com teclado e mouse ilustrados por código. Exploração destaca WASD, Shift, E, F, Tab e Esc; Provador explica clique, arraste e roda. As setas alternam páginas; Esc e Voltar retornam à tela anterior, preservando a pausa.
- Verificação visual em 1440×900 e 1280×720, acesso pelos dois menus, criação de personagem e recaptura por um clique em Retomar passaram em iframe local. Prévias e validação em Art/Releases/Web03. Sem novas texturas, remapeamento, mudanças de inventário ou fontes globais.
- Build em Builds/FitCheck-Web-Test03.zip. Substituição no itch.io pelo usuário. Tutorial automático de novos saves continua pendente; esta entrega é uma ajuda acessível pelas Configurações.

## Revisão web — Teste 04

- Pausa no primeiro desbloqueio do ponteiro/Esc, com deduplicação; Sair restaurado para sair da Fullscreen API sem encerrar o player. Expansão CSS do itch.io depende da página.
- Hover do Jogar reforçado; sensibilidade 0–100, padrão 35; sliders compartilhados e mouse ilustrado curvo nas duas páginas.
- Seletor janela/tela cheia só na web. Windows Teste 08 preservado; próxima compilação deve validar Art/Releases/Web04/PENDENCIAS-WINDOWS.md.
- Entrega Builds/FitCheck-Web-Test04.zip; validação local em iframe em Art/Releases/Web04/VALIDACAO.md. Substituição no itch.io pelo usuário.

## Ajuste web — Teste 05

- Botão Sair removido do título web a pedido do usuário. Navegação por teclado alterna somente Jogar e Configurações; posições, tamanhos e comportamento dos botões restantes preservados. Windows mantém Sair.
- Entrega atualizada em Builds/FitCheck-Web-Test05.zip, preservando as correções da Web04.

## Revisão web — Teste 06

- Catálogo de teste retirado da UI e desativado no início do jogo, inclusive para preferências antigas. Removida a reativação automática ao entrar roupa nova; saves e inventários históricos preservados.
- Web: P pausa/retoma sem sair da tela cheia; ajuda e configurações informam a alternativa. Esc usa captura quando o navegador permite, mas Chromium bloqueia Keyboard Lock em iframe. Não prometer que Esc preserve tela cheia no itch.io.
- Fonte compartilhada Windows também volta aos desbloqueios normais; recompilar/validar na próxima Windows. Entrega web em Builds/FitCheck-Web-Test06.zip.

## Revisão web — Teste 07

- P é a única tecla de pausa/retomada na web. Esc não abre nem fecha a pausa, inclusive quando o navegador libera o mouse ou sai de tela cheia.
- Esc mantém voltar/cancelar em painéis existentes; controles indicam P para pausa e Esc para voltar. Captura exclusiva de Esc removida da ponte JavaScript; browser conserva comportamento normal.
- Windows mantém Esc para pausar. Build web atualizada em Builds/FitCheck-Web-Test07.zip.

## Revisão web — Teste 08

- Esc retirado de todos os comandos do jogo na web: não pausa/retoma, não volta/cancela nem pula a intro. Navegação por Voltar/Cancelar; P controla pausa/retomada.
- Ajuda sem destaque ou legenda de Esc; guia da loja sem esse atalho. Ponte JavaScript sem listener de teclado: observa somente liberação do ponteiro/tela cheia para recuperar a câmera.
- Comportamento nativo do navegador preservado; Windows continua com Esc. Build em Builds/FitCheck-Web-Test08.zip.

## Provador — revisão visual do interior

- Aplicados piso texturizado, tapete com detalhe Kaos, ripado, moldura do GIF, banco estofado com pés, ganchos, prateleira e luminária decorativa. Trama e bainhas da cortina melhoradas, mantendo suas cores e as do GIF personalizáveis.
- Texturas existentes reaproveitadas; sem novas luzes em tempo real. Cena isolada validada: +2.330 triângulos e um grupo estático. Prévias em Captures/FittingRoom/Final; registro em Art/Reviews/FittingRoom/README.md.
- Fontes atualizadas na Unity; builds distribuídas anteriores ainda não incluem esta revisão.

## Affordances e iluminação inspirada na capa

- Resposta de tecla pressionada, cor e pulso curto nas affordances E/F. Movimento reduzido mantém apenas cor; loading cobre e pausa congela feedback. Ações/limites de interação preservados.
- Personagens com luz quente, sombra mais definida e recorte vermelho inspirado na capa. Olhos com efeito suave e sem borda vermelha; shaders de pele/cabelo/roupas compartilham o efeito, sem novas luzes reais ou texturas.
- Revisão em Art/Reviews/InteractionLighting/README.md e prévias em Captures/CoverLighting/Final. Build web atualizada em Builds/FitCheck-Web-Test09.zip também inclui o provador revisado. Windows precisa recompilar.

## Emotes de braços

- Emotes revisados: joinha (1), bater palmas (2), dab (3) e silêncio (4); 0 encerra. Gesto secreto na vírgula, sem divulgação na tela de controles.
- Visíveis no espelho, com entrada/saída suave, pausa respeitada e cancelamento por caminhada/interação. Preservadas as animações normais e as roupas.
- Corrigida a abertura lateral dos braços: cotovelos dobram para a frente, mãos aparecem na visão normal e acompanham a inclinação da câmera. Espelho usa a pose do corpo inteiro, sem o deslocamento visual da primeira pessoa.
- Punhos com orientação própria e mãos distintas para polegar, palmas abertas e indicador; palma do gesto secreto voltada à câmera. Aplauso tem contato/afastamento em ciclos, silêncio alinhado à boca, dab com um braço estendido e outro dobrado.
- Revisão isolada em Art/Reviews/Emotes/README.md; capturas atuais em Captures/Emotes/Gestures. Fontes prontas na Unity; incluir na próxima build web/Windows.
