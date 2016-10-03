using System;
using System.Collections.Generic;
using System.Drawing;
using Exostasis.QR.Common.Image;

namespace Exostasis.QR.Image
{
    public class FinderPattern : Element
    {
        public static int ModulesWidth
        {
            get { return 7; }
        }

        public static int ModulesHeight
        {
            get { return 7; }
        }

        private List<Module> Modules { get; set; }

        public FinderPattern(Cord topLeftCord)
        {
            TopLeftCord = topLeftCord;
            TopRightCord = new Cord(topLeftCord.X + ModulesWidth, topLeftCord.Y);
            BottomLeftCord = new Cord(topLeftCord.X, topLeftCord.Y + ModulesHeight);
            BottomRightCord = new Cord(topLeftCord.X + ModulesWidth, topLeftCord.Y + ModulesHeight);

            WriteModules();
        }

        private void WriteModules()
        {
            Modules = new List<Module>();
            for (int y = 0; y < 7; ++y)
            {
                for (int x = 0; x < 7; ++x)
                {
                    if (x == 0 || y == 0 || x == 6 || y == 6)
                    {
                        Modules.Add(new Module(new Cord(TopLeftCord.X + x, TopLeftCord.Y + y), Color.Black));
                    }
                    else if (y == 1 || y == 5 || x == 1 || x == 5)
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