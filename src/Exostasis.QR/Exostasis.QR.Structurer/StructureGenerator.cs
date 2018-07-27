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
        private int Version { get; }
        private int BlocksInGroup1 { get; }
        private int BlocksInGroup2 { get; }
        private int CodeWordsPerBlockGroup1 { get; }
        private int CodeWordsPerBlockGroup2 { get; }
        private int RequiredRemainder { get; }
        private int RequiredECwords { get; }

        private byte[] EncodedArray { get; }

        private ErrorCorrectionLevel ErrorCorrectionLevel { get; }

        private List<BitArray> IntelevedBlockArray { get; set; }

        private List<Group> Groups { get; }

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
            var spotInEncodedArray = 0;

            Groups.Add(new Group());

            for (var i = 0; i < BlocksInGroup1; ++i)
            {
                BuildBlock(EncodedArray.SubArray(spotInEncodedArray, CodeWordsPerBlockGroup1));
                spotInEncodedArray += CodeWordsPerBlockGroup1;
            }

            if (BlocksInGroup2 != 0)
            {
                Groups.Add(new Group());
                for (var i = 0; i < BlocksInGroup2; ++i)
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
            var tempBytes = new List<byte>();
            var maxCodeWords = CodeWordsPerBlockGroup1 > CodeWordsPerBlockGroup2 ? CodeWordsPerBlockGroup1 : CodeWordsPerBlockGroup2;
            
            for (var i = 0; i < maxCodeWords; ++i)
            {
                for (var j = 0; j < Groups.Count; ++j)
                {
                    for (var k = 0; k < Groups.ElementAt(j).Blocks.Count; ++k)
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

            for (var i = 0; i < RequiredECwords; ++i)
            {
                for (var j = 0; j < Groups.Count; ++j)
                {
                    for (var k = 0; k < Groups.ElementAt(j).Blocks.Count; ++k)
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
