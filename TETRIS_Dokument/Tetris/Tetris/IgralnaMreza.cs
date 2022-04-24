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
        {                                              //konstruktor, ki vzame stevilke vrstic in stolpcev kot parametre
            Vrstice = vrstice;
            Stolpci = stoplci;      //v nasi spremenljivki shranimo vrednost, ki jo dobimo vnešeno iz konstruktorja, ter iz nje naredimo mrežo teh dimenzij
            grid = new int[vrstice, stoplci];
        }

        public bool AliJeZnotrajMreze(int r, int c)  //preveri ali sta vrstica in stoplec znotraj igralne mreze
        {
            return ((r >= 0 && r < Vrstice) && (c >= 0 && c < Stolpci));
        }

        public bool AliJeCelicaPrazna(int r, int c)  //preveri ali je dolocena celica/ploscica prazna ali ne, mora biti znotraj mreze in vrednost mora biti 0
        {
            return AliJeZnotrajMreze(r, c) && grid[r, c] == 0;
        }

        public bool AliJeVrsticaDokoncana(int r)  //preveri ali je celotna vrstica dokoncana
        {
            for (int c = 0; c < Stolpci; c++)   // c = column in pomeni stolpec, ker se for zanka premika po stolpcih, r=row pa pomeni katero vrstico preverja ali je dokoncana
            {
                if (grid[r, c] == 0)    //ce na katerem mestu ni nobenega blocka, nam vrne false, ker ni dokončana
                {
                    return false;
                }
            }
            return true;    //ce na nobenem mestu ni ne-bilo blocka, nam vrne true, ker vrstica je dokoncana
        }

        public bool AliJeVrsticaNedokoncana(int r)
        {
            for (int c = 0; c < Stolpci; c++)  //zanka ki se premika po vrstici in preverja ali je slucajno nedokoncana
            {
                if (grid[r, c] != 0)
                {
                    return false;   //ce na kateremkoli mestu v vrstici vrednost ni enaka 0, potem vrne false
                }
            }
            return true;  //ce na nobenem mestu v vrstici ni blocka (vrstica ni dokoncana) vrne true
        }

        private void IzbrisiVrstico(int r)      //metoda ki izbrise vrstico, kot vnos dobi r, ki pomei vrstica, torej katero vrstico bo pocistilo
        {
            for (int c = 0; c < Stolpci; c++)  //zanka ki se premika po vrstici (torej po y-ih) oz. stevilkah stolpcev (c) in enega in po enega izbrise oz. nastavi na y=0
            {
                grid[r, c] = 0;
            }
        }

        private void PremakniVrsticoDol(int r, int SteviloVrstic)  //metoda ki vrsticno premakne dol, tolikokrat kot je potrebno
        {
            for (int c = 0; c < Stolpci; c++)
            {
                grid[r + SteviloVrstic, c] = grid[r, c];    //na mesto v tabeli, kjer smo naprej naredili praznino (kjer smo prvic izbrisali vrstico (r + SteviloVrstic),
                grid[r, c] = 0;                             //se s pomočjo for zanke premakne vrstica v kateri smo sedaj, enden in po eden block nazaj (grid[r, c])
            }
        }

        public int IzbrisiPolneVrstice()                 //s pomocjo zgornjih metod izbriše vse dokoncane vrstice
        {
            int izbrisane = 0;

            for (int r = Vrstice-1; r >= 0; r--)    //zacnemo pri vrstici (Vrstice-1), torej visina tablele - 1, ker se zacne index pri 0
            {
                if (AliJeVrsticaDokoncana(r))   //naprej preveri ce je vrstica r polna, in ce je jo izbrise in poveca izbrisane,
                {
                    IzbrisiVrstico(r);
                    izbrisane++;
                }
                else if (izbrisane > 0)
                {
                    PremakniVrsticoDol(r, izbrisane);   //vrednost izbrisane, se vnese v SteviloVrstic metode PremakniVrsticoDol
                }   //tako se dol premaknejo vse vrstice
            }
            return izbrisane;   //vrnemo število počiščenih vrstic za nadaljno uporabo pri štetju rezultatov
        }
        




    }
}
