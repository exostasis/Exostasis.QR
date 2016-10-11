using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Exostasis.QR.Common;
using Exostasis.QR.Common.Enum;

namespace QREncoder
{
    public abstract class EncoderBase
    {
        private readonly byte[] _padBytes = {0xEC, 0x11}; 

        public int Version { get; protected set; }

        protected BitArray CharacterCountIndicator { get; set; }

        protected BitArray ModeIndicator { get; set; }

        protected byte[] EncodedBytes { get; private set; }

        protected string UnencodedString { get; set; }

        public ErrorCorrectionLevel ErrorCorrectionLevel { get; protected set; }

        protected int DataPerBitString { get; set; }

        protected int BitsPerBitString { get; set; }

        protected int BitsPerCharacterCountIndicator { get; set; }

        protected int MaximumPossibleCharacterCount { get; set; }

        protected abstract List<BitArray> Encode();

        protected abstract void DetermineBitsPerCharacterCountIndicator();

        private int GetRequiredDataBits ()
        {
            return 8 * Constants.CodewordTable[Version, (int)ErrorCorrectionLevel];
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
            int numberOf0SToAdd = 8 - dataBitCount % 8;

            if (numberOf0SToAdd > 0)
            {
                bitArray.Add(new BitArray(numberOf0SToAdd));
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

        private byte[] ConvertListBitArrayToByteArray(List<BitArray> bitArray)
        {
            List <byte> bytes = new List<byte>();
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
            int maximumLength = 0;

            for (int i = (int)ErrorCorrectionLevel.H; i >= 0; --i)
            {
                ErrorCorrectionLevel = (ErrorCorrectionLevel)i;

                for (int j = 0; j < 40; ++j)
                {
                    Version = j;

                    DetermineBitsPerCharacterCountIndicator();

                    maximumLength = GetMaximumCharacterCount();

                    if (maximumLength >= UnencodedString.Length)
                    {
                        break;
                    }
                }

                if (maximumLength >= UnencodedString.Length)
                {
                    break;
                }
            }
        }

        protected int GetMaximumCharacterCount()
        {
            return (GetRequiredDataBits() - 4 - BitsPerCharacterCountIndicator) * DataPerBitString / BitsPerBitString;
        }

        public EncoderBase(string unencodedString)
        {
            UnencodedString = unencodedString;
            CharacterCountIndicator = new BitArray(BitConverter.GetBytes(UnencodedString.Length));
        }

        public byte[] DataEncode()
        {
            List<BitArray> bitArray;
            bitArray = Encode();
            Terminate(bitArray);
            MakeMultipleOf8(bitArray);
            Pad(bitArray);
            EncodedBytes = ConvertListBitArrayToByteArray(bitArray);
            
            return EncodedBytes;
        }
    }
}
