using QREncoder.Enum;
using Exostasis.Polynomial;
using System;
using Exostasis.Polynomial.Extensions;

namespace Exostasis.QR.ErrorCorrection
{
    public class ErrorCorrectionGenerator
    {
        private readonly int[,] _requiredErrorCorrectionCodesPerBlock = { { 7, 10, 13, 17 }, { 10, 16, 22, 28 }, { 15, 26, 18, 22 }, { 20, 18, 26, 16 }, { 26, 24, 18, 22 }, { 18, 16, 24, 28 }, { 20, 18, 18, 26 }, { 24, 22, 22, 26 }, 
            { 30, 22, 20, 24 }, { 18, 26, 24, 28 }, { 20, 30, 28, 24 }, { 24, 22, 26, 28 }, { 26, 22, 24, 22 }, { 30, 24, 20, 24 }, { 22, 24, 30, 24 }, { 24, 28, 24, 30 }, { 28, 28, 28, 28 }, { 30, 26, 28, 28 }, { 28, 26, 26, 26 }, 
            { 28, 26, 30, 28 }, { 28, 26, 28, 30 }, { 28, 28, 30, 24 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, { 26, 28, 30, 30 }, { 28, 28, 28, 30 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, 
            { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 }, { 30, 28, 30, 30 } };

        private readonly int[,] _requiredBlocksInGroup1 = { { 1, 1, 1, 1 }, { 1, 1, 1, 1 }, { 1, 1, 2, 2 }, { 1, 2, 2, 4 }, { 1, 2, 2, 2 }, { 2, 4, 4, 4 }, { 2, 4, 2, 4 }, { 2, 2, 4, 4 }, { 2, 3, 4, 4 }, { 2, 4, 6, 6 }, { 4, 1, 4, 3 }, 
            { 2, 6, 4, 7 }, { 4, 8, 8, 12 }, { 3, 4, 11, 11 }, { 5, 5, 5, 11 }, { 5, 7, 15, 3 }, { 1, 10, 1, 2 }, { 5, 9, 17, 2 }, { 3, 3, 17, 9 }, { 3, 3, 15, 15 }, { 4, 17, 17, 19 }, { 2, 17, 7, 34 }, { 4, 4, 11, 16 }, { 6, 6, 11, 30 }, 
            { 8, 8, 7, 22 }, { 10, 19, 28, 33 }, { 8, 22, 8, 12 }, { 3, 3, 4, 11 }, { 7, 21, 1, 19 }, { 5, 19, 15, 23 }, { 13, 2, 42, 23 }, { 17, 10, 10, 19 }, { 17, 14, 29, 11 }, { 13, 14, 44, 59 }, { 12, 12, 39, 22 }, { 6, 6, 46, 2 }, 
            { 17, 29, 49, 24 }, { 4, 13, 48, 42 }, { 20, 40, 43, 10 }, { 19, 18, 34, 20 } };

        private readonly int[,] _requiredBlocksInGroup2 = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 2, 2 }, { 0, 0, 0, 0 }, { 0, 0, 4, 1 }, { 0, 2, 2, 2 }, { 0, 2, 4, 4 }, { 2, 1, 2, 2 }, { 0, 4, 4, 8 }, 
            { 2, 2, 6, 4 }, { 0, 1, 4, 4 }, { 1, 5, 5, 5 }, { 1, 5, 7, 7 }, { 1, 3, 2, 13 }, { 5, 1, 15, 17 }, { 1, 4, 1, 19 }, { 4, 11, 4, 16 }, { 5, 13, 5, 10 }, { 4, 0, 6, 6 }, { 7, 0, 16, 0 }, { 5, 14, 14, 14 }, { 4, 14, 16, 2 }, 
            { 4, 13, 22, 13 }, { 2, 4, 6, 4 }, { 4, 3, 26, 28 }, { 10, 23, 31, 31 }, { 7, 7, 37, 26 }, { 10, 10, 25, 25 }, { 3, 29, 1, 28 }, { 0, 23, 35, 35 }, { 1, 21, 19, 46 }, { 6, 23, 7, 1 }, { 7, 26, 14, 41 }, { 14, 34, 10, 64 }, 
            { 4, 14, 10, 46 }, { 18, 32, 14, 32 }, { 4, 7, 22, 67 }, { 6, 31, 34, 61 } };

        private readonly int[,] _requiredCodeWordsInBlocksGroup1 = { { 19, 16, 13, 9 }, { 34, 28, 22, 16 }, { 55, 44, 17, 13 }, { 80, 32, 24, 9 }, { 108, 43, 15, 11 }, { 68, 27, 19, 15 }, { 78, 31, 14, 13 }, { 97, 38, 18, 14 }, { 116, 36, 16, 12 }, 
            { 68, 43, 19, 15 }, { 81, 50, 22, 12 }, { 92, 36, 20, 14 }, { 107, 37, 20, 11 }, { 115, 40, 16, 12 }, { 87, 41, 24, 12 }, { 98, 45, 19, 15 }, { 107, 46, 22, 14 }, { 120, 43, 22, 14 }, { 113, 44, 21, 13 }, { 107, 41, 24, 15 }, 
            { 116, 42, 22, 16 }, { 111, 46, 24, 13 }, { 121, 47, 24, 15 }, { 117, 45, 24, 16 }, { 106, 47, 24, 15 }, { 114, 46, 22, 16 }, { 122, 45, 23, 15 }, { 117, 45, 24, 15 }, { 116, 45, 23, 15 }, { 115, 47, 24, 15 }, { 115, 46, 24, 15 }, 
            { 115, 46, 24, 15 }, { 115, 46, 24, 15 }, { 115, 46, 24, 16 }, { 121, 47, 24, 15 }, { 121, 47, 24, 15 }, { 122, 46, 24, 15 }, { 122, 46, 24, 15 }, { 117, 47, 24, 15 }, { 118, 47, 24, 15 } };

        private readonly int[,] _requiredCodeWordsInBlocksGroup2 = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 16, 12 }, { 0, 0, 0, 0 }, { 0, 0, 15, 14 }, { 0, 39, 19, 15 }, { 0, 37, 17, 13 }, { 69, 44, 20, 16 }, 
            { 0, 51, 23, 13 }, { 93, 37, 21, 15 }, { 0, 38, 21, 12 }, { 116, 41, 17, 13 }, { 88, 42, 25, 13 }, { 99, 46, 20, 16 }, { 108, 47, 23, 15 }, { 121, 44, 23, 15 }, { 114, 45, 22, 14 }, { 108, 42, 25, 16 }, { 117, 0, 23, 17 }, 
            { 112, 0, 25, 0 }, { 122, 48, 25, 16 }, { 118, 46, 25, 17 }, { 107, 48, 25, 16 }, { 115, 47, 23, 17 }, { 123, 46, 24, 16 }, { 118, 46, 25, 16 }, { 117, 46, 24, 16 }, { 116, 48, 25, 16 }, { 116, 47, 25, 16 }, { 0, 47, 25, 16 }, 
            { 116, 47, 25, 16 }, { 116, 47, 25, 17 }, { 122, 48, 25, 16 }, { 122, 48, 25, 16 }, { 123, 47, 25, 16 }, { 123, 47, 25, 16 }, { 118, 48, 25, 16 }, { 119, 48, 25, 16 } };

        private Byte[] _errorCorrectionArray { get; set; }
        private Byte[] _encodedArray { get; set; }

        private int _codeWordsPerBlockGroup1 { get; set; }
        private int _codeWordsPerBlockGroup2 { get; set; }
        private int _errorCorrectionCodeWordsPerBlock { get; set; }
        private int _blocksInGroup1 { get; set; }
        private int _blocksInGroup2 { get; set; }

        private Expression _errorCorrectionExp { get; set; }
        private Expression _messageExp { get; set; }

        public ErrorCorrectionGenerator (Byte[] encodedArray, int version, ErrorCorrectionLevel errorCorrectionLevel)
        {
            _encodedArray = encodedArray;                        
            _errorCorrectionCodeWordsPerBlock = _requiredErrorCorrectionCodesPerBlock[version, (int)errorCorrectionLevel];
            _codeWordsPerBlockGroup1 = _requiredCodeWordsInBlocksGroup1[version, (int)errorCorrectionLevel];
            _codeWordsPerBlockGroup2 = _requiredCodeWordsInBlocksGroup2[version, (int)errorCorrectionLevel];
            _blocksInGroup1 = _requiredBlocksInGroup1[version, (int)errorCorrectionLevel];
            _blocksInGroup2 = _requiredBlocksInGroup2[version, (int)errorCorrectionLevel];

            CreateMessageExpression();
            GenerateErrorCorrectionExpression();
            _errorCorrectionExp.DisplayExpression();
        }

        public Byte[] GenerateErrorCorrectionArray(Byte[] encodedArray)
        {
            throw new NotImplementedException();
        }

        private void CreateMessageExpression ()
        {
            _messageExp = new Expression();
            for (int i = 0; i < _encodedArray.Length; ++i)
            {
                int temp = _encodedArray[i];
                if (temp != 0)
                {
                    _messageExp.AddTerm(temp.Log("x", _encodedArray.Length - i - 1));
                }
            }
        }

        private void GenerateErrorCorrectionExpression()
        {
            Expression startExpression1 = new Expression();
            startExpression1.AddTerm(new AlphaTerm(0, "x", 1));
            startExpression1.AddTerm(new AlphaTerm(0, "x", 0));

            Expression startExpression2 = new Expression();
            startExpression2.AddTerm(new AlphaTerm(0, "x", 1));
            startExpression2.AddTerm(new AlphaTerm(1, "x", 0));

            Expression results = startExpression1 * startExpression2;
            
            for (int i = 2; i < _errorCorrectionCodeWordsPerBlock; ++ i)
            {
                Expression tempExp = new Expression();
                tempExp.AddTerm(new AlphaTerm(0, "x", 1));
                tempExp.AddTerm(new AlphaTerm(i, "x", 0));

                results = results * tempExp;
            }

            _errorCorrectionExp = results;
        }
    }
}
