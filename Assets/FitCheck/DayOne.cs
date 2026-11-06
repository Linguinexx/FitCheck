using System;
using System.Linq;
using System.Collections.Generic;

namespace FitCheck
{
    public class CustomerRequest
    {
        public string id, name, request, caption, location;
        public string[] questions, answers, criteria;
        public CustomerRequest(string id, string name, string request, string[] questions, string[] answers, string[] criteria, string caption, string location)
        { this.id=id; this.name=name; this.request=request; this.questions=questions; this.answers=answers; this.criteria=criteria; this.caption=caption; this.location=location; }
    }
    public static class DayOne
    {
        static readonly int[] CustomerProfiles={0,1,2,3,0,4,1,3,5};
        internal static int CustomerProfile(int index)=>CustomerProfiles[index];
        internal const string KaioName="Kaio", GalleryArtistName="Lia", ShowRegularName="Júlia";
        public static readonly CustomerRequest[] Customers = new CustomerRequest[] {
            new CustomerRequest("DAY01_K7_TUTORIAL", KaioName,
                "Hoje tem som na praça! Vou encontrar a galera e provavelmente ficar por lá até esquecer a hora. Quero uma camiseta folgada, umas cores mais de boa… e estampa sem gritaria. O DJ já vai fazer barulho suficiente.",
                new[]{"O que são cores ‘mais de boa’ pra você?", "E o que seria uma estampa sem gritaria?"},
                new[]{"Branco, cinza, preto… essas que combinam sem precisar fazer reunião.", "Um desenho pequeno funciona. Se der pra ler do outro lado da praça, passou do ponto."},
                new[]{"Parte de cima larga", "Maioria de peças neutras", "Estampas discretas ou peças lisas"},
                "Hoje tem som na praça. O visual passou na DripLab!", "Praça"),
            new CustomerRequest("DAY01_BIA_GALLERY", GalleryArtistName,
                "A exposição é de uma galera que mistura cartaz, fita e coisa feita à mão. Quero entrar nesse clima. Na foto, não quero que meu corpo vire um bloco só: as duas metades precisam se destacar uma da outra. E queria uma aba fazendo uma moldura pro rosto.",
                new[]{"Como você imagina essa separação no visual?", "O que te atrai nessa exposição?"},
                new[]{"Pensa numa fotografia em preto e branco: quero perceber as duas peças mesmo sem enxergar as cores.", "As colagens, as experiências, a impressão de que alguém montou tudo no próprio quarto. Tem uma coleção de vocês com essa energia."},
                new[]{"Contraste claro/escuro entre cima e baixo", "Pelo menos uma peça SIDE B", "Usar boné"},
                "Vim ver a exposição. Saí querendo montar a minha.", "Galeria"),
            new CustomerRequest("DAY01_PICO_COURT", "Breno ‘Pico’",
                "Hoje a quadra vira sala de estar do bairro. Quero sentir o vento na canela e levar um pouco dessa ideia de espaço compartilhado no visual. Pode ter uma cor puxando a conversa. As outras peças precisam saber escutar.",
                new[]{"Que ideia de espaço compartilhado?", "Como uma cor puxa a conversa?"},
                new[]{"Praça, pista, quadra… lugares que ficam com a cara de quem aparece por lá.", "Ela aparece primeiro quando você olha. Se outra peça tentar fazer a mesma coisa, já vira discussão."},
                new[]{"Usar bermuda", "Pelo menos uma peça COMMON GROUND", "Apenas uma peça vibrante"},
                "A quadra é de geral. O estilo hoje teve assistência da DripLab.", "Quadra")
        }.Concat(CampaignContent.More).ToArray();
        public static bool[] Evaluate(int customer, string[] outfit)
        {
            Rules.Evaluate(outfit); // Also validates required slots.
            var pieces=outfit.Select(Catalog.Get).ToArray();
            if(Catalog.IsArrivalPiece(outfit[0]))pieces[0]=StudioView.CivilianTop(customer);
            if(Catalog.IsArrivalPiece(outfit[1]))pieces[1]=StudioView.CivilianBottom(customer);
            var garments=pieces.Where(piece=>piece!=null).ToArray();
            if(customer==0)return Rules.EvaluatePieces(pieces);
            if(customer==1)return new[]{pieces[0].light!=pieces[1].light,
                garments.Any(g=>g.drop=="SIDE B"), Catalog.Get(outfit[3])?.cap==true};
            if(customer==2)return new[]{pieces[1].shorts,
                garments.Any(g=>g.drop=="COMMON GROUND"), garments.Count(g=>g.vibrant)==1};
            var top=pieces[0];var bottom=pieces[1];
            if(customer==3)return new[]{top.light!=bottom.light,top.loose,string.IsNullOrEmpty(outfit[3])};
            if(customer==4)return new[]{bottom.loose,garments.Any(g=>g.drop=="SIDE B"),top.discreet};
            if(customer==5)return new[]{bottom.shorts&&bottom.vibrant,top.light,Catalog.Get(outfit[3])?.cap==true};
            if(customer==6)return new[]{garments.Any(g=>g.drop=="SIDE B"),top.discreet,!bottom.shorts};
            if(customer==7)return new[]{garments.Any(g=>g.drop=="AFTER HOURS"),top.loose,!bottom.light};
            if(customer==8)return new[]{top.light&&!bottom.light,garments.Any(g=>g.drop=="COMMON GROUND"),string.IsNullOrEmpty(outfit[3])};
            throw new ArgumentOutOfRangeException(nameof(customer));
        }
        public static int Score(int customer,string[] outfit)
        { var c=Evaluate(customer,outfit); return (c[0]?4:0)+(c[1]?3:0)+(c[2]?3:0); }
    }
}
