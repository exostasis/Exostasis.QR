using System;
using System.Collections.Generic;
using System.Drawing;
using Exostasis.QR.Common.Image;

namespace Exostasis.QR.Image
{
    public class Seperator : Element
    {
        private List<Module> Modules { get; set; }

        public Seperator(Cord topLeftCord, int width, int height)
        {
            TopLeftCord = topLeftCord;
            TopRightCord = new Cord(topLeftCord.X + width, topLeftCord.Y);
            BottomLeftCord = new Cord(topLeftCord.X, topLeftCord.Y + height);
            BottomRightCord = new Cord(topLeftCord.X + width, topLeftCord.Y + height);

            WriteModules();
        }

        private void WriteModules()
        {
            for (int y = 0; y < BottomLeftCord.Y; ++y)
            {
                for (int x = 0; x < TopRightCord.X; ++x)
                {
                    Modules.Add(new Module(new Cord(TopLeftCord.X + x, TopLeftCord.Y + y), Color.White));
                }
            }
        }
    }
}
