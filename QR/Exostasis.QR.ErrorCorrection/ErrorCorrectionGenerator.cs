using QREncoder.Enum;
using Exostasis.Polynomial;
using System;
using Exostasis.Polynomial.Extensions;

namespace Exostasis.QR.ErrorCorrection
{
    public class ErrorCorrectionGenerator
    {
        private Byte[] _errorCorrectionArray { get; set; }
        private Byte[] _encodedArray { get; set; }

        private int _codeWords { get; set; }
        private int _errorCorrectionCodeWords { get; set; }

        private Expression _errorCorrectionExp { get; set; }
        private Expression _messageExp { get; set; }

        public ErrorCorrectionGenerator (Byte[] encodedArray)
        {
            _codeWords = encodedArray.Length;
            _encodedArray = encodedArray;
            CreateMessageExpression();
        }

        public Byte[] GenerateErrorCorrectionArray(Byte[] encodedArray)
        {
            throw new NotImplementedException();
        }

        private void CreateMessageExpression ()
        {
            _messageExp = new Expression();
            for (int i = 0; i < _codeWords; ++i)
            {
                _messageExp.AddTerm(((int)_encodedArray[i]).Log("x", _codeWords - i - 1));
            }
        }
    }
}
