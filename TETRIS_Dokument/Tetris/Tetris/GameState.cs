namespace Tetris
{
    public class GameState  //interakcije med deli katere smo napisali do sedaj
    {
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();  //po tem ko posodobimo sedajsnji block, se klice metoda reset, ki nastavi pravilno startno rotacijo in pozicijo

                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Premakni(1, 0);

                    if(PrileganjeBlocka())
                    {
                        currentBlock.Premakni(-1, 0);
                    }
                }
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
            IgralnaMreza = new IgralnaMreza(22, 10);
            CakalniBlock = new CakalniBlock();
            CurrentBlock = CakalniBlock.ZamenjajNaslednjiBlock();
            ZačetniMenu = zacetniMeni;
            Level = 1;
        }

        private bool PrileganjeBlocka()  //preveri ali je block v mozni poziciji ali ne, ali bo segel cez rob mreze ali drug block
        {
            foreach (Pozicija p in CurrentBlock.TilePozicije())
            {
                if (!IgralnaMreza.AliJeCelicaPrazna(p.Vrstica, p.Stolpec))
                {
                    return false;
                }
            }
            return true;
        }

        public void ZvartiBlockDesno()  //zavrti block desno, ce pa ta pristane v nemogoci poziciji, pa se zavrti nazaj
        {
            CurrentBlock.ZavrtiDesno();

            if(!PrileganjeBlocka())
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



        private bool JeIgraKoncana()
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
        

        private void PostaviBlock()
        {
            foreach (Pozicija p in CurrentBlock.TilePozicije())
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
                KonecIgre = true;
            }
            else if (UstavitevIgre == true)
            {
                UstavitevIgre = true;
            }
            else
            {
                CurrentBlock = CakalniBlock.ZamenjajNaslednjiBlock();
            }
            

            
            
        }

        public void PremakniBlockDol()
        {
            CurrentBlock.Premakni(1, 0);

            if(!PrileganjeBlocka())
            {
                CurrentBlock.Premakni(-1, 0);
                PostaviBlock();
            }
        }

        private int TileDropDistance(Pozicija p)  //metoda ugotovi za koliko vrstic se lahko posamezna kocka lika premakne navzdol
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

            foreach (Pozicija p in CurrentBlock.TilePozicije())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }
            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.Premakni(BlockDropDistance(), 0);
            PostaviBlock();
        }

    }
}
