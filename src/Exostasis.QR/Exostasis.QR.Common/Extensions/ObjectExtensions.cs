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

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Exostasis.QR.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static T DeepCopy<T>(this T objectToCopy)
        {
            using (var memStream = new MemoryStream())
            {
                var binaryFomatter = new BinaryFormatter();
                binaryFomatter.Serialize(memStream, objectToCopy);
                memStream.Position = 0;

                return (T) binaryFomatter.Deserialize(memStream);
            }
        }
    }
}
