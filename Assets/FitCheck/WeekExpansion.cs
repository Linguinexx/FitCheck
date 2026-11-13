using System;
using System.Linq;

namespace FitCheck
{
    internal static class WeekExpansion
    {
        internal const int Dandara=7, Ravi=8, Joana=9, Teo=10;

        internal sealed class Encounter
        {
            internal readonly CustomerRequest Request;
            internal readonly int Profile, Scenery;
            internal readonly Func<Garment[],bool[]> Evaluate;

            internal Encounter(int profile,int scenery,CustomerRequest request,Func<Garment[],bool[]> evaluate)
            {Profile=profile;Scenery=scenery;Request=request;Evaluate=evaluate;}
        }

        internal static PlayerAppearance Appearance(int profile)
        {
            switch(profile){
                case Dandara:return new PlayerAppearance{skin=2,hair=PlayerAppearance.ShortCoilsHair,hairColor=4,expression=4,appearanceBase=1};
                case Ravi:return new PlayerAppearance{skin=3,hair=2,hairColor=1,expression=2};
                case Joana:return new PlayerAppearance{skin=4,hair=1,hairColor=PlayerAppearance.HotPinkHairColor,expression=1,appearanceBase=1};
                case Teo:return new PlayerAppearance{skin=1,hair=5,hairColor=PlayerAppearance.CopperHairColor,expression=3};
                default:return SpecialCustomers.Appearance(profile);
            }
        }

        static bool Drop(Garment[] fit,string drop)=>fit.Any(piece=>piece?.drop==drop);

        internal static readonly Encounter[] Encounters={
            new Encounter(Dandara,2,new CustomerRequest("DAY04_DANDARA_COURT","Dandara",
                "Vou apitar o treino aberto na quadra. Preciso de espaço pra mexer os braços e nada de tecido até o tornozelo. Quero vestir alguma coisa que tenha a cara da quadra.",
                new[]{"O que vai rolar no treino?","Que coleção você tem em mente?"},
                new[]{"Vou demonstrar os movimentos junto com o pessoal. Camiseta folgada e bermuda resolvem.","COMMON GROUND. O nome já parece convite pra entrar na roda."},
                new[]{"Usar bermuda","Parte de cima larga","Pelo menos uma peça COMMON GROUND"},
                "Treino aberto. Quem chegou ganhou time.","Quadra"),f=>new[]{f[1].shorts,f[0].loose,Drop(f,"COMMON GROUND")}),
            new Encounter(Ravi,0,new CustomerRequest("DAY04_RAVI_PHOTO","Ravi",
                "Vou fotografar o bairro da primeira luz até o último banco vazio. Quero calça e boné. Em cima, deixa a estampa sossegada que eu vou carregar câmera e alça.",
                new[]{"Por que a aba?","Como fica a parte de cima?"},
                new[]{"Ajuda a cortar a luz quando olho pelo visor.","Lisa ou com um detalhe pequeno. A alça já cruza o peito."},
                new[]{"Usar calça","Usar boné","Parte de cima lisa ou com estampa discreta"},
                "Primeiro retrato da noite saiu antes de eu ligar a câmera.","Praça"),f=>new[]{!f[1].shorts,f[3]?.cap==true,f[0].discreet}),
            SpecialCustomers.Encounter(11),
            new Encounter(Teo,4,new CustomerRequest("DAY05_TEO_SOUND","Téo",
                "Vim do ensaio direto pro teste de som. Vou passar a noite levantando caixa. Em cima, deixa largo. A cabeça precisa ficar livre. Quero um toque daquela coleção que tem cara de rua acesa à noite.",
                new[]{"Qual coleção combina com o teste?","O que você vai usar na cabeça?"},
                new[]{"AFTER HOURS. Combina com quem ainda tá montando o palco quando os outros vão pra casa.","Nada. Já vou passar parte da noite com um fone emprestado."},
                new[]{"Parte de cima larga","Sem acessório na cabeça","Pelo menos uma peça AFTER HOURS"},
                "Som passou. Agora só falta lembrar onde deixei o cabo.","Terraço do DJ"),f=>new[]{f[0].loose,f[3]==null,Drop(f,"AFTER HOURS")}),
            new Encounter(Joana,5,new CustomerRequest("DAY05_JOANA_DANCE","Joana",
                "Vou puxar o aquecimento do baile. Quero bermuda pra dançar, camiseta clara pra aparecer na luz e um boné. Aquele com aba, que dá pra virar no meio da coreografia.",
                new[]{"O que precisa aparecer na luz?","Por que bermuda?"},
                new[]{"A parte de cima. Na pista, é por ali que a galera acompanha o movimento.","Vou demonstrar os passos e quero a canela livre."},
                new[]{"Usar bermuda","Parte de cima clara","Usar boné"},
                "A aula terminou. A pista ficou aberta.","Rua do baile"),f=>new[]{f[1].shorts,f[0].light,f[3]?.cap==true}),
            SpecialCustomers.Encounter(14),
            SpecialCustomers.Encounter(15),
            new Encounter(Dandara,6,new CustomerRequest("DAY06_DANDARA_GALLERY","Dandara",
                "A Lia chamou a gente pra ver as fotos da quadra na galeria. Quero ir de bermuda, com estampa pequena em cima. Deixa a cabeça livre que hoje eu arrumei o cabelo.",
                new[]{"Como entra a estampa?","Vai querer boné?"},
                new[]{"Se for lisa ou discreta, tá ótimo. Quero ver as fotos com calma.","Hoje não. Quero mostrar o cabelo."},
                new[]{"Usar bermuda","Parte de cima lisa ou com estampa discreta","Sem acessório na cabeça"},
                "Na quadra a gente joga. Na galeria a gente vê a história.","Galeria da Lia"),f=>new[]{f[1].shorts,f[0].discreet,f[3]==null}),
            new Encounter(0,4,new CustomerRequest("DAY06_KAIO_SESSION","Kaio",
                "Vou gravar meu primeiro set no terraço. Quero boné, uma peça SIDE B e contraste entre cima e baixo. A câmera vai ficar longe e eu quero que as duas metades apareçam.",
                new[]{"Como você quer esse contraste?","O que entra de SIDE B?"},
                new[]{"Uma metade clara e a outra escura. A luz do terraço é traiçoeira.","Qualquer peça da coleção. Quero a energia da fita demo."},
                new[]{"Usar boné","Pelo menos uma peça SIDE B","Contraste claro/escuro entre cima e baixo"},
                "Set gravado. A versão com o vizinho reclamando é faixa bônus.","Terraço do DJ"),f=>new[]{f[3]?.cap==true,Drop(f,"SIDE B"),f[0].light!=f[1].light}),
            new Encounter(Ravi,0,new CustomerRequest("DAY07_RAVI_LAST_SHOT","Ravi",
                "Falta a foto do bairro reunido na praça. Hoje quero bermuda, cabeça livre e uma peça AFTER HOURS pra fechar o ensaio.",
                new[]{"O que muda no visual de hoje?","Por que AFTER HOURS?"},
                new[]{"Canela livre e sem aba. Dessa vez também vou aparecer na foto.","Foi a coleção que mais me lembrou a luz das lojas depois do expediente."},
                new[]{"Usar bermuda","Sem acessório na cabeça","Pelo menos uma peça AFTER HOURS"},
                "A última foto foi a primeira com todo mundo junto.","Praça"),f=>new[]{f[1].shorts,f[3]==null,Drop(f,"AFTER HOURS")}),
            new Encounter(Joana,5,new CustomerRequest("DAY07_JOANA_LAST_DANCE","Joana",
                "Hoje o baile é pra todo mundo. Quero uma parte de cima clara e folgada pra puxar os passos. Embaixo, quero escuro pra fazer contraste.",
                new[]{"O que deixa o movimento visível?","Como precisa vestir a camiseta?"},
                new[]{"A parte de cima clara, com a de baixo escura. A luz da pista cuida do resto.","Folga nos ombros e no tronco. Quero levantar os braços sem puxar a roupa junto."},
                new[]{"Parte de cima clara","Parte de cima larga","Parte de baixo escura"},
                "O último passo virou convite pro próximo baile.","Rua do baile"),f=>new[]{f[0].light,f[0].loose,!f[1].light}),
            new Encounter(5,8,new CustomerRequest("DAY07_NINA_ENCORE","Nina Faísca",
                "A galera pediu mais uma no terraço. Vou fechar a semana com um som que nasceu aqui, então quero uma peça COMMON GROUND. Em cima, larga. E meu Black Power sobe ao palco livre.",
                new[]{"O que o bairro trouxe pro show?","Como você quer se mexer no palco?"},
                new[]{"O refrão, os passos e a turma que ensaiou na praça. Quero COMMON GROUND no visual.","Quero soltar os braços. Camiseta folgada, sem boné nem gorro."},
                new[]{"Pelo menos uma peça COMMON GROUND","Parte de cima larga","Sem acessório na cabeça"},
                "Sete noites, um bairro inteiro no refrão.","Palco do terraço"),f=>new[]{Drop(f,"COMMON GROUND"),f[0].loose,f[3]==null})
        };
    }
}
