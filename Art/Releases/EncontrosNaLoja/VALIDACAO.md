# Fit Check — Encontros na loja — 14/11/2026

## Conteúdo desta entrega

Inclui as novidades posteriores à entrega de 13/11: Pinscher, Shih-tzu e Golden; celebração com fogos antes da foto final; adesivos/mural, basquete, desafios de emotes e histórias diárias do bairro; resenhas/perguntas do Nando, escolha de trilha na caixa de som e álbum/foto favorita no balcão.

Campanha mantém sete dias e 21 encontros. Identidade companyName/productName preservada; nenhum save real ou preparado acompanha os pacotes. Menu e distribuição da UI mantidos; Windows e Web possuem armazenamentos independentes.

## Verificações

- Unity 6000.6.7f1. Compilação Windows concluída com zero erros/avisos.
- Executável sanitizado e empacotado: TEST_BUILD_01_PASSED. Validação de roupas e encaixes, 63 combinações, decoração, gato, percurso casa/loja, oito loadings, atendimento completo e três saves isolados.
- Mesmo executável: SHOP_MOMENTS_PASSED. Rádio, álbum vazio/fotos reais/favorita, persistência, resposta errada/certa/repetida no desafio, sem mudança da campanha; lazer apenas com loja fechada, guia disponível durante expediente, cortina vazia e atendimento completo.
- Revisões anteriores das novidades: PETS_FINALE_PASSED e NEIGHBOURHOOD_ACTIVITIES_PASSED; fontes e evidências em Art/Reviews/PetsFinale, Art/Reviews/NeighbourhoodActivities e Art/Reviews/ShopMoments.



## Entrega validada

- Web compilada com zero erros/avisos. Verificação em iframe com perfil isolado: criação com Golden, entrada em casa, pausa por P, retorno com mouse capturado no primeiro clique, recarga e reabertura do save com companheiro/controle preservados. Evidências em Captures/EncontrosRelease/Web; relatório web-validation.json.
- Não foram detectados erros de execução do jogo. O wrapper local teve favicon ausente e o renderizador de software exibiu avisos de ReadPixels; esta conferência não mede FPS em hardware real.
- Windows: ZIP 77477146 bytes; pasta 139569155 bytes. Ambos abaixo de 200 MB. Hash SHA-256 em delivery.json e ao lado dos ZIPs.
- Web: ZIP 52306764 bytes; pasta 52418026 bytes. Ambos abaixo de 200 MB. Hash SHA-256 em delivery.json e ao lado dos ZIPs.
- Arquivos atuais Builds/FitCheck-Windows.zip e Builds/FitCheck-Web.zip, pastas Windows/Web, sem “teste” no nome. index.html na raiz do ZIP Web. Entregas de 13/11 preservadas em Windows-Previous/Web-Previous e ZIPs correspondentes; hashes anteriores em previous.json. Publicação no itch.io manual.

- Limpeza após validação: 1458669928 bytes de caches/compilações duplicadas e saves de verificação; entregas antigas Windows-Test09/Web-Test10 removidas somente após conferir os hashes da atual e da anterior de 13/11. Originais, referências, saves reais e capturas finais preservados.
