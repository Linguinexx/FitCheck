# Emotes do jogador

Teclas de exploração: 1 joinha, 2 bater palmas, 3 aceno, 4 “67”, 5 Absolute Cinema e 0 para encerrar. A vírgula aciona o gesto secreto e não aparece na ajuda. O ID interno 5 continua reservado ao segredo; a tecla 5 chama o ID 6 (Cinema). O 67 substitui o silêncio por solicitação do usuário.

As poses usam braços e mangas existentes, com alvos de punho e solução de dois ossos. A nova referência de mão cartoon enviada pelo usuário em 12/11/2026 orienta a base `EmoteHandDistance`: palma arredondada, dedos separados de comprimentos diferentes, pontas redondas, base carnuda do polegar e polegar curvado com três segmentos unidos por membrana contínua. Uma anatomia é usada em todos os emotes, variando as articulações. Joinha com punho lateral e dedos recolhidos; segredo com médio estendido e polegar curvado. Mão normal fora dos emotes preservada. Malhas prontas no carregamento, sem objetos de dedos encaixados ou geração durante o gesto.

Aplauso alterna aproximação/contato/afastamento a 2,5 ciclos por segundo, com pequena inclinação entre as mãos para melhorar o contato. Cinco contatos por execução, cada um com evento de som; pausa/cancelamento interrompem a emissão. Som original sintetizado `Assets/Resources/Audio/Game/emote_clap.wav`, 0,18 s/mono/24 kHz, gerado por `tools/make_emote_clap.py`, sem gravações ou licenças externas. Usa o canal normal de efeitos, volume e mute do jogo. Dab substituído por aceno com mão aberta e balanço do punho.

A apresentação em primeira pessoa mantém as mãos na visão normal e acompanha a inclinação da câmera; o espelho restaura a pose do corpo inteiro antes de renderizar. Entrada/saída com mistura suave, cancelamento ao caminhar ou interagir e relógio congelado na pausa. Malhas alternativas compartilham os materiais de pele e ficam desativadas fora do gesto correspondente; as mãos normais são restauradas ao encerrar.

Perspectiva: no aceno e Cinema as palmas apontam para fora; em primeira pessoa aparecem os dorsos. O 67 usa ambas as palmas para cima e alterna suas alturas a aproximadamente 1,43 ciclos/s, com cotovelos dobrados. A referência fornecida orienta a forma da mão, sem importação da imagem como textura. Movimento e som das palmas aprovadas mantidos. Cinema usa duas mãos abertas, dedos afastados, entrada suave e pequena acomodação da pose.

Em primeira pessoa, com câmera voltada para frente, somente os braços recebem a atualização extra. O espelho solicita a pose completa antes de renderizar. Sem novas luzes, texturas ou atualização dos emotes de todos os NPCs.

Revisão isolada: `EmoteReview.cs` nesta pasta (copiar temporariamente para Assets/FitCheck/Editor de um projeto isolado). Verifica seis poses, mãos no enquadramento normal, palmas do aceno/Cinema para fora, palmas para cima e inversão das alturas no 67, direção do polegar, inclinações de -35/0/40 graus, espelho real, restauração da cabeça, pausa, término automático, troca suave, cancelamento por caminhada, restauração dos transforms, cinco eventos de palma e áudio carregável. Capturas atuais em `Captures/Emotes/RoundedHands`. Resultado `EMOTES_REVIEW_PASSED`. Nenhum save do usuário é usado.

Alterações nas fontes da Unity. A build web Teste 09 anterior não contém os emotes; recompilar na próxima entrega.
