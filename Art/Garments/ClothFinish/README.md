# Acabamento de tecidos — 11/11/2026

`refine_cloth_textures.py` prepara as texturas no Blender, mantendo cores, estampas, IDs e dimensões dos arquivos. O processo usa as imagens Fabric030 e Fabric084 já recebidas do ambientCG (Lennart Demes, CC0); créditos e origens em `Art/AmbientCG/CREDITS.md`.

- Algodão, malha esportiva, moletom, nylon, couro, sarja, tricô e jeans recebem acabamentos distintos.
- As texturas com atlas recebem trama e dobras suaves, posicionadas pelas coordenadas UV da malha principal. Etiquetas, estampas e peças de ferragem permanecem existentes.
- Materiais sem atlas, roupas civis e partes procedurais usam acabamentos compartilhados de 128–256 px. Texturas de roupas permanecem em até 512 px. Não há novas luzes, passes de shader ou processamento de textura por quadro.
- `OriginalTextures.zip` preserva os mapas anteriores para reedição. O script compara hashes com `manifest.json` para não acumular detalhe em execuções repetidas. Cinco acabamentos neutros novos são gerados sem fonte adicional.

Conferência visual em `Captures/ClothingPolish/After`: 22 peças superiores/inferiores do catálogo, oito trajes do jogador e roupas civis. Calçados e acessórios mantêm seus mapas. O pé do slide tem base arredondada e cinco dedos combinados em uma malha por pé, com o mesmo vínculo de movimento usado pelo calçado.

A compilação Windows da cena isolada da intro passou. A tentativa de captura em janela oculta não recebeu eventos de Repaint da interface; os quadros pretos desse diagnóstico não servem como validação visual da logo. Conferir a intro no Play normal. O código alterado somente desenha o fundo em coordenadas físicas da tela, restaurando a matriz antes de desenhar a logo existente.
