using System;

namespace Exostasis.QR.Common.Image
{
    [Serializable]
    public class Cord
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Cord(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
