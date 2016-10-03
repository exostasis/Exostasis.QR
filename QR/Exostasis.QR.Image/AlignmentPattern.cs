using System.Collections.Generic;
using System.Drawing;
using Exostasis.QR.Common.Image;

namespace Exostasis.QR.Image
{
    public class AlignmentPattern : Element
    {
        public int ModulesWide
        {
            get { return 5; }
        }

        public int ModulesHeigh
        {
            get { return 5; }
        }

        private List<Module> Modules { get; set; }

        public AlignmentPattern(Cord topLeftCord)
        {
            TopLeftCord = topLeftCord;
            TopRightCord = new Cord(topLeftCord.X + ModulesWide, topLeftCord.Y);
            BottomLeftCord = new Cord(topLeftCord.X, topLeftCord.Y + ModulesHeigh);
            BottomRightCord = new Cord(topLeftCord.X + ModulesWide, topLeftCord.Y + ModulesHeigh);

            WriteModules();
        }

        private void WriteModules()
        {
            Modules = new List<Module>();
            for (int y = 0; y < 5; ++y)
            {
                for (int x = 0; x < 5; ++x)
                {
                    if (x == 0 || y == 0 || x == 4 || y == 4)
                    {
                        Modules.Add(new Module(new Cord(TopLeftCord.X + x, TopLeftCord.Y + y), Color.Black));
                    }
                    else if (y == 1 || y == 3 || x == 1 || x == 3)
                    {
                        Modules.Add(new Module(new Cord(TopLeftCord.X + x, TopLeftCord.Y + y), Color.White));
                    }
                    else
                    {
                        Modules.Add(new Module(new Cord(TopLeftCord.X + x, TopLeftCord.Y + y), Color.Black));
                    }
                }
            }
        }        
    }
}
