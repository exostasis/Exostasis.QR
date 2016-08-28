using System;
using System.Collections.Generic;

namespace Exostasis.QR.Structurer
{
    public class Block
    {
        public List<Byte> _codeWords { get; private set; }
        public List<Byte> _ecWords { get; private set; }

        public Block (Byte[] codeWords, Byte[] ecWords)
        {
            _codeWords = new List<Byte>(codeWords);
            _ecWords = new List<Byte>(ecWords);
        }
    }
}
