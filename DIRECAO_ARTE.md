# Direção de arte — passe PS1 / Mii

## Revisão atual — cabelo e junção com as peças de cabeça

O usuário aprovou o charme das expressões e pediu refinamento do cabelo e da junção com bonés/toucas. Corrigido o comportamento que eliminava todo o cabelo ao equipar uma peça. O cabelo sob a cobertura usa uma faixa ajustada à linha da abertura do boné ou à parte inferior da dobra da touca; conserva laterais, costeletas e nuca, com espessura reduzida na área coberta.

Os volumes/franjas são esculpidos na superfície contínua, com linhas distintas para curto, preso, franja, grisalho e assimétrico. Grisalho usa região de material da própria malha, sem volumes colados nas têmporas. O perfil sem cabelo continua sem cabelo. O coque permanece ligado à nuca e muda de posição com a cobertura.

Bonés têm abertura traseira arqueada acima do ajuste; o cabelo aparece por ela e o coque pode sair nessa região. Toucas deixam o coque abaixo da dobra. Na prévia, o botão Cabelo permite comparar os seis cortes usando a mesma roupa/peça de cabeça. Fora dela, o corte segue o perfil do cliente.

Validação percorreu 48 combinações de seis cabelos, quatro estados de cobertura e dois corpos. Os 30 encaixes com cabelo e peça de cabeça mantiveram cabelo exposto abaixo da cobertura, limites de espessura e coque unido à nuca. Cabeça esférica, expressões, tênis e campanha foram verificados novamente. Capturas reais estão listadas no README; esta rodada permanece para avaliação do usuário.

## Correções atuais — esfera, expressões, construções de streetwear

Diretriz explícita do usuário: cabeça ESFÉRICA, remoção da bolinha bege, expressões por pupilas/pálpebras/bocas, calçados estruturalmente distintos e bonés com melhor encaixe.

A cabeça tem diâmetro 1,12 nos três eixos. A raiz do rosto e as peças da cabeça compensam a escala não uniforme do corpo. A antiga animação que esticava a raiz foi retirada, pois deformava a esfera. O teste valida a distância de cada vértice ao centro em espaço mundial, inclusive ao trocar corpo e roupa com o avatar girado. A mecha bege separada do K7 foi eliminada.

Seis expressões funcionais: neutra, curiosa, desconfiada, sonolenta, animada e surpresa. Há variações de tamanho/forma/posição das pupilas projetadas na superfície do olho, pálpebras superiores/inferiores em malha curva, bocas projetadas na esfera e piscada pelas pálpebras. Clientes usam poses por perfil; na prévia um botão permite comparar todas. Não se trata ainda de uma atuação facial narrativa completa.

Três construções de tênis em StudioSneakers.cs: runner volumoso com entressola e tração separadas, suporte lateral e puxador; skate de cano médio com lona, cadarços cruzados e biqueira de borracha pertencente à própria malha; baixo de couro com perfil estreito, sobreposição da biqueira e sola fina de borracha. O teste compara as posições dos vértices, evitando aceitar apenas recolorações como modelos diferentes.

Bonés em StudioCaps.cs: copa com abertura inclinada acompanhando a esfera, seis painéis/costuras, ajuste traseiro e aba integrada. Neighborhood tem aba transversal curva; Independent tem copa mais alta e aba larga/reta. A revisão foi renderizada de frente e de perfil. A prévia abre todo o catálogo para avaliação.

## Direção atual — personagens arredondados

O usuário pediu aproximação maior da simplicidade arredondada de PEAK e YAPYAP, corrigindo a interpretação angular anterior. A implementação atual em teste usa cabeça/olhos arredondados, traços mínimos, mãos simples e membros com normais suaves. O aspecto retrô vem da resolução, texturas pontuais e cor, sem impor planos grandes à silhueta. O formato angular anterior e a seleção B permanecem como histórico de exploração, não como modelo obrigatório atual.

Cabelo acompanha a elipsoide da cabeça, com linha frontal e comprimentos por perfil. Sob boné/gorro, o volume de cabelo é omitido para evitar atravessamentos; óculos e bigode continuam. Boné tem copa, costura e aba unida; gorro tem malha e dobra; óculos usam aros e hastes curvos. Tênis usam formas longitudinais do calcanhar à ponta, sola com a mesma planta, colarinho e cadarços apoiados no cabedal. Cores/identidades das três famílias do catálogo foram mantidas.

Mapeamento de textura: UVs de superfície com escala física aproximada de 1,4 unidade por repetição; tampas usam projeção planar. Pele tem detalhe sutil estático; cabelo/couro/borracha/malha têm quatro atlas estáveis novos, com quatro tiles idênticos para compatibilidade com o shader. Pontos de amostragem ficam dentro do quadro do atlas. A iluminação suave é ativada nos personagens e peças; o ambiente mantém o tratamento anterior.

Corrigida a montagem com parentesco local: troca de roupa/cabelo/acessório não conserva giro ou escala do mundo anterior. Teste percorre 42 trocas em duas proporções e valida 247 superfícies com UVs não colapsados, além da escala de olhos durante expressões. Capturas reais e executável em `Builds/Windows-RoundedCharacters` estão listados no README. O resultado permanece para avaliação do usuário.

## Diretriz do usuário — 31/10/2026

Antes de ampliar o jogo, aproximar esta parte do GDD: visual PlayStation 1, personagens inspirados nas proporções e expressividade dos Miis do Wii, texturas animadas vibrantes. Storebound foi indicado como referência para a sensação de materiais vivos. Referência consultada: https://store.steampowered.com/app/3417410/Storebound/.

## Implementação desta rodada

- Avatar retrô inspirado na linguagem dos Miis: cabeça oval com queixo, frente mais plana, olhos pequenos com pupilas, nariz em cunha, boca e sobrancelhas simples, sem brilhos de emoji ou bochechas pintadas. Formato do rosto varia por perfil. Corpo mais estreito e mãos simples, preservando volume e cortes das roupas. Não utiliza assets da Nintendo.
- Malhas de roupas mantêm volume, cortes e categorias. Foram acrescentadas coordenadas UV e materiais de pintura, evitando a aparência de blocos plásticos sem textura.
- Seis atlas originais: tecido, reboco, papel, madeira, pele e colagem de fundo. Cada atlas tem quatro quadros de 64 × 64, organizados em 256 × 64, com filtragem pontual e sem mipmaps/compressão.
- Tecido e superfícies usam pequenas variações do mesmo desenho. Transições contínuas de 0,4 segundo entre quadros, com aceleração/desaceleração suave e ciclo sem corte. A paleta é quantizada antes da interpolação para evitar degraus temporais. Os atlas aprovados foram preservados. Pele e traços do rosto permanecem estáveis.
- Provador permanece preto/roxo, com colagem em violeta, verde e amarelo. Luzes decorativas não tingem as roupas. Não foram acrescentados tremor de vértices nem distorção de perspectiva no provador.
- A imagem 3D é renderizada em uma superfície de até 360 linhas e ampliada sem suavização. A interface é desenhada depois, na resolução da janela, preservando português, nomes, notas e cadeados.
- Movimento reduzido congela os quadros de pintura, piscadas e movimentos sutis. O filtro de resolução pode ser ligado/desligado na pausa.
- Fotos antigas podem ser regeneradas com a revisão visual, preservando notas, Reputação, inventário original e pergunta. Arquivos de imagem anteriores permanecem para compatibilidade com backups.

As texturas e a técnica são originais deste projeto. A implementação interpreta a referência de Storebound; não afirma reproduzir seu shader ou usar seus materiais.

## Revisão após retorno do usuário

O usuário esclareceu que deseja um avatar retrô que remeta à época dos Miis, com roupas que mantenham presença, sem reproduzir integralmente a silhueta Wii Sports. A versão anterior exagerava a cabeça redonda e mantinha as trocas de pintura a 6 fps; esses dois pontos foram revistos.

Foi pesquisado o shader externo Squigglevision, de tentabrobpy (CC0): https://godotshaders.com/shader/squigglevision/. Ele também utiliza saltos temporais, portanto não foi importado. A solução entregue continua no shader do projeto, com interpolação contínua. Referência visual Mii: https://www.nintendo.co.jp/wii/rspj/mii/index.html. Nenhum modelo externo foi incorporado.

## Escopo

### Teste atual: humanos simples com geometria retrô

O usuário aproximou a referência de YAPYAP/PEAK e pediu que a própria forma, além dos efeitos e texturas, remetesse a PS1/PS2. Após estudos de imagem, autorizou testar no jogo `Art/Concepts/teste-humanos-forma-retro-v1.png`.

A implementação experimental usa cabeça com planos amplos e quinas suavizadas por bevel geométrico, olhos grandes de oito lados, pupilas pequenas e rosto mínimo. Mãos com polegar, membros, orelhas e cabelo têm poucos volumes. Pele e cabelo permanecem estáticos e sem ruído pintado. As roupas preservam cores, critérios e estampas do catálogo. Cabeça, corpo e altura variam por perfil; a prévia permite comparar compacto/esguio. O enquadramento dá espaço aos modelos altos.

Capturas reais e executável estão indicados no README e no documento de direção da base humana. Esta é uma versão para avaliação do usuário, não arte final aprovada. A animação do fundo continua pendente conforme seu retorno e não foi retrabalhada nesta rodada.

Esta rodada é visual. Não acrescenta pedidos, dias, moedas, minigames ou outras mecânicas. Caminhada, loja física, animações completas e arte final da cidade continuam pendentes.

## Revisão atual — braços e postura (31/10/2026)

A forma atual já usa cabeça esférica e membros arredondados, conforme os retornos posteriores aos estudos acima. A pedido do usuário, os braços foram reconstruídos como superfícies contínuas do ombro ao punho. Cada manga segue a mesma curva do braço, entra no volume da blusa no ombro e termina com abertura, espessura e forro. Mangas curtas, largas e compridas mantêm folga ao redor da pele.

Cotovelos e punhos ficam levemente dobrados, com dedos recolhidos e polegares. O tronco inclina discretamente, o quadril desloca o peso, as pernas têm pequeno desnível de apoio em profundidade e os pés abrem em ângulos diferentes. Uma oscilação suave de espera move tronco/braços; cabeça e acessórios acompanham por translação para preservar a esfera mesmo com escalas diferentes de corpo. Pausa e movimento reduzido são respeitados.

Conferência nativa: `Captures/relaxed-posture-review.png` tem três colunas (camiseta regular, larga e manga longa) e três linhas (frente compacta, perfil esguio, costas compactas). Capturas do provador: `relaxed-short-game.png` e `relaxed-long-game.png`. A animação do fundo continua pendente do retorno anterior.

## Revisão posterior — mangas, gorro, identidades e fundo

O retorno seguinte pediu corrigir mangas, aproximar o gorro de uma peça de malha, ampliar a diferença entre os personagens e trocar a colagem por abstração preta/roxa descendente.

StudioShirts constrói o corpo da blusa com duas aberturas laterais; as raízes das mangas usam os mesmos pontos de contorno. Mangas mais estreitas substituem os volumes arredondados sobrepostos. A pele coberta não é renderizada, mantendo uma pequena sobreposição por dentro da barra. Esta revisão conserva cotovelos e mãos relaxados.

O gorro tem dobra de 0,21 de altura, topo mais alto e deslocado para o lado/trás, nervuras e etiqueta frontal. O perfil sem cabelo permanece sem cabelo. Os demais perfis distinguem cabelo curto ondulado escuro, coque ruivo, franja escura, grisalho e corte loiro assimétrico. Os seis tons de pele são distintos; as expressões iniciais percorrem curiosa, animada, desconfiada, sonolenta, neutra e surpresa. Retornos de um mesmo personagem mantêm sua identidade.

O fundo agora usa `AbstractFlow.png`, textura original de faixas orgânicas pretas e violetas, com repetição vertical e deslocamento contínuo de UV. Não há troca de quadros nem colagem verde/amarela nesse painel. O ciclo dura aproximadamente 15,38 segundos; as formas descem 6,5% da altura por segundo. A textura usa amostragem bilinear para suavizar o movimento, enquanto a composição 3D mantém a resolução retrô. Pausa congela o tempo; movimento reduzido exibe um quadro estático.

Capturas nativas: sleeves-integrated-review.png e sleeves-junction-detail.png (regular/larga/comprida em frente/perfil/costas), soft-beanie-review.png (três ângulos), characters-identity-review.png (seis rostos) e refinement-final-game.png. Nenhum modelo ou textura externo foi incorporado nesta revisão.

## Primeiro trecho da cidade — Praça do Encontro

Após aprovar os personagens como base por enquanto, o usuário autorizou começar pela fachada da DripLab, calçada e parte da praça. O bairro inicial usa creme, terracota, verde-azulado e carvão/violeta, com azulejos, piso português em ondas, marquises, vitrines, venezianas, laje, caixa-d'água, bancos e vegetação tropical. Os volumes têm quinas chanfradas, molduras e fachadas em camadas. Há uma grande etiqueta na loja como marco ligado ao streetwear, sem transformar o bairro inteiro em objetos de moda gigantes.

A vista isométrica e o funcionário em escala permitem avaliar a relação entre pessoas, portas, bancos e edifícios. Dois moradores usam cópias das malhas aprovadas; o player usa seu uniforme. Toldos e copa da árvore foram elevados após inspeção de perto para melhorar a folga e a visibilidade. Imagem 3D permanece com filtro retrô; letreiros do mundo são geometria de letras pixeladas e a UI continua nítida.

CityStone.png é textura original estática de pedras em ondas; o shader urbano usa cores por vértice, luz em faixas e pintura interpolada nas demais superfícies. As sombras do piso são estilizadas, sem pretensão de iluminação final. Modelagem, letreiros e textura desta fatia foram produzidos no projeto; não foram importados modelos externos nem redesenhado o logotipo oficial da marca.

Capturas reais do jogo: city-square-final.png, city-driplab-final.png, city-cafe-final.png e city-entry-menu.png. Esta fatia segue para avaliação visual do usuário. Interiores conectados, percurso da casa, rotinas de NPCs e monumento distante continuam fora deste primeiro trecho.

## Direção posterior — primeira pessoa e pixelização leve

Usuário pediu primeira pessoa, citou Anger Foot e forneceu um vídeo como referência exclusivamente de estilo gráfico. Foram examinados cinco quadros distribuídos no arquivo local. Leitura adotada: malhas estilizadas simples, cores em áreas amplas, detalhe de textura visível nos materiais e resolução suficiente para ler superfícies/expressões. O tema, personagens, objetos, textos e ações do vídeo não orientaram o conteúdo do jogo. Página oficial consultada como referência complementar: https://store.steampowered.com/app/1978590/Anger_Foot/.

O passeio usa câmera à altura dos olhos e mantém os personagens aprovados como NPCs; o funcionário é ocultado apenas nessa vista. A cidade continua com identidade brasileira, sem conversão do cenário ao ambiente do vídeo. Não foram importados assets das referências. A renderização até 720 linhas reduz os blocos da imagem anterior; as texturas continuam estilizadas. O piso possui tiles de maior definição com mipmaps, e a força de detalhe pintado foi reduzida nas outras superfícies.

Conferência em renderizações reais do executável: Captures/FirstPerson/first-person-driplab.png, first-person-cafe.png, first-person-character.png e first-person-pavement.png. São imagens da câmera 3D, sem a sobreposição da interface. A aprovação visual segue para o usuário; o bairro continua sendo o primeiro trecho externo.


## Bairro — céu, identidade DripLab e fantasia arquitetônica

Revisão solicitada após o primeiro trecho ter parecido sem vida e convencional. Consultado o chat INTERFACE (01a19354-2b02-7820-80c9-f24fec20eeb4): preto e branco como base e Vermelho Kaos #E30613 como destaque. A cidade aplica essa hierarquia na loja, etiqueta, marquises, cartazes e bandeiras; vegetação e construções vizinhas preservam verdes e tons terrosos de apoio. Não foi redesenhado o logotipo oficial nem implementada a interface planejada naquele chat.

Anger Foot passa a ser referência geral de direção gráfica: contraste forte, cores expressivas e formas exageradas, mantendo o bairro brasileiro e os personagens aprovados. Skybox original em shader: horizonte quente, céu verde-azulado, disco solar e nuvens arredondadas com deslocamento contínuo. Não há imagens nem modelos extraídos da referência. Pixelização leve em 720 linhas preservada.

Loja com volumes deslocados, pavimento superior em balanço, cobertura curva com borda costurada e grande etiqueta com ilhós e código gráfico. Café com terraço curvo, vegetação e vitrines com luminárias; rádio com fachada de ondas e cobertura dentada. Prédios em camadas e ruas laterais substituem o fundo de caixas cinzentas. Monumento distante original: fita tecida e aro inclinado, inacessível. Bandeiras ao vento e pequenos movimentos de postura dos moradores dão atividade ambiente; não são rotinas completas de NPCs.

Pausa e opção de movimento reduzido congelam céu, bandeiras e postura ambiente. O material de céu é exclusivo da câmera durante o passeio; flags, material e estado anteriores são restaurados ao sair. Capturas nativas em Captures/CityIdentity. Continua sendo uma fatia externa para avaliar direção visual, sem interiores conectados ou percurso casa–trabalho completo.


## Revisão — bairro à noite e texturas de PS1/PS2

Substitui a direção de fim de tarde anterior: usuário pediu jogo noturno, retirou o azul como cor do bairro e pediu superfícies com mais caráter retro. Céu quase preto com lua, estrelas discretas e nuvens escuras, fachadas de carvão/reboco/terracota, Vermelho Kaos e luzes quentes. Reflexos de vidro também passam a âmbar. Vegetação continua verde; não há tonalização azul generalizada.

Novo atlas original de reboco gasto, tijolo, metal com ferrugem e asfalto, gerado pela ferramenta integrada de imagens; arquivos e prompt em Art/CityNight/README.md. Importação em 512, mipmaps e point; amostragem quantizada por material para o aspecto de jogos antigos sem aumentar a pixelização da tela. Variação de luz estilizada por posição permite ler roupas e piso à noite. Janela, letreiro e luminárias recebem emissão; não são luzes físicas nem iluminação global final. Personagens aprovados preservados.

Quatro moradores: três com trajetos, paradas, mudança de direção e ciclo articulado de braços, pernas e mãos; um gesticula diante do café. Moradores param diante do jogador e de outros moradores. Colisões acompanham a posição atual dos NPCs. Carro atravessa a rua e para diante do jogador; painel da rádio anima barras, bandeiras ondulam e vegetação recebe vento leve. Pausa/movimento reduzido congelam esta atividade. Ainda não há rotina complexa de lojas, diálogo de rua, tráfego sistêmico ou interiores conectados.

Evidência nativa do executável: Captures/CityNight, incluindo quadros de animação, noite da loja, café, piso e trânsito. Prévia em vídeo é montada apenas desses quadros renderizados pelo Unity, sem alterar a aparência do jogo.


## Direção aprovada — materialidade retro e base compacta

Preferência permanente do usuário: manter o tipo de textura aprovado no bairro noturno. Superfícies precisam ter detalhe de material, desgaste e variação legível em baixa resolução, como reboco, tijolo, metal e asfalto atuais; evitar voltar a grandes superfícies lisas. AmbientCG está autorizado como fonte opcional de materiais. Não houve importação de AmbientCG nesta revisão; registrar fonte/licença quando for usado.

Única base corporal ativa: compacta (.98/.94/1), cabeça esférica e pescoço curto. Removidas as elevações de cabeça e as proporções esguias por perfil; a opção de alternar corpo saiu da experimentação. Tons de pele, cabelo, roupas e expressões permanecem diversos. Os antigos flags de comparação ficam compatíveis com os checks existentes, mas ambos geram a mesma base compacta.

Caminhada passa a seguir a distância percorrida: aceleração/parada suaves, giro antes do deslocamento, apoio dos pés e flexão de joelho por dois segmentos, pés rígidos e balanço menor dos braços com a junção da manga ancorada. O tronco acompanha uma flexão mínima; não há giro oscilante do corpo inteiro em repouso.

Carro ampliado em comprimento, largura e altura; cabine agora tem proporção mais próxima dos moradores. Colisão, distância de parada e rotação das rodas acompanham a nova escala.

Usuário confirmou que a mudança de textos se aplica a letreiros e cartazes da cidade. Substituídas as letras formadas por pequenos quadrados de geometria por atlas CitySigns.png, com tipografia Impact rasterizada e pequenas falhas de tinta. Uma superfície por texto, recorte alpha e mipmaps; iluminação dos letreiros preservada. Os menus não foram alterados. É lettering do cenário, não uma recriação do logotipo oficial.


## Personagem do jogador e cabelo com volume

Criação funcional usa exclusivamente a base compacta de pescoço curto. Identidade é escolhida por pele, cabelo, cor e expressão; os atributos do jogador não alteram os NPCs. Cabelo preserva o material retro e ganha volume na casca e mechas curvas, evitando fios finos realistas. Movimento das pontas é discreto e respeita pausa/movimento reduzido. Bonés/gorros mantêm o encaixe anterior com cabelo recolhido.

A animação dos braços combina ombro e antebraço com flexão suave de cotovelo, em vez de giro único do braço. Calçado e abertura inferior da perna compartilham a mesma transformação durante o passo. O acabamento visual continua sujeito à avaliação do usuário.


## Revisão dos cabelos — formas contínuas

Após o usuário considerar os cabelos estranhos, retiradas as mechas tubulares sobre a casca. O cabelo agora usa volumes integrados à mesma superfície: cachos em lobos largos, topo/franja assimétricos e sulcos discretos de cor no material. Coque permanece conectado; raspado preservado. Não há fios grossos isolados nem animação de tiras. A superfície é mais suave e não usa o contorno invertido que provocava traços pretos nas concavidades.

Corrigida a interpretação de Mathf.SmoothStep no taper da casca: os limites do intervalo agora usam InverseLerp antes de SmoothStep(0,1,t). Uma distância mínima geométrica mantém a casca fora do crânio. Bonés/gorros conservam a casca recolhida. Retirada a faixa lateral branca imposta no corte grisalho; cores do criador passam a cobrir o cabelo inteiro.

Capturas comparáveis: seis cortes, sempre com a mesma cor e iluminação, de frente/perfil/costas, em Captures/HairRefined. A avaliação estética continua pelo usuário; testes de encaixe e criação acompanham a revisão.


## Escala dos edifícios — referência na loja aprovada

Usuário aprovou a proporção da DripLab e pediu revisão dos demais prédios. Fachada, pavimentos, cobertura e etiqueta da loja permanecem sem alterações nesta etapa. Os prédios cenográficos passam a compartilhar térreo de 4,7 e pavimentos de 4,1 unidades, portas próximas de quatro unidades de altura e janelas de 2,05×2,1; moradores têm aproximadamente 3,2 de altura. Largura/profundidade dos módulos, espaçamento entre fachadas e detalhes da cobertura também foram revistos.

Prédios de fundo usam três a cinco pavimentos legíveis em vez de comprimir muitos andares em caixas estreitas. Laterais usam dois/três andares, com esquadrias, faixas de laje e algumas sacadas como referências de escala. Mantidos os materiais retro e a paleta noturna. Calçadas/rua cenográfica acompanham os novos volumes.

Café e rádio recebem pavimento superior mais alto e janelas compatíveis; terraço/jardineiras do café e torre/cobertura da rádio foram reposicionados para acompanhar essa altura. Térreos comerciais e circulação da praça preservados. Prédios de fundo continuam cenográficos: não representam novos interiores ou áreas acessíveis.


## Expressões, linha do cabelo e imagem nítida

Usuário preferiu as capturas diretas sem pixelização adicional. Filtro passa a desligado por padrão; opção continua no menu de pausa. Migração única de preferências visuais para versão 2 aplica a nova preferência aos saves existentes e depois preserva as escolhas manuais. Materiais, paleta e shaders retro permanecem. Provas automáticas não gravam preferências do usuário.

Franja e Assimétrico têm topo menos pontudo, volumes mais baixos e linhas frontais próprias. Todos os cabelos recebem uma região frontal de folga para as sobrancelhas. Sobrancelhas agora são formas fechadas com arco, corpo e pontas afiladas, ajustadas por expressão e projetadas na cabeça; não são o antigo traço uniforme.

Movimento reduzido congela efeitos decorativos (nuvens/vento/painel), mas não paralisa moradores e trânsito. Pausa explícita continua parando a simulação. Na cidade, perder foco solta o mouse e desabilita entrada do jogador sem congelar o bairro; atendimentos cronometrados mantêm pausa por perda de foco.

## Materiais externos ambientCG — 01/11/2026

Incorporados oito mapas CC0 de madeira, tecido, metal, concreto, pintura, tijolos, papelão e asfalto. Fontes e licença em Art/AmbientCG/CREDITS.md. O detalhe vem das superfícies reais, com importação 256/512, mipmaps e adaptação da luminância à paleta; não se pretende trocar a direção retro por um acabamento PBR realista. Madeira deve manter veios contínuos sobre tampos e peças arredondadas; metais não recebem textura de madeira. Tecido acompanha UVs das roupas/cortinas, sem contaminar pele ou cabelo. Calçada portuguesa externa preservada, concreto reservado às placas do piso interno da loja. Filtro de pixelização permanece desligado por padrão.
