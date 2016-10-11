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
    public class TimingPattern : Element
    {
        public TimingPattern(Cord topLeftCord, int width, int height, ref Module[,] elements)
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
                    new Module(new Cord(x, y), x % 2 == 0 && y  % 2 == 0 ? Color.Black : Color.White, ref elements);
                }
            }
        }
    }
}
