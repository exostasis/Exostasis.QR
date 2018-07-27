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

using Exostasis.QR.Polynomial;
using Exostasis.QR.Polynomial.Extensions;
using System.Linq;

namespace Exostasis.QR.ErrorCorrection
{
    public class ErrorCorrectionGenerator
    {
        private byte[] EncodedArray { get; }

        private int ErrorCorrectionCodeWordsPerBlock { get; }

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

            var results = Expression.LongDivisionXTimes(MessageExp, ErrorCorrectionExp, EncodedArray.Length);
                       
            var temp = results.GetConstantExpression();
            var bytes = temp.Select(term => term.ToByte());

            return bytes.ToArray();
        }

        private void CreateMessageExpression ()
        {
            MessageExp = new Expression();
            for (var i = 0; i < EncodedArray.Length; ++i)
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
            var startExpression1 = new Expression();
            startExpression1.AddTerm(new AlphaTerm(0, "x", 1));
            startExpression1.AddTerm(new AlphaTerm(0, "x", 0));

            var startExpression2 = new Expression();
            startExpression2.AddTerm(new AlphaTerm(0, "x", 1));
            startExpression2.AddTerm(new AlphaTerm(1, "x", 0));

            var results = startExpression1 * startExpression2;
            
            for (var i = 2; i < ErrorCorrectionCodeWordsPerBlock; ++ i)
            {
                var tempExp = new Expression();
                tempExp.AddTerm(new AlphaTerm(0, "x", 1));
                tempExp.AddTerm(new AlphaTerm(i, "x", 0));

                results = results * tempExp;
            }

            ErrorCorrectionExp = results;
        }
    }
}
