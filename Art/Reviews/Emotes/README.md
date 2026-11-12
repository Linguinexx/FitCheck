# Emotes do jogador

Teclas de exploração: 1 joinha, 2 bater palmas, 3 dab, 4 silêncio e 0 para encerrar. A vírgula aciona o gesto secreto e não aparece na ajuda.

As poses usam os braços e mangas do corpo existente, com alvos de punho e solução de dois ossos. Joinha, mão aberta, indicador e gesto secreto têm malhas contínuas próprias, preparadas durante o carregamento. A orientação do punho é independente da dobra do cotovelo: a palma do gesto secreto aponta para a câmera, as palmas se encaram ao bater e o indicador fica diante da boca. Aplauso alterna aproximação/contato/afastamento a 2,5 ciclos por segundo; joinha tem pequeno balanço e silêncio tem movimento sutil do indicador.

A apresentação em primeira pessoa mantém as mãos na visão normal e acompanha a inclinação da câmera; o espelho restaura a pose do corpo inteiro antes de renderizar. Entrada/saída com mistura suave, cancelamento ao caminhar ou interagir e relógio congelado na pausa. Malhas alternativas compartilham os materiais de pele e ficam desativadas fora do gesto correspondente; as mãos normais são restauradas ao encerrar.

Em primeira pessoa, com câmera voltada para frente, somente os braços recebem a atualização extra. O espelho solicita a pose completa antes de renderizar. Sem novas luzes, texturas ou atualização dos emotes de todos os NPCs.

Revisão isolada: `EmoteReview.cs` nesta pasta (copiar temporariamente para Assets/FitCheck/Editor de um projeto isolado). Verifica cinco poses, mãos no enquadramento normal, inclinações de -35/0/40 graus, renderização real do espelho, restauração da cabeça oculta, pausa, término automático, troca suave, cancelamento ao caminhar e restauração dos transforms dos braços. Capturas atualizadas em `Captures/Emotes/Gestures`, incluindo palmas afastadas e em contato. Nenhum save do usuário é usado.

Alterações nas fontes da Unity. A build web Teste 09 anterior não contém os emotes; recompilar na próxima entrega.
