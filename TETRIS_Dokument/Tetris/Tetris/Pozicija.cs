namespace Tetris
{
    public class Pozicija  //naredimo class, ki nam pove pozicijo v kateri je pojavljen/nastali lik
    {
        public int Vrstica { get; set; }
        public int Stolpec { get; set; }

        public Pozicija(int vrstica, int stolpec)
        {
            Vrstica = vrstica;
            Stolpec = stolpec;
        }
    }
}
