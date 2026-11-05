# Comércio local — 05/11/2026

Fachadas decorativas entre a mercearia e a oficina, com escala compatível com as construções existentes:

- Barbearia: X=-98, largura 14, dois pavimentos, cadeiras/espelhos e poste listrado.
- Lavanderia: X=-85, largura 10, dois pavimentos e três máquinas na vitrine.
- Consertos: X=-49, largura 18, três pavimentos e bancada/ferramentas.

Edificações em Z=12, profundidade 10, com retângulos de navegação correspondentes. Corredores X=-108/-76/-61 e travessa Z=21 permanecem abertos. Jardineiras com bases, árvore e banco ocupam bordas; painéis antigos ficam atrás das novas fachadas, sem atravessá-las. Calçada mantém superfície/UVs contínuos.

Fonte: Assets/FitCheck/CommerceInfill.cs. Todos os elementos entram nos lotes estáticos existentes, exceto um lote novo para letreiros. Atlas CommerceSigns.png de 512×256, com quatro linhas, gerado por make_signs.py usando Pillow e Archivo Black já embarcada no jogo (licença em Art/Fonts). Os demais materiais são reutilizados. Três fontes de cor usam o campo de iluminação existente, sem novas Unity Lights ou sombras.

Revisão nativa -city-proof -commerce-infill-review passou, saída 0. Adição medida de 13.644 triângulos e um lote. Acessos, colisões, sete moradores, locais de fotografia e caminhada de 31,75 s/31,18 s verificados. Capturas: Captures/CommerceInfill. Logs: tools/commerce-infill-build.log e tools/commerce-infill-proof.log no workspace. Orçamento de geometria não é medição de FPS.

FitFeed foi adiado pelo usuário. O espaço entre SIDE B e AO VIVO foi preenchido na etapa seguinte, documentada em ../MusicInfill/README.md.
