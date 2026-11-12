# Web Teste 06 — progressão e tela cheia

Catálogo de teste desativado por padrão e no início do jogo, inclusive com preferência antiga ligada. Removida a reativação automática ao aumentar o catálogo. Botão e mensagem de teste retirados. Saves, reputação, recompensas e inventários históricos preservados.

Tela cheia solicita captura de Esc onde permitida. Chromium restringe Keyboard Lock ao documento principal; iframe do itch.io não pode forçar essa permissão. P pausa/retoma na web sem sair da tela cheia, sem interferir com digitação do nome no criador. Esc continua abrindo pausa se o navegador encerrar tela cheia/liberar mouse.

Referências técnicas: https://developer.mozilla.org/en-US/docs/Web/API/Keyboard/lock e https://developer.mozilla.org/en-US/docs/Web/API/Element/requestFullscreen .

Validação desta revisão será registrada após compilar.

## Resultado

- Compilação final Unity 6000.6.7f1 concluída: zero erros/avisos. Arquivos finais comparados às fontes.
- FITCHECK_CAMPAIGN_PASSED: 21 pedidos, sete dias, 2100 RP, replays, migração e backup. FITCHECK_DROPS_PASSED: 32 peças, entregas e histórico preservados.
- Ponte JavaScript passou nos testes: Esc consumido, deduplicação, recaptura, permissão recusada, captura em documento principal, bloqueio em iframe e fallback de modo não suportado.
- Edge 157 em iframe local entre origens distintas: save QA Catalogo criado na Web05 com catálogo de teste ligado permaneceu disponível na Web06; Configurações sem o botão de teste. Ajuda com P conferida.
- Continuação do save capturou câmera; P abriu pausa, retorno em um clique recapturou; em tela cheia solicitada pelas Configurações, P abriu/retomou mantendo document.fullscreenElement ativo. Capturas em Captures/Web06.
- Esc automatizado também abriu pausa. O envio de tecla por automação não reproduz necessariamente a saída reservada do navegador com teclado físico; não declarar que Esc foi corrigido universalmente. A tentativa explícita de Keyboard Lock dentro do iframe foi recusada com InvalidStateError (documento principal necessário).
- Sem publicação no itch.io ou nova build Windows. ZIP com 50.753.000 bytes; saves e perfis de teste não incluídos.
