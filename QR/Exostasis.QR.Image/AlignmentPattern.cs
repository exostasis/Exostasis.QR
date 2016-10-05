using System.Drawing;
using Exostasis.QR.Common.Image;

namespace Exostasis.QR.Image
{
    public class AlignmentPattern : Element
    {
        public static int ModulesWide => 5;

        public static int ModulesHeigh => 5;

        public AlignmentPattern(Cord middleCord, ref Module[,] elements)
        {
            TopLeftCord = new Cord(middleCord.X - ModulesWide / 2, middleCord.Y - ModulesHeigh / 2);
            TopRightCord = new Cord(TopLeftCord.X + ModulesWide, TopLeftCord.Y);
            BottomLeftCord = new Cord(TopLeftCord.X, TopLeftCord.Y + ModulesHeigh);
            BottomRightCord = new Cord(TopLeftCord.X + ModulesWide, TopLeftCord.Y + ModulesHeigh);

            WriteModules(ref elements);
        }

        private void WriteModules(ref Module[,] elements)
        {            
            for (int y = 0; y < ModulesHeigh; ++y)
            {
                for (int x = 0; x < ModulesWide; ++x)
                {
                    if (x == 0 || y == 0 || x == 4 || y == 4)
                    {
                        new Module(new Cord(TopLeftCord.X + x, TopLeftCord.Y + y), Color.Black, ref elements);
                    }
                    else if (y == 1 || y == 3 || x == 1 || x == 3)
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
