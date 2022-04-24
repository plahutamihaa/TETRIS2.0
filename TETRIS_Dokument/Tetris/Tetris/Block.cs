using System.Collections.Generic;
namespace Tetris
{
    public abstract class Block  //class, ki nam pove kateri block je nastal in njegova pozicija od roba in vrha
    {
        protected abstract Pozicija[][] Tiles { get; }      //2D tabela s pozicijami vsake ploščice v 4x4 tabeli, katere bomo kasneje uporabili pri vsakem podrazredu, ki bo predstavljal posamezen block
        protected abstract Pozicija StartOffset { get; }    //StartOffSet, ki nam pove kje na tabeli/igralni površini se bo block pojavil, ob nastanku/pojavitvi/generiranju, tudi ima vsak izmed blockov
        public abstract int Id { get; }     //Id bo vsakemu izmed sedmih blockov določil številko oz. id, s katerim bomo posamezne blocke identificirali

        private int rotationState;      //int, ki nam pove stanje rotacije, katerih ima vsak block 4, torej 1x zavrten, 2x, 3x, ko pa se četrtič zavrti, pa je nazaj na svojem mestu
        private Pozicija offset;        //za vsak block se tudi shranjuje trenutni offset, ki nam pove koliko je block oddaljen od zgornjega in spodnjega roba

        public Block()      //takoj že na začetku, moramo offset, nastaviti na StartOffSet, ki nam block postavi v začetno pozicijo, katero ima vsak block drugačno na začetku
        {
            offset = new Pozicija(StartOffset.Vrstica, StartOffset.Stolpec);    //posebej offset za vrstico in stolpec, torej Vrstica=koliko je oddaljen od vrha in Stolpec=koliko je oddaljen od levega roba
        }

        public IEnumerable<Pozicija> TilePozicije()  //metoda, ki vrne pocicije na mreži, katere zasega posamezen block, upošteva pa še stanje rotacije (eno izmed 4) ter odmik od robov (offset)
        {
            foreach (Pozicija p in Tiles[rotationState])
            {
                yield return new Pozicija(p.Vrstica + offset.Vrstica, p.Stolpec + offset.Stolpec);
            }
        }

        public void ZavrtiDesno()  //metoda ki lik zavrti v desno
        {
            rotationState = (rotationState + 1) % Tiles.Length;     //stanje rotacije poveča za 1, če pa lik zarotiramo 4x, pa nastavi satanje na 0, saj ko lik zavrtimo 4x, pride nazaj na isto mesto kot na začetku
        }

        public void ZavrtiLevo()  //metoda ki lik zavrti v levo
        {
            if (rotationState == 0)     //enako kot za desno rotiranje, le da, imamo dodaten pogoj, ki preveri, če je stanje rotacije 0, in če je, od 4 (velikost tabele) oz zadnja možna rotacija odšteje 1
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;    //stanje rotacije zmanjša za 1, če stanje rotacije ni enako 0
            }
        }

        public void Premakni(int vrstice, int stolpci)  //metoda ki premakne lik za doloceno stevilo vrstic in stolpcev
        {
            offset.Vrstica += vrstice;      //offset, ki pomeni odmik od zgornjega in levega roba
            offset.Stolpec += stolpci;
        }

        public void Reset()  //reset metoda resetira stanje rotacije in pozicijo lika nazaj na osnovno oziroma 0
        {
            rotationState = 0;
            offset.Vrstica = StartOffset.Vrstica;
            offset.Stolpec = StartOffset.Stolpec;
        }
    }
}
