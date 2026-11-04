# Acabamentos das roupas — teste autorizado em 04/11/2026

O usuário aprovou testar no jogo os materiais mostrados nas prévias. A versão GarmentFinishes mantém WardrobeFinish como anterior recuperável.

- Night Route (AH_TOP_02): cromado estilizado somente na estrela. Letras e restante do tecido ficam foscos.
- Meeting Point (CG_TOP_03): brilho acetinado suave no corpo, mangas e acabamentos, conforme a prévia. Efeito depende do ângulo de visão.
- Configurações do título e da pausa: **Brilho das roupas** liga/desliga os dois efeitos imediatamente. A preferência é global e persistente; não altera progresso ou aparência dos saves. Começa ligada nesta versão de teste. Fotos e miniaturas já capturadas são imagens estáticas.
- Last Stop e Night Route compartilham a mesma base geométrica/UV e as mesmas três malhas imutáveis em cache. Estampas, IDs, cores, materiais e critérios continuam independentes. Não foram unificadas todas as roupas da coleção; outras silhuetas continuam próprias. FBX/JSON editáveis originais são preservados; o alias desta família é explícito em GarmentFinishes.cs.
- O tecido pronto Fabric030 já licenciado é reutilizado com intensidade moderada na camiseta clara, preservando estampa e cor. Não foi criada uma peça extra no catálogo nem alterada a progressão de desbloqueios.
- Civis continuam usando roupas básicas próprias, sem os dois efeitos ou identificação da DripLab. Personagens mantêm os modelos e animações anteriores; o protótipo de esqueleto Blender não participa desta versão.

Os efeitos usam shader separado somente nas duas peças, um passe, as texturas existentes e geometria igual. O cromado é uma reflexão estilizada, sem captura real do cenário, câmera extra, transparência, simulação de pano ou luz emitida pela roupa. Desligar o brilho retorna ao tecido fosco nas peças 3D; a versão anterior também permanece disponível.

## Medições

Player Windows, Unity 6000.6.7f1, D3D11, GTX 1650. Mesmo executável e enquadramento, sequências desligado/ligado/ligado/desligado, seis aquecimentos e 24 amostras por rodada. Mediana entre rodadas. Cada amostra mede Camera.Render em 1280×720 e leitura síncrona de um pixel para aguardar a GPU.

| Cena | Desligado (ms) | Ligado (ms) | Diferença (ms) |
| --- | ---: | ---: | ---: |
| shop | 0.509 | 0.524 | +0.015 |
| foil-client | 0.516 | 0.515 | -0.001 |
| nylon-client | 0.849 | 0.817 | -0.033 |

A diferença observada foi pequena. Esses tempos de renderização e leitura de GPU não representam FPS completo, não incluem todo o processamento do jogo e não garantem desempenho igual em outro equipamento. O ensaio de entrada também passou: cidade com 411 renderers/1.089.420 vértices e loja com 178/189.524; atualização da cidade/loja permaneceu próxima de 4 ms. Nenhum mapa novo em alta resolução ou polígono adicional foi exigido pelos efeitos.

## Verificação

Compilação sem erros/avisos. `-garment-finishes-review`: shader restrito aos dois itens e um passe, troca global imediata, família compartilhada, civis independentes, comparação no provador real e configuração legível. Inclui 17 assets/63 fits nos sete perfis, costuras de mangas, piscadas esféricas, araras, atendimento completo, câmera/jogador no balcão, FitFeed e três saves isolados. Os testes não modificam as preferências nem os saves do usuário.

Capturas e relatórios locais: Captures/GarmentFinishes/in-shop-finishes.png, settings-finishes.png, render-performance.json, entry-performance.json e checks.txt. Logs tools/garment-*-release-proof.log.
