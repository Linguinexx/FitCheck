> Atualização: o usuário autorizou testar estes acabamentos no jogo em 04/11/2026. A integração está na versão GarmentFinishes, com opção para desligar. O restante deste documento registra a prévia original, anterior à autorização.

# Proposta de materiais — aguardando aprovação

Prévia isolada, criada em 04/11/2026. Nenhuma alteração foi aplicada aos Assets do jogo, ao executável publicado, aos modelos ou aos saves. Os hashes de todos os arquivos de Assets foram comparados antes/depois e permaneceram iguais.

## Mesma modelagem, materiais diferentes

`same-model-textures.png`, da esquerda para a direita:

1. Camiseta ampla atual, clara com estampa pequena.
2. Exatamente a mesma malha, com a textura azul e estampa de outra peça.
3. A mesma malha com tecido pronto Fabric030 do ambientCG, com ajuste de cor/escala.

Os três avatares mantêm 23.126 vértices ativos. Isso demonstra reaproveitamento dentro de uma família de corte; polos, moletons, jaquetas e calças de cortes distintos ainda devem ter bases adequadas à silhueta. O ensaio não troca automaticamente todos os modelos por uma única base.

Fabric030 já estava embarcado no projeto, importado em 512px. Fonte: https://ambientcg.com/a/Fabric030 — CC0, conforme https://docs.ambientcg.com/license/. Fonte e licença também constam em Art/AmbientCG/CREDITS.md. Não foram comprados nem baixados pacotes novos para esta comparação.

## Efeitos

`effects-before-after.png`: coluna esquerda atual, coluna direita proposta. Linha superior: estrela com acabamento cromado estilizado; letras e algodão preservados. Linha inferior: acabamento acetinado/nylon na peça azul.

`material-preview.mp4`: quatro segundos, 24 quadros/s, primeiro cromado e depois acetinado, com rotação de apresentação para observar o reflexo. A taxa do vídeo não representa a taxa de quadros do jogo.

O protótipo usa um passe, a mesma geometria e reflexos estilizados por direção de visão. Não captura reflexões reais do cenário, não usa câmera extra, transparência ou simulação de tecido. O cromado é uma proposta artística, não um material PBR com reflexão física. Se aprovado, o desempenho deve ser verificado em cenas reais com a quantidade prevista de roupas e NPCs; estas capturas não são uma garantia de FPS.

## Arquivos

O shader e a rotina de captura estão em `prototype-source/`, fora de Assets. Compilação e captura ocorreram exclusivamente em `tools/.city-build-project`; os pontos de entrada de teste dessa cópia foram restaurados após as capturas. A versão atual publicada permanece WardrobeFinish, com EntryOptimized como anterior recuperável.
