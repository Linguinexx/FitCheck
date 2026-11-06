# Fit Check — Teste 01 para Windows

Primeira entrega organizada para teste do conjunto, solicitada em 05/11/2026. Usa o estado atual do jogo; trajes comuns do player, decoração do quarto, nova UI do FitFeed e áudio definitivo continuam pendentes. Não é uma declaração de versão final.

Artefatos locais: Builds/Windows-Test01/FitCheck.exe e Builds/FitCheck-Windows-Test01.zip. O pacote inclui LEIA-ME.txt, roteiro, modelo de bug, VERSAO.json e créditos/licenças. Não distribuir o executável isolado.

Preservar companyName/productName do projeto para manter a identificação do aplicativo e seus saves. A edição Teste 01 é identificada no pacote; não migra nem apaga progresso. Não incluir saves, Captures, caches, integrações ou fontes no ZIP.

## Reprodução

1. Sincronizar Assets para o projeto isolado tools/.city-build-project, preservando a configuração local do Editor.
2. Compilar com Unity 6000.6.7f1: FitCheck.Editor.CitySliceBuild.Build, saída Windows-Test01.
3. Executar o binário com `-batchmode -force-d3d11 -screen-width 1440 -screen-height 900 -screen-fullscreen 0 -city-proof -test-build-review -proof-output <Captures/Test01> -logFile <tools/test01-proof.log>`.
4. Exigir saída 0 e marcador TEST_BUILD_01_PASSED em Captures/Test01/checks.txt. A rotina usa armazenamento de teste isolado e cobre bairro/rota, decoração, gato/miado, tênis civis e da loja, 63 combinações de roupa, oito loadings, casa/loja, atendimento e saves.
5. Rodar `prepare_delivery.py` desta pasta para adicionar instruções e metadados ao build isolado.
6. Rodar `tools/envs/unity/Scripts/python.exe tools/package_body_motion.py Test01` a partir da raiz TCC3D. Verifica igualdade dos Assets, cópia do build e CRC do ZIP.
7. Gerar checksum SHA256 do ZIP. Após validação, manter esta versão e uma anterior recuperável.

Logs de compilação final: tools/test01-final-build.log. Revisão: tools/test01-proof.log. Imagens: Captures/Test01. Relatório manual do grupo usa o modelo incluído. A validação automatizada não substitui o teste do grupo em outros computadores nem define requisitos mínimos de hardware.
