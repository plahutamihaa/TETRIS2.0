namespace Tetris
{
    public class Pozicija  //naredimo class, ki nam pove pozicijo v kateri je pojavljen/nastali lik, torej vrstico in stolpec, oz x in y koordinato na naši mreži
    {
        public int Vrstica { get; set; }
        public int Stolpec { get; set; }

        public Pozicija(int vrstica, int stolpec)   //konstruktor za preprostejši dostop do pozicije lika, ki
        {
            Vrstica = vrstica;
            Stolpec = stolpec;
        }
    }
}
