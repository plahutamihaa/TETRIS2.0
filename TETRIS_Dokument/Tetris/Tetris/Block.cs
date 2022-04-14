using System.Collections.Generic;
namespace Tetris
{
    public abstract class Block  //class, ki nam pove kateri block je nastal in njegova pozicija od roba in vrha
    {
        protected abstract Pozicija[][] Tiles { get; }
        protected abstract Pozicija StartOffset { get; }
        public abstract int Id { get; }

        private int rotationState;
        private Pozicija offset;

        public Block()
        {
            offset = new Pozicija(StartOffset.Vrstica, StartOffset.Stolpec);
        }

        public IEnumerable<Pozicija> TilePozicije()  //metoda pregleda tile pozicije in rotacije, ter odmik od roba ter vrha
        {
            foreach (Pozicija p in Tiles[rotationState])
            {
                yield return new Pozicija(p.Vrstica + offset.Vrstica, p.Stolpec + offset.Stolpec);
            }
        }

        public void ZavrtiDesno()  //metoda ki lik zavrti v desno
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        public void ZavrtiLevo()  //metoda ki lik zavrti v levo
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        public void Premakni(int vrstice, int stolpci)  //metoda ki premakne lik za doloceno stevilo vrstic in stolpcev
        {
            offset.Vrstica += vrstice;
            offset.Stolpec += stolpci;
        }

        public void Reset()  //resetira rotacijo in pozicijo lika
        {
            rotationState = 0;
            offset.Vrstica = StartOffset.Vrstica;
            offset.Stolpec = StartOffset.Stolpec;
        }
    }
}
