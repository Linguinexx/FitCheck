# Espelho da loja — lateral do provador

Espelho de corpo inteiro na face externa da divisória do provador, centro local (-2.365, 1.88, 6.15). Moldura fina em metal carvão, borda interna discreta e pequeno detalhe Kaos; vidro de 1,26 × 3,30 unidades. Banco, ganchos, prateleira, GIF, cortina e rota do cliente preservados. Sem novos obstáculos, luzes ou arquivos de textura.

Renderização compartilhada com o espelho do quarto, com orientação oposta da parede. Textura 128 × 352 / 16-bit depth, máximo 15 Hz; câmera auxiliar desativada fora da renderização manual. Ignora espelho fora do enquadramento e observação pelo verso, não atualiza quando a campanha está pausada. Apenas o espaço ativo solicita o reflexo. Emotes usam pose externa no reflexo e restauram braços/cabeça para a câmera original, inclusive durante acompanhamento externo. Recursos registrados no descarte do ambiente.

## Revisão

Copiar ShopMirrorReview.cs e HomeDecorReview.cs para Assets/FitCheck/Editor de um projeto isolado; executar `FitCheck.Editor.ShopMirrorReview.Capture -city-proof -mirror-output <pasta>`.

Verificador cobre três posições livres, espelho real e enquadramento de corpo inteiro; cinco emotes; cabeça escondida na primeira pessoa/restaurada na câmera externa; orçamento 128×352/15 Hz; verso/fora da tela; abertura da cortina; regressão do espelho Paint do quarto. Somente cena e dados de revisão, sem saves reais. Capturas em Captures/ShopMirror/Final.

Fontes atualizadas na Unity. Builds Windows Teste 09 e Web Teste 10 anteriores não incluem este espelho; recompilar na próxima entrega.

Resultado: SHOP_MIRROR_PASSED. Sem erros de compilação ou de execução do revisor; o editor emitiu a exceção de indexação de SearchDatabase já presente em revisões anteriores, sem relação com o jogo.
