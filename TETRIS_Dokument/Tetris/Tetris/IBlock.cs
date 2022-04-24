namespace Tetris
{
    public class IBlock : Block
    {
        private readonly Pozicija[][] tiles = new Pozicija[][]
        {
            new Pozicija[] { new(1,0), new(1,1), new(1,2), new(1,3) },      //pozicije so dolocene s pozicijo v tabeli velikosti 4x4, koordinate pa veljajo za vsako ploščico posebej
            new Pozicija[] { new(0,2), new(1,2), new(2,2), new(3,2) },      //torej nam povejo kje je posamezna ploščica v tabeli 4x4,v določeni poziciji rotacije
            new Pozicija[] { new(2,0), new(2,1), new(2,2), new(2,3) },
            new Pozicija[] { new(0,1), new(1,1), new(2,1), new(3,1) }
        };

        public override int Id => 1;
        protected override Pozicija StartOffset => new Pozicija(-1, 3);     //StartOffSet nam pove kje na mapi/mreži se pojavi naš block takoj ob nastanku/generiranju/pojavi
        protected override Pozicija[][] Tiles => tiles;                     //(-1) pomeni vrstica oziroma (r = row), kar pomeni da je malo od vrha, da se ga malo vidi, saj je tabela v resnici visoka 22 vrstic
                                                                            //(-3) pomeni odmik od roba z leve strani torej se nas block zacne 3 ploščice stran od roba
    }                                                                       //to lastnost ima vsak izmed likov Tetrisa, tako kot tudi pozicije za vsako rotacijo
}
