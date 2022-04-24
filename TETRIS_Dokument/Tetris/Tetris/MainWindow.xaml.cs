using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;  //WPF je project template in .NET 5.0 

namespace Tetris
{
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]       //tabela ki vsebuje vse slikice ploščic
        {
            new BitmapImage(new Uri("Assets/Block-EmptyNew.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),      //index 1, ker je I-block id enak 1 in je cyan barve
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),      //enako za vse
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-EmptyNew.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),       //index 1, ker je I-block id enak 1 in je cyan barve
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;        //2D tabela
        private readonly int maxDelay = 1000;       //vrednosti, katere se bojo spreeminjale ob igranju in igro oteževale, saj se bo hitrost povečevala, oziroma čas med pojavljanjem manjšal
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 20;

        private GameState gameState = new GameState(false);     //postavimo še GameState objekt
        


        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.IgralnaMreza);     //v konstruktorju sedaj inicializiramo tabelo image controls-ov tako, da kličemo to metodo
        }

        private Image[,] SetupGameCanvas(IgralnaMreza grid)     
        {
            Image[,] imageControls = new Image[grid.Vrstice, grid.Stolpci];     //tabelo naredimo enako veliko kot je naša igralna mreža
            int cellSize = 25;      //spremenjljivka, ki določa višino in širino vsake ploščice posebej (širino smo nastavili na 250, torej 25x10, višino pa na 500), 25pixlov za vsako ploščico

            for (int r = 0; r < grid.Vrstice; r++)
            {
                for (int c = 0; c < grid.Stolpci; c++)
                {
                    Image imageControl = new Image      //sprehodimo se čez celotno tabelo igralne mreže in vsaki pozidiji določimo image control širine in višine 25 pixlov
                    {
                        Width = cellSize,       //vsaka celica ima eno "slikico"
                        Height = cellSize
                    };
                                                                                //+10 pa zato, da se blocka ki nas je npr. ubil, vidimo vsaj malo, da vem kaj nas je pokončalo
                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);       //-2 premakne zgornji dve vrstici nad vidno polje, saj se tam pojavljajo novi blocki in jih nočemo videti prej
                    Canvas.SetLeft(imageControl, c * cellSize);     //enako naredimo za širino, brez da bi kam zamaknili
                    GameCanvas.Children.Add(imageControl);      //sliko naredimo "otroka" našega platna/canvasa in jo dodamo v našo tabelo, ki bo vrnjena izven zanke - spodaj
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;  //2D array z eno slikico v vsakem prostorcku, zgornji dve vrstici pa sta nad platnom in se jih zato ne vidi
        }

        private void DrawGrid(IgralnaMreza grid)        //sedaj je preprosto narisati igralno mrežo tako, da kličemo to metodo
        {
            for (int r = 0; r < grid.Vrstice; r++)      //gremo čez vse pozicije
            {
                for (int c = 0; c < grid.Stolpci; c++)      //gremo čez vse pozicije
                {
                    int id = grid[r, c];        //za vsako pozicijo dobimo začetni id
                    imageControls[r, c].Opacity = 1;  //vidnost blocka da nazaj na 1, ker se je pri predogledu kam bo padel zmanjsala na 0.25
                    imageControls[r, c].Source = tileImages[id];        //image sounce nastavi z uporabo zgoraj klicanega id-ja
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach(Pozicija p in block.TilePozicije())     //sprehodi se čez pozicije ploščic in nastavi image source na enak način kot zgoraj
            {
                imageControls[p.Vrstica, p.Stolpec].Opacity = 1;  //vidnost blocka da nazaj na 1, ker se je pri predogledu kam bo padel zmanjsala na 0.25
                imageControls[p.Vrstica, p.Stolpec].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(CakalniBlock cakalniBlock)       //metoda, ki nam omogoči, da imam predogled naslednjega blocka, ki se bo pojavil
        {
            Block next = cakalniBlock.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawGhostBlock(Block block)  //narise kam bo nas block oziroma lik priletel
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach (Pozicija p in block.TilePozicije())        //vsaki ploščici blocka doda maksimalno drop distance in tako dobimo kje bi/bo block bil ko pade do dol, vidnost pa zmanjšamo
            {
                imageControls[p.Vrstica + dropDistance, p.Stolpec].Opacity = 0.25;
                imageControls[p.Vrstica + dropDistance, p.Stolpec].Source = tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)      //metoda ki nam nariše vse (mrežo, trenutni block, itd.)
        {
            DrawGrid(gameState.IgralnaMreza);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.CakalniBlock);
            ScoreText.Text = $"Točke: {gameState.Score}";
            LevelText.Text = $"Level: {gameState.Level}";
            PocisceneText.Text = $"Pociscene: {gameState.PocisceneVrstice}";

            string readText = File.ReadAllText("G:\\Tetris\\Tetris\\REZULTATI.txt");
            RekordText.Text = $"Rekord: {Int32.Parse(readText)}";
        }

        public void ZaustaviIgro()      //metoda ki konca celotno igro in preveri ce smo slucajno podrli rekord ter ga spremeni, uporabi pa se ko zgubimo ali sami koncamo
        {
            GameOverMenu.Visibility = Visibility.Visible;
            GamePauseMenu.Visibility = Visibility.Hidden;
            FinalScoreText.Text = $"Točke: {gameState.Score}";
            FinalLevelText.Text = $"Level: {gameState.Level}";
            FinalPodrtihText.Text = $"Počiščene vrstice: {gameState.PocisceneVrstice}";

            string readText = File.ReadAllText("G:\\Tetris\\Tetris\\REZULTATI.txt");
            if (Int32.Parse(readText) < gameState.Score)
            {
                File.WriteAllText("G:\\Tetris\\Tetris\\REZULTATI.txt", gameState.Score.ToString());
            }
        }
        private async Task GameLoop()
        {
            Draw(gameState);
            while (!gameState.KonecIgre && !gameState.UstavitevIgre && gameState.ZačetniMenu) //zanka se izvaja ves čas, dokler se ne izpolni eden izmed teh pogojev
            {
                
                int delay = (maxDelay - (gameState.PocisceneVrstice * delayDecrease));           //za vsako pocisceno vrstico se pospesi, na začetku pa je delax max = 1000
                await Task.Delay(delay);
                gameState.PremakniBlockDol();
                Draw(gameState);
            }

            if (gameState.UstavitevIgre)                                                                //ko kliknemo ESC se igra ustavi
            {
               GamePauseMenu.Visibility = Visibility.Visible;
                FinalScoreText.Text = $"Točke: {gameState.Score}";
                FinalLevelText.Text = $"Level: {gameState.Level}";
                FinalPodrtihText.Text = $"Počiščene vrstice: {gameState.PocisceneVrstice}";
                                                                                                         //za vsak level d ruga barva ozadnja //muska  //blocki se zasvetjo
            }
            else if (!gameState.ZačetniMenu)
            {
            
            }
            else
            {
                ZaustaviIgro();
            }
        }


        public bool JeIgraUstavljena = false;
        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.KonecIgre || gameState.UstavitevIgre) return;     //stavek, ki nam ob koncu ali ustavitvi igre preprečuje, da bi s tipkovnico lahko naredili karkoli

            switch (e.Key)      //switch stavek s katerim upravljamo trenutni lik in pause menu
            {
                case Key.Left:
                    gameState.PremakniBlockLevo();
                    break;
                case Key.Right:
                    gameState.PremakniBlockDesno();
                    break;
                case Key.Down:
                    gameState.PremakniBlockDol();
                    break;
                case Key.D:
                    gameState.ZvartiBlockDesno();
                    break;
                case Key.A:
                    gameState.ZvartiBlockLevo();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;

                case Key.Escape:
                    GameStartMenu.Visibility = Visibility.Hidden;
                    gameState.JeIgraUstavljena();
                    break;

                case Key.Enter:
                    gameState.ZačetekIgre();
                    GameStartMenu.Visibility = Visibility.Hidden;
                    await GameLoop();
                    break;

                default:        //ponovno narišemo samo v primeru, da je igralec kliknil tipko, ki sploh kaj naredi
                    return;
            }
            
            Draw(gameState);        //na koncu se vse nariše
        }



        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();       //ko naloži celotno platno/to kar mi od igre vidimo, pokličemo GameLoop in posledično tudi Draw metodo, ki je prva notri in vse nariše
        }

        private async void ZacniPonovnoKLIK(object sender, RoutedEventArgs e)       //tipka ki nam omogoči začetek ponovne igre
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        public async void NadaljujIgroKLIK(object sender, RoutedEventArgs e)  //vrni se nazaj na igranje igre
        {
            gameState.NadaljujIgro();
            GamePauseMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        public async void ResetirajIgroKLIK(object sender, RoutedEventArgs e)  //požene popolnoma novo igro
        {
            gameState = new GameState();
            GamePauseMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        public async void ZaključiIgroKLIK(object sender, RoutedEventArgs e)  //predčasno zaključi igro
        {
            ZaustaviIgro();
        }

    }
    
}
