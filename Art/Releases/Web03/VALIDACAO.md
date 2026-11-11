# Web Teste 03 — tela de controles

## Implementação

Configurações → Controles no título e na pausa. Teclado compacto e mouse desenhados com os componentes visuais existentes, fontes e paleta do jogo. Duas páginas: Exploração e Provador. Destaques correspondem aos comandos existentes, sem remapeamento ou indicação de segundo jogador. Nenhum mapa de textura ou dependência nova adicionado.

O acesso divide a antiga linha de Áudio entre Áudio e Controles, mantendo posição e tamanho das outras opções. Abas e setas esquerda/direita trocam a página. Voltar e Esc fecham somente a ajuda. Abrir Configurações novamente reinicia a ajuda em Exploração; sair dela não altera o fit nem avança o jogo.

## Verificações realizadas

- Unity 6000.6.7f1: tools/web03-controls-build.log, FITCHECK_WEB_BUILD Succeeded, zero erros e zero avisos no relatório final.
- Edge/Chromium, iframe com origem diferente e permissão de pointer lock, viewport 1440×900 e 1280×720. Capturas reais em Captures/Web03; prévias escolhidas junto deste arquivo.
- Acesso por Configurações no título; páginas Exploração e Provador e troca por setas verificadas visualmente. Teclado, mouse, legendas e Voltar cabem nas duas resoluções.
- Esc na ajuda retorna às Configurações. Esc nas Configurações abertas pelo título retorna ao título.
- Novo personagem Controls QA chega ao quarto com ponteiro capturado.
- Pausa → Configurações → Controles → Provador → Esc → Configurações → Esc: menu de pausa preservado. Ponteiro livre na ajuda e na pausa; um clique em Retomar captura o ponteiro. Não houve retorno antecipado ao gameplay.
- Sem erros de execução no percurso testado. Avisos limitados ao renderizador SwiftShader/ReadPixels e ao atributo allowfullscreen redundante no iframe de QA.
- Windows Teste 08 preservado. Não houve alteração de fontes globais, progressão, inventários, roupas ou cenas.

## Entrega

Builds/Web-Test03 e Builds/FitCheck-Web-Test03.zip. Index na raiz e relatório de bugs/roteiro incluídos. Tamanhos reais e hashes em delivery.json, abaixo do limite de 200 MB para pasta e ZIP. Teste 02 é a versão anterior recuperável. Enviar o novo ZIP no projeto HTML existente do itch.io; não houve publicação automática ou nova partida completa de sete dias nesta revisão.

Integridade final conferida com WEB03_PACKAGE_VERIFIED. Limpeza de 901.119.465 bytes lógicos em temporários e Web Teste 01 antigo, mantendo Web Teste 03 atual, Teste 02 anterior, Windows Teste 08, biblioteca da Unity, referências e saves. Relatório em tools/web03-cleanup.json.
