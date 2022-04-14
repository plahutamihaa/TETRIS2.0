namespace Tetris
{
    public class IgralnaMreza
    {
        private readonly int[,] grid;  //naredi pravokotni array/igralno povrsino/mrezo

        public int Vrstice { get; }  //lastnosti za številke vrstic
        public int Stolpci { get; }  //lastnosti za številke stolpcev

        public int this[int r, int c]  //omogoca nam preprost dostop do arraya
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }
        
        public IgralnaMreza(int vrstice, int stoplci)  //dolocimo velikost igralne mreze, lahko naredimo tudi vecjo kot je obicajno
        {
            Vrstice = vrstice;
            Stolpci = stoplci;
            grid = new int[vrstice, stoplci];
        }

        public bool AliJeZnotrajMreze(int r, int c)  //preveri ali sta vrstica in stoplec znotraj igralne mreze
        {
            return r >= 0 && r < Vrstice && c >= 0 && c < Stolpci;
        }

        public bool AliJeCelicaPrazna(int r, int c)  //preveri ali je dolocena celica/ploscica prazna ali ne, mora biti znotraj mreze in vrednost mora biti 0
        {
            return AliJeZnotrajMreze(r, c) && grid[r, c] == 0;
        }

        public bool AliJeVrsticaDokoncana(int r)  //preveri ali je celotna vrstica dokoncana
        {
            for (int c = 0; c < Stolpci; c++)
            {
                if (grid[r, c] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool AliJeVrsticaNedokoncana(int r)
        {
            for (int c = 0; c < Stolpci; c++)  //zanka ki se premika po vrstici in preverja ali je slucajno nedokoncana
            {
                if (grid[r, c] != 0)
                {
                    return false;
                }
            }
            return true;  //ce na katerem mestu ni blocka (vrstica ni dokoncana) vrne true
        }

        private void IzbrisiVrstico(int r)      //metoda ki izbrise vrstico
        {
            for (int c = 0; c < Stolpci; c++)  //zanka ki se premika po vrstici (torej po y-ih) in enega in po enega izbrise oz. nastavi na y=0
            {
                grid[r, c] = 0;
            }
        }

        private void PremakniVrsticoDol(int r, int SteviloVrstic)  //metoda ki vrsticno premakne dol, tolikokrat kot je potrebno
        {
            for (int c = 0; c < Stolpci; c++)
            {
                grid[r + SteviloVrstic, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }

        public int IzbrisiPolneVrstice()                 //s pomocjo zgornjih metod izbriše vse dokoncane vrstice
        {
            int izbrisane = 0;

            for (int r = Vrstice-1; r >= 0; r--)
            {
                if (AliJeVrsticaDokoncana(r))
                {
                    IzbrisiVrstico(r);
                    izbrisane++;
                }
                else if (izbrisane > 0)
                {
                    PremakniVrsticoDol(r, izbrisane);
                }
            }
            return izbrisane;
        }
        




    }
}
