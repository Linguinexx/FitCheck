# Web Teste 01 — 11/11/2026

Unity 6000.6.7f1, WebGL/WebAssembly IL2CPP, WebGL 2, sem threads. Player comprimido em gzip com fallback de descompressão. Relatório físico e hashes em delivery.json; o total do BuildReport da Unity contabiliza somente parte da entrega web e não representa o tamanho da pasta.

## Conferências executadas

- Compilação final: `tools/web01-pointer-retry.log`, `FITCHECK_WEB_BUILD: Succeeded`, zero erros de compilação. O paralelismo de Emscripten/Binaryen foi limitado a dois processos após uma tentativa ficar sem espaço temporário.
- Edge/Chromium automatizado, viewport 1440×900, perfil de QA separado. Renderização por SwiftShader: este teste não mede FPS da GPU do usuário.
- Menu animado, seleção dos três espaços, criação de Web QA com Black Power e cor cobre. Reabertura do navegador e recarregamento restauraram nome, cabelo e campanha no primeiro espaço; os outros dois permaneceram vazios.
- Entrada na casa em primeira pessoa, captura real do ponteiro após clique (`document.pointerLockElement` presente), caminhada, carinho em Fubá e pausa. Saída física pela porta carregou o bairro. Evidências em Captures/Web01.
- IndexedDB `/idbfs`: aparência, campanha e cópia `.bak` presentes após a interação. Confirma gravação inicial e substituição de arquivo existente na plataforma web, sem tocar nos saves Windows.
- Correção de stripping: MeshFilter/MeshRenderer e colliders de primitivas explicitamente preservados em link.xml. A versão corrigida não apresenta os erros de BoxCollider encontrados na primeira tentativa.
- Pedido de mouse recusado por falta de foco não interrompe o player: Promise tratada no template; o jogo solicita captura em resposta a novo clique. A recusa foi reproduzida no teste sem foco, seguida de aquisição bem-sucedida com foco ativo.
- ZIP conferido com teste de integridade, index.html na raiz e limites de 200 MB para pasta e ZIP. Nenhum perfil de navegador, save, cache Unity ou executável Windows incluído.
- Windows Teste 08 preservado: SHA256 `bd3b65967b1f4c2ed1e38c1c70e355cf7ffc56ed9b78bd6506b10fac59a5bd48`.

## Limites desta validação

A campanha completa e a loja foram verificadas na entrega nativa Teste 08; não houve nova partida completa dos sete dias no navegador. Não foram medidos FPS em GPU física, testados celular/Safari/Firefox ou hospedagem pública. O teste usa servidor HTTP local. O botão Sair do menu não fecha abas por restrição do navegador; fechar a aba encerra a sessão.

## Reprodução

Use Editor/WebPlayerBuild.cs em uma cópia isolada com o módulo Web instalado. Copie o template do projeto junto com Assets; use manifests portáveis e preserve os manifests locais de MCP. Depois execute `package_delivery.py` desta pasta. O servidor local serve.py deve ficar junto do index.html.

Para hospedar em iframe, a plataforma precisa permitir captura do ponteiro e tela cheia. A build não depende de cabeçalhos de isolamento para threads. Saves são por navegador/origem, separados da versão Windows.

## Espaço em disco

A Unity 6000.6.0f1 sem uso foi removida pelo desinstalador oficial após confirmação do usuário, liberando cerca de 8,4 GiB para instalar o módulo Web da 6000.6.7f1. A versão atual e a Unity aberta do projeto foram preservadas. Cópias web malsucedidas e o projeto temporário de compilação são descartáveis; referências, fontes e saves não fazem parte dessa limpeza.

Limpeza final concluída: 906.732.859 bytes lógicos em duas cópias web intermediárias e tools/.web-build-project. Relatório em tools/web01-cleanup.json. O módulo instalado permanece necessário para recompilar a versão web.
