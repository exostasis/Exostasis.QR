using Exostasis.QR.Encoder;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Exostasis.QR.Generator
{
    public class QRCode
    {
        private string _UnencodedString { get; set; }
        private Byte[] _EncodedString { get; set; }

        private EncoderBase _QREncoder { get; set; }

        private const string NumericModeRegex = "^[0-9]+$";
        private const string AlphanumericModeRegex = "^[0-9a-z$%*+-./:]+$";

        public QRCode(string UnencodedString)
        {
            _UnencodedString = UnencodedString;
        }

        public void Generate ()
        {
            _QREncoder = DataAnalyse();
            _EncodedString = _QREncoder.DataEncode();
        }

        private EncoderBase DataAnalyse ()
        {
            EncoderBase TheEncoder;
        
            if (Regex.IsMatch(_UnencodedString, NumericModeRegex))
            {
                TheEncoder = new NumericMode(_UnencodedString);
            }
            else if (Regex.IsMatch(_UnencodedString, AlphanumericModeRegex))
            {
                TheEncoder = new AlphanumericMode(_UnencodedString);
            }
            else if (IsISO8859(_UnencodedString))
            {
                TheEncoder = new ByteMode(_UnencodedString);
            }
            else
            {
                throw new Exception("Unsuporrted string for QR encoding");
            }

            return TheEncoder;
        }

        private bool IsISO8859(string InputString)
        {
            byte[] ISOBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(InputString);
            string ISOString = Encoding.GetEncoding("ISO-8859-1").GetString(ISOBytes);

            return InputString.Equals(ISOString);
        }
    }
}
