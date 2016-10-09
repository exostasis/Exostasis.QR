using System.Collections.Generic;

namespace Exostasis.QR.Structurer
{
    public class Group
    {
        public List<Block> Blocks { get; private set; }
        public Group ()
        {
            Blocks = new List<Block>();
        }

        public void AddBlock (Block block)
        {
            Blocks.Add(block);
        }
    }
}
