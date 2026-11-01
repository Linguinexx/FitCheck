# Fit Check by DripLab

Jogo 3D de montagem de looks streetwear em Unity **6000.6.7f1**. Arte em desenvolvimento: personagens humanos estilizados, cenário noturno e texturas inspiradas em PS1/PS2. A pixelização é opcional e começa desativada.

## Abrir e jogar

1. Adicione esta pasta ao Unity Hub com a versão indicada.
2. Abra `Assets/Scenes/FirstCustomer.unity` e pressione **Play**.
3. Na tela da fachada, clique em **Jogar** e selecione um dos três saves. Um espaço vazio abre a criação do personagem; um save existente retoma sua semana em casa.
4. Em casa, escolha **Ir trabalhar**. Ao terminar um dia, feche o expediente e durma para começar o seguinte.

Cada save mantém aparência, campanha e fotos separadas em `saves/slot-1`, `slot-2` ou `slot-3`, dentro da pasta de dados persistentes da Unity. Na primeira seleção, o progresso da versão anterior é copiado para o primeiro espaço; os arquivos originais e as fotos antigas permanecem preservados. Não há sobrescrita ou exclusão de saves pela interface.

Na exploração, **WASD** move, **mouse** olha, **E** interage, **Esc** pausa e **Tab** libera ou captura o cursor. No Editor, dê foco à aba **Game**. Menus e provador usam o mouse livre.

O rapaz encostado na parede é um frequentador fixo da loja, separado dos atendimentos. O cliente entra, caminha até a frente do balcão e espera. Passe por trás do balcão e pressione **E** para atendê-lo. Após conversar e escolher uma pergunta opcional, ele caminha até o provador; a câmera se aproxima e a cortina abre antes da montagem do fit. Ao finalizar, o cliente sai usando o look e o resultado aparece no FitFeed. O frequentador permanece na loja.

A montagem acontece no próprio provador da loja. Arraste o personagem para girar e use a roda do mouse para aproximar ou afastar. O cliente vira-se ao chegar e apresenta o fit antes de sair. Pausa interrompe a sequência; cancelar e voltar ao início reinicia o atendimento sem finalizar um look.

O passeio no bairro é separado da loja nesta versão. A ligação física entre ambientes e as transições gerais ainda estão em desenvolvimento.

## Conteúdo atual

- Campanha de três dias, com nove pedidos e pontuação por ocasião, estilo e restrições.
- Catálogo de 21 peças em três drops, desbloqueios por Reputação e repetição de atendimentos.
- Criação do jogador com nome, tons de pele, cabelos, cores e expressões.
- Loja com araras, tênis, caixa e provador; bairro com moradores e veículos em movimento.
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

Foram incorporados oito mapas de cor do ambientCG, adaptados à direção de arte e com resolução de importação limitada. Fontes e licença CC0: [créditos ambientCG](Art/AmbientCG/CREDITS.md). Não foram incorporados modelos ou marcas externas de calçados.

As decisões e revisões anteriores estão em [DEVLOG.md](DEVLOG.md), [DIRECAO_ARTE.md](DIRECAO_ARTE.md) e [TESTES.md](TESTES.md).
