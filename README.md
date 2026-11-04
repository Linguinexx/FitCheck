# Fit Check by DripLab

Jogo 3D de montagem de looks streetwear em Unity **6000.6.7f1**. Arte em desenvolvimento: personagens humanos estilizados, cenário noturno e texturas inspiradas em PS1/PS2. A pixelização é opcional e começa desativada.

## Abrir e jogar

1. Adicione esta pasta ao Unity Hub com a versão indicada.
2. Abra `Assets/Scenes/FirstCustomer.unity` e pressione **Play**.
3. Na tela da fachada, clique em **Jogar** e selecione um dos três saves. Um espaço vazio abre a criação do personagem; criar ou continuar começa no quarto em primeira pessoa.
4. Ande até a porta de casa e pressione **E — Sair de casa**. Você sai pelo vestíbulo do residencial **AP 03**, na Rua das Lajes. O caminho direto segue pelo comércio até a DripLab; passagens laterais dão acesso à galeria, à quadra e à casa de shows. No bairro, caminhe até a porta sob o letreiro **DRIPLAB** e pressione **E — Entrar na DripLab**. O prédio RADIO é independente da casa.
5. A loja começa fechada: passe por trás do balcão e pressione **E — Abrir a loja** quando estiver pronto. Ao terminar um dia, feche o expediente e volte para casa. A cama abre as opções de descanso; o guarda-roupa e a mesa dão acesso ao uniforme, à aparência e ao FitFeed.

Cada save mantém aparência, campanha e fotos separadas em `saves/slot-1`, `slot-2` ou `slot-3`, dentro da pasta de dados persistentes da Unity. Na primeira seleção, o progresso da versão anterior é copiado para o primeiro espaço; os arquivos originais e as fotos antigas permanecem preservados. **Excluir save** abre uma confirmação com o espaço e o nome do personagem; **Cancelar** mantém os arquivos e **Excluir definitivamente** libera somente o espaço escolhido. Um save excluído não volta a ser importado automaticamente da versão antiga.

Na exploração, **WASD** move, **Shift** corre, **mouse** olha, **E** interage, **Esc** pausa e **Tab** libera ou captura o cursor. No Editor, dê foco à aba **Game**. Menus e provador usam o mouse livre.

Com a loja fechada, **F — Personalizar loja** abre uma prévia do ambiente: paredes, três pisos, cortina, quadro e plantas. Os botões **Geral / Provador / Quadro** mudam o enquadramento da prévia. **Salvar decoração** guarda as escolhas somente neste save; **Cancelar** ou **Esc** restaura a decoração anterior. A decoração volta ao entrar na loja novamente.

A galeria e a quadra ficam ao norte, ligadas pelas passagens e pela Travessa do Encontro. A casa de shows fica do outro lado da avenida, ao sul. O percurso direto casa–loja permanece próximo de 32 segundos andando; correr é opcional. Muros, casario e vegetação fecham o perímetro. O céu noturno usa nuvens contínuas, halo lunar e estrelas com variação suave. Os moradores têm aparência própria, roupas casuais sem identificação da DripLab e poses de espera ou caminhada.

O rapaz encostado na parede é um frequentador fixo da loja, separado dos atendimentos. Após abrir a loja, o cliente entra, caminha até a frente do balcão e espera; você continua livre para andar e olhar durante a chegada. Passe por trás do balcão e pressione **E** para atendê-lo. Após conversar e escolher uma pergunta opcional, ele caminha até o provador; a câmera acompanha e a cortina abre antes da montagem do fit. O personagem completo do jogador permanece no balcão durante a câmera externa. Ao finalizar, o cliente sai usando o look e o resultado aparece no FitFeed. O frequentador permanece na loja.

A montagem acontece no próprio provador da loja. Arraste o personagem para girar e use a roda do mouse para aproximar ou afastar. O cliente vira-se ao chegar e apresenta o fit antes de sair. Pausa interrompe a sequência; cancelar e voltar ao início reinicia o atendimento sem finalizar um look.

As portas ligam casa, bairro e loja nos dois sentidos, com interações próximas e controle em primeira pessoa. As mudanças entre locais têm transição breve; pausa e movimento reduzido são respeitados.

Na troca de uniforme em casa, o personagem aparece inteiro, de frente, com botões de giro. A entrada do prédio tem pórtico, porta aberta e soleira; sair de casa inclui um passo curto pela entrada, que congela durante a pausa.

Fubá, o gato laranja deitado, aparece na cama em casa e num canto da loja. Olhe para ele de perto e pressione **E — Fazer carinho em Fubá** para ouvir seu miado. Pausa congela o movimento e o som; a opção de silenciar também vale para ele. Procedência e licença do som estão em `Art/Audio/CREDITS.md`.

O gato usa detalhe estático de pelo e marcações de pelagem, com patas recolhidas e cauda afilada. A biblioteca de superfícies inclui 12 materiais do ambientCG, mantendo resoluções baixas, mipmaps e a paleta do jogo. Créditos e procedência estão em `Art/AmbientCG/CREDITS.md`.

## Conteúdo atual

- Campanha de três dias, com nove pedidos e pontuação por ocasião, estilo e restrições.
- Catálogo de 21 peças em três drops, desbloqueios por Reputação e repetição de atendimentos.
- Criação do jogador com nome, tons de pele, sete cabelos (incluindo ondulado longo e coque alto), cores e expressões.
- Loja com araras, tênis, caixa e provador; bairro com moradores e veículos em movimento.
- Bairro conectado pela Rua das Lajes: AP 03, mercearia, Galeria Lado B, quadra, casa de shows, oficina, café e Praça do Encontro. Caminhada casa–loja medida a 60 Hz: aproximadamente 32 segundos, na velocidade normal de 4,4 unidades/s; sete moradores, iluminação noturna e sinalização nos dois sentidos.
- Galeria, quadra e casa de shows compartilham a mesma arquitetura com as novas fotos do FitFeed. Fachadas e cenografia não abrem interiores adicionais.
- Progresso, aparência e fotos salvos localmente. Os saves não fazem parte do repositório.

Cabelo, mãos, roupas e demais elementos visuais continuam sujeitos à revisão artística.

## Estrutura e build

- `Assets/FitCheck/`: lógica, personagens e cenário procedurais, interface e verificações.
- `Assets/Resources/`: shaders, texturas e materiais do jogo.
- `Assets/Scenes/FirstCustomer.unity`: cena de entrada.
- `Art/`: conceitos, documentação e créditos das texturas.
- `Packages/` e `ProjectSettings/`: configuração portável da Unity.

Para gerar o player Windows, use **Tools > Fit Check > Build Windows first person**. O destino está em `Assets/FitCheck/Editor/CitySliceBuild.cs`. Mantenha todos os arquivos gerados junto ao executável.

Builds, capturas, caches e ferramentas locais de integração não são versionados. Os manifests publicados excluem a integração MCP usada na máquina de desenvolvimento; ela não é necessária para abrir ou jogar o projeto.

## Verificações

O código de verificação acompanha o projeto. Há checagens do catálogo/campanha, expressões, encaixe das roupas, malhas das mãos, caminhada, câmera, navegação e fluxo do atendimento.

O player aceita `-city-proof -shop-review -proof-output "<pasta>"` para executar as verificações nativas e salvar capturas. Esse modo utiliza armazenamento separado dos saves do jogador e encerra o processo ao concluir.

## Texturas e créditos

Foram incorporados 12 mapas de cor do ambientCG, adaptados à direção de arte e com resolução de importação limitada. Fontes e licença CC0: [créditos ambientCG](Art/AmbientCG/CREDITS.md). Não foram incorporados modelos ou marcas externas de calçados.

As decisões e revisões anteriores estão em [DEVLOG.md](DEVLOG.md), [DIRECAO_ARTE.md](DIRECAO_ARTE.md) e [TESTES.md](TESTES.md).


## Revisão Kaos da interface e do bairro

O início usa a fachada em uma cena ao vivo, com moradores, tráfego, céu e movimento sutil da câmera, além do Selo Kaos oficial. Jogar continua levando à seleção dos três saves. A interface compartilha botões em Vermelho Kaos com relevo, sombra e resposta ao cursor/clique.

Configurações está disponível no início e na pausa: sensibilidade do mouse de 25–250%, silenciar, movimento reduzido e filtro retrô. A sensibilidade é salva localmente. Durante a exploração, as interações próximas recebem destaque e uma tecla junto ao alvo; não há rodapé fixo de controles. Falas usam balões de HQ com entrada animada.

A decoração oferece oito cores de parede e oito de cortina, incluindo roxo, rosa, azul e turquesa. A cidade recebeu cores mais vivas, um morro com relevo e vegetação atrás da quadra, mural de passinho, espaço de batalha de rima, vinis, bicicleta e placas brasileiras em pontos específicos. O celular acompanha a mão do morador e a surpresa alterna uma reação breve com a boca em repouso.

Build desta revisão: `Builds/Windows-Streetwear/FitCheck.exe`. Evidências e capturas reais da interface: `Captures/KaosCity`.


A revisão de identidade usa Montserrat nos textos e botões e Barlow Condensed nos títulos. As fontes e suas licenças acompanham o jogo em `Art/Fonts`. Botões secundários são neutros e reagem em Kaos ao mouse; a marca inicial vermelha tem movimento proporcional leve, desligado com movimento reduzido. O crédito do início é “A game by DripLab”.

Vidros usam textura fotográfica de desgaste já licenciada no projeto, reflexos estilizados e caixilhos; a janela de casa recebeu acabamento, cortinas e silhueta noturna. Pálpebras independentes fazem todos os moradores, clientes, frequentador e jogador visível piscarem, inclusive durante a montagem do fit. Há um observador original escondido numa janela da galeria. Balões de HQ têm contorno, cauda, retícula discreta e entrada com pequeno salto.

As referências de roupas trazidas pelo usuário e as futuras artes dos quadros estão registradas em `Art/REFERENCIAS_PENDENTES.md`; não foram aplicadas automaticamente.


Revisão PrintStyle: Archivo Black em títulos e ações curtas, Space Grotesk no texto e ações longas. Botões viram placas de impressão com recortes diagonais, contorno/sombra pretos e fundo papel nos secundários; seleção e hover em Kaos. As piscadas retomam o movimento esférico original do player, compartilhado com NPCs: a pálpebra percorre a superfície do olho sem afundar no meio da animação e sem apagar pupilas.


Revisão GalleryFix: botões restaurados à aparência arredondada escura anterior aos recortes. Balões ambientais aparecem exclusivamente no bairro; os balões do atendimento continuam na loja. Oito artes fornecidas em `artes quadros` substituem os elementos gráficos provisórios da casa e loja. O quadro personalizável oferece as oito imagens, com escolha salva por espaço e proporções originais preservadas. Os outros quadros ficam distribuídos sobre as araras/prateleiras e no quarto. Procedência em `Art/UserArtwork/sources.json`. Roupas permanecem como próxima etapa.


Revisão StreetArtwork: cinco outdoors horizontais com estrutura/refletores distribuídos pelo trecho residencial, mercearia, oficina, quadra e casa de shows. Quatro cartazes ficam em fachadas e passagens. As artes inteiras e suas proporções são preservadas; o percurso permanece livre. Os botões seguem o formato arredondado da referência, com profundidade e contorno apenas nas opções selecionadas. O estado normal/hover não tem borda, encaixes ou recortes laterais.


Revisão MapPolish: calçada externa contínua com escala/projeção de textura únicas, sem os remendos de chão e retângulos de terreno exposto. Palavra solta do café removida. Túneis modelados nos dois extremos da avenida permitem a passagem exclusiva do trânsito; o jogador permanece no bairro. O AP 03 recebeu parede traseira, janelas, teto do vestíbulo, acabamento da folha aberta e jardineiras. Copas que atravessavam muros foram substituídas por árvores menores enraizadas; novos canteiros ocupam áreas livres verificadas contra a arquitetura.

Postes, bases, rodízios e roupas das araras usam posições comuns. As três araras foram redistribuídas com folga superior a um metro para a cortina, preservando os nove modelos de blusa. A reformulação artística do vestuário continua como etapa separada.


Revisão NormalFlow: removida a faixa de asfalto sobreposta à calçada perto dos bancos e o objeto sob a sacola do balcão. No provador vazio, E somente abre/fecha a cortina; clientes continuam entrando pela porta e aguardando atendimento no balcão. F próximo ao balcão, antes de abrir a loja, permite personalizar a loja: aba Provador oferece cores independentes para cortina, fundo animado e LED, com prévia interna e salvar/cancelar por espaço.

A barra fixa superior foi removida. Esc abre a pausa simplificada; o fechamento retorna à loja, e o FitFeed retorna ao quarto. Sem botões para viajar entre ambientes: use as portas do bairro. A cama cuida do descanso, o guarda-roupa da aparência/uniforme e a mesa do FitFeed. Executável atual: Builds/Windows-Streetwear/FitCheck.exe.


Revisão GifGallery: o observador tem corpo completo e sacada dimensionada para apoiá-lo; a base da galeria foi fechada para ocultar a porta por trás. Em F → Provador, as duas escolhas do GIF são agora cor do fundo e cor das formas animadas, permitindo combinações como roxo/verde ou marinho/rosa. O padrão preto/roxo permanece disponível. A escolha de LED físico foi retirada da interface. As combinações possuem prévia, Cancelar e Salvar por espaço, com compatibilidade dos arquivos anteriores.


### Primeiro protótipo de vestuário no Blender — jeans baggy
A jeans Open Court (CG_BOTTOM_01) passou a usar malha e UV autorados no Blender, com pernas largas, barra acumulada sobre o tênis, bolsos traseiros e costuras. As outras peças aguardam revisão. IDs, critérios, drops e desbloqueios existentes foram preservados.

Fonte editável: Art/Garments/BaggyDenim01/BaggyDenim01.blend. FBX e textura 512 estão em Assets/Resources/Garments. Tools/Fit Check/Bake Blender jeans converte a malha importada para o recurso de runtime; a Unity aplica postura e pesos contínuos na cintura/entreperna para manter as partes unidas. O shader de tecido usa as UVs reais da peça e textura estática, sem atlas animado ou contorno duplicado. O arquivo Blender inclui uma referência do personagem e mantém a cena anterior separada.

Esta é a primeira peça para avaliar silhueta e caimento; a coleção ainda não foi refeita. Executável: Builds/Windows-Streetwear/FitCheck.exe.


### Streetwear — coleção Blender
Foram produzidas 17 novas peças: nove partes de cima (camisetas, jerseys, jaqueta de couro, fleece e moletom), cinco calças/bermudas e três construções de tênis. Junto da baggy Open Court, são 18 modelos de vestuário/calçados com fontes Blender. Novos recortes, bolsos, lapelas, golas, cadarços, solados e estampas; fonte editável e mapa dos IDs em Art/Garments/StreetwearCollection. IDs e desbloqueios da campanha foram preservados. A malha de binding das mangas continua na Unity para manter os encaixes animados.

Versão jogável atual: Builds/Windows-Streetwear/FitCheck.exe. Exemplos de nove looks e comparação dos três tênis em Captures/Streetwear/collection-lookbook.png e collection-shoes.png. A coleção pode ser revisada pelos arquivos Blender e refinada sem refazer o corpo dos personagens.


Limpeza desta entrega: removidos 39 diretórios/pacotes antigos gerados em Builds, liberando 2.33 GiB antes de remover a cópia temporária de compilação. Preservados executável e ZIP de Streetwear e da jeans baggy anterior, todos os fontes Unity/Blender, texturas, referências, artes, capturas e saves. Nenhum cache da Unity ativa foi removido. Relatório local: tools/streetwear-cleanup.json.

## Desempenho da cidade e loja

Roupas e expositores reutilizam malhas e materiais; cabeças rígidas usam transforms e a deformação do corpo reutiliza pesos e matrizes. NPCs próximos continuam animando integralmente. Medições e limites da otimização estão em [Art/Performance/EntryOptimization.md](Art/Performance/EntryOptimization.md).
