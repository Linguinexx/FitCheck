# Base humana B — desenvolvimento visual

## Decisão do usuário

Após explorar humanoides, o usuário corrigiu a direção para humanos estilizados e escolheu B na prancha `exploracao-humanos-abc-v1.png`. A referência Mii isolada não atendia à identidade desejada. YAPYAP e PEAK foram citados como referências de identidade própria e reconhecimento visual, não como modelos a copiar.

## Características a preservar

- Cabeça larga e compacta com mandíbula marcada, orelhas pronunciadas e nariz curto.
- Olhos horizontais com branco e pupila, sobrancelhas grossas e boca expressiva.
- Corpo compacto, ombros com espaço para camisetas largas, mãos humanas simplificadas e tênis grandes.
- Identidade reconhecível por silhueta e expressão. As roupas continuam centrais ao jogo.
- Cabelo e tom de pele da prancha exemplificam o estilo; não limitam a diversidade do elenco.

## Estado

### Refino aprovado para trabalho: cabelo e junção

Após gostar das expressões, o usuário pediu foco em cabelo e encaixe com bonés/toucas. Cabelo deixa de ser removido integralmente sob acessórios: faixa de cabelo conserva laterais/nuca e recolhe o volume coberto. Franja/volume lateral integrados à malha, grisalho como região de material, abertura traseira do boné e coque reposicionado conforme a peça. Novo botão Cabelo na prévia. Capturas atuais: `hair-fit-short-review.png`, `hair-fit-tied-review.png`, `hair-cuts-review.png` e `hair-fit-game-final.png`. Versão: `Builds/Windows-HairFit/FitCheck.exe`.

### Cinco correções solicitadas após a versão arredondada

Cabeça agora é esfera de raio 0,56, com compensação da escala do corpo e sem esticamento de idle; mecha/bolinha bege separada removida; seis expressões combinam pupilas, pálpebras e bocas. Os três tênis têm geometrias/construções distintas e os dois bonés têm abertura ajustada à esfera, painéis, fecho e abas diferentes.

Referências atuais: capturas reais `sphere-character-final.png`, `sphere-expressions-review.png` (ordem: neutra, curiosa, desconfiada / sonolenta, animada, surpresa), `streetwear-shoes-review.png` (runner / skate médio / baixo de couro) e `caps-construction-review.png` (aba curva à esquerda / aba reta à direita). Executável de teste: `Builds/Windows-SphereStreetwear/FitCheck.exe`. A bolinha removida não deve ser reintroduzida como detalhe de cabelo.

### Nova orientação: redondinho, mais próximo de PEAK/YAPYAP

Após o teste angular no jogo, o usuário pediu personagens bem mais parecidos com a linguagem arredondada de PEAK/YAPYAP e melhor aplicação de texturas, destacando problemas em cabelo, acessórios e tênis. Essa orientação substitui a necessidade de planos angulares grandes do teste anterior.

Implementação atual: `StudioSurfaces.cs`, `StudioDetails.cs`, `StudioFace.cs` e `StudioView.cs`. Cabeça e olhos arredondados, cabelo ajustado à superfície, peças e tênis reconstruídos, UVs corrigidos e materiais estáveis por tipo de superfície. Correção de parentesco local evita desalinhamentos ao trocar peças depois de girar. Referências atuais são as capturas reais `Captures/rounded-*-final.png`, não a prancha angular antiga. Executável em `Builds/Windows-RoundedCharacters/FitCheck.exe`. Ainda é uma revisão para teste.

### Teste no jogo autorizado

Após o estudo `teste-humanos-forma-retro-v1.png`, o usuário autorizou experimentar essa direção no jogo. Implementação de teste em `StudioFace.cs` e `StudioView.cs`: cabeça com planos/bevels próprios, olhos grandes octogonais com pupilas, nariz em cunha e boca mínima; pele lisa sem movimento; cabelo, mãos e membros com geometria econômica. Proporções distintas por perfil, com roupas e acessórios adaptados ao conjunto.

Na prévia isolada, o botão de proporções permite comparar compacto/esguio com o mesmo visual. A saída limpa a escolha de teste. A autorização é para avaliar no jogo, não aprovação definitiva do modelo. Capturas reais: `Captures/retro-human-compact-final.png` e `Captures/retro-human-tall-final.png`. Executável: `Builds/Windows-RetroHumans/FitCheck.exe`.

Este teste não altera o shader/texturas do ambiente. Fotos antigas da revisão 3 ficam preservadas durante a avaliação; não foi incrementada a revisão global de fotos para forçar regeneração da galeria. Novas fotos e prévias usam o modelo de teste.

### Refinamento: referência retrô na própria forma

O usuário disse que a linguagem humana simplificada aproxima da intenção, mas falta a vibe retrô nos personagens em si, além das futuras texturas e efeitos PS1/PS2. Isso é aproximação parcial, não aprovação do modelo final.

`teste-humanos-forma-retro-v1.png` explora a mesma dupla com planos maiores na cabeça e nos membros, mandíbula e cabelo mais angulares, mãos econômicas e volumes de roupas simplificados. Os olhos presentes, rostos mínimos e diferenças de proporção permanecem. Cores lisas permitem avaliar a contribuição da geometria sem filtro retrô. É uma ilustração de estudo, não uma malha já construída ou evidência de contagem de polígonos.

### Retorno posterior: direção ainda distante da referência

Após o estudo de silhuetas, o usuário disse que o estilo ainda estava bem longe de friendslop como YAPYAP e PEAK. A escolha anterior de B não deve ser tratada como aprovação final da linguagem: a caricatura angular elaborada continua em revisão. Não avançar a modelagem de todo o elenco com base nela.

Novo teste `teste-humanos-simples-v1.png`: humanos com poucos volumes, olhos brancos mais presentes, boca simples, mãos simplificadas e posturas desajeitadas, em duas proporções. Estudo exploratório gerado por imagem, sem aprovação do usuário e sem implementação. O acabamento da imagem continua mais suave que o alvo retrô; não constitui textura final.

### Correção de escopo do usuário

O usuário gostou do desenvolvimento B, mas esclareceu que seu formato não deve ser o único. B orienta a linguagem gráfica; não é uma anatomia obrigatória para todo o elenco. Variar altura, volume do corpo, formato de cabeça/mandíbula, nariz, proporções dos membros e postura, preservando a simplificação de formas, construção de olhos, nível de detalhe e tratamento visual compartilhados. A restrição do GDD a um editor de proporções do player não impede diversidade autoral entre os NPCs.

`variacao-silhuetas-humanas-v1.png` explora quatro exemplos com roupas neutras iguais: compacto, alto/esguio, largo e intermediário. Trata-se de estudo gerado por imagem, não de elenco final aprovado ou malhas implementadas. Os exemplos não estão atribuídos aos clientes do GDD. A diversidade deve alcançar rostos, idades e corpos sem atrelar personalidade a um tipo físico.

`base-humana-b-turnaround-v1.png` desenvolve a escolha em frente, perfil, costas e expressões. É uma ilustração de referência, ainda sem malha 3D ou rig. O turnaround é aproximado: a modelagem deverá resolver a coerência entre vistas. A aparência facetada ilustrada não exige reproduzir manchas triangulares na textura final.

Não houve substituição dos personagens do jogo nesta etapa. Animação de textura permanece pendente conforme retorno do usuário, especialmente o fundo do provador.

## Geração

Ferramenta integrada de geração de imagens, usando a prancha humana anterior como referência. Prompt final registrado abaixo.

```text
Use case: stylized-concept. Input image role: reference board, ONLY the CENTER human character labeled B is the approved design. Ignore A and C entirely. Create a model-development turnaround sheet for this exact selected B character for DripLab, a retro streetwear fashion game. Preserve the selected B's identity: young-adult HUMAN with warm medium-brown skin, broad squat softly angular rectangular head, short strongly angular dark haircut with asymmetrical fringe, pronounced human ears, thick straight dark eyebrows, wide horizontal white eyes with dark pupils, small broad projecting human nose, small offset skeptical mouth; compact short human body, broad relaxed shoulders, sturdy forearms, simplified human hands, loose pants and large simple sneakers. Maintain the source B's actual proportions (compact, head around one quarter of height), not the tall thin bodies of A or C; no Mii oval head, no baby, no creature, no pointed ears, no overly realistic rendering. Important: preserve the selected reference face and cartoon design, not a generic attractive adult man. This is a human game avatar base, age-neutral adult styling, no beard, no jewelry, no accessory. Model sheet with THREE LARGE equal-height orthographic full-body views FRONT, SIDE, BACK, aligned at ground, hips, shoulders and crown; feet and crown fully visible. Straight neutral relaxed A-pose in all views, arms 20 degrees away from torso, feet planted and separated. Front view must face camera squarely and be symmetrical enough to model; preserve asymmetrical fringe and distinctive face. SIDE means strict 90-degree profile, so nose depth, ear location, head back shape and neck attachment are clear. BACK shows coherent matching haircut and shoulder/body structure. Each wears identical plain off-white slightly boxy oversized short-sleeved tee with no logo, charcoal loose straight trousers and chunky off-white low-top sneakers. No hands in pockets: show simplified human palms, thumb and joined-finger silhouette. Below the three body views, place THREE small front-facing head closeups of the SAME character with expressions: NEUTRO, DESCONFIADO, ANIMADO. Keep head proportions, ears and hairstyle consistent across expressions; vary eyebrows, eyelids and mouth only. Text labels verbatim: 'B — MODELO BASE', 'FRENTE', 'PERFIL', 'COSTAS', 'NEUTRO', 'DESCONFIADO', 'ANIMADO'. Small clean black typography with generous space; no other captions or decoration. Neutral pale warm-gray background, quiet flat diffuse lighting, clear graphic silhouette, matte broad simplified geometry, subtle low-resolution painted surfaces suitable for PS1-inspired production, NO crumpled triangle noise over skin, NO glossy PBR, NO plush/felt/knit appearance, NO photoreal fabric, NO dramatic lighting, NO environment, NO hats. Preserve uncluttered wearable garment silhouette. Do not add a wireframe, fake polygon counts, measurements, invented logos or in-game UI. This is a concept turnaround illustration for human review, not a claim of an existing mesh. Prioritize internal view consistency and accurate preservation of the center B from the supplied image over prettifying the design.
```
