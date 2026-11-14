# Momentos na loja — 14/11/2026

## Como experimentar

Entre na DripLab antes de abrir o expediente ou depois de atender os três clientes do dia.

- Aproxime-se do Nando e aperte E. **Guia** mantém a ajuda de gameplay; **Resenha** muda a conversa nos sete dias; **Desafio** traz uma pergunta por dia. Resposta errada permite tentar novamente. Resposta certa rende um adesivo cosmético do mural, sem mudar reputação, notas ou roupas.
- Olhe para a caixa de som no banco da entrada e aperte E. Escolha uma das três músicas já presentes no jogo. A escolha vale na loja e fica salva no espaço atual; volume, mute e pausa continuam funcionando nas configurações.
- Atrás do balcão, olhe para o porta-retrato sobre o álbum e aperte E. Antes do primeiro atendimento, o álbum explica como conseguir fotos. Depois, permite rever as fotos reais dos atendimentos e escolher uma para expor no balcão.
- Use Voltar à loja ou a tecla de pausa para fechar o painel e recuperar o controle da câmera.

Durante o expediente, o Nando continua ajudando, mas resenha, desafio, rádio e álbum aguardam a loja fechar. Não há clientes novos, atendimentos extras ou deslocamentos pelo menu.

## Implementação e limites

Estado em DayProgress.neighbourhood por save: shopChatDays/shopQuizDays, musicChoice e shopPhoto (índice do atendimento + 1; zero sem escolha). Saves antigos recebem os padrões. A foto usa o cache existente do FitFeed; geometria do rádio e suporte usa materiais existentes e agrupamento estático. Sem novas texturas, músicas, luzes em tempo real ou NPCs permanentes.

Fontes: ShopMoments.cs, ShopMomentsWorld.cs, ShopMomentsChecks.cs e Editor/ShopMomentsReview.cs. Roteiro automatizado com dados isolados; imagens reais em Captures/ShopMoments. Builds publicáveis anteriores permanecem intactas; incluir estas alterações na próxima entrega solicitada.

Validação final: SHOP_MOMENTS_PASSED e FITCHECK_SHOP_SERVICE_PASSED. Compilação sem erros/avisos; acesso físico, retorno do controle, três escolhas de trilha, álbum vazio/fotos reais/favorita, resposta errada/certa/repetida, persistência e isolamento de saves, bloqueio do lazer durante o expediente, guia disponível, atores/luzes preservados, cortina vazia e atendimento completo. Conferência visual das telas e porta-retrato aprovada após corrigir a ocultação imediata da cabeça ao abrir os painéis.

Limpeza concluída: 847,616,798 bytes de instalador substituível, páginas antigas de revisão, compilação temporária, seus caches e saves isolados. Preservados originais, referências, ferramentas úteis, saves reais, capturas finais e builds atual/anterior. Auditoria local em tools/shop-moments-cleanup.json.
