# Quarto, quadra, discos e encaixe do pulso — 17/11/2026

A cama e a cesta foram refeitas no Blender, com fonte editável em `Art/Environment/RoomCourtRefresh/RoomCourt.blend` e programa de geração ao lado. A cama ganhou pés recuados, base, colchão arredondado, travesseiros, coberta com costuras e cabeceira. O contorno é exclusivo de seus três materiais; as cores de roupa de cama continuam personalizáveis. A cesta mantém o centro do aro e a lógica do arremesso, com suporte acolchoado, tabela emoldurada, retângulo vazado, aro e rede em losangos.

O placar do basquete mostra as três tentativas, zona de acerto, tecla E, bola no ar e resultado. O mesmo comando não aparece duplicado numa affordance separada. Uma nova rodada limpa apenas o placar temporário, preservando recordes e adesivos. Física, pausa, três arremessos e progressão não mudaram.

O rádio agora é um seletor de discos com capas originais, vinil e agulha. Navegar não muda a faixa; **Colocar para tocar** confirma a escolha e **Retirar disco** interrompe só a música. Três instrumentais novos e exclusivos: Laje 92 (92 BPM), Passo Torto (98 BPM) e Depois das Dez (84 BPM). São partituras/síntese locais originais, sem amostras das músicas existentes. Fontes FLAC, código, capas e conceito em `Art/Audio/ShopRecords`; cópias leves em Resources. As gerações preparadas no fal foram recusadas por saldo e não foram usadas. Não houve escuta humana nesta validação; volume e pico foram medidos por ffmpeg. As faixas podem ser substituídas depois mantendo os mesmos IDs.

Preferência e retirada são salvas por espaço em `neighbourhood.musicChoice/musicStopped`. Saves antigos continuam com índice 0–2 e reprodução ligada. Pausa, mute, volumes, ambiente, fechamento do painel e transição entre ambientes foram verificados. Trocas rápidas atualizam o destino do crossfade; retirar o disco limpa as duas fontes de música.

A junção do pulso adapta somente os anéis terminais existentes do antebraço (até 14,5 cm), afunilando-os para dentro da raiz da mão e orientando as normais da transição. Não acrescenta malha, material ou renderer. Não remodela dedos, palma, gesto ou o restante do braço. A adaptação acompanha a mesma apresentação em primeira pessoa e o corpo anatômico no espelho; cancelar o emote restaura a pose comum.

O contorno aprovado passa a entrar no jogo em personagens, cabelos e pets. Roupas ficam fora dos passes novos e dos filetes antigos, preservando suas texturas. A cama recebe um material próprio; não foi aplicado um pós-processamento global à cidade. A prévia de comparação continua disponível no editor. O custo não foi medido novamente; não há afirmação de ganho de FPS. A revisão atual é nativa; Web e builds públicas precisam ser conferidas na próxima entrega.

Olhos dos pets preservam o visual aprovado. O olho aberto e o desenho fechado nunca ficam ativos juntos, inclusive logo após criar o modelo. Testados 24 gatos e 36 cães nas combinações de raça/pelagem, movimento reduzido e pausa.

O retângulo de seleção dos lambe-lambes agora compõe a rotação na matriz lógica da UI e usa a profundidade do papel. Coletáveis saíram dos marcadores baixos e foram para pequenos suportes de aviso na altura dos olhos. Os sete IDs, progresso, temas e posições gerais foram mantidos, com acesso físico verificado. **Pendente:** receber as artes do usuário e convertê-las em lambe-lambes, com recorte, papel e proporção ligados ao contexto de cada tema. As imagens atuais são provisórias; não inventar novos prêmios para substituir as futuras artes.

## Conferência

- Entrada de revisão: `-city-proof -comfort-refresh-review`, armazenamento isolado dentro da pasta de captura.
- Executável temporário: `tools/.comfort-refresh-review`; não é uma nova entrega Windows.
- Imagens reais: `Captures/ComfortRefresh`: cama, olhos abertos/fechados, seis emotes, manga curta e espelho; três discos/retirada; cesta, HUD, sete pontos de coleta e seleção inclinada do mural.
- `COMFORT_REFRESH_PASSED`, `NEIGHBOURHOOD_ACTIVITIES_PASSED`, `SHOP_MOMENTS_PASSED` e `FITCHECK_SHOP_SERVICE_PASSED` são as saídas esperadas. Conferir o manifesto de validação junto às imagens.
- Manter as builds públicas MuralLambes atual/anterior, saves, originais do usuário e referências. Esta rodada atualiza o projeto da Unity.
