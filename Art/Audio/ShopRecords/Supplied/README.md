# Trilhas fornecidas pelo usuário

Recebidas em 18/11/2026 na pasta Soundtracks, organizada por disco. Originais permanecem intactos fora de Assets; hashes e nomes de origem estão no manifest.json. A primeira faixa de Laje 92 foi atualizada pelo usuário para Ice Cube antes da integração; a versão de OMA não foi usada.

15 cópias estéreo Ogg Vorbis, 32 kHz, qualidade 2, alvo −18 LUFS e pico −2 dBTP, em Assets/Resources/Audio/ShopRecords. Tim Maia usa limite pré-encode mais conservador (−5 dBTP) após detectar pico alto na primeira conversão. Total dos arquivos de jogo: 40.214.133 bytes; originais: 124.652.908 bytes. Nenhum corte de duração deliberado; versões e durações seguem os arquivos recebidos, não uma gravação diferente escolhida automaticamente. Medições finais em audio-checks.json, sem avaliação por escuta direta.

Preparação reproduzível: tools/prepare-supplied-soundtracks.py; medição: tools/check-supplied-audio.py (ferramentas locais na raiz do workspace). A tabela JSON por faixa registra título/artista, duração, hash de entrada/saída e medição inicial. Artistas e títulos são mostrados no player. A origem fornecida não comprova licença de distribuição; nenhuma licença foi inferida ou declarada como recebida. A curadoria de alternativas foi rejeitada, não foi integrada e não substitui estes arquivos.

UI mantém três discos e cinco faixas em cada, com nomes corrigidos para leitura, tempo, busca na faixa, anterior/próxima, pausar/continuar/retirar e avanço automático. Estado musicChoice/musicTrack/musicStopped por save; musicTrack ausente em saves antigos começa em 0. Navegar entre capas não muda a reprodução até clicar em uma faixa/Colocar disco. Capas finais pretas sobre vermelho/lima/roxo aplicadas preservando os GUIDs.

Carregamento assíncrono com uma solicitação pendente por vez; cache libera clipes que já não são atuais nem usados pelas fontes do crossfade. Streaming no importador desktop e sem pré-carregar os 15 áudios. Web precisa ser conferida em build própria; não presumir que o navegador respeita o mesmo streaming do desktop. Builds públicas existentes não foram alteradas nesta integração.
