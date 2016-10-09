using System.Drawing;
using Exostasis.QR.Common.Image;

namespace Exostasis.QR.Image
{
    public class Seperator : Element
    {
        public Seperator(Cord topLeftCord, int width, int height, ref Module[,] elements)
        {
            TopLeftCord = topLeftCord;
            TopRightCord = new Cord(topLeftCord.X + width, topLeftCord.Y);
            BottomLeftCord = new Cord(topLeftCord.X, topLeftCord.Y + height);
            BottomRightCord = new Cord(topLeftCord.X + width, topLeftCord.Y + height);

            WriteModules(ref elements);
        }

        private void WriteModules(ref Module[,] elements)
        {
            for (int y = TopLeftCord.Y; y < BottomLeftCord.Y; ++y)
            {
                for (int x = TopLeftCord.X; x < TopRightCord.X; ++x)
                {
                    new Module(new Cord(x, y), Color.White, ref elements);
                }
            }
        }
    }
}
