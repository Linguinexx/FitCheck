# Mural e lambe-lambes — 14/11/2026

Mural movido para a face norte do muro, voltado para a travessa acessível, em (-103, 1.23, 17.99). Frente, impressão, câmera e interação usam o mesmo referencial; verso não oferece E. A câmera do editor deixa o corpo do jogador fora da imagem e devolve o enquadramento ao fechar.

Os sete IDs, as coletas e os desbloqueios são mantidos. Papéis com proporções diferentes, bordas rasgadas, canto dobrado, impressão e texto do atlas existente; fundo escuro permite distinguir os contornos. Sem texturas, luzes ou NPCs novos.

E abre o mural. Lista ou clique no papel seleciona; arrastar reposiciona, Girar -/+ altera em 5 graus (até 30 para cada lado), Retirar devolve à coleção. Seleção traz o papel à frente. Limites mantêm todo o papel dentro do mural. Salvar/Cancelar incluem presença, posição, rotação e sobreposição.

DayProgress.neighbourhood agora armazena muralX/muralY/muralAngle/muralLayer, sete entradas de cada. Campos ausentes/vazios migram para composição inicial, preservando placed/stickers e campanha. Arrays de prévia são cópias; Cancelar não grava. Ordem deve ser permutação de 0 a 6; valores não finitos ou composição malformada são rejeitados. Dados isolados por save.

MURAL_EDITOR_PASSED: face/câmera corretas, acesso pela frente, conversão do raio para o papel e seleção/movimento, tamanhos variados, giro/limites, prévia/cancelar/salvar/reentrada, persistência da ordem, migração e espaços independentes, sem mudança de reputação/atendimentos. NEIGHBOURHOOD_ACTIVITIES_PASSED refeito com as sete coletas, histórias, emotes e basquete. Compilação nativa sem erros/avisos. Evidências em Captures/MuralLambes; fonte MuralChecks.cs e Editor/NeighbourhoodReview.cs.
