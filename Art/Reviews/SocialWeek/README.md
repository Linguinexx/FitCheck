# Vida no bairro e lembrança da semana — 12/11/2026

Implementado na Unity. Builds Windows 09/Web 10 preservadas; gerar novas entregas quando solicitado, mantendo limite de 200 MB.

## Conteúdo

- Moradores visíveis a até 7 m reagem a emotes, viram para o jogador, respondem em balão e acenam; quem segura um objeto mantém o objeto na mão e responde com a cabeça. Apenas no bairro, sem atravessar paredes; intervalo de 8 s por morador, retorno ao percurso e pausa respeitada.
- Dois comentários de outros personagens em cada post do FitFeed, com retratos já embarcados. Comentários locais do jogo, gerados conforme o atendimento; sem serviços externos, contas ou envio de mensagens. Notas baixas recebem respostas construtivas. Rolagem e avaliação preservadas.
- Sete estados de cartazes na praça/mercearia, bandeirolas a partir do dia 3 e preparativos no pátio musical nos últimos dias. Apenas um estado ativo; zero obstáculos, atores ou luzes novos. Atlas compartilhado WeekSigns 1024×512, 28 KB na fonte.
- Companheiros abrem os olhos brevemente, piscam, movem orelhas e se espreguiçam discretamente; carinho os desperta. Gatos brancos e siameses com olhos azuis; orelhas com volume e interior. Sphynx sem textura de pelo, orelhas maiores e pequenas dobras na testa. Quatro tipos de gato e três de cachorro, seis cores cada. Fubá continua separado dos companheiros e com o progresso histórico preservado.
- Novos pedidos especiais apenas em atendimentos ao vivo: Ravi no dia 4 mantém a camiseta de chegada; Téo no dia 5 exige cima/baixo/calçado After Hours; Joana no dia 7 pede o mesmo calçado usado por Ravi no atendimento anterior. Nada troca automaticamente; a seleção e a confirmação do jogador continuam obrigatórias. Regras antigas de nota e replays não foram alterados.
- Ao fechar a última noite, foto coletiva com os 13 clientes distintos e o jogador; cada cliente usa o último look escolhido para ele. Renderização única 960×540, arquivo week-group.png dentro do save, sem NPCs permanentes adicionais. Tela para ver a lembrança e porta-retrato horizontal na mesa do quarto. Saves já concluídos podem gerar a foto interagindo com a moldura. Cada espaço tem sua própria foto; apagar/recriar um save não reaproveita a imagem antiga.

## Teste rápido

1. No bairro, próximo a um morador e olhando para ele: 1/2/3/4/5 ou gesto secreto. Observe reação e retorno ao caminho. Pausar interrompe a animação.
2. No quarto, abra o FitFeed; role a publicação para ver os comentários e os retratos. Ver avaliação continua abrindo o post correto.
3. Avance os dias e confira o palco de rima e o cartaz da mercearia. O caminho casa–loja permanece igual.
4. Criação/edição → Companheiro → Gato → Sphynx, ou gato Branco. Espere alguns segundos/acaricie para ver os olhos abrirem.
5. Nos três pedidos especiais, tente confirmar um look fora da proposta; o provador explica o que falta. Troque as peças manualmente.
6. Feche o sétimo dia para a foto. Em save já zerado, olhe para o porta-retrato e pressione E. Fechar a foto retorna ao lugar/tela anterior.

## Verificação

Revisor isolado SocialWeekReview: SOCIAL_WEEK_PASSED. Testados 42 conjuntos de animais, olhos azuis, pausa, orçamento de malhas; validação dos 21 encontros/notas/inventários históricos; requisitos novos e bloqueio de envio inválido/replay antigo; comentários; sete dias sem mudanças nos obstáculos; reação/pause/fim; foto, retorno, proporção da moldura e isolamento entre saves. Capturas em Captures/SocialWeek.

A Unity isolada também emite o aviso já existente do índice de busca do Editor (SearchDatabase). Não houve erro de compilação nem falha nos testes do jogo. Nenhum save real foi usado.
