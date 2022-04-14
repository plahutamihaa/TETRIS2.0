using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-EmptyNew.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-EmptyNew.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 20;

        private GameState gameState = new GameState(false);
        


        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.IgralnaMreza);
        }

        private Image[,] SetupGameCanvas(IgralnaMreza grid)
        {
            Image[,] imageControls = new Image[grid.Vrstice, grid.Stolpci];
            int cellSize = 25;

            for (int r = 0; r < grid.Vrstice; r++)
            {
                for (int c = 0; c < grid.Stolpci; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;  //2D array z eno slikico v vsakem prostorcku, zgornji dve vrstici pa sta nad CANVAS in se jih zato ne vidi
        }

        private void DrawGrid(IgralnaMreza grid)
        {
            for (int r = 0; r < grid.Vrstice; r++)
            {
                for (int c = 0; c < grid.Stolpci; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;  //vidnost blocka da nazaj na 1, ker se je pri predogledu kam bo padel zmanjsala na 0.25
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach(Pozicija p in block.TilePozicije())
            {
                imageControls[p.Vrstica, p.Stolpec].Opacity = 1;  //vidnost blocka da nazaj na 1, ker se je pri predogledu kam bo padel zmanjsala na 0.25
                imageControls[p.Vrstica, p.Stolpec].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(CakalniBlock cakalniBlock)
        {
            Block next = cakalniBlock.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawGhostBlock(Block block)  //narise kam bo nas block oziroma lik priletel
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach (Pozicija p in block.TilePozicije())
            {
                imageControls[p.Vrstica + dropDistance, p.Stolpec].Opacity = 0.25;
                imageControls[p.Vrstica + dropDistance, p.Stolpec].Source = tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)
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
            while (!gameState.KonecIgre && !gameState.UstavitevIgre && gameState.ZačetniMenu) //////////////////////////////////////////////////////////000000000000000000000000
            {
                
                int delay = (maxDelay - (gameState.PocisceneVrstice * delayDecrease));           //za vsako pocisceno vrstico se pospesi
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
            switch (e.Key)
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
                    gameState.JeIgraUstavljena();
                    break;

                case Key.Enter:
                    gameState.ZačetekIgre();
                    GameStartMenu.Visibility = Visibility.Hidden;
                    await GameLoop();
                    break;

                default:
                    return;
            }
            
            Draw(gameState);
        }



        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void ZacniPonovnoKLIK(object sender, RoutedEventArgs e)
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
