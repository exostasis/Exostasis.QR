using QREncoder.Enum;
using Exostasis.Polynomial;
using System;
using Exostasis.Polynomial.Extensions;
using Exostasis.QR.Common;
using System.Linq;

namespace Exostasis.QR.ErrorCorrection
{
    public class ErrorCorrectionGenerator
    {
        private Byte[] _errorCorrectionArray { get; set; }
        private Byte[] _encodedArray { get; set; }

        private int _errorCorrectionCodeWordsPerBlock { get; set; }

        private Expression _errorCorrectionExp { get; set; }
        private Expression _messageExp { get; set; }

        public ErrorCorrectionGenerator (Byte[] encodedArray, int version, ErrorCorrectionLevel errorCorrectionLevel)
        {
            _encodedArray = encodedArray;                        
            _errorCorrectionCodeWordsPerBlock = Constants._requiredErrorCorrectionCodesPerBlock[version, (int)errorCorrectionLevel];         
        }

        public Byte[] GenerateErrorCorrectionArray()
        {
             CreateMessageExpression();
            GenerateErrorCorrectionExpression();
            _messageExp = _messageExp * new AlphaTerm(0, "x", _errorCorrectionCodeWordsPerBlock);

            Expression results = _messageExp / _errorCorrectionExp;
            var temp = results.GetConstantExpression();
            var bytes = temp.Select(term => term.ToByte());

            return bytes.ToArray();
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
