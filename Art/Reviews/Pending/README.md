# Companheiros, espelho e clientes especiais — 12/11/2026

- Gato bicolor agora branco, inclusive no Siamês. IDs e saves mantidos; cachorro bicolor preservado.
- Júlia no primeiro atendimento do dia 2: 30 segundos. Iara Ponteiro no último atendimento do dia 4: 45 segundos. Cronômetros começam no provador, pausam com o jogo e avisam nos últimos 10 segundos. Confirmação continua consumindo tempo.
- Iara Ponteiro, Dante Remendo e Cora Semente substituem retornos de Lia (dia 4), Breno (dia 5) e Seu Jota (dia 6). Aparências próprias, novos pedidos e três retratos do modelo do jogo. Campanha mantém 7 dias/21 encontros, IDs, regras de pontuação, locais, ordem e desbloqueios. Novos temas: pressa, uniforme de rua e paleta calma. Não foi criado sistema de dinheiro/orçamento.
- Casa: aba Espelho com Original, Paint 95 e Fit Player. Arte própria inspirada em interfaces antigas, quatro faixas opacas ao redor do vidro; superfície refletiva livre e inalterada. Preview, foco automático, Salvar/Cancelar e home-style.json por save. Saves antigos começam na moldura original. Reflexo continua 128 × 352 / 15 Hz.
- Sons do pacote recebido: miados alternados para o companheiro gato e ronronado depois do carinho; Fubá mantém o miado próprio. Latido original sintetizado já existente preservado, sem gravação nova de cachorro disponível no pacote. Palmas mantidas e ruído baixo de tecido ao iniciar gestos. Skate, aplausos ocasionais da quadra, buzina rara, lavanderia e geladeira localizados no bairro; retorno sonoro ao cancelar decoração. Pausa, mute, alcance e intervalos respeitados. Originais preservados; importação mono 22.050 Hz de 10 sons / 2,22 MB em PCM, com origem/hashes em Art/Audio/COMPANION_AND_CITY_SOUNDS.json.

## Validação antes da build

Unity 6000.6.7f1, projeto isolado: `PENDING_REVIEW_PASSED`. Confere campanha e inventários, soluções de pontuação máxima para os 21 encontros, três novas identidades/perfis, relógios 30/45 e pausa, gatos brancos em três tipos, cachorro bicolor, variação de miado e ronronado, oito bancos de som locais, migração/cancelamento/salvamento das molduras, reflexo real e malha de apenas oito triângulos.

Revisor em Art/Reviews/Pending/PendingReview.cs; depende dos revisores CompanionsReview.cs e HomeDecorReview.cs já documentados. Copiar os três para Editor de uma cópia isolada e executar `FitCheck.Editor.PendingReview.Capture -city-proof -pending-output <pasta>`. Nenhum save real é usado. Prévia: Captures/Pending/Final.

## Entregas e teste nativo

Windows Teste 09: compilação final sem erros/avisos. Executável validado com `-city-proof -test-build-review`, armazenamento separado: `TEST_BUILD_01_PASSED` e `LOADING_REVIEW_PASSED`. Inclui casa/bairro/loja, trajeto de 31,87 s na ida e 31,18 s na volta, decoração prévia/cancelar/salvar, carinho/pausa/intervalo e Fubá distinto, roupas/corpo, oito transições, sequência completa de atendimento, continuidade de câmera e três saves independentes. A campanha inteira não foi jogada manualmente; as soluções/notas/progressão dos 21 encontros passaram na validação de dados.

Web Teste 10: compilação final sem erros/avisos. O resumo incremental da Unity informou somente o tamanho dos arquivos alterados; o manifesto de entrega usa a soma real de TODOS os arquivos (52.178.274 bytes), não aquele resumo.

ZIP Windows: 75.477.162 bytes; pasta completa 134.553.465 bytes. ZIP Web: 52.069.355 bytes; pasta 52.178.274 bytes. Ambos abaixo de 200.000.000 bytes, inclusive extraídos. ZIPs abertos/testados, SHA-256 e manifesto em delivery.json. Web tem index.html na raiz. Sem saves, perfis de navegador ou caches nos arquivos enviados.

Na primeira importação Windows, três ambientes OGG existentes emitiram aviso de duração truncada e houve um aviso de variável sem uso. A variável foi corrigida; a compilação final incremental ficou sem avisos. Os originais de áudio foram preservados.


## Navegador e limpeza final

Edge em iframe local entre origens diferentes, perfil exclusivo de QA, renderização por software: título, seleção de save, criação, aba Companheiro/gato branco Siamês, salvar/entrar em casa, reabrir o navegador e continuar o mesmo save com pet preservado. P abre pausa e um clique em Retomar recaptura o ponteiro; confirmação final feita na página limpa, sem instrumentação, com pointerLocked=true e o jogo pronto. Prévia em Captures/Web10.

O diagnóstico inicial encontrou WrongDocumentError porque a aba automatizada não era a ativa; trazer a aba para frente resolveu. Uma primeira tentativa de sequência após recarregar também clicou antes da intro terminar; repetida depois da intro, passou. Nenhuma mudança de código foi necessária para estes dois casos. Console teve somente favicon ausente do servidor externo de teste e avisos do driver de software por ReadPixels, sem erro de runtime/shader do jogo observado. Áudio e câmera foram verificados pelo código/estado; não houve avaliação subjetiva de todos os sons por alto-falantes nem medição de FPS no hardware do usuário. A campanha inteira não foi repetida no navegador. Não houve publicação no itch.io.

Limpeza: 454.543.889 bytes recuperados de builds antigas excedentes e cópias temporárias já verificadas. Mantidas Windows 09/08 e Web 10/09. Hashes dos dois novos ZIPs conferidos antes da limpeza; fontes, referências, modelos recebidos e saves do usuário preservados. Registro em tools/pending-release-cleanup.json.
