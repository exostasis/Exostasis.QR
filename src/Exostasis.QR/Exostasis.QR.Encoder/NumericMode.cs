using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QREncoder
{
    public class NumericMode : EncoderBase
    {
        public NumericMode (string unencodedString) : base (unencodedString)
        {
            ModeIndicator = new BitArray(BitConverter.GetBytes(0x01));
            ModeIndicator.Length = 4;
            DataPerBitString = 3;
            BitsPerBitString = 10;
            MaximumPossibleCharacterCount = 7089;

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
                    BitsPerCharacterCountIndicator = 10;
                    break;
                case 9:
                    BitsPerCharacterCountIndicator = 12;
                    break;
                case 26:
                    BitsPerCharacterCountIndicator = 14;
                    break;
            }
        }

        protected override List<BitArray> Encode()
        {
            List<BitArray> bitArrays = new List<BitArray>();

            bitArrays.Add(ModeIndicator);
            bitArrays.Add(new BitArray(CharacterCountIndicator));

            for (int i = 0; i < UnencodedString.Length; i +=3)
            {
                int charsLeftToCopy = UnencodedString.Length - i;
                var tempValue = int.Parse(UnencodedString.Substring(i, charsLeftToCopy >= 3 ? 3 : charsLeftToCopy));

                bitArrays.Add(new BitArray(BitConverter.GetBytes(tempValue)));
                if (charsLeftToCopy >= 3)
                {
                    bitArrays.Last().Length = 10;
                }
                else if (charsLeftToCopy == 2)
                {
                    bitArrays.Last().Length = 7;
                }
                else
                {
                    bitArrays.Last().Length = 4;
                }
            }

            return bitArrays;
        }
    }
}
