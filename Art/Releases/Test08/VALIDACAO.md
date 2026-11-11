# Validação — Teste 08

Entrega Windows x64 de 11/11/2026, Unity 6000.6.7f1, dados LZ4HC. Compilação sem erros ou avisos. Pasta com 138.722.620 bytes e ZIP com 76.627.523 bytes; ambos abaixo do limite de 200.000.000 bytes. Hash e relatório em delivery.json e no arquivo SHA-256 ao lado do ZIP.

Os arquivos de Assets usados na cópia de build foram comparados com o projeto principal: nenhuma diferença. VERSAO.json registra o commit-base e o hash dos assets, incluindo as alterações locais desta entrega. Company/product e os saves existentes foram preservados. O ZIP não contém saves, capturas, caches, referências ou integrações locais.

## Conferências

- Campanha de sete dias e 21 atendimentos: 2.100 RP, 32 peças, entregas, inventários históricos, repetição, migração e backup.
- Interface nativa do provador: After Hours bordô, Common Ground verde e Side B violeta; filtros, peças originais, seleção por categoria, testes reversíveis, entrega e replay. Mil consultas estáveis do provador: zero atualizações do cache e zero bytes alocados nessas consultas. Isso não é uma declaração de zero alocação da interface inteira.
- Fluxo nativo: percurso casa–bairro–loja, loja fechada até abertura manual, chegada livre, balcão, câmera do provador, montagem, saída, publicação, próximo cliente, cancelamento e dia seguinte.
- Oito loadings, movimento reduzido, bloqueio de comandos e três saves independentes. Marcadores `LOADING_REVIEW_PASSED` e `TEST_BUILD_01_PASSED` no teste final; o segundo é o nome histórico da suíte, aplicada ao executável do Teste 08.
- Materiais das roupas do catálogo corrigidos para não carregarem a identificação interna Civilian. Roupas de chegada continuam civis; o teste de troca do cliente passou sem relaxar suas verificações.
- CRC do ZIP atual e do ZIP anterior verificado. Executável anterior conferido contra a cópia arquivada antes de remover sua pasta extraída.

## Desempenho medido

Teste nativo em 1440 × 900, Direct3D 11, GTX 1650, limite de 60 FPS. Trinta quadros de aquecimento e 180 amostras por fase na loja.

| Situação | Mediana do quadro | P95 | Mediana de Update |
|---|---:|---:|---:|
| Parado | 16,667 ms | 16,697 ms | 2,559 ms |
| Andando | 16,667 ms | 16,668 ms | 2,587 ms |
| Andando e olhando para baixo | 16,667 ms | 16,681 ms | 4,549 ms |

Construção da primeira entrada: casa 91 ms, bairro 849 ms e loja 117 ms. Retornos entre 19 e 21 ms. Esses números medem a construção do destino; o loading mantém seus dois segundos. Não representam uma garantia para outros equipamentos.

Relatórios e capturas em Captures/Test08. Testes finais em tools/test08-final-*.log, fora da distribuição.

## Limpeza

Removidos cerca de 652 MiB: cópia temporária de compilação, Teste 07 extraído (ZIP validado preservado), ZIP do Teste 06, capturas da tentativa de fluxo substituída e bytecode Python regenerável. Ficaram Teste 08 extraído/ZIP e Teste 07 em ZIP. O cache do editor principal aberto foi preservado. Referências, originais de modelos, fontes Blender, ambiente de desenvolvimento e saves mantidos.
