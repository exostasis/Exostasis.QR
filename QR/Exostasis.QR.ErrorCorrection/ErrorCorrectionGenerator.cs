using System;
using Exostasis.Polynomial;
using Exostasis.Polynomial.Extensions;
using System.Linq;

namespace Exostasis.QR.ErrorCorrection
{
    public class ErrorCorrectionGenerator
    {
        private byte[] EncodedArray { get; set; }

        private int ErrorCorrectionCodeWordsPerBlock { get; set; }

        private Expression ErrorCorrectionExp { get; set; }
        private Expression MessageExp { get; set; }

        public ErrorCorrectionGenerator (byte[] encodedArray, int errorCorrectionCodeWordsPerBlock)
        {
            EncodedArray = encodedArray;                        
            ErrorCorrectionCodeWordsPerBlock = errorCorrectionCodeWordsPerBlock;         
        }

        public byte[] GenerateErrorCorrectionArray()
        {
            CreateMessageExpression();
            GenerateErrorCorrectionExpression();
            MessageExp = MessageExp * new AlphaTerm(0, "x", ErrorCorrectionCodeWordsPerBlock);

            Expression results = Expression.LongDivisionXTimes(MessageExp, ErrorCorrectionExp, EncodedArray.Length);
                       
            var temp = results.GetConstantExpression();
            var bytes = temp.Select(term => term.ToByte());

            return bytes.ToArray();
        }

        private void CreateMessageExpression ()
        {
            MessageExp = new Expression();
            for (int i = 0; i < EncodedArray.Length; ++i)
            {
                int temp = EncodedArray[i];
                if (temp != 0)
                {
                    MessageExp.AddTerm(temp.Log("x", EncodedArray.Length - i - 1));
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
            
            for (int i = 2; i < ErrorCorrectionCodeWordsPerBlock; ++ i)
            {
                Expression tempExp = new Expression();
                tempExp.AddTerm(new AlphaTerm(0, "x", 1));
                tempExp.AddTerm(new AlphaTerm(i, "x", 0));

                results = results * tempExp;
            }

            ErrorCorrectionExp = results;
        }
    }
}
