using System;
using Exostasis.QR.Generator;

namespace QRConsole
{
    class Terminal
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a string to encode");
            QrCode myQrCode = new QrCode(Console.ReadLine());

            myQrCode.Generate();

            Console.ReadLine();
        }
    }
}
