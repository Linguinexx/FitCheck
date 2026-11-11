# Validação — Web Teste 04

Unity 6000.6.7f1: compilação concluída com zero erros e zero avisos. Arquivos finais comparados às fontes. Edge/Chromium com renderização de software, iframe local entre duas origens (portas 8717/8718), perfil exclusivo e save QA separado. Não houve publicação ou teste no domínio público do itch.io.

- Desbloqueio externo do ponteiro sem evento de teclado abriu pausa; Retomar recapturou com um clique. Cobre o Esc consumido pelo navegador que a validação anterior não cobria.
- Esc por teclado durante exploração e em tela cheia abriu pausa com ponteiro livre. Retomar recapturou. Prévias em Captures/Web04/first-consumed-escape.png e fullscreen-escape-pause.png.
- Sair no título com Fullscreen API ativa saiu da tela cheia. Repetir em janela não encerrou o player; Configurações e Jogar continuaram funcionando.
- Entrada limpa mostrou sensibilidade 35. Home/End mostraram 0/100. Áudio mostrou 25%/75% no componente compartilhado. Retorno de Controles/Áudio por Esc passou.
- Conferidos visualmente: hover do Jogar, sliders, mouse curvo nas duas páginas, criação e início em casa. Capturas em Captures/Web04. Campanha completa não repetida nesta revisão.
- Teste isolado da ponte JavaScript passou: primeiro desbloqueio, deduplicação, retorno, transições, recusa recuperável e Sair seguro; complementa o teste visual.

Limite da hospedagem: Fullscreen API difere da expansão CSS do iframe pelo itch.io. O runtime público inspecionado não fornece protocolo documentado de recolhimento. Sair não navega, encerra Unity ou manipula o documento externo; o controle da página recolhe essa expansão CSS.

Windows Teste 08 preservado. Próxima compilação/validação em PENDENCIAS-WINDOWS.md.
