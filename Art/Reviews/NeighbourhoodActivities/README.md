# Atividades do bairro — 14/11/2026

Quatro atividades opcionais ligadas à exploração. Não aumentam os 21 atendimentos, não concedem reputação nem roupas e não são necessárias para terminar o jogo.

## Como jogar

- **Emotes:** procure a indicação em E nos moradores disponíveis perto da praça, da DripLab ou da quadra. Observe e repita usando 2 (palmas), 3 (aceno), 4 (67) ou 5 (cinema), conforme a tela. Há um desafio concluído por dia. Pode tentar novamente sem penalidade; vencer guarda um adesivo. O morador retoma sua atividade depois.
- **Quadra:** na quadra ao norte, pegue a bola em E. Mire a cesta e pressione E quando o marcador estiver no centro da barra. São três arremessos por rodada. A bola percorre a trajetória e retorna ao local; a câmera fica com o jogador. O melhor resultado é salvo por dia. Dois ou três acertos dão o adesivo Só cesta. Rodadas podem ser repetidas.
- **Adesivos e mural:** sete adesivos estão distribuídos entre Lajes, comércio, galeria, quadra, casa de shows e praça. Aproxime-se e use E para guardar. O quadro ENCONTRO fica na passagem perto da galeria. E abre a montagem, a câmera enquadra o quadro e as escolhas aparecem nele. Salvar confirma; Cancelar ou a tecla de voltar restaura a composição anterior. Desafios e conversas também podem conceder adesivos, sem duplicá-los.
- **Conversas:** pare para conversar com a pessoa do celular perto do café, o morador que circula pelas Lajes e a pessoa junto à galeria. Há sete capítulos para cada uma das três histórias. Você pode apoiar ou acompanhar; a conversa seguinte reconhece o apoio do dia anterior. São moradores existentes, separados dos clientes da loja.

As indicações aparecem perto dos objetos, seguindo as affordances existentes. A barra de arremesso aparece somente enquanto a bola está na mão. Pausa congela bola, marcador e desafio. Sair do bairro encerra a atividade em curso, mantendo os resultados já salvos.

## Persistência e limites

`DayProgress.neighbourhood` armazena os resultados por save dentro do arquivo de campanha existente: desafios por dia, sete adesivos e composição do mural, capítulos/respostas e recordes diários da quadra. Saves antigos recebem valores vazios; IDs de roupa, inventários históricos, notas, reputação e progressão não mudam.

Reutiliza os sete moradores, materiais, fontes e sons existentes. Bola única com trajetória calculada, sem nova simulação de física; desenhos e suportes são malhas leves. Nenhuma textura, luz em tempo real ou ator permanente foi acrescentado. Os caminhos principais continuam livres. Não foi feito benchmark comparativo de FPS; a validação confirma os limites de conteúdo e funcionamento.

## Conferência

Compilação de revisão concluída sem erros ou avisos. `NEIGHBOURHOOD_ACTIVITIES_PASSED`: sete colecionáveis acessíveis, prévia/Cancelar/Salvar no mural, gesto errado/correto, três trajetórias de cesta e pausa, continuação da conversa no dia seguinte, defaults de saves antigos, saves separados, recarga/reentrada, rotas existentes e contagem de moradores/luzes preservada.

Prévia real em `Captures/NeighbourhoodActivities/`: `mural.png`, `emote-challenge.png`, `basketball-meter.png`, `basketball-shot.png`, `story-day-1.png`, `story-day-2.png` e `atividades-do-bairro.jpg`.

Revisor: `FitCheck.Editor.NeighbourhoodReview.Build`; executar a build temporária com `-city-proof -neighbourhood-activities -proof-output <pasta isolada>`. O revisor usa dados próprios. Build temporária e caches da cópia de revisão são removidos depois dos testes; a Unity principal aberta e as builds entregues não são fechadas/substituídas. Incluir as atividades na próxima entrega Windows/Web solicitada.
