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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QREncoder
{
    public class NumericMode : EncoderBase
    {
        public NumericMode (string unencodedString) : base (unencodedString)
        {
            ModeIndicator = new BitArray(BitConverter.GetBytes(0x01));
            ModeIndicator.Length = 4;
            DataPerBitString = 3;
            BitsPerBitString = 10;
            MaximumPossibleCharacterCount = 7089;

            if (UnencodedString.Length > MaximumPossibleCharacterCount)
            {
                throw new Exception("This string is unable to be converted into a qr code due to the length of it");
            }

            DetermineMinimumVersionAndMaximumErrorCorrection();
            CharacterCountIndicator.Length = BitsPerCharacterCountIndicator;
        }

        protected override void DetermineBitsPerCharacterCountIndicator()
        {
            switch (Version)
            {
                case 0:
                    BitsPerCharacterCountIndicator = 10;
                    break;
                case 9:
                    BitsPerCharacterCountIndicator = 12;
                    break;
                case 26:
                    BitsPerCharacterCountIndicator = 14;
                    break;
            }
        }

        protected override List<BitArray> Encode()
        {
            List<BitArray> bitArrays = new List<BitArray>();

            bitArrays.Add(ModeIndicator);
            bitArrays.Add(new BitArray(CharacterCountIndicator));

            for (int i = 0; i < UnencodedString.Length; i +=3)
            {
                int charsLeftToCopy = UnencodedString.Length - i;
                var tempValue = int.Parse(UnencodedString.Substring(i, charsLeftToCopy >= 3 ? 3 : charsLeftToCopy));

                bitArrays.Add(new BitArray(BitConverter.GetBytes(tempValue)));
                if (charsLeftToCopy >= 3)
                {
                    bitArrays.Last().Length = 10;
                }
                else if (charsLeftToCopy == 2)
                {
                    bitArrays.Last().Length = 7;
                }
                else
                {
                    bitArrays.Last().Length = 4;
                }
            }

            return bitArrays;
        }
    }
}
