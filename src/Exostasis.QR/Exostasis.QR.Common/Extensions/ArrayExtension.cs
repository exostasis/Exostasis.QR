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

namespace Exostasis.QR.Common.Extensions
{
    public static class ArrayExtension
    {
        public static T[] SubArray<T>(this T[] source, int start, int length)
        {
            T[] destination = new T[length];

            if (source == null)
            {
                throw new NullReferenceException("Source cannot be null");
            }
            else if (source.Length - start - length < 0)
            {
                throw new ArgumentException("Cannot index out of range");
            }

            Array.Copy(source, start, destination, 0, length);

            return destination;
        }
    }
}
