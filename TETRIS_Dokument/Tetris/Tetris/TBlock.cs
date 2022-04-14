namespace Tetris
{
    public class TBlock : Block
    {
        private readonly Pozicija[][] tiles = new Pozicija[][]
        {
            new Pozicija[] { new(0,1), new(1,0), new(1,1), new(1,2) },
            new Pozicija[] { new(0,1), new(1,1), new(1,2), new(2,1) },
            new Pozicija[] { new(1,0), new(1,1), new(1,2), new(2,1) },
            new Pozicija[] { new(0,1), new(1,0), new(1,1), new(2,1) }
        };

        public override int Id => 6;
        protected override Pozicija StartOffset => new Pozicija(0, 3);
        protected override Pozicija[][] Tiles => tiles;

    }
}
