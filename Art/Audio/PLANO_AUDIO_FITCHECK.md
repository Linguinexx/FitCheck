# Fit Check — lista de produção de áudio

Entrega para coleta/composição pelo grupo — 05/11/2026.

As durações, BPMs e quantidades abaixo são sugestões de produção para o jogo atual e as etapas planejadas. Não significam que esses arquivos já estejam implementados. O GDD orienta hip-hop instrumental com elementos eletrônicos/retrô e identidade brasileira, ambientes urbanos e falas estilizadas. As decisões posteriores mantêm exploração em primeira pessoa, bairro noturno, casa, loja e provador.

Hoje há cliques/foto/recompensa, passos, porta, cortina e vento provisórios gerados por código. O único áudio gravado encontrado nos Assets é o miado do Fubá: `Resources/Audio/CatMeow.wav`, 1,127 s; fonte/licença registradas em CREDITS.md. Os novos arquivos devem melhorar/substituir os provisórios, sem simplesmente sobrepor todos.

## Como ler e priorizar

- **P1 — essencial:** coletar primeiro para a primeira versão de teste com áudio.
- **P2 — acabamento:** amplia variedade, ambientação e feedback após o pacote essencial.
- **P3 — opcional/futuro:** produzir se houver tempo e se a situação existir na cena.
- **Duração** é a duração de cada arquivo. Um loop de 90 s pode tocar durante vários minutos.
- **Variações** são gravações/execuções diferentes da mesma ação; ajudam a evitar repetição mecânica.
- **Loop** precisa emendar sem estalo, silêncio extra ou mudança brusca. Avisar BPM e pontos de loop nas músicas.

## Direção sonora geral

Bairro brasileiro à noite, streetwear, encontros e música de garagem. Bateria seca, baixo com groove, samples/timbres eletrônicos, teclas ou guitarra discretas e percussão brasileira pontual, como pandeiro, tamborim ou agogô em pequenos detalhes. Vozes e efeitos com personalidade, curtos e claros. Textura retrô discreta; gravações limpas permitem ajustar o tratamento no jogo. A trilha deve deixar espaço para a leitura e os sons de fala. Cada ambiente precisa de identidade sem disputar volume com os outros.

## 1. Músicas

Todas instrumentais. Os BPMs são faixas sugeridas; informar o BPM real do arquivo entregue. Preferir versões com final de loop musical, sem longo fade de entrada/saída gravado.

| ID / nome sugerido | Uso | Duração por faixa | BPM sugerido | Como deve soar | Prioridade |
|---|---|---:|---:|---|---|
| `mus_menu` | Fachada inicial, saves e criação de personagem | 90–120 s, loop | 84–96 | Tema marcante e acolhedor; hip-hop de sampler, bateria seca, baixo redondo e motivo curto que dê identidade ao Fit Check. Criador pode reutilizar esta faixa. | P1 |
| `mus_bairro_noite` | Caminhada pela cidade | 120–180 s, loop | 88–102 | Groove urbano noturno, curioso e relaxado; percussão brasileira sutil, teclas/synths com espaço. Deve aguentar muitas caminhadas sem cansar. | P1 |
| `mus_loja` | Loja aberta e montagem de fit | 120–180 s, loop | 92–108 | Mais ritmo e atitude; bateria com swing, baixo e pequenos recortes de sampler. Energia de experimentar roupa e montar combinações, com melodia moderada. | P1 |
| `mus_quarto` | Casa, guarda-roupa e consulta ao FitFeed | 90–150 s, loop | 72–88 | Íntima, tranquila e noturna; acordes macios, baixo suave, bateria reduzida e pequenas texturas de fita. | P1 |
| `mus_encerramento` | Retrospectiva/final dos três dias | 60–90 s; final definido | 80–96 | Versão mais emotiva e completa do motivo do menu; sensação de comunidade e fechamento. Pode ser um arranjo da faixa do menu, não exige composição inteiramente nova. | P2 |
| `mus_tensao_final` | Últimos segundos do atendimento apressado | 8–16 s, loop | Compatível com a loja | Camada leve de percussão ou pulso que possa entrar sobre a trilha sem virar sirene. Entregar separada da música base. | P3 |

**Reutilização prevista:** provador usa a música da loja; criação usa a do menu; personalização pode usar a faixa do ambiente atual. Loading mantém/abaixa a faixa em transição: não precisamos de uma música de dois segundos. SIDE B, estúdio e AO VIVO podem usar versões filtradas/trechos das faixas existentes, com reprodução localizada; não tocar várias trilhas completas sobrepostas.

Se o compositor puder, entregar separadamente bateria, baixo, harmonia/melodia e percussão das quatro faixas principais. Esses stems são opcionais e não devem duplicar o pacote básico sem necessidade.

## 2. Ambientes contínuos

Ambientes de fundo sem ações muito próximas ou chamadas que se repitam de forma óbvia. Música e ambiente devem ser arquivos separados.

| ID | Duração | Variações / loop | Como deve soar | Prioridade |
|---|---:|---|---|---|
| `amb_bairro_noite` | 60–120 s | 1 loop | Ar de rua urbana à noite: vento leve, trânsito distante e vida ao fundo. Espaçoso, com pouca informação em primeiro plano. | P1 |
| `amb_loja_interior` | 45–90 s | 1 loop | Interior pequeno, ar/ventilação ou eletrônica discreta e ressonância curta. Volume baixo para receber cliente e provar roupa. | P1 |
| `amb_quarto_noite` | 45–90 s | 1 loop | Quarto quieto com cidade abafada pela janela, ar leve e pequenos ruídos do prédio. Aconchegante e noturno. | P1 |
| `amb_conversas_distantes` | 30–60 s | 2 loops | Pessoas conversando longe, palavras indistintas, risadas ocasionais suaves. Separado do fundo da cidade para usar só onde houver gente. | P2 |
| `amb_terraco_vento` | 30–60 s | 1 loop | Vento um pouco mais aberto e cidade distante, para fotos/local do terraço. Sem rajadas que escondam os outros sons. | P2 |
| `amb_publico_show` | 30–60 s | 1 loop | Público pequeno esperando/apreciando música, sem locutor ou frases compreensíveis. Usar apenas se houver evento/atividade representada. | P3 |

## 3. Interface, menus e transições

Feedback curto, seco e agradável; família sonora consistente. Evitar efeitos longos em ações que o jogador repete muito.

| ID | Duração por arquivo | Quantidade | Como deve soar / uso | Prioridade |
|---|---:|---:|---|---|
| `ui_click` | 0,07–0,15 s | 2 | Clique firme, macio e levemente digital para botões e abas. | P1 |
| `ui_confirm` | 0,20–0,40 s | 1 | Pequena subida de duas notas ou clique com resolução; confirmar ação/salvar escolha. | P1 |
| `ui_back` | 0,10–0,25 s | 1 | Retorno curto da mesma família, mais grave e discreto. | P1 |
| `ui_unavailable` | 0,20–0,40 s | 1 | Toque seco e arredondado indicando opção indisponível; evitar alarme agressivo. | P1 |
| `ui_hover` | 0,04–0,09 s | 1 | Toque muito leve ao mudar o foco entre opções. Uso com intervalo; não disparar continuamente sobre o texto. | P2 |
| `ui_panel_open` / `ui_panel_close` | 0,15–0,35 s | 1 de cada | Deslizamento de papel/cartão com acabamento digital, para painéis e balões. | P2 |
| `ui_save_ok` | 0,15–0,35 s | 1 | Confirmação delicada e rápida de salvamento; menor destaque que uma conquista. | P2 |
| `fx_transition` | 0,25–0,60 s | 2 | Sopro/swish curto e macio acompanhando entrada/saída de ambiente. Não precisa durar o loading inteiro. | P2 |

Criar/excluir save e confirmar/cancelar personalização reutilizam essa família. A confirmação visual de exclusão permanece obrigatória; som não a substitui.

## 4. Passos e movimento

Gravar tênis comum, próximo e sem música/vozes. Cada arquivo contém **um único apoio de pé**, com pouca sobra antes do ataque. NPCs podem reutilizar o mesmo banco em volume e posição adequados.

| ID | Duração por apoio | Quantidade | Como deve soar | Prioridade |
|---|---:|---:|---|---|
| `step_calcada_01…04` | 0,15–0,35 s | 4 | Borracha em calçada/pedra: impacto seco, pequena aspereza, peso moderado. | P1 |
| `step_loja_01…04` | 0,15–0,35 s | 4 | Borracha em piso rígido/cerâmico: um pouco mais definido, ressonância curta. | P1 |
| `step_madeira_01…04` | 0,18–0,40 s | 4 | Passo no piso do quarto, impacto mais opaco e rangido muito sutil. | P1 |
| `step_asfalto_01…04` | 0,15–0,35 s | 4 | Impacto abafado e áspero, para atravessar a avenida. | P2 |
| `foley_roupa_movimento_01…03` | 0,20–0,55 s | 3 | Atrito leve de tecido ao virar/andar/correr, sem parecer papel amassado. | P2 |
| `foley_arrastar_pe_01…03` | 0,20–0,50 s | 3 | Ajuste de apoio/borracha curta para poses e mudanças de direção. | P2 |
| `player_folego_01…02` | 0,50–1,20 s | 2 | Respiração discreta de esforço para corrida, se necessária. Não sugere uma mecânica de stamina. | P3 |

Corrida usa inicialmente os mesmos passos em outra cadência. Não é necessário um banco por marca de tênis, um loop de caminhada gravado nem sons de salto para o fluxo atual.

## 5. Portas, loja, provador e roupa

| ID | Duração | Quantidade | Como deve soar | Prioridade |
|---|---:|---:|---|---|
| `door_casa_open` / `door_casa_close` | 0,5–1,2 s | 1 de cada | Maçaneta/trinco e porta de madeira residencial; peso moderado, pouca reverberação. | P1 |
| `door_loja_open` / `door_loja_close` | 0,4–1,0 s | 1 de cada | Porta de loja com estrutura/vidro, dobradiça limpa e batida contida. | P1 |
| `door_loja_bell` | 0,5–1,2 s | 1 | Sininho curto de entrada; acolhedor, reconhecível e sem cauda estridente. | P1 |
| `curtain_open` / `curtain_close` | 0,7–1,4 s | 1 de cada | Tecido pesado deslizando e argolas no trilho; movimento contínuo, sem batidas metálicas exageradas. | P1 |
| `shop_open_sign` / `shop_close_sign` | 0,25–0,60 s | 1 de cada | Pequeno giro/batida de placa ou trinco sinalizando abrir/encerrar atendimento. | P2 |
| `equip_tecido_01…03` | 0,20–0,50 s | 3 | Troca de camiseta/calça: tecido com swish curto; pode atender as duas categorias. | P1 |
| `equip_tenis_01…02` | 0,20–0,45 s | 2 | Apoio de borracha/encaixe suave, sem efeito de armadura. | P2 |
| `equip_acessorio_01…02` | 0,12–0,35 s | 2 | Tecido/encaixe leve para boné, touca e acessórios. | P2 |
| `wardrobe_open` / `wardrobe_close` | 0,4–0,9 s | 1 de cada | Porta pequena de armário ou cabide leve. Usar se a apresentação do guarda-roupa tiver esse movimento. | P3 |
| `sleep_tecido` | 0,6–1,2 s | 1 | Lençol/colchão suave para descanso e passagem de dia. Sem despertador obrigatório. | P2 |

O GIF do provador pode permanecer silencioso. A troca de cor de decoração reutiliza seleção/confirmação da UI, sem precisar de um efeito para cada cor.

## 6. Falas estilizadas dos clientes

Sem dublagem completa nesta entrega. Precisamos de sílabas curtas não verbais, humm/ba/da/ah e pequenas inflexões que possam acompanhar os textos com pausas naturais. Gravação seca, limpa e em volume consistente, evitando frases reconhecíveis e gritos. Não disparar áudio para cada letra sem intervalo.

São **seis personagens distintos para nove atendimentos**. Retornos do mesmo cliente mantêm sua voz.

| Pasta / personagem | Pacote essencial | Duração por sílaba | Direção da voz |
|---|---|---:|---|
| `voice_caio_k7` | 6 sílabas diferentes | 0,08–0,22 s | Jovem, comunicativo, descontraído, ritmo com balanço; médio a médio-grave. |
| `voice_bia` | 6 sílabas diferentes | 0,08–0,22 s | Curiosa e expressiva, fala com intenção e pequenas mudanças de entonação. |
| `voice_davi_pico` | 6 sílabas diferentes | 0,08–0,22 s | Relaxado, direto e simpático; ataques macios e intervalos confortáveis. |
| `voice_malu` | 6 sílabas diferentes | 0,08–0,22 s | Viva e decidida; articulação rápida, mantendo clareza. O atendimento apressado muda o ritmo, não a identidade da voz. |
| `voice_seu_jota` | 6 sílabas diferentes | 0,10–0,25 s | Mais maduro, caloroso, bem-humorado e com ressonância de peito; entonação natural. |
| `voice_nina_faisca` | 6 sílabas diferentes | 0,08–0,22 s | Confiante, energética e musical, com presença sem gritar. |

**P1:** 36 sílabas no total, entregues separadamente (`voice_caio_k7_01.wav` etc.). As direções são propostas de interpretação, ajustáveis ao casting do grupo.

**P2:** por personagem, três reações curtas de 0,3–0,9 s: aprovação, dúvida e risinho/saudação. Total opcional de 18 arquivos. Moradores genéricos podem reutilizar vozes em volume distante; não exigem dublagem individual nesta etapa.

## 7. Foto, FitFeed, avaliação e progressão

| ID | Duração | Quantidade | Como deve soar | Prioridade |
|---|---:|---:|---|---|
| `photo_capture` | 0,20–0,55 s | 1 | Obturador curto com pequeno detalhe digital; satisfação de registrar o fit. | P1 |
| `feed_publish` | 0,40–0,80 s | 1 | Envio/postagem com movimento ascendente e conclusão discreta. | P1 |
| `result_0` / `result_3` / `result_7` / `result_10` | 0,8–1,8 s | 4 | Uma família musical: resultado baixo contido, 3 neutro, 7 positivo, 10 mais brilhante. Sem ridicularizar o jogador ou tocar vaia. | P1 |
| `rep_gain` / `rep_loss` | 0,25–0,60 s | 1 de cada | Pequena subida/descida eletrônica, suave e reconhecível. Não somar agressivamente ao jingle de avaliação. | P2 |
| `unlock_drop` | 0,8–1,6 s | 1 | Abertura de novidade, com percussão/sampler e resolução curta. | P1 |
| `achievement` | 1,0–2,0 s | 1 | Assinatura de conquista com mais presença, sem bloquear o jogo e sem cauda longa. | P1 |
| `day_open` / `day_close` | 0,8–1,8 s | 1 de cada | Abertura dá impulso; fechamento dá sensação de expediente concluído. Podem usar motivos das músicas. | P2 |
| `campaign_complete` | 2–4 s | 1 | Assinatura final calorosa e celebrativa, ligada ao tema principal. Pode anteceder a retrospectiva. | P2 |
| `feed_notification` | 0,20–0,45 s | 1 | Notificação própria do jogo, curta e amistosa. | P2 |
| `feed_like` | 0,10–0,25 s | 1 | Pequeno toque/pop macio para a futura interface do FitFeed. | P3 |

A interface do FitFeed continua adiada, mas os arquivos podem ser coletados. Uma ação não deve tocar foto + envio + nota + Reputação + conquista no mesmo instante: a integração vai organizar a sequência e os volumes.

## 8. Cidade e fontes localizadas

Efeitos de objetos próximos devem ser separados do ambiente geral. Entregar arquivos secos quando possível; distância, direção e abafamento serão tratados na integração.

| ID | Duração | Quantidade / loop | Como deve soar | Prioridade |
|---|---:|---|---|---|
| `car_pass_01…03` | 3–6 s | 3 | Carro urbano comum passando, motor moderado e pneus no asfalto. Evitar buzina embutida em toda passagem. | P1 |
| `car_engine` | 5–10 s | 1 loop | Motor estável e discreto, sem arrancada/corte brusco; alternativa para ligar o som ao veículo em movimento. | P2 |
| `car_horn` | 0,20–0,60 s | 2 | Buzinada curta para uso raro e contextual. | P3 |
| `skate_pass_01…02` | 2–5 s | 2 | Rodinhas em piso urbano e pequena vibração; ambiente distante, sem criar mecânica de skate. | P2 |
| `court_ball_01…03` | 0,25–0,60 s | 3 | Bola de basquete quicando em quadra; boa variação de intensidade. Usar se houver atividade representada. | P3 |
| `court_squeak_01…02` | 0,15–0,45 s | 2 | Atrito de sola curto, moderado; acompanhamento opcional da atividade da quadra. | P3 |
| `shop_fridge` | 10–25 s | 1 loop | Compressor/ventoinha suave para a mercearia; baixo, reconhecível só perto. | P2 |
| `laundry_machine` | 10–25 s | 1 loop | Tambor/motor em rotação estável, vibração leve e sem batidas recorrentes fortes. | P2 |
| `barber_clipper` | 2–5 s | 2 trechos | Máquina de cortar cabelo próxima, curta e espaçada; usar somente com atividade na barbearia. | P3 |
| `workshop_tool_01…03` | 0,4–1,5 s | 3 | Pequeno serviço mecânico: chave, metal e ferramenta manual, sem explosões ou oficina industrial pesada. | P2 |
| `music_speaker_air` | 10–20 s | 1 loop | Ruído muito discreto de amplificador/eletrônica para estúdio/caixa, se acrescentar algo perceptível. | P3 |
| `small_applause` | 2–4 s | 2 | Palmas de um grupo pequeno para evento/show; uso eventual e condicionado à cena. | P3 |

Motor em loop e passagem pronta são **alternativas de implementação**, não duas camadas obrigatórias tocando juntas. A mesma lógica vale para música vazando da SIDE B/estúdio/AO VIVO: reutilizar músicas licenciadas do próprio pacote em vez de coletar outra faixa para cada fachada. Cenários vazios não precisam emitir público ou apresentação inexistentes.

## 9. Fubá

| ID | Duração | Quantidade | Como deve soar | Prioridade |
|---|---:|---:|---|---|
| `cat_meow` | Aproximadamente 1 s | 1 já existente | Miado real, próximo, curto e simpático. O mesmo gato usa o mesmo banco em casa e na loja. | P1 já atendido |
| `cat_meow_alt_01…02` | 0,5–1,5 s | 2 | Variações naturais e tranquilas, com timbre semelhante ao arquivo atual. | P2 |
| `cat_purr` | 4–10 s | 1 loop | Ronronar baixo e macio ao receber carinho; não deve dominar o ambiente. | P2 |

Não substituir o miado existente automaticamente. Carinho pode reutilizar o atrito suave de tecido/movimento se fizer sentido.

## 10. Especificação de entrega e organização

Criar/usar a pasta **`audio recebidos` na raiz TCC3D**, fora de `FitCheck/Assets`, para conferência antes de importar. Nesta tarefa só foi criada a lista; nenhum novo áudio foi integrado.

```text
audio recebidos/
  01_musicas/
  02_ambientes/
  03_interface/
  04_passos_foley/
  05_portas_roupas/
  06_vozes/caio_k7/  (e uma pasta por personagem)
  07_resultados_fitfeed/
  08_cidade/
  09_fuba/
  FONTES_E_LICENCAS.md
```

- **Efeitos e vozes:** preferir WAV PCM, 16 ou 24 bits, 44,1 ou 48 kHz. Mono para sons curtos e fontes localizadas. Manter a frequência nativa quando já estiver boa; não é necessário aumentar resolução artificialmente.
- **Músicas:** OGG estéreo, aproximadamente 160–192 kbps, com nome, autor, BPM, duração e informação de loop. Se houver master WAV, pode ficar com o responsável pelo áudio; não duplicar todos os masters na pasta do jogo enquanto o espaço estiver curto.
- **Ambientes:** OGG, normalmente 96–128 kbps. Estéreo para fundos gerais; mono para uma máquina/objeto localizado. Uma versão utilizável por asset já basta para a primeira entrega.
- Sem saturação, cortes bruscos, estalos, silêncio grande antes do ataque ou vozes/músicas indesejadas no fundo. Deixar margem de volume e evitar volumes muito diferentes entre arquivos do mesmo banco. Não aplicar reverberação pesada nos efeitos próximos.
- Música pode ter textura de fita/vinil como decisão artística, mas entregar também uma versão limpa quando possível. Ruído constante não é necessário em toda faixa.
- Nomes em minúsculas, sem acento e sem espaços: `step_calcada_01.wav`, `voice_bia_03.wav`, `mus_loja_98bpm_loop.ogg`.
- Para cada som/música, registrar em `FONTES_E_LICENCAS.md`: ID/arquivo, autor, título original, link de origem, licença/autorização e crédito solicitado; anotar edições feitas. Para produção própria, registrar os integrantes responsáveis. Guardar comprovantes de autorização quando aplicável.
- Para loops, informar pontos de início/fim se não forem o arquivo inteiro. Para faixas, indicar se existe introdução separada. Não unir todas as falas ou todos os passos em um áudio longo.
- A coleta não exige plugins, arquivos de projeto de DAW, bibliotecas inteiras nem ZIPs com centenas de efeitos sem seleção. Entregar os itens escolhidos, organizados e identificados.

## Ordem prática de coleta

1. Quatro músicas principais e três fundos (cidade, loja, quarto).
2. Cliques/confirmar/voltar/indisponível; passos na calçada, loja e madeira; portas, sininho, cortina e troca de tecido.
3. Seis bancos de fala, com seis sílabas cada; foto, publicação, quatro resultados, desbloqueio e conquista.
4. Trânsito e refinamentos P2; conferir o miado já existente antes de procurar substituto.
5. P3 somente depois: sons condicionais de eventos, atividade nas fachadas, tensão musical e detalhes da futura UI do FitFeed.

Não há necessidade de produzir sons de combate, salto, caixa registradora/venda, gestão de estoque, chuva ou despertador para completar o fluxo atual. Novas mecânicas ou mudanças de apresentação podem ampliar esta lista posteriormente.
