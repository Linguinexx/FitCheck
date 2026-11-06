# Teaser do jogo atual — 05/11/2026

Nova edição de 48 segundos, 1280×720, 30 fps, MP4 H.264/AAC, seguindo o formato do vídeo anterior. Imagens novas do conteúdo da Teste 01; sem bloco de funcionalidades futuras. Logo oficial Selo Kaos e fontes Archivo Black/Space Grotesk do projeto.

## História mostrada

| Tempo | Cena |
|---|---|
| 0–3,6 s | Fachada animada e marca |
| 3,6–9,6 s | Criação real do personagem e prévia do avatar |
| 9,6–12 s | Casa em primeira pessoa |
| 12–18 s | Bairro atual, comércio e ligação musical |
| 18–21 s | Abrir loja e chegada do cliente com roupa cotidiana |
| 21–24,6 s | Pedido e resposta no diálogo real |
| 24,6–29,4 s | Acompanhamento até o provador e abertura da cortina |
| 29,4–35,4 s | Interface real de montagem e três combinações |
| 35,4–39 s | Cliente com o visual final saindo |
| 39–42 s | Foto/avaliação real, nota 10 obtida pelos critérios do pedido |
| 42–44,4 s | Fubá e interação de carinho |
| 44,4–48 s | Marca e identificação de build em desenvolvimento |

Os cortes encurtam deslocamentos; não alteram a ordem do atendimento. Menus mantêm sua proporção original com margens laterais na edição. Capturas usam avanço de simulação a 30 Hz; o vídeo não é uma medição de FPS.

## Produção e arquivos

- Captura: Assets/FitCheck/CurrentTeaserCapture.cs, apenas pelo modo de revisão.
- Compilar temporariamente: FitCheck.Editor.CitySliceBuild.BuildTeaserCapture, saída Windows-TeaserCurrent. O comando Build normal continua apontando para Windows-Test01.
- Executar com -batchmode -force-d3d11 -screen-width 1440 -screen-height 900 -screen-fullscreen 0 -city-proof -current-teaser -proof-output <Captures/TeaserAtual/source> -logFile <tools/teaser-current-final-capture.log>.
- A resolução de janela 1440×900 é necessária para as coordenadas da interface. Cenas sem UI são capturadas em 1280×720, e a UI em 1440×900. Texturas de captura são reutilizadas para limitar alocações.
- Renderizar com Blender 5.0 em background/factory-startup usando editar_video.py. --preview gera imagens de revisão. verificar_video.py decodifica o MP4, verifica quadros/dimensões/áudio e produz amostras.
- Vídeo final e projeto editável gerado: Captures/TeaserAtual/FitCheck-Teaser-Atual.mp4 e .blend.
- Fontes visuais: Captures/TeaserAtual/source, com 15 clipes e manifesto. Sessões salvas de captura são sintéticas e isoladas dos saves do jogador.
- Manifesto editorial: teaser-manifest.json; validação final: teaser-validation.json, na pasta do vídeo.

## Áudio

Reutiliza a trilha instrumental original de 100 BPM criada para a edição anterior, copiada para FitCheck-teaser-beat.wav. É trilha de edição; não afirma que a trilha definitiva já foi integrada ao jogo. Cliques, porta, cortina, foto e recompensa são exportados dos efeitos atuais do jogo. O miado é o arquivo real já usado por Fubá, de Kerzoven/CC0, documentado em Art/Audio/CREDITS.md. Nenhum novo som foi integrado ao gameplay nesta tarefa.

Preservar vídeo anterior, editáveis, fontes e referências. O executável temporário pode ser removido após a validação; a build Teste 01 entregue ao usuário deve permanecer intacta. Não versionar vídeos, frames ou sessões de captura como fontes do jogo.
