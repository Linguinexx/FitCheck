# Próxima build Windows — revisão após Web Teste 06

As alterações compartilhadas abaixo já estão no código do projeto. A build Windows Teste 08 existente não foi substituída neste pedido. Compilar e validar uma nova versão Windows para distribuí-las.

| Item | Código compartilhado | Verificação na próxima build Windows |
|---|---|---|
| Hover do Jogar | Vermelho de hover mais escuro e perceptível, mantendo botões neutros sem contorno | Conferir mouse e foco por teclado em resoluções diferentes |
| Sensibilidade | Escala 0–100; padrão 35; zero realmente não gira a câmera | Conferir entrada limpa, migração do antigo padrão, persistência e extremos 0/100 |
| Sliders de áudio | Mesmo componente visual/interativo da sensibilidade: trilho, preenchimento vermelho, puxador claro, foco e teclado | Arrastar e usar setas/Home/End; conferir volume geral, música, mute e persistência |
| Tela cheia/janela | Seletor retirado da interface Windows/Editor; permanece somente na web | Confirmar que o Windows inicia no modo previsto, sem alterar distribuição da UI |
| Mouse ilustrado | Botões esquerdo/direito acompanham a curva superior da carcaça; scroll com ranhuras | Conferir Exploração e Provador, inclusive em janela menor |
| Esc e Retomar | Ação de Esc centralizada e retorno libera o modo de cursor livre | Conferir um Esc para pausar/retomar, ajuda e áudio voltando um nível, sem despausar acidentalmente |

## Exclusivos da web

- Esc consumido pelo navegador: observar pointerlockchange/fullscreenchange e deduplicar o pedido de pausa. Não aplicar essa ponte JavaScript ao Windows.
- Botão Sair removido do título web. No Windows, Sair continua fechando o aplicativo.
- O navegador/itch.io controla a liberação do ponteiro e a tela cheia. A versão Windows não deve herdar limitações ou fallbacks de iframe.

Preservar companyName/productName e saves, referências originais, IDs das roupas e histórico de progressão. Manter a entrega Windows atual até a nova passar pelas verificações.

## Revisao Web06

Catalogo de teste tambem removido do codigo compartilhado Windows; validar desbloqueios de save novo e antigo na proxima compilacao. P e captura de teclado do navegador sao exclusivos da web.
