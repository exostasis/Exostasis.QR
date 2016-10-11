/*
 * Copyright 2016 Shawn Abtey. This source code is protected under the GNU General Public License 
 *  This file is part of Exostasis.QR.
 *  
 *  Exostasis.QR is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the 
 *  Free Software Foundation, either version 3 of the License, or (at your option) any later version.
 *  
 *  Exostasis.QR is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of 
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License along with Exostasis.QR.  If not, see <http://www.gnu.org/licenses/>.
 */

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