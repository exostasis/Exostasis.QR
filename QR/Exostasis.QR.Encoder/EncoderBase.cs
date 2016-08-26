using Exostasis.QR.Common;
using QREncoder.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exostasis.QR.Encoder
{
    public abstract class EncoderBase
    {
        private readonly Byte[] _padBytes = {0xEC, 0x11}; 

        public int _version { get; protected set; }

        protected BitArray _characterCountIndicator { get; set; }

        protected BitArray _modeIndicator { get; set; }

        protected Byte[] _encodedBytes { get; private set; }

        protected string _unencodedString { get; set; }

        public ErrorCorrectionLevel _errorCorrectionLevel { get; protected set; }

        protected int _dataPerBitString { get; set; }

        protected int _bitsPerBitString { get; set; }

        protected int _bitsPerCharacterCountIndicator { get; set; }

        protected int _maximumPossibleCharacterCount { get; set; }

        protected abstract List<BitArray> Encode();

        protected abstract void DetermineBitsPerCharacterCountIndicator();

        private int GetRequiredDataBits ()
        {
            return 8 * Constants._codewordTable[_version, (int)_errorCorrectionLevel];
        }

        private int GetDifferenceBetweenRequiredVsActual (List<BitArray> bitArray)
        {
            return GetRequiredDataBits() - CalculateListBitArrayDataBitCount(bitArray);
        }

        private int CalculateListBitArrayDataBitCount(List<BitArray> bitArray)
        {
            int sum = 0;

            foreach (BitArray tempBitArray in bitArray)
            {
                sum += tempBitArray.Length;
            }

            return sum;
        }

        private void Terminate(List<BitArray> bitArray)
        {
            int differenceBetweenRequiredVsActual = GetDifferenceBetweenRequiredVsActual(bitArray);
            if (differenceBetweenRequiredVsActual >= 4)
            {
                bitArray.Add(new BitArray(4));
            }
            else if (differenceBetweenRequiredVsActual < 4 && differenceBetweenRequiredVsActual > 0)
            {
                bitArray.Add(new BitArray(differenceBetweenRequiredVsActual));
            }
        }

        private void MakeMultipleOf8(List<BitArray> bitArray)
        {
            int dataBitCount = CalculateListBitArrayDataBitCount(bitArray);
            int numberOf0sToAdd = 8 - (dataBitCount % 8);

            if (numberOf0sToAdd > 0)
            {
                bitArray.Add(new BitArray(numberOf0sToAdd));
            }
        }

        private void Pad(List<BitArray> bitArray)
        {

            int numberOfBytesToAdd = GetDifferenceBetweenRequiredVsActual(bitArray) / 8;

            for(int i = 0; i < numberOfBytesToAdd; ++ i)
            {
                bitArray.Add(new BitArray(BitConverter.GetBytes(_padBytes[i % 2])));
                bitArray.Last().Length = 8;
            }
        }

        private Byte[] ConvertListBitArrayToByteArray(List<BitArray> bitArray)
        {
            List <Byte> bytes = new List<Byte>();
            int currentSpot = 7;
            int tempByte = 0;

            foreach (BitArray array in bitArray)
            {
                for (int i = array.Count - 1; i >= 0; i--)
                {         
                    if (array[i])
                    {
                        tempByte = tempByte | (1 << currentSpot);
                    }
                    --currentSpot;
                    if (currentSpot == -1)
                    {
                        bytes.Add(Convert.ToByte(tempByte));
                        tempByte = 0;
                        currentSpot = 7;
                    }
                }
            }

            return bytes.ToArray();
        }

        protected void DetermineMinimumVersionAndMaximumErrorCorrection()
        {
            int MaximumLength = 0;

            for (int i = (int)ErrorCorrectionLevel.H; i <= 0; ++i)
            {
                _errorCorrectionLevel = (ErrorCorrectionLevel)i;

                for (int j = 0; j < 40; ++j)
                {
                    _version = j;

                    DetermineBitsPerCharacterCountIndicator();

                    MaximumLength = GetMaximumCharacterCount();

                    if (MaximumLength >= _unencodedString.Length)
                    {
                        break;
                    }
                }

                if (MaximumLength >= _unencodedString.Length)
                {
                    break;
                }
            }
        }

        protected int GetMaximumCharacterCount()
        {
            return ((GetRequiredDataBits() - 4 - _bitsPerCharacterCountIndicator) * _dataPerBitString / _bitsPerBitString);
        }

        public EncoderBase(string unencodedString)
        {
            _unencodedString = unencodedString;
            _characterCountIndicator = new BitArray(BitConverter.GetBytes(_unencodedString.Length));
        }

        public Byte[] DataEncode()
        {
            List<BitArray> bitArray;
            bitArray = Encode();
            Terminate(bitArray);
            MakeMultipleOf8(bitArray);
            Pad(bitArray);
            _encodedBytes = ConvertListBitArrayToByteArray(bitArray);
            
            return _encodedBytes;
        }
    }
}
