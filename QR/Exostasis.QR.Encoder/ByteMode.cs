using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exostasis.QR.Encoder
{
    public class ByteMode : EncoderBase
    {
        public ByteMode (string unencodedString) : base (unencodedString)
        {
            _modeIndicator = new BitArray(BitConverter.GetBytes(0x01));
            _modeIndicator.Length = 4;
            _dataPerBitString = 1;
            _bitsPerBitString = 8;
            _maximumPossibleCharacterCount = 2953;

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
                    _bitsPerCharacterCountIndicator = 8;
                    break;
                case 9:
                    _bitsPerCharacterCountIndicator = 16;
                    break;
                case 26:
                    _bitsPerCharacterCountIndicator = 16;
                    break;
            }
        }

        protected override List<BitArray> Encode()
        {
            List<BitArray> bitArrays = new List<BitArray>();

            bitArrays.Add(_modeIndicator);
            bitArrays.Add(new BitArray(_characterCountIndicator));

            for (int i = 0; i < _unencodedString.Length; ++i)
            {
                var temp = Encoding.GetEncoding("ISO-8859-1").GetBytes(_unencodedString.ToCharArray(), i, 1);
                bitArrays.Add(new BitArray(temp));
                bitArrays.Last().Length = 8;
            }

            return bitArrays;
        }
    }
}
