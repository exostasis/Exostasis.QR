using Exostasis.QR.Common;
using Exostasis.QR.Common.Enum;
using Exostasis.QR.Common.Extensions;
using Exostasis.QR.ErrorCorrection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exostasis.QR.Structurer
{
    public class StructureGenerator
    {
        private int _version { get; set; }
        private int _blocksInGroup1 { get; set; }
        private int _blocksInGroup2 { get; set; }
        private int _codeWordsPerBlockGroup1 { get; set; }
        private int _codeWordsPerBlockGroup2 { get; set; }
        private int _requiredRemainder { get; set; }
        private int _requiredECwords { get; set; }

        private Byte[] _encodedArray { get; set; }

        private ErrorCorrectionLevel _errorCorrectionLevel { get; set; }

        private List<BitArray> _intelevedBlockArray { get; set; }

        private List<Group> _groups { get; set; }

        private ErrorCorrectionGenerator _qRErrorGenerator { get; set; }

        public StructureGenerator(Byte[] encodedArray, int version, ErrorCorrectionLevel errorCorrectionLevel)
        {
            _version = version;
            _errorCorrectionLevel = errorCorrectionLevel;
            _blocksInGroup1 = Constants._requiredBlocksInGroup1[_version, (int)_errorCorrectionLevel];
            _blocksInGroup2 = Constants._requiredBlocksInGroup2[_version, (int)_errorCorrectionLevel];
            _codeWordsPerBlockGroup1 = Constants._requiredCodeWordsInBlocksGroup1[_version, (int)_errorCorrectionLevel];
            _codeWordsPerBlockGroup2 = Constants._requiredCodeWordsInBlocksGroup2[_version, (int)_errorCorrectionLevel];
            _requiredRemainder = Constants._remainderBitsRequired[_version];
            _requiredECwords = Constants._requiredErrorCorrectionCodesPerBlock[_version, (int)_errorCorrectionLevel];
            _encodedArray = encodedArray;
            _groups = new List<Group>();
        }

        public List<BitArray> Generate ()
        {
            _intelevedBlockArray = new List<BitArray>();
            BuildGroups();
            Interleve();
            AddRemainders();

            return _intelevedBlockArray;
        }

        private void BuildGroups ()
        {
            int spotInEncodedArray = 0;

            _groups.Add(new Group());

            for (int i = 0; i < _blocksInGroup1; ++i)
            {
                BuildBlock(_encodedArray.SubArray(spotInEncodedArray, _codeWordsPerBlockGroup1));
                spotInEncodedArray += _codeWordsPerBlockGroup1;
            }

            if (_blocksInGroup2 != 0)
            {
                _groups.Add(new Group());
                for (int i = 0; i < _blocksInGroup2; ++i)
                {
                    BuildBlock(_encodedArray.SubArray(spotInEncodedArray, _codeWordsPerBlockGroup2));
                    spotInEncodedArray += _codeWordsPerBlockGroup2;
                }
            }
        }

        private void BuildBlock (Byte[] codeWords)
        {
            Byte[] errorCorrectionArray;

            _qRErrorGenerator = new ErrorCorrectionGenerator(codeWords, _requiredECwords);
            errorCorrectionArray = _qRErrorGenerator.GenerateErrorCorrectionArray();
            _groups.Last().AddBlock(new Block(codeWords, errorCorrectionArray));
        }

        private void Interleve ()
        {
            List<Byte> tempBytes = new List<Byte>();
            int maxCodeWords = _codeWordsPerBlockGroup1 > _codeWordsPerBlockGroup2 ? _codeWordsPerBlockGroup1 : _codeWordsPerBlockGroup2;
            
            for (int i = 0; i < (maxCodeWords); ++i)
            {
                for (int j = 0; j < _groups.Count; ++j)
                {
                    for (int k = 0; k < _groups.ElementAt(j)._blocks.Count; ++k)
                    {
                        if (i < _groups.ElementAt(j)._blocks.ElementAt(k)._codeWords.Count)
                        {
                            tempBytes.Insert(0, _groups.ElementAt(j)._blocks.ElementAt(k)._codeWords.ElementAt(i));
                        }
                    }
                }
                _intelevedBlockArray.Add(new BitArray(tempBytes.ToArray()));
                tempBytes.Clear();
            }

            for (int i = 0; i < _requiredECwords; ++i)
            {
                for (int j = 0; j < _groups.Count; ++j)
                {
                    for (int k = 0; k < _groups.ElementAt(j)._blocks.Count; ++k)
                    {
                        tempBytes.Insert(0, _groups.ElementAt(j)._blocks.ElementAt(k)._ecWords.ElementAt(i));
                    }
                }
                _intelevedBlockArray.Add(new BitArray(tempBytes.ToArray()));
                tempBytes.Clear();
            }
        }

        private void AddRemainders ()
        {
            _intelevedBlockArray.Add(new BitArray(_requiredRemainder));
        }
    }
}
