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
