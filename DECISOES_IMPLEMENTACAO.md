# Decisões provisórias de implementação

Estas decisões resolvem lacunas técnicas do GDD e não substituem aprovação de direção de arte.

- Predominância neutra: duas de três peças principais (cima, baixo, tênis); acessório excluído. Discrição considera todas as peças vestidas.
- Creme é claro, mas não neutro, seguindo a explicação do K7. Cinza da Demo Tape e da Remix é claro. Os demais bottoms atuais são escuros. Home Turf e City Glow têm cor predominante vibrante; pequenos detalhes não contam.
- Moletons recebem corte largo nesta modelagem. Seus grafismos são grandes; Closing Time e Unreleased têm capuz, Meeting Point não.
- Contabilidade de replay: cada post registra a contribuição efetivamente aplicada, incluindo perda parcial ou absorvida pelo piso zero. Melhorar uma nota substitui essa contribuição por uma melhor, creditando apenas a diferença positiva. Melhorias entre duas notas da mesma faixa negativa não recriam uma perda absorvida. Notas iguais/piores não alteram resultado, roupa ou foto.
- Sintonia com os Clientes considera os melhores resultados dos três pedidos de um dia, incluindo replays, e é permanente. Uniformes só são trocados em casa.
- Marcos alcançados permanecem pendentes mesmo após queda de Reputação. Entrega normal é no fechamento; no pós-jogo existe uma ação equivalente de entrega em casa, fora de atendimentos.
- Saída durante provador: reiniciar somente o atendimento incompleto. Não há save de cada troca/pergunta não concluída.
- As legendas iniciais de K7 e Bia são provisórias. Os seis pedidos adicionais e suas legendas foram transcritos do GDD, sem inventar dias 4–7.
- A casa usa interação por menus e o percurso casa–loja ainda não existe. Há uma prévia independente da praça/comércio, acessível por Passear pelo bairro. A conclusão atual cobre os nove atendimentos e a progressão, sem declarar a rotina completa do GDD pronta.
- Uniformes são quatro combinações geométricas provisórias, sem reconstruir o logotipo. Sons de interface são síntese original simples; não são trilha musical ou dublagem final.

Fotos de melhores resultados são imutáveis por arquivo: uma melhora cria novo PNG e atualiza a referência no save. A foto anterior é mantida para continuar compatível com o backup. Falha de gravação mantém bytes pendentes em memória e permite Salvar novamente, sem sobrescrever a foto associada ao save anterior.

O primeiro trecho urbano usa malhas próprias com quinas chanfradas, toldos curvos, vegetação, vitrines e letreiros geométricos; as superfícies são agrupadas por sete materiais. Piso em atlas original estático, demais pinturas com interpolação contínua. NPCs da praça são cópias independentes das malhas/materiais aprovados, para não sumirem quando o player muda de roupa. Nesta rodada são moradores parados, sem simulação de rotina.

A navegação inicial é plana: limites e obstáculos explícitos para fachadas, canteiros, bancos, postes e moradores; rotas em grade com simplificação por visibilidade. Não depende de novos pacotes ou de NavMesh. Mantida a configuração de entrada já presente no projeto (activeInputHandler=0); teclado usa a entrada existente e cliques passam pelo mesmo sistema de eventos da interface. Preferência WASD/clique salva separadamente do progresso.

O passeio abre uma câmera isométrica com acompanhamento e zoom e restaura a câmera do provador ao sair. Fotos de resultado forçam a perspectiva temporariamente e restauram o modo anterior. Passos provisórios deformam apenas as pernas/pés do personagem, sem redesenhar os modelos aprovados ou esticar a cabeça. A primeira fatia não substitui ainda os cenários das fotos nem implementa a rota de 30–45 segundos da casa.

Direção posterior do usuário substitui a câmera isométrica do passeio por primeira pessoa, com WASD e mouse. Altura de visão 2,78, campo vertical 68 graus, inclinação limitada a ±75 graus e corpo do funcionário oculto nessa vista. Esc pausa/libera cursor; Tab alterna cursor/olhar; sair restaura visibilidade, campo de visão e distância mínima da câmera do provador. O movimento não recebe corrida, salto, combate ou tremor de câmera por associação às referências. O provador mantém a apresentação girável do cliente.

Pixelização global reduzida de 360 para até 720 linhas. Materiais de cidade recebem menos contraste de granulação; CityStone passa a 1024x256 (quatro tiles idênticos de 256), filtro bilinear, mipmaps e anisotropia. A referência em vídeo é estritamente gráfica. Quadros de análise ficam fora de Assets e não são incorporados à distribuição.


## Revisão posterior do usuário — corpo e letreiros

A única base corporal aprovada em uso é a compacta com pescoço curto. O corpo esguio e o botão de comparação saíram do jogo; variantes de identidade continuam por roupa, pele, cabelo e expressão. Texturas urbanas com desgaste e detalhe PS1/PS2 são padrão permanente, com AmbientCG autorizado como recurso opcional. Letreiros/cartazes do mundo usam textura; o usuário confirmou que esta mudança não se aplica aos menus. Novos artefatos devem evitar multiplicar builds antigas: manter a versão atual e uma cópia funcional anterior/cópias justificadas.

## Interior antes da conexão da rotina

Usuário priorizou construir o interior da DripLab antes de ligar rua/loja/casa. Primeiro interior tem circulação em primeira pessoa, mobiliário, produtos do catálogo e provador preto/roxo. Acesso por Visitar a loja ou Ir trabalhar; interações por E no balcão/provador/saída. Bairro permanece independente. Cliente da vez aguarda na loja; chegada/saída animada e vista de atendimento completa ficam pendentes. Cabelo e personalização não receberam aprovação final e estão explicitamente reservados para revisão futura.

Revisão posterior: cliente espera apoiado na parede com um pé levantado e sorriso de canto; braços relaxados em todos os personagens. Cortina deve começar fechada e abrir ao interagir antes de entrar na troca de roupas. Usuário quer transições ao entrar/sair de locais futuramente e dispensou implementar essa parte agora; nesta etapa existe somente abertura da cortina seguida da troca de estado existente.


Pendências adiadas por pedido do usuário (03/11/2026):
- O lado da rua em frente à DripLab está vazio; planejar fachadas e detalhes de bairro, preservando passagem, escala e estilo gráfico. Não implementar agora.
- No balão de fala do cliente, passar o mouse sobre o texto deixa as letras brancas e ilegíveis. Corrigir o estado de hover para manter contraste com o fundo do balão. Não implementar agora.


## 04/11/2026 — Loadings e próxima revisão de calçados

Loading aprovado para Jogar e entradas/saídas da casa/loja: logo vermelha com o efeito do menu, Loading com pontos abaixo. Usuário ainda considera fundo preto ou diagonais pretas/vermelhas; primeira implementação combina preto e diagonais Kaos discretas. Sem barra de progresso artificial. Manter o fluxo título → saves → criação/casa, a loja fechada até abertura manual e o percurso físico pelo bairro.

**Tênis refeitos em 05/11/2026**, usando as quatro referências fornecidas em refs roupas/refs tenis. Três modelos Blender mantêm os IDs e desbloqueios; fonte editável em Art/Garments/SneakerCollection.


## 05/11/2026 — Ajustes de movimento e balões

Loading mantém dois segundos; destaques finitos percorrem a inclinação das faixas em vez de depender da translação de linhas infinitas. Balões ambientais e legendas de acompanhamento do cliente ficam sem a ponta solta; no balcão a ponta lateral continua conectada. O usuário ofereceu modelos de IA como alternativa futura e pediu continuar a produção Blender atual.
