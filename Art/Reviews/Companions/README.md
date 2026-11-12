# Companheiro, Fubá e enquadramento da decoração

Na criação/edição, abrir **Companheiro**. Escolher gato (SRD, Persa ou Siamês) ou cachorro (vira-lata, Shiba ou Dachshund), seis pelagens e nome de até 20 caracteres. Prévia giratória com corpo inteiro. Salvar personagem confirma as duas abas; voltar descarta o rascunho. Dados dentro de `player-appearance.json`, isolados por espaço; saves antigos recebem gato padrão Pipoca sem alterar campanha, inventário ou aparência humana.

O mesmo companheiro descansa em casa e na loja. Modelos leves no estilo do jogo, respiração e resposta ao carinho, som com pausa/mute/intervalo. Materiais e textura de pelo existentes; variantes geradas apenas ao criar ou mudar a configuração. O latido curto `PetBark.wav` é síntese original, mono/24 kHz/0,34 s, sem amostra externa. O miado existente permanece.

Fubá é o gato laranja único na praça perto da DripLab, sobre almofada e caixote. Somente ele conta para o traje secreto, mesmo que o jogador dê o nome Fubá ao próprio pet. Contagem histórica preservada.

Casa e loja: alterar uma opção enquadra o objeto correspondente com câmera suave, sem reiniciar a interpolação quando a câmera de exploração é atualizada. Movimento reduzido troca diretamente; seis vistas manuais, Salvar e Cancelar continuam disponíveis.

## Verificação isolada

Copiar `CompanionsReview.cs` e `../HomeDecor/HomeDecorReview.cs` temporariamente para `Assets/FitCheck/Editor` de uma cópia isolada. Executar Unity em batch com `-executeMethod FitCheck.Editor.CompanionsReview.Capture -city-proof -companions-output <pasta>`.

Verifica 36 combinações de espécie/tipo/pelagem, orçamento abaixo de 18 mil triângulos por animal, áudio, pausa e intervalo; migração de aparência antiga, cópia profunda, isolamento dos três espaços; prévia/cancelamento/salvamento real do criador; enquadramentos de todos os grupos de decoração, interpolação e movimento reduzido; acesso ao Fubá, diferenciação de pet homônimo e desbloqueio/salvamento do traje. Usa sessões UUID próprias, sem saves reais.

Resultado: `COMPANIONS_REVIEW_PASSED`. Capturas em `Captures/Companions/Final`. Revisão de emotes separada em `Captures/Companions/Emotes`.

Fontes prontas na Unity. A build distribuída anterior não contém estas alterações; incluir na próxima build web/Windows.
