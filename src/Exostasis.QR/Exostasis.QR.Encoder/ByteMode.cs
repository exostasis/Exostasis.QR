using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QREncoder
{
    public class ByteMode : EncoderBase
    {
        public ByteMode (string unencodedString) : base (unencodedString)
        {
            ModeIndicator = new BitArray(BitConverter.GetBytes(0x04));
            ModeIndicator.Length = 4;
            DataPerBitString = 1;
            BitsPerBitString = 8;
            MaximumPossibleCharacterCount = 2953;

            if (UnencodedString.Length > MaximumPossibleCharacterCount)
            {
                throw new Exception("This string is unable to be converted into a qr code due to the length of it");
            }

            DetermineMinimumVersionAndMaximumErrorCorrection();
            CharacterCountIndicator.Length = BitsPerCharacterCountIndicator;
        }

        protected override void DetermineBitsPerCharacterCountIndicator()
        {
            switch (Version)
            {
                case 0:
                    BitsPerCharacterCountIndicator = 8;
                    break;
                case 9:
                    BitsPerCharacterCountIndicator = 16;
                    break;
                case 26:
                    BitsPerCharacterCountIndicator = 16;
                    break;
            }
        }

        protected override List<BitArray> Encode()
        {
            List<BitArray> bitArrays = new List<BitArray>();

            bitArrays.Add(ModeIndicator);
            bitArrays.Add(new BitArray(CharacterCountIndicator));

            for (int i = 0; i < UnencodedString.Length; ++i)
            {
                var temp = Encoding.GetEncoding("ISO-8859-1").GetBytes(UnencodedString.ToCharArray(), i, 1);
                bitArrays.Add(new BitArray(temp));
                bitArrays.Last().Length = 8;
            }

            return bitArrays;
        }
    }
}
