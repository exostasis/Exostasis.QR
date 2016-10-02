using Exostasis.QR.Common;
using Exostasis.QR.Common.Enum;
using Exostasis.QR.Common.Extensions;
using Exostasis.QR.ErrorCorrection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exostasis.QR.Structurer
{
    public class StructureGenerator
    {
        private int Version { get; set; }
        private int BlocksInGroup1 { get; set; }
        private int BlocksInGroup2 { get; set; }
        private int CodeWordsPerBlockGroup1 { get; set; }
        private int CodeWordsPerBlockGroup2 { get; set; }
        private int RequiredRemainder { get; set; }
        private int RequiredECwords { get; set; }

        private byte[] EncodedArray { get; set; }

        private ErrorCorrectionLevel ErrorCorrectionLevel { get; set; }

        private List<BitArray> IntelevedBlockArray { get; set; }

        private List<Group> Groups { get; set; }

        private ErrorCorrectionGenerator QrErrorGenerator { get; set; }

        public StructureGenerator(byte[] encodedArray, int version, ErrorCorrectionLevel errorCorrectionLevel)
        {
            Version = version;
            ErrorCorrectionLevel = errorCorrectionLevel;
            BlocksInGroup1 = Constants.RequiredBlocksInGroup1[Version, (int)ErrorCorrectionLevel];
            BlocksInGroup2 = Constants.RequiredBlocksInGroup2[Version, (int)ErrorCorrectionLevel];
            CodeWordsPerBlockGroup1 = Constants.RequiredCodeWordsInBlocksGroup1[Version, (int)ErrorCorrectionLevel];
            CodeWordsPerBlockGroup2 = Constants.RequiredCodeWordsInBlocksGroup2[Version, (int)ErrorCorrectionLevel];
            RequiredRemainder = Constants.RemainderBitsRequired[Version];
            RequiredECwords = Constants.RequiredErrorCorrectionCodesPerBlock[Version, (int)ErrorCorrectionLevel];
            EncodedArray = encodedArray;
            Groups = new List<Group>();
        }

        public List<BitArray> Generate ()
        {
            IntelevedBlockArray = new List<BitArray>();
            BuildGroups();
            Interleve();
            AddRemainders();

            return IntelevedBlockArray;
        }

        private void BuildGroups ()
        {
            int spotInEncodedArray = 0;

            Groups.Add(new Group());

            for (int i = 0; i < BlocksInGroup1; ++i)
            {
                BuildBlock(EncodedArray.SubArray(spotInEncodedArray, CodeWordsPerBlockGroup1));
                spotInEncodedArray += CodeWordsPerBlockGroup1;
            }

            if (BlocksInGroup2 != 0)
            {
                Groups.Add(new Group());
                for (int i = 0; i < BlocksInGroup2; ++i)
                {
                    BuildBlock(EncodedArray.SubArray(spotInEncodedArray, CodeWordsPerBlockGroup2));
                    spotInEncodedArray += CodeWordsPerBlockGroup2;
                }
            }
        }

        private void BuildBlock (byte[] codeWords)
        {
            byte[] errorCorrectionArray;

            QrErrorGenerator = new ErrorCorrectionGenerator(codeWords, RequiredECwords);
            errorCorrectionArray = QrErrorGenerator.GenerateErrorCorrectionArray();
            Groups.Last().AddBlock(new Block(codeWords, errorCorrectionArray));
        }

        private void Interleve ()
        {
            List<byte> tempBytes = new List<byte>();
            int maxCodeWords = CodeWordsPerBlockGroup1 > CodeWordsPerBlockGroup2 ? CodeWordsPerBlockGroup1 : CodeWordsPerBlockGroup2;
            
            for (int i = 0; i < maxCodeWords; ++i)
            {
                for (int j = 0; j < Groups.Count; ++j)
                {
                    for (int k = 0; k < Groups.ElementAt(j).Blocks.Count; ++k)
                    {
                        if (i < Groups.ElementAt(j).Blocks.ElementAt(k).CodeWords.Count)
                        {
                            tempBytes.Insert(0, Groups.ElementAt(j).Blocks.ElementAt(k).CodeWords.ElementAt(i));
                        }
                    }
                }
                IntelevedBlockArray.Add(new BitArray(tempBytes.ToArray()));
                tempBytes.Clear();
            }

            for (int i = 0; i < RequiredECwords; ++i)
            {
                for (int j = 0; j < Groups.Count; ++j)
                {
                    for (int k = 0; k < Groups.ElementAt(j).Blocks.Count; ++k)
                    {
                        tempBytes.Insert(0, Groups.ElementAt(j).Blocks.ElementAt(k).EcWords.ElementAt(i));
                    }
                }
                IntelevedBlockArray.Add(new BitArray(tempBytes.ToArray()));
                tempBytes.Clear();
            }
        }

        private void AddRemainders ()
        {
            IntelevedBlockArray.Add(new BitArray(RequiredRemainder));
        }
    }
}
