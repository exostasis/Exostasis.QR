using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exostasis.QR.Encoder
{
    public class AlphanumericMode : EncoderBase
    {
        public AlphanumericMode(string unencodedString) : base (unencodedString)
        {         
            _modeIndicator = new BitArray(BitConverter.GetBytes(0x02));
            _modeIndicator.Length = 4;           
            _dataPerBitString = 2;
            _bitsPerBitString = 11;
            _maximumPossibleCharacterCount = 4926;
            _unencodedString = unencodedString.ToUpper();

            if (_unencodedString.Length > _maximumPossibleCharacterCount)
            {
                throw new Exception("This string is unable to be converted into a qr code due to the length of it");
            }

            base.DetermineMinimumVersionAndMaximumErrorCorrection();
            _characterCountIndicator.Length = _bitsPerCharacterCountIndicator;
        }

        protected override List<BitArray> Encode()
        {
            List<BitArray> bitArrays = new List<BitArray>();

            int length = _unencodedString.Length;
            int tempValue = 0;

            Byte[] tempBytes;

            bitArrays.Add(new BitArray(_modeIndicator));
            bitArrays.Add(new BitArray(_characterCountIndicator));       

            for (int i = 0; i < _unencodedString.Length; i += 2)
            {
                if (i + 1 >= _unencodedString.Length)
                {
                    bitArrays.Add(new BitArray(6));
                    tempValue = GetIntValueOfChar(_unencodedString.ElementAt(i));
                }
                else
                {
                    bitArrays.Add(new BitArray(_bitsPerBitString));
                    tempValue = GetIntValueOfChar(_unencodedString.ElementAt(i)) * 45 + GetIntValueOfChar(_unencodedString.ElementAt(i + 1));
                }

                tempBytes = BitConverter.GetBytes(tempValue);

                for (int j = 0; j < bitArrays.Last().Count; j++)
                {
                    bitArrays.Last().Set(j, ((tempBytes[j/8] & (1 << (j % 8))) != 0));
                }
            }

            return bitArrays;
        }

        protected override void DetermineBitsPerCharacterCountIndicator()
        {
            switch (_version)
            {
                case 0:
                    _bitsPerCharacterCountIndicator = 9;
                    break;
                case 9:
                    _bitsPerCharacterCountIndicator = 11;
                    break;
                case 26:
                    _bitsPerCharacterCountIndicator = 13;
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
                    return (int) Char.GetNumericValue(Char);
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
