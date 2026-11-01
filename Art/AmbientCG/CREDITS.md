# Texturas ambientCG — Fit Check by DripLab

Fonte: ambientCG, Lennart Demes. Arquivos de material sob [Creative Commons CC0 1.0 Universal](https://docs.ambientcg.com/license/), permitindo uso, modificação e redistribuição no jogo. Não foram importados modelos, marcas ou HDRIs do site.

| Material | Fonte | Aplicação |
|---|---|---|
| Madeira | [Wood 051](https://ambientcg.com/a/Wood051) | Tampos, cabides, banco, araras e detalhes de madeira do bairro |
| Tecido | [Fabric 030](https://ambientcg.com/a/Fabric030) | Cortinas, assentos, roupas dobradas, toldos e detalhe das roupas dos personagens |
| Metal | [Metal 032](https://ambientcg.com/a/Metal032) | Ferragens e equipamentos internos |
| Concreto | [Concrete 034](https://ambientcg.com/a/Concrete034) | Placas do piso interno da loja |
| Pintura/reboco | [Painted Plaster 017](https://ambientcg.com/a/PaintedPlaster017) | Paredes internas da loja e base do apoio do cliente |
| Tijolos | [Bricks 074](https://ambientcg.com/a/Bricks074) | Alvenaria da loja e do bairro |
| Papelão | [Cardboard 001](https://ambientcg.com/a/Cardboard001) | Fibras e desgaste de embalagens, caixas e papel |
| Asfalto | [Asphalt 012](https://ambientcg.com/a/Asphalt012) | Ruas e pavimentos do bairro |

Importados somente os mapas Color dos pacotes 1K-JPG. Os ZIPs temporários e mapas PBR não utilizados não ficam no projeto. Fontes originais em `Assets/Resources/Surfaces`, com resolução de importação 256 (tecido: 512), mipmaps e repetição. O shader usa luminância normalizada para preservar a paleta dos objetos, com contraste e escala próprios por material. Não há novo filtro global de pixelização. Pele/cabelo/rostos não recebem a trama de tecido.

`catalog.json` preserva os metadados da API oficial; `sources.json` registra URLs, pacote de origem, licença e SHA-256 de cada arquivo importado. `statistics.json` registra dimensões e luminância média das fontes, usadas no ajuste de cor. Mapeamento de tampos e placas tem coordenadas contínuas para evitar veios em leque entre triângulos.
