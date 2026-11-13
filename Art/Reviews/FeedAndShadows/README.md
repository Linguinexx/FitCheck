# FitFeed e discos de sombra — 12/11/2026

Removidos os discos opacos cinza dos moradores, clientes, frequentador e jogador. Eles eram malhas planas com material de cor sólida, sem relação com a iluminação. Não foram adicionadas passagens de sombra em tempo real. Caminhada, poses, visibilidade dos clientes e atendimento independem desses discos. Revisão de cenografia/rig 39 reconstrói caches de cenas antigas no Editor; sombras de contato próprias dos animais e decoração estática não foram alteradas.

FitFeed reutiliza o arquivo original Art/Marketing/Itch/Background/fitcheck-background-estrelas-v2.png: preto e estrelas cinza em fluxo diagonal. Cópia em Resources/UI/FitFeedStars.png, importada a no máximo 1024 pixels, comprimida, sem mipmaps/leitura CPU. Uma textura carregada e reutilizada, estática, com barra lateral protegida por carvão quase opaco e cartões originais opacos. Layout, botões, tamanhos, tipografia, rolagem e dados mantidos. Guia UI/UX Pro Max consultado para contraste e legibilidade; a identidade do projeto permanece.

Resultado: FEED_SHADOW_PASSED. Teste isolado cobre ausência das malhas nos ambientes e jogador, visibilidade/quantidade de clientes, caminhada, espelho da loja, FitFeed vazio/com 18 posts/rolado, recurso de fundo em cache e dados de campanha inalterados. Inclui CampaignChecks.Validate para os 21 encontros. Revisor depende de HomeDecorReview.cs e CompanionsReview.cs; copiar temporariamente os três para Editor da cópia isolada e executar FitCheck.Editor.FeedShadowReview.Capture -city-proof -feed-output <pasta>. Capturas: Captures/FeedAndShadows. Sem acesso aos saves reais.

A Unity registrou a exceção de indexação do SearchDatabase já existente no projeto, sem erro de compilação/execução do revisor. Fontes prontas na Unity; builds Windows 09/Web 10 anteriores não incluem esta revisão nem o novo espelho da loja.

Lista para fotos de perfil em Art/Characters/NewClients/NOVOS_PERSONAGENS.md.
