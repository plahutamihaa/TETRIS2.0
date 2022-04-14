namespace Tetris
{
    public class OBlock : Block
    {
        private readonly Pozicija[][] tiles = new Pozicija[][]
        {
            new Pozicija[] { new(0,0), new(0,1), new(1,0), new(1,1) },
            new Pozicija[] { new(0,0), new(0,1), new(1,0), new(1,1) },
            new Pozicija[] { new(0,0), new(0,1), new(1,0), new(1,1) },
            new Pozicija[] { new(0,0), new(0,1), new(1,0), new(1,1) }
        };

        public override int Id => 4;
        protected override Pozicija StartOffset => new Pozicija(0, 4);
        protected override Pozicija[][] Tiles => tiles;

    }
}
