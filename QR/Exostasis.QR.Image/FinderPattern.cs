using System.Drawing;
using Exostasis.QR.Common.Image;

namespace Exostasis.QR.Image
{
    public class FinderPattern : Element
    {
        public static int ModulesWide => 7;

        public static int ModulesHeigh => 7;

        public FinderPattern(Cord topLeftCord, ref Module[,] elements)
        {
            TopLeftCord = topLeftCord;
            TopRightCord = new Cord(topLeftCord.X + ModulesWide, topLeftCord.Y);
            BottomLeftCord = new Cord(topLeftCord.X, topLeftCord.Y + ModulesHeigh);
            BottomRightCord = new Cord(topLeftCord.X + ModulesWide, topLeftCord.Y + ModulesHeigh);

            WriteModules(ref elements);
        }

        private void WriteModules(ref Module[,] elements)
        {
            for (int y = 0; y < 7; ++y)
            {
                for (int x = 0; x < 7; ++x)
                {
                    if (x == 0 || y == 0 || x == 6 || y == 6)
                    {
                        new Module(new Cord(TopLeftCord.X + x, TopLeftCord.Y + y), Color.Black, ref elements);
                    }
                    else if (y == 1 || y == 5 || x == 1 || x == 5)
                    {
                        new Module(new Cord(TopLeftCord.X + x, TopLeftCord.Y + y), Color.White, ref elements);
                    }
                    else
                    {
                        new Module(new Cord(TopLeftCord.X + x, TopLeftCord.Y + y), Color.Black, ref elements);
                    }
                }
            }
        }
    }
}