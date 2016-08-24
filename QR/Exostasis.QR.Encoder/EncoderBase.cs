using QREncoder.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exostasis.QR.Encoder
{
    public abstract class EncoderBase
    {
        private readonly int[,] _CodewordTable = { { 19, 16, 13, 9 }, { 34, 28, 22, 16 }, { 55, 44, 34, 26 }, { 80, 64, 48, 36 }, { 108, 86, 62, 46 }, { 136, 108, 76, 60 }, { 156, 124, 88, 66 }, { 194, 154, 110, 86 }, { 232, 182, 132, 100 },
            { 274, 216, 154, 122 }, { 324, 254, 180, 140 }, { 370, 290, 206, 158 }, { 428, 334, 244, 180 }, { 461, 365, 261, 197 }, { 523, 415, 295, 223 }, { 589, 453, 325, 253 }, { 647, 507, 367, 283 }, { 721, 563, 397, 313 }, 
            { 795, 627, 445, 341 }, { 861, 669, 485, 385 }, { 932, 714, 512, 406 }, { 1006, 782, 568, 442 }, { 1094, 860, 614, 464 }, { 1174, 914, 664, 514 }, { 1276, 1000, 718, 538 }, { 1370, 1062, 754, 596 }, { 1468, 1128, 808, 628 },
            { 1531, 1193, 871, 661 }, { 1631, 1267, 911, 701 }, { 1735, 1373, 985, 745 }, { 1843, 1455, 1033, 793 }, { 1955, 1541, 1115, 845 }, { 2071, 1631, 1171, 901 }, { 2191, 1725, 1231, 961 }, { 2306, 1812, 1286, 986 }, 
            { 2434, 1914, 1354, 1054 }, { 2566, 1992, 1426, 1096 }, { 2702, 2102, 1502, 1142 }, { 2812, 2216, 1582, 1222}, { 2956, 2334, 1666, 1276 } };

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
            return 8 * _CodewordTable[_version, (int)_errorCorrectionLevel];
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
