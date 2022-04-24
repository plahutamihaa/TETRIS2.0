namespace Tetris
{
    public class GameState  //interakcije med deli katere smo napisali do sedaj
    {
        private Block currentBlock;

        public Block CurrentBlock       //lastnost CurrentBlock, ki je block s katerim ta trenutek opravljamo, oziroma sedanji block
        {
            get => currentBlock;    //pokliče/dobi vrednost, ki jo ime currentBlock, ter blockove lastnosti (rotacijo, pozicijo) resetira na začetno postavitev
            private set
            {
                currentBlock = value;   //s tem posodobimo currentBlock
                currentBlock.Reset();  //po tem ko posodobimo sedajsnji block, se klice metoda reset, ki nastavi pravilno startno rotacijo in pozicijo
            }
        }


        public IgralnaMreza IgralnaMreza { get; }
        public CakalniBlock CakalniBlock { get; }
        public bool KonecIgre { get; private set;  }
        public bool UstavitevIgre { get; private set; }
        public bool ZačetniMenu { get; private set; }
        public int Score { get; private set; }
        public int Level { get; private set; }
        public int PocisceneVrstice { get; private set; }

        
        public GameState(bool zacetniMeni = true)
        {
            IgralnaMreza = new IgralnaMreza(22, 10);    //nastavimo velikost igralne mreže na željeno
            CakalniBlock = new CakalniBlock();      //inicializiramo tudi CakalniBlock, ter ga spodaj uporabimo, da dobimo CurrentBlock, oziroma sedanji block
            CurrentBlock = CakalniBlock.ZamenjajNaslednjiBlock();
            ZačetniMenu = zacetniMeni;
            Level = 1;
        }

        private bool PrileganjeBlocka()  //preveri ali je block v mozni poziciji ali ne, ali bo segel cez rob mreze ali drug block
        {
            foreach (Pozicija p in CurrentBlock.TilePozicije())     //preveri pozicijo vsake ploščice izmed tistih, ki sestavljajo block, če slučajno katera sega čez igralno mrežo ali drugo ploščico
            {
                if (!IgralnaMreza.AliJeCelicaPrazna(p.Vrstica, p.Stolpec))      //če katera sega, vrnemo false
                {
                    return false;
                }
            }
            return true;    //če pa nam uspe priti čez celoten loop, nam vrne true
        }

        public void ZvartiBlockDesno()  //zavrti block desno, ce pa ta pristane v nemogoci poziciji, pa se zavrti nazaj
        {
            CurrentBlock.ZavrtiDesno();

            if(!PrileganjeBlocka())     //prileganje preverja z zgoraj napisano metodo
            {
                CurrentBlock.ZavrtiLevo();
            }
        }

        public void ZvartiBlockLevo()   //zavrti block levo, ce pa ta pristane v nemogoci poziciji, pa se zavrti nazaj
        {
            CurrentBlock.ZavrtiLevo();

            if (!PrileganjeBlocka())
            {
                CurrentBlock.ZavrtiDesno();
            }
        }

        public void PremakniBlockDesno()  //premakne block desno, ce pa ta pristane v nemogoci poziciji, pa se premakne nazaj
        {
            CurrentBlock.Premakni(0, 1);

            if (!PrileganjeBlocka())
            {
                CurrentBlock.Premakni(0, -1);
            }
        }

        public void PremakniBlockLevo()  //premakne block levo, ce pa ta pristane v nemogoci poziciji, pa se premakne nazaj
        {
            CurrentBlock.Premakni(0, -1);

            if (!PrileganjeBlocka())
            {
                CurrentBlock.Premakni(0, 1);
            }
        }

        public void ZačetekIgre()
        {
            ZačetniMenu = true;
        }



        private bool JeIgraKoncana()    //vrne true, če katera izmed zgornjih treh vrstic ni prazna in smo igro izgubili
        {
            return !(IgralnaMreza.AliJeVrsticaNedokoncana(0) && IgralnaMreza.AliJeVrsticaNedokoncana(1) && IgralnaMreza.AliJeVrsticaNedokoncana(2));
        }
        
        public void JeIgraUstavljena()      
        {
            UstavitevIgre = true;
        }

        public void NadaljujIgro()
        {
            UstavitevIgre = false;
        }
        

        private void PostaviBlock()     //metoda je klicana, v primeru da block ne more biti premaknjen dol, in izvede naslednje stvari
        {
            foreach (Pozicija p in CurrentBlock.TilePozicije())     //preleti čez vse pozicije ploščic za uporabljen block in nastavi na tiste od CurrentBlock-a, oz trenutnega blocka
            {
                IgralnaMreza[p.Vrstica, p.Stolpec] = CurrentBlock.Id;
            }


            int SteviloTock = IgralnaMreza.IzbrisiPolneVrstice();               //zanka ki preveri koliko vrstic se je izbrisalo in na podlagi tega doloci za koliko se bo score povecal
            switch (SteviloTock)
            {
                case 1: Score += 40*(Level); PocisceneVrstice += 1; break;
                case 2: Score += 100*(Level); PocisceneVrstice += 2; break;
                case 3: Score += 300*(Level); PocisceneVrstice += 3; break; 
                case 4: Score += 1200*(Level); PocisceneVrstice += 4; break;    
            }
            Level = PocisceneVrstice / 10 + 1;                                   //v PocisceneVrstice se zapisuje koliko vrstic smo ze pocistili, vsakih 10 vrstic pa se nam poviša level

            
            if (JeIgraKoncana() == true)
            {
                KonecIgre = true;       //če je igra končana, nastavimo lastnost KonecIgre na true
            }
            else if (UstavitevIgre == true)
            {
                UstavitevIgre = true;
            }
            else
            {
                CurrentBlock = CakalniBlock.ZamenjajNaslednjiBlock();   //če pa igra ni končana ali ustavljena, pa samo zamenjamo trenutni block za cakalnega
            }        
        }


        public void PremakniBlockDol()      //deluje na enak način kot ostale metode za premikanje, le da ta, če block ne more biti premaknjen dol, še pokliče metodo PostaviBlock
        {
            CurrentBlock.Premakni(1, 0);

            if(!PrileganjeBlocka())
            {
                CurrentBlock.Premakni(-1, 0);
                PostaviBlock();
            }
        }

        private int TileDropDistance(Pozicija p)  //metoda ugotovi za koliko vrstic se lahko posamezna kocka lika premakne navzdol in vzamemo najmanjse število, saj mora veljati za cel lik
        {
            int drop = 0;

            while (IgralnaMreza.AliJeCelicaPrazna(p.Vrstica + drop + 1, p.Stolpec))
            {
                drop++;
            }
            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = IgralnaMreza.Vrstice;

            foreach (Pozicija p in CurrentBlock.TilePozicije()) //metoda ugotovi za koliko vrstic se lahko posamezna kocka lika premakne navzdol in vzamemo najmanjse število, saj mora veljati za cel lik
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }
            return drop;        //vrne število, za kolikor se lahko cel block maximalno premakne, torej za koliko se lahko premaknejo vse ploščice, brez da bi katera šla čez karkoli
        }

        public void DropBlock()     //metoda, ki s pomočjo zgornjih dveh, premakne block do dol
        {
            CurrentBlock.Premakni(BlockDropDistance(), 0);
            PostaviBlock();
        }

    }
}
