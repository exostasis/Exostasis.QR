/*
 * Copyright 2016 Shawn Abtey. This source code is protected under the GNU General Public License 
 *  This file is part of Exostasis.QR.
 *  
 *  Exostasis.QR is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the 
 *  Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 *  
 *  Exostasis.QR is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of 
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License along with Exostasis.QR.  If not, see <http://www.gnu.org/licenses/>.
 */

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
            QrErrorGenerator = new ErrorCorrectionGenerator(codeWords, RequiredECwords);
            var errorCorrectionArray = QrErrorGenerator.GenerateErrorCorrectionArray();
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
                            IntelevedBlockArray.Add(new BitArray(tempBytes.ToArray()));
                            tempBytes.Clear();
                        }
                    }
                }               
            }

            for (int i = 0; i < RequiredECwords; ++i)
            {
                for (int j = 0; j < Groups.Count; ++j)
                {
                    for (int k = 0; k < Groups.ElementAt(j).Blocks.Count; ++k)
                    {
                        tempBytes.Insert(0, Groups.ElementAt(j).Blocks.ElementAt(k).EcWords.ElementAt(i));
                        IntelevedBlockArray.Add(new BitArray(tempBytes.ToArray()));
                        tempBytes.Clear();
                    }
                }                
            }
        }

        private void AddRemainders ()
        {
            if (RequiredRemainder != 0)
            {
                IntelevedBlockArray.Add(new BitArray(RequiredRemainder));
            }
        }
    }
}
