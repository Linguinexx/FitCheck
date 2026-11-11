# Web Teste 02 — captura do mouse e saída

## Correções

- Retirada a chamada Application.Quit do menu inicial WebGL, inclusive da navegação por teclado. Jogar e Configurações mantêm posições e dimensões. O botão Sair da versão Windows permanece.
- Bridge WebGL solicita pointer lock diretamente pela ativação do navegador. Não usa a solicitação adiada da Unity que acabava consumindo outro clique. A câmera verifica a captura real no documento.
- Loading entre ambientes mantém a captura existente; a desmontagem do ambiente de origem não a libera durante a transição. A pausa, o Tab e os menus continuam liberando o cursor.
- Erros de permissão são recuperáveis, sem bloquear o player. Uma nova interação pode solicitar a captura de novo.

## Testes

- Unity 6000.6.7f1: tools/web02-build.log, FITCHECK_WEB_BUILD Succeeded, zero erros e avisos no relatório final.
- tools/check_browser_pointer.cjs: BROWSER_POINTER_PASSED, cobrindo captura em um clique, recaptura pelo botão, manutenção do estado, rejeição de permissão e liberação nos menus.
- Edge/Chromium via Playwright, renderização SwiftShader. Iframe com origem/porta diferente, sandbox e allow-pointer-lock, reproduzindo as condições de incorporação sem acessar a conta itch.io do usuário.
- Menu inicial sem Sair; clicar na antiga área não interrompe o jogo. Seleção de saves e criação continuam funcionando.
- Novo personagem Web 02 QA: chegada ao quarto com document.pointerLockElement ativo, cursor oculto e sem clique extra.
- Três ciclos de pausa/Retomar: captura ausente na pausa e presente após um único clique em Retomar. Movimento do mouse alterou a câmera, comprovado nas capturas.
- Tab libera o ponteiro e um clique o captura novamente.
- Porta da casa para o bairro: captura presente após o loading; nenhum evento pointerlockchange registrado durante a passagem.
- Recarregamento do iframe preservou o personagem Web 02 QA; continuar capturou o ponteiro ao chegar ao quarto. Voltar ao início liberou a captura e restaurou o título.
- Capturas em Captures/Web02. Avisos observados: ReadPixels do renderizador por software e allowfullscreen redundante no iframe de QA; sem erros de execução nos cenários testados.
- Windows Teste 08 preservado, SHA256 bd3b65967b1f4c2ed1e38c1c70e355cf7ffc56ed9b78bd6506b10fac59a5bd48.

## Entrega e limites

ZIP em Builds/FitCheck-Web-Test02.zip, index.html na raiz; hashes e tamanhos reais em delivery.json. Limites de 200 MB conferidos para pasta e ZIP. Web Teste 01 preservado como anterior. Não houve publicação na conta do usuário nem nova campanha completa de sete dias; a prévia real do itch.io ainda deve ser conferida após a substituição do ZIP.

Após os testes, removida a cópia temporária tools/.web-fix-build-project: 801.097.956 bytes lógicos. Relatório em tools/web02-cleanup.json. Biblioteca da Unity aberta, referências, saves e entregas atuais/anteriores preservados. Integridade do ZIP final e ausência de saves verificadas novamente após o empacotamento.

## Tipografia da página

Comparação visual de Bungee, Bowlby One SC e Passion One Black em Art/Marketing/Itch/titulos-comparacao-v1.png. Arquivos oficiais Google Fonts, renderizados em navegador. Tipografia do jogo e tema publicado não alterados.
