using System.Linq;
namespace FitCheck
{
    internal static class SpecialCustomers
    {
        internal const int Iara=11,Dante=12,Cora=13;
        internal static float Seconds(int encounter)=>encounter==3?30:encounter==11?45:0;
        internal static string Theme(int encounter)=>encounter==3?"CONTRA O RELÓGIO":encounter==11?"ÚLTIMA CHAMADA":encounter==14?"UNIFORME DE RUA":encounter==15?"PALETA CALMA":"";
        internal static PlayerAppearance Appearance(int profile){
            switch(profile){
                case Iara:return new PlayerAppearance{skin=5,hair=1,hairColor=11,expression=1,appearanceBase=1};
                case Dante:return new PlayerAppearance{skin=3,hair=8,hairColor=7,expression=2};
                case Cora:return new PlayerAppearance{skin=1,hair=6,hairColor=8,expression=4,appearanceBase=1};
                default:return null;
            }
        }
        // Keep encounter IDs, scoring and positions stable for historical saves.
        internal static WeekExpansion.Encounter Encounter(int index){
            if(index==11)return new WeekExpansion.Encounter(Iara,1,new CustomerRequest("DAY04_LIA_PRINT","Iara Ponteiro",
                "O último ônibus pro festival sai já já. Tenho 45 segundos no provador! Quero uma peça SIDE B, estampa discreta em cima e contraste claro/escuro entre as duas metades. Se eu perder o ônibus, viro decoração da galeria.",
                new[]{"Quando começa a contar?","Como você quer esse contraste?"},
                new[]{"Só quando eu estiver no provador e você puder montar o visual. Conversar aqui não gasta meu tempo.","Uma metade clara e a outra escura. Quero que apareçam separadas até na foto em preto e branco."},
                new[]{"Pelo menos uma peça SIDE B","Parte de cima lisa ou com estampa discreta","Contraste claro/escuro entre cima e baixo"},
                "Deu tempo do look. O ônibus que lute.","Galeria"),f=>new[]{f.Any(g=>g?.drop=="SIDE B"),f[0].discreet,f[0].light!=f[1].light});
            if(index==14)return new WeekExpansion.Encounter(Dante,2,new CustomerRequest("DAY05_BRENO_REPAIR","Dante Remendo",
                "Faço reparo de bike na rua e hoje tem mutirão na quadra. Meu uniforme tem três regras: calça protegendo a perna, camiseta folgada pro braço trabalhar e uma peça COMMON GROUND. Roupa de trabalho também merece personalidade.",
                new[]{"Por que a calça é importante?","O que faz esse uniforme ser seu?"},
                new[]{"Vou ajoelhar pra regular corrente e freio. Bermuda hoje não dá conta.","COMMON GROUND lembra a oficina aberta pra todo mundo. E nada agarrando meus ombros."},
                new[]{"Usar calça","Parte de cima larga","Pelo menos uma peça COMMON GROUND"},
                "A bike voltou a rodar. Eu também.","Quadra"),f=>new[]{!f[1].shorts,f[0].loose,f.Any(g=>g?.drop=="COMMON GROUND")});
            return new WeekExpansion.Encounter(Cora,0,new CustomerRequest("DAY06_JOTA_MARKET","Cora Semente",
                "Vou montar uma troca de plantas na praça. Hoje a paleta é calma: pelo menos duas peças neutras entre blusa, calça e calçado. Quero calça e parte de cima clara. A cor fica por conta das folhas que vou carregar.",
                new[]{"O que entra nessa paleta?","Por que parte de cima clara?"},
                new[]{"Branco, cinza, preto e tons neutros. Das três peças principais, quero pelo menos duas assim.","Ajuda o pessoal a me achar perto das mudas. E fica bonito na foto de fim de feira."},
                new[]{"Usar calça","Parte de cima clara","Maioria de peças neutras"},
                "Uma muda, uma conversa. Voltei com o bairro inteiro no vaso.","Praça"),f=>new[]{!f[1].shorts,f[0].light,f.Take(3).Count(g=>g.neutral)>=2});
        }
    }
}
