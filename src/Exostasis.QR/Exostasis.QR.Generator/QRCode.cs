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

using Exostasis.QR.Common.Enum;
using Exostasis.QR.Image;
using Exostasis.QR.Structurer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Exerostasis.QR.Encoder;

namespace Exostasis.QR.Generator
{
    public class QrCode
    {
        private const string NumericModeRegex = "^[0-9]+$";
        private const string AlphanumericModeRegex = "^[0-9a-z$%*+-./: ]+$";

        private int Version { get; }

        private ErrorCorrectionLevel ErrorCorrectionLevel { get; }

        private string UnencodedString { get; }

        private byte[] EncodedArray { get; set; }

        private EncoderBase QrEncoder { get; }
        private StructureGenerator QrStructurer { get; set; }

        private List<BitArray> StructuredArray { get; set; }

        private QrImage QrImage { get; set; }

        public QrCode(string unencodedString)
        {
            UnencodedString = unencodedString;
            QrEncoder = DataAnalyse();
            Version = QrEncoder.Version;
            ErrorCorrectionLevel = QrEncoder.ErrorCorrectionLevel;
        }

        public void GenerateFile(string filename, int scale = 12)
        {            
            EncodedArray = QrEncoder.DataEncode();            
            QrStructurer = new StructureGenerator(EncodedArray, Version, ErrorCorrectionLevel);
            StructuredArray = QrStructurer.Generate();
            QrImage = new QrImage(Version, scale, StructuredArray, ErrorCorrectionLevel);
            QrImage.WriteImage(filename);
        }
        
        public void GenerateStream(string filename, int scale = 12)
        {            
            EncodedArray = QrEncoder.DataEncode();            
            QrStructurer = new StructureGenerator(EncodedArray, Version, ErrorCorrectionLevel);
            StructuredArray = QrStructurer.Generate();
            QrImage = new QrImage(Version, scale, StructuredArray, ErrorCorrectionLevel);
            QrImage.WriteImage(filename);
        }

        public int  GetSize()
        {
            return Version * 4 + 21;
        }

        private EncoderBase DataAnalyse()
        {
            EncoderBase theEncoder;

            if (Regex.IsMatch(UnencodedString, NumericModeRegex))
            {
                theEncoder = new NumericMode(UnencodedString);
            }
            else if (Regex.IsMatch(UnencodedString, AlphanumericModeRegex))
            {
                theEncoder = new AlphanumericMode(UnencodedString);
            }
            else if (IsIso8859(UnencodedString))
            {
                theEncoder = new ByteMode(UnencodedString);
            }
            else
            {
                throw new Exception("Unsuporrted string for QR encoding");
            }

            return theEncoder;
        }

        private bool IsIso8859(string inputString)
        {
            var isoBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(inputString);
            var isoString = Encoding.GetEncoding("ISO-8859-1").GetString(isoBytes);

            return inputString.Equals(isoString);
        }
    }
}
