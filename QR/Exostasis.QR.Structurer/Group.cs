using System.Collections.Generic;

namespace Exostasis.QR.Structurer
{
    public class Group
    {
        public List<Block> _blocks { get; private set; }
        public Group ()
        {
            _blocks = new List<Block>();
        }

        public void AddBlock (Block block)
        {
            _blocks.Add(block);
        }
    }
}
