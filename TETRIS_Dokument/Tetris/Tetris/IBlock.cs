namespace Tetris
{
    public class IBlock : Block
    {
        private readonly Pozicija[][] tiles = new Pozicija[][]
        {
            new Pozicija[] { new(1,0), new(1,1), new(1,2), new(1,3) },
            new Pozicija[] { new(0,2), new(1,2), new(2,2), new(3,2) },
            new Pozicija[] { new(2,0), new(2,1), new(2,2), new(2,3) },
            new Pozicija[] { new(0,1), new(1,1), new(2,1), new(3,1) }
        };

        public override int Id => 1;
        protected override Pozicija StartOffset => new Pozicija(-1, 3);
        protected override Pozicija[][] Tiles => tiles;

    }
}
