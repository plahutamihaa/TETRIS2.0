using System;
namespace Tetris
{
    public class CakalniBlock
    {
        private readonly Block[] blocks = new Block[]   //tabela z vsemo 7 blocki Tetrisa
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random random = new Random();      //random generator

        public Block NextBlock { get; private set; }    //lastnost za naslednji block v cakanju in so bo prikazoval na zaslonu, da lahko igralec ve kaj prihaja

        public CakalniBlock()       //konstruktor, ki inicializira NextBlock z likom, ki nam ga vrne random generator v metodi RandomBlock
        {
            NextBlock = RandomBlock();
        }
        private Block RandomBlock()     //metoda, ki vrne random/nakljično izbran block
        {
            return blocks [random.Next(blocks.Length)];     //vrne enega izmed blockov, katere imao shranjene v tabeli blocks, izbere pa naključno število izmed 7, ki je velikost tabele blocks
        }

        public Block ZamenjajNaslednjiBlock()  //naredi in zamenja naslednji block v cakalni vrsti, tako da se enak block ne ponovi
        {
            Block block = NextBlock;    //block, katerega sedaj uporabljamo, zamenja z novogeneriranim NextBlock-om

            do
            {
                NextBlock = RandomBlock();      //to pa dela toliko časa, dokler ne dobi blocka, kateri ni bil zgeneriran nazadnje, da se ne ponavljajo
            }
            while (block.Id == NextBlock.Id);

            return block;
        }
    }
}
