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

using System;
using System.Drawing;

namespace Exostasis.QR.Common.Image
{
    [Serializable]
    public class Module : Element
    {
        public Color PixelColor { get; private set; }

        public Module(Cord topLeftCord, Color pixelColor, ref Module[,] elements)
        {
            PixelColor = pixelColor;
            TopLeftCord = topLeftCord;
            TopRightCord = new Cord(topLeftCord.X + 1, topLeftCord.Y);
            BottomLeftCord = new Cord(topLeftCord.X, topLeftCord.Y + 1);
            BottomRightCord = new Cord(topLeftCord.X + 1, topLeftCord.Y + 1);

            WriteModule(ref elements);
        }

        private void WriteModule(ref Module[,] elements)
        {
            elements[TopLeftCord.X, TopLeftCord.Y] = this;
        }

        public void InvertPixelColor()
        {
            PixelColor = PixelColor == Color.Black? Color.White: Color.Black;
        }
    }
}
