using System.Collections.Generic;

namespace Exostasis.QR.Structurer
{
    public class Block
    {
        public List<byte> CodeWords { get; private set; }
        public List<byte> EcWords { get; private set; }

        public Block (byte[] codeWords, byte[] ecWords)
        {
            CodeWords = new List<byte>(codeWords);
            EcWords = new List<byte>(ecWords);
        }
    }
}
