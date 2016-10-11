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

namespace Exostasis.QR.Common.Image
{
    [Serializable]
    public abstract class Element
    {
        public Cord TopLeftCord { get; protected set; }
        public Cord TopRightCord { get; protected set; }
        public Cord BottomLeftCord { get; protected set; }
        public Cord BottomRightCord { get; protected set; }

        public bool IsWithinSpace(Element other)
        {
            if (other.TopLeftCord.X <= TopRightCord.X && other.TopLeftCord.X >= TopLeftCord.X &&
                other.TopLeftCord.Y <= BottomLeftCord.Y && other.TopLeftCord.Y >= TopLeftCord.Y)
            {
                return true;
            }

            if (other.TopRightCord.X <= TopRightCord.X && other.TopRightCord.X >= TopLeftCord.X &&
                other.TopRightCord.Y <= BottomLeftCord.Y && other.TopRightCord.Y >= TopLeftCord.Y)
            {
                return true;
            }

            if (other.BottomLeftCord.X <= TopRightCord.X && other.BottomLeftCord.X >= TopLeftCord.X &&
                other.BottomLeftCord.Y <= BottomLeftCord.Y && other.BottomLeftCord.Y >= TopLeftCord.Y)
            {
                return true;
            }

            return other.BottomRightCord.X <= TopRightCord.X && other.BottomRightCord.X >= TopLeftCord.X &&
                   other.BottomRightCord.Y <= BottomLeftCord.Y && other.BottomRightCord.Y >= TopLeftCord.Y;
        }

        public bool IsWithinSpace(int x, int y)
        {
            return x <= TopRightCord.X && x >= TopLeftCord.X && y <= BottomLeftCord.Y && y >= TopLeftCord.Y;
        }
    }
}
