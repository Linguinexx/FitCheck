# Quarto personalizável e espelho

No quarto, olhar o catálogo vermelho e creme na mesa, ao lado do celular, e pressionar **F**. Affordance próxima destaca o catálogo e mostra “Personalizar o quarto”. O celular continua em E, sem atalhos de viagem ou novas opções no menu de pausa.

Painel segue a loja: abas Ambiente/Têxteis/Artes, seis vistas da própria cena, Salvar decoração e Cancelar. Paredes e parede da cama independentes; piso original, escuro ou cerâmico; cores da roupa de cama e do tapete, três quadros independentes com todas as artes existentes e planta recolhível. Texturas, proporções dos quadros, posição dos móveis, colisões, companheiro escolhido, guarda-roupa, cama e celular preservados.

Prévia não grava. Cancelar ou voltar restaura cores, textura do piso, quadros e planta. Confirmar grava `home-style.json` no `SessionFolder` do save ativo, com validação e arquivo `.bak`; estilos de casa e loja são independentes. Erro ao salvar mantém o painel aberto e o rascunho, com mensagem visível. Confirmação de sucesso é temporária, sem usar o aviso de conquista. Saves antigos recebem a decoração original e novos saves não herdam o estilo de outro espaço.

Estado modal da decoração compartilhado com o da loja: bloqueia movimento, emotes e demais interações, libera o cursor e devolve a câmera à posição original ao fechar. Acesso à loja continua com suas condições anteriores. A prévia recolore lotes existentes, sem recriar móveis ou gerar novas texturas/luzes. Catálogo acrescentado ao lote de papel existente.

Espelho: removido o limite de seis unidades e ajustado o plano distante da câmera refletida conforme a distância ao espelho e a diagonal do quarto. Permanece limitado à frente do vidro e à presença do espelho no enquadramento; resolução **128 × 352**, atualização **15 Hz**, sem aumentar o custo por imagem. Cabeça e transform dos braços em primeira pessoa são restaurados após a reflexão.

## Verificação

`HomeDecorReview.cs` nesta pasta deve ser copiado temporariamente para `Assets/FitCheck/Editor` de um projeto isolado. Executar Unity em batch com `-executeMethod FitCheck.Editor.HomeDecorReview.Capture -city-proof -home-decor-output <pasta>`.

Verifica espelho em cinco posições, incluindo o lado mais distante; plano distante e resolução; restauração da cabeça; catálogo/affordance F; bloqueio de movimento; UI real nas três abas; mudanças sem crescimento de geometria; cancelar e voltar; cores sem acúmulo; salvar/recarregar; isolamento de espaços; recuperação da cópia de segurança; valores inválidos; falha de gravação sem perda do rascunho; artes com proporção correta; abertura/cancelamento da personalização da loja. Dados de teste ficam em subpasta UUID da captura, sem acessar saves reais.

Resultado: `HOME_DECOR_REVIEW_PASSED`. Capturas atuais em `Captures/HomeDecor/Final`.

Fontes prontas na Unity. Incluir na próxima build web/Windows; a build web Teste 09 anterior não contém estas mudanças.

Ao mudar uma opção, a câmera enquadra automaticamente o item: paredes, cama, piso/tapete, quadros ou plantas. Transição suave com movimento reduzido desativado; troca direta com movimento reduzido. Vistas manuais preservadas. A loja também usa foco automático para cortina, GIF, piso, quadro, paredes e plantas.
