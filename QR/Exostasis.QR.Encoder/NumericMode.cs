using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exostasis.QR.Encoder
{
    public class NumericMode : EncoderBase
    {
        public NumericMode (string unencodedString) : base (unencodedString)
        {
            _modeIndicator = new BitArray(BitConverter.GetBytes(0x01));
            _modeIndicator.Length = 4;
            _dataPerBitString = 3;
            _bitsPerBitString = 10;
            _maximumPossibleCharacterCount = 7089;

            if (_unencodedString.Length > _maximumPossibleCharacterCount)
            {
                throw new Exception("This string is unable to be converted into a qr code due to the length of it");
            }

            base.DetermineMinimumVersionAndMaximumErrorCorrection();
            _characterCountIndicator.Length = _bitsPerCharacterCountIndicator;
        }

        protected override void DetermineBitsPerCharacterCountIndicator()
        {
            switch (_version)
            {
                case 0:
                    _bitsPerCharacterCountIndicator = 10;
                    break;
                case 9:
                    _bitsPerCharacterCountIndicator = 12;
                    break;
                case 26:
                    _bitsPerCharacterCountIndicator = 14;
                    break;
            }
        }

        protected override List<BitArray> Encode()
        {
            List<BitArray> bitArrays = new List<BitArray>();

            bitArrays.Add(_modeIndicator);
            bitArrays.Add(new BitArray(_characterCountIndicator));

            for (int i = 0; i < _unencodedString.Length; i +=3)
            {
                int charsLeftToCopy = _unencodedString.Length - i;
                var tempValue = int.Parse(_unencodedString.Substring(i, (charsLeftToCopy >= 3) ? 3 : charsLeftToCopy));

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
