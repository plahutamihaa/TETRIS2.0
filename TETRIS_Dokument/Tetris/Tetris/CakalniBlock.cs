using System;
namespace Tetris
{
    public class CakalniBlock
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random random = new Random();

        public Block NextBlock { get; private set; }

        public CakalniBlock()
        {
            NextBlock = RandomBlock();
        }
        private Block RandomBlock()
        {
            return blocks [random.Next(blocks.Length)];
        }

        public Block ZamenjajNaslednjiBlock()  //naredi in zamenja naslednji block v cakalni vrsti, tako da se enak block ne ponovi
        {
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);

            return block;
        }
    }
}
