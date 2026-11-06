# Validação do teaser atual

05/11/2026. Captura nativa final com saída 0 (tools/teaser-current-final-capture.log): 15 clipes, interfaces reais, fluxo completo do atendimento e resultado 10/10. Janela 1440×900 para manter as coordenadas da UI; imagens de mundo 1280×720. Saves sintéticos separados dos saves do jogador.

Render final Blender 5.0.1 com saída 0: tools/teaser-current-final-render.log. Verificação do MP4 com saída 0: tools/teaser-current-final-validation.log.

- Arquivo: Captures/TeaserAtual/FitCheck-Teaser-Atual.mp4.
- 48 segundos, 1.440 quadros a 30 fps, 1280×720.
- Vídeo H.264 e áudio AAC; 20.812.697 bytes.
- Áudio decodificado: pico 0,335, RMS 0,042; presente e sem saturação detectada por pico.
- Nove quadros decodificados conferidos: 1,8; 5,4; 13,2; 22,8; 28,8; 32,4; 40,2; 43,2; 46,2 s. Criação, diálogo, montagem, resultado e marca inspecionados visualmente.
- Proporção da UI preservada; margem do quadro de resultado limpa de notificação transitória parcialmente entrando.
- SHA256: `464583b47e3f7967502176297d11a81fc6fcf512096b6bd23345654068572226`.

Build Teste 01 previamente entregue permaneceu intacta: ZIP continua com SHA256 `ef42d5adb2ff76f1fffc349576b1e9b1eda195bbff99aa3705c3684804dc665a`. Vídeo anterior preservado em Captures/Teaser. Editável/frames do novo vídeo mantidos localmente. Trilha usada na edição não foi integrada ao gameplay.
