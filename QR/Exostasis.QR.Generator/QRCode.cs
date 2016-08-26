using Exostasis.QR.Encoder;
using Exostasis.QR.ErrorCorrection;
using QREncoder.Enum;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Exostasis.QR.Generator
{
    public class QRCode
    {
        private string _unencodedString { get; set; }

        private Byte[] _encodedArray { get; set; }
        private Byte[] _errorCorrectionArray { get; set; }

        private EncoderBase _qREncoder { get; set; }

        private ErrorCorrectionGenerator _qRErrorGenerator { get; set; }

        private const string NumericModeRegex = "^[0-9]+$";
        private const string AlphanumericModeRegex = "^[0-9a-z$%*+-./:]+$";

        private int _version;

        private ErrorCorrectionLevel _errorCorrectionLevel; 

        public QRCode(string UnencodedString)
        {
            _unencodedString = UnencodedString;
        }

        public void Generate ()
        {
            _qREncoder = DataAnalyse();
            _encodedArray = _qREncoder.DataEncode();
            _version = _qREncoder._version;
            _errorCorrectionLevel = _qREncoder._errorCorrectionLevel;
            _qRErrorGenerator = new ErrorCorrectionGenerator(_encodedArray, _version, _errorCorrectionLevel);
            _errorCorrectionArray = _qRErrorGenerator.GenerateErrorCorrectionArray();
        }

        private EncoderBase DataAnalyse ()
        {
            EncoderBase TheEncoder;
        
            if (Regex.IsMatch(_unencodedString, NumericModeRegex))
            {
                TheEncoder = new NumericMode(_unencodedString);
            }
            else if (Regex.IsMatch(_unencodedString, AlphanumericModeRegex))
            {
                TheEncoder = new AlphanumericMode(_unencodedString);
            }
            else if (IsISO8859(_unencodedString))
            {
                TheEncoder = new ByteMode(_unencodedString);
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
