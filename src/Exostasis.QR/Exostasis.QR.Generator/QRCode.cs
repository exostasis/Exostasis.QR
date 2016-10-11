using Exostasis.QR.Common.Enum;
using Exostasis.QR.Image;
using Exostasis.QR.Structurer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using QREncoder;

namespace Exostasis.QR.Generator
{
    public class QrCode
    {
        private const string NumericModeRegex = "^[0-9]+$";
        private const string AlphanumericModeRegex = "^[0-9a-z$%*+-./: ]+$";

        private int Version { get; set; }

        private ErrorCorrectionLevel ErrorCorrectionLevel { get; set; }

        private string UnencodedString { get; set; }

        private byte[] EncodedArray { get; set; }

        private EncoderBase QrEncoder { get; set; }
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

        public void Generate(string filename, int scale = 12)
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
            byte[] isoBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(inputString);
            string isoString = Encoding.GetEncoding("ISO-8859-1").GetString(isoBytes);

            return inputString.Equals(isoString);
        }
    }
}
