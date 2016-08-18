using Exostasis.QR.Generator;
using System;

namespace Exostasis.QR.Terminal
{
    class Terminal
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a string to encode");
            QRCode MyQRCode = new QRCode(Console.ReadLine());

            MyQRCode.Generate();

            Console.ReadLine();
        }
    }
}
