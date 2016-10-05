using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QREncoder
{
    public class AlphanumericMode : EncoderBase
    {
        public AlphanumericMode(string unencodedString) : base (unencodedString)
        {         
            ModeIndicator = new BitArray(BitConverter.GetBytes(0x02));
            ModeIndicator.Length = 4;           
            DataPerBitString = 2;
            BitsPerBitString = 11;
            MaximumPossibleCharacterCount = 4926;
            UnencodedString = unencodedString.ToUpper();

            if (UnencodedString.Length > MaximumPossibleCharacterCount)
            {
                throw new Exception("This string is unable to be converted into a qr code due to the length of it");
            }

            DetermineMinimumVersionAndMaximumErrorCorrection();
            CharacterCountIndicator.Length = BitsPerCharacterCountIndicator;
        }

        protected override List<BitArray> Encode()
        {
            List<BitArray> bitArrays = new List<BitArray>();

            bitArrays.Add(new BitArray(ModeIndicator));
            bitArrays.Add(new BitArray(CharacterCountIndicator));       

            for (int i = 0; i < UnencodedString.Length; i += 2)
            {
                int tempValue;
                if (i + 1 >= UnencodedString.Length)
                {
                    bitArrays.Add(new BitArray(6));
                    tempValue = GetIntValueOfChar(UnencodedString.ElementAt(i));
                }
                else
                {
                    bitArrays.Add(new BitArray(BitsPerBitString));
                    tempValue = GetIntValueOfChar(UnencodedString.ElementAt(i)) * 45 + 
                        GetIntValueOfChar(UnencodedString.ElementAt(i + 1));
                }

                var tempBytes = BitConverter.GetBytes(tempValue);

                for (int j = 0; j < bitArrays.Last().Count; j++)
                {
                    bitArrays.Last().Set(j, (tempBytes[j/8] & (1 << (j % 8))) != 0);
                }
            }

            return bitArrays;
        }

        protected override void DetermineBitsPerCharacterCountIndicator()
        {
            switch (Version)
            {
                case 0:
                    BitsPerCharacterCountIndicator = 9;
                    break;
                case 9:
                    BitsPerCharacterCountIndicator = 11;
                    break;
                case 26:
                    BitsPerCharacterCountIndicator = 13;
                    break;
            }
        }

        private int GetIntValueOfChar(char Char)
        {
            switch(Char)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return (int) char.GetNumericValue(Char);
                case 'A':
                    return 10;
                case 'B':
                    return 11;
                case 'C':
                    return 12;
                case 'D':
                    return 13;
                case 'E':
                    return 14;
                case 'F':
                    return 15;
                case 'G':
                    return 16;
                case 'H':
                    return 17;
                case 'I':
                    return 18;
                case 'J':
                    return 19;
                case 'K':
                    return 20;
                case 'L':
                    return 21;
                case 'M':
                    return 22;
                case 'N':
                    return 23;
                case 'O':
                    return 24;
                case 'P':
                    return 25;
                case 'Q':
                    return 26;
                case 'R':
                    return 27;
                case 'S':
                    return 28;
                case 'T':
                    return 29;
                case 'U':
                    return 30;
                case 'V':
                    return 31;
                case 'W':
                    return 32;
                case 'X':
                    return 33;
                case 'Y':
                    return 34;
                case 'Z':
                    return 35;
                case ' ':
                    return 36;
                case '$':
                    return 37;
                case '%':
                    return 38;
                case '*':
                    return 39;
                case '+':
                    return 40;
                case '-':
                    return 41;
                case '.':
                    return 42;
                case '/':
                    return 43;
                case ':':
                    return 44;
            }

            throw new Exception("Invalid char");
        }
    }
}
