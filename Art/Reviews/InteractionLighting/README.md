# Resposta de interação e luz dos personagens

Referência visual: `Art/Marketing/Itch/fitcheck-cover-v1.png`. A capa é uma ilustração promocional; o jogo recebeu uma aproximação estilizada da luz principal quente, sombra mais definida e recorte vermelho lateral, conservando a geometria/texturas existentes.

## Affordances

- E/F disparam resposta visual somente ao acionar uma interação disponível para a tecla indicada: tecla afunda brevemente, inverte cor e a borda emite um pulso de 0,30 s. Texto e posição do aviso preservados.
- A resposta curta continua ao abrir um painel. Loading cobre o feedback; pausa congela o tempo. Movimento reduzido elimina deslocamento e expansão, conservando só a cor.
- Sem atrasar ações, acrescentar sons ou alterar os limites de interação.

## Personagens

Shader compartilhado em `Resources/CharacterLighting.cginc`, usado por pele, cabelo e roupas, incluindo materiais com acabamento. Materiais de cenário não são marcados como personagens. Olhos/detalhes do rosto usam força reduzida e sem recorte vermelho, preservando leitura e expressões.

Não foram acrescentadas luzes em tempo real, mapas de sombra, texturas ou cópias de materiais por personagem. Há cálculo adicional no shader; isso não significa custo zero de GPU e não substitui um teste de FPS no equipamento do jogador.

Prévias Unity/D3D11 em `Captures/CoverLighting/Final`: comparações antes/depois com três tons de pele. Os materiais são os mesmos em cada par; a comparação desliga apenas o novo sombreamento. Mesma geometria e contagem de renderers da revisão anterior do provador.

Validação web e detalhes de entrega em `Art/Releases/Web09/VALIDACAO.md`. Fonte da Unity atualizada; Windows precisa recompilar para distribuir a alteração.
