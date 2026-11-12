# Web Teste 09 — interação e iluminação de personagens

Affordances: resposta de 0,30 s ao acionar E/F numa interação disponível, com tecla pressionada, troca breve de cor e pulso na borda. Com movimento reduzido, só a cor muda; a pausa congela o tempo. Resposta curta preservada ao abrir um painel, sem sobrepor loadings.

Personagens: luz principal quente, sombras suaves mais marcadas e recorte vermelho lateral inspirado na capa do itch.io. Texturas/UVs e expressões preservadas; olhos recebem efeito suave e sem recorte vermelho. Shader compartilhado por pele, cabelo e roupas, incluindo peças com acabamentos. Cenário não recebe essa marcação de material. Nenhuma nova luz em tempo real ou textura; geometria/material count preservados.

Provador atualizado incluído nesta entrega. Esc permanece sem comandos na web; P controla pausa. Catálogo de teste desligado. Windows não recompilado.

Prévias Unity D3D11 com três tons de pele em Captures/CoverLighting/Final. Compilação web final Unity 6000.6.7f1 concluída com zero erros/avisos; validações de campanha, entregas e histórico passaram. Sem erros de shader no teste WebGL.

Edge 157 em iframe local entre origens distintas e perfil QA: título e seleção de save mostraram personagens com a nova iluminação. Em casa, Q não disparou feedback; E em Fubá mostrou a tecla clara/pressionada e pulso no contorno; depois de 0,5 s o aviso voltou ao normal. Com movimento reduzido ligado, resposta de cor sem pulso expansivo/deslocamento. P e Retomar continuaram capturando a câmera. Prévias em Captures/Web09. Não foi repetida manualmente toda a campanha, nem testado cada objeto/F na loja no navegador; todos os handlers E/F usam a mesma resposta, sem mudar a lógica das ações.

ZIP validado com index.html na raiz, sem saves/perfis/cache, 50.752.675 bytes. Não houve publicação no itch.io nem nova compilação Windows. FPS no equipamento do jogador não foi medido; renderização de teste usou GPU por software.
