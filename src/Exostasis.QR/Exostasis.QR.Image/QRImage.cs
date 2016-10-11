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
using System.Collections;
using System.Collections.Generic;
using Exostasis.QR.Common.Image;
using System.Drawing;
using Exostasis.QR.Common;
using Exostasis.QR.Common.Enum;
using Exostasis.QR.DataMask;

namespace Exostasis.QR.Image
{
    public class QrImage
    {
        private int Version { get; }
        private int Scale { get; }
        private Module[,] _elements;

        private FinderPattern TopLeftFinderPattern { get; set; }
        private FinderPattern TopRightFinderPattern { get; set; }
        private FinderPattern BottomLeftFinderPattern { get; set; }

        private TimingPattern LeftTimingPattern { get; set; }
        private TimingPattern TopTimingPattern { get; set; }

        private List<Element> ExcludedElments { get; }

        private ErrorCorrectionLevel ErrorCorrectionLevel { get; }

        public QrImage(int version, int scale, List<BitArray> structuredArray, ErrorCorrectionLevel errorCorrectionLevel)
        {
            Version = version;
            Scale = scale;
            _elements = new Module[GetModuleSize(), GetModuleSize()];
            ErrorCorrectionLevel = errorCorrectionLevel;
            ExcludedElments = new List<Element>();           
            AddFinderPatterns();
            AddSeperators();
            AddAlignmentPatterns();
            AddTimingPatterns();
            AddDarkModule();
            WriteBitArray(structuredArray);

            var dataMasker = new DataMasker(Version, ExcludedElments, _elements, GetModuleSize());
            dataMasker.CalculateDataMask();
            _elements = dataMasker.MaskedImage;
            WriteFormatStringAndVersionInformation(dataMasker.MaskVerion);
        }        

        private void AddAlignmentPatterns()
        {
            List<Cord> possibleCords = CalculateAlignmentPatternCords();

            possibleCords.ForEach(cord =>
            {
                for (int y = 0; y < AlignmentPattern.ModulesHeigh; ++y)
                {
                    for (int x = 0; x < AlignmentPattern.ModulesWide; ++x)
                    {
                        if (_elements[cord.X - AlignmentPattern.ModulesWide / 2 + x, cord.Y - AlignmentPattern.ModulesHeigh / 2 + y] 
                            != null)
                        {
                            return;
                        }
                    }
                }

                ExcludedElments.Add(new AlignmentPattern(cord, ref _elements));
            });          
        }

        private void AddDarkModule()
        {
            ExcludedElments.Add(new Module(new Cord(BottomLeftFinderPattern.TopRightCord.X + 1, BottomLeftFinderPattern.TopRightCord.Y - 1), 
                Color.Black, ref _elements));
        }

        private void AddFinderPatterns()
        {
            TopLeftFinderPattern = new FinderPattern(new Cord(0, 0), ref _elements);
            TopRightFinderPattern = new FinderPattern(new Cord(GetModuleSize() - FinderPattern.ModulesWide, 0), ref _elements);
            BottomLeftFinderPattern = new FinderPattern(new Cord(0, GetModuleSize() - FinderPattern.ModulesHeigh), ref _elements);

            ExcludedElments.Add(TopLeftFinderPattern);
            ExcludedElments.Add(TopRightFinderPattern);
            ExcludedElments.Add(BottomLeftFinderPattern);
        }

        private void AddSeperators()
        {        
            ExcludedElments.Add(new Seperator(new Cord(TopLeftFinderPattern.TopRightCord.X, TopLeftFinderPattern.TopRightCord.Y), 1,
                FinderPattern.ModulesHeigh + 1, ref _elements));
            ExcludedElments.Add(new Seperator(new Cord(TopLeftFinderPattern.BottomLeftCord.X, TopLeftFinderPattern.BottomLeftCord.Y),
                FinderPattern.ModulesWide, 1, ref _elements));

            ExcludedElments.Add(new Seperator(new Cord(TopRightFinderPattern.TopLeftCord.X - 1, TopRightFinderPattern.TopLeftCord.Y), 1,
                FinderPattern.ModulesHeigh + 1, ref _elements));
            ExcludedElments.Add(new Seperator(new Cord(TopRightFinderPattern.BottomLeftCord.X, TopRightFinderPattern.BottomLeftCord.Y),
                FinderPattern.ModulesWide, 1, ref _elements));

            ExcludedElments.Add(new Seperator(new Cord(BottomLeftFinderPattern.TopLeftCord.X, BottomLeftFinderPattern.TopLeftCord.Y - 1) ,
                FinderPattern.ModulesWide, 1, ref _elements));
            ExcludedElments.Add(new Seperator(new Cord(BottomLeftFinderPattern.TopRightCord.X, BottomLeftFinderPattern.TopRightCord.Y - 1), 1,
                FinderPattern.ModulesHeigh + 1, ref _elements));
        }

        private void AddTimingPatterns()
        {            
            TopTimingPattern = 
                new TimingPattern(new Cord(TopLeftFinderPattern.BottomRightCord.X + 1, TopLeftFinderPattern.BottomRightCord.Y - 1), 
                TopRightFinderPattern.BottomLeftCord.X - TopLeftFinderPattern.BottomRightCord.X, 1, ref _elements);

            LeftTimingPattern = 
                new TimingPattern(new Cord(TopLeftFinderPattern.BottomRightCord.X - 1, TopLeftFinderPattern.BottomRightCord.Y + 1),
                1, BottomLeftFinderPattern.TopRightCord.Y - 1 - TopLeftFinderPattern.BottomRightCord.Y - 1, ref _elements);

            ExcludedElments.Add(TopTimingPattern);
            ExcludedElments.Add(LeftTimingPattern);
        }        

        private List<Cord> CalculateAlignmentPatternCords()
        {
            List<Cord> cords = new List<Cord>();
            int[] possibleCords = Constants.AlignmentCords[Version]?.Values;

            if (possibleCords != null)
            {
                for (int i = 0; i < possibleCords.Length; ++i)
                {
                    for (int j = i; j < possibleCords.Length; ++j)
                    {
                        cords.Add(new Cord(possibleCords[i], possibleCords[j]));
                        if (possibleCords[i] != possibleCords[j])
                        {
                            cords.Add(new Cord(possibleCords[j], possibleCords[i]));
                        }
                    }
                }
            }

            return cords;
        }

        private int GetModuleSize()
        {
            return Version * 4 + 21;
        }

        private void WriteFormatStringAndVersionInformation(int maskVersion)
        {
            int errorCorrectionValue = 0;

            if (ErrorCorrectionLevel == ErrorCorrectionLevel.H)
            {
                errorCorrectionValue = 2;
            }
            else if (ErrorCorrectionLevel == ErrorCorrectionLevel.L)
            {
                errorCorrectionValue = 1;
            }
            else if (ErrorCorrectionLevel == ErrorCorrectionLevel.Q)
            {
                errorCorrectionValue = 3;
            }

            BitArray formatBits = new BitArray(BitConverter.GetBytes((errorCorrectionValue << 13) + (maskVersion << 10)));
            for (int i = formatBits.Count - 1; i >= 0; --i)
            {
                if (!formatBits[i])
                {
                    --formatBits.Length;
                }
                else
                {
                    break;
                }
            }

            int countBits = formatBits.Count;
            var errorCorrection = new BitArray(formatBits);
            while (countBits > 10)
            {
                BitArray generatorBits = new BitArray(BitConverter.GetBytes(1335 << (countBits - 11)));
                generatorBits.Length = countBits;

                errorCorrection = errorCorrection.Xor(generatorBits);

                for (int i = errorCorrection.Count - 1; i >= 0; --i)
                {
                    if (errorCorrection[i])
                    {
                        countBits = i + 1;
                        break;
                    }
                }
                errorCorrection.Length = countBits;
            }

            errorCorrection.Length = 10;

            var informationString = new BitArray(15);

            for (int i = formatBits.Count - 1; i >= 0; --i)
            {
                if (i > 14 - 5)
                {
                    informationString[i] = formatBits[i];
                }
                else
                {
                    informationString[i] = errorCorrection[i];
                }
            }

            var maskString = new BitArray(BitConverter.GetBytes(21522));
            maskString.Length = 15;
            informationString = informationString.Xor(maskString);

            for (int i = 0; i < informationString.Count; ++i)
            {                
                switch (i)
                {
                    case 0:
                        new Module(new Cord(8, i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 1 - i, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 1:
                        new Module(new Cord(8, i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 1 - i, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 2:
                        new Module(new Cord(8, i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 1 - i, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 3:
                        new Module(new Cord(8, i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 1 - i, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 4:
                        new Module(new Cord(8, i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 1 - i, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 5:
                        new Module(new Cord(8, i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 1 - i, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 6:
                        new Module(new Cord(8, i + 1), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 1 - i, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 7:
                        new Module(new Cord(8, i + 1), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 1 - i, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 8:
                        new Module(new Cord(i - 1, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(8, GetModuleSize() + 1 - i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 9:
                        new Module(new Cord(i - 4, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(8, GetModuleSize() + 3 - i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 10:
                        new Module(new Cord(i - 6, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(8, GetModuleSize() + 5 - i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 11:
                        new Module(new Cord(i - 8, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(8, GetModuleSize() + 7 - i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 12:
                        new Module(new Cord(i - 10, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(8, GetModuleSize() + 9 - i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 13:
                        new Module(new Cord(i - 12, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(8, GetModuleSize() + 11 - i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 14:
                        new Module(new Cord(i - 14, 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(8, GetModuleSize() + 13 - i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                }
            }

            BitArray versionBits = new BitArray(BitConverter.GetBytes((Version + 1) << 12));
            for (int i = versionBits.Count - 1; i >= 0; --i)
            {
                if (!versionBits[i])
                {
                    --versionBits.Length;
                }
                else
                {
                    break;
                }
            }

            countBits = versionBits.Count;
            errorCorrection = new BitArray(versionBits);
            while (countBits > 12)
            {
                BitArray generatorBits = new BitArray(BitConverter.GetBytes(7973 << (countBits - 13)));
                generatorBits.Length = countBits;

                errorCorrection = errorCorrection.Xor(generatorBits);

                for (int i = errorCorrection.Count - 1; i >= 0; --i)
                {
                    if (errorCorrection[i])
                    {
                        countBits = i + 1;
                        break;
                    }
                }
                errorCorrection.Length = countBits;
            }

            errorCorrection.Length = 13;

            informationString = new BitArray(18);

            for (int i = versionBits.Count - 1; i >= 0; --i)
            {
                if (i > 17 - 6)
                {
                    informationString[i] = versionBits[i];
                }
                else
                {
                    informationString[i] = errorCorrection[i];
                }
            }

            for (int i = 0; i < informationString.Count; ++i)
            {
                switch (i)
                {
                    case 0:
                        new Module(new Cord(i, GetModuleSize() - 11), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 11, i), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 1:
                        new Module(new Cord(i - 1, GetModuleSize() - 10), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 10, i - 1), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 2:
                        new Module(new Cord(i - 2, GetModuleSize() - 9), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 9, i - 2), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 3:
                        new Module(new Cord(i - 2, GetModuleSize() - 11), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 11, i - 2), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 4:
                        new Module(new Cord(i - 3, GetModuleSize() - 10), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 10, i - 3), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 5:
                        new Module(new Cord(i - 4, GetModuleSize() - 9), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 9, i - 4), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 6:
                        new Module(new Cord(i - 4, GetModuleSize() - 11), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 11, i - 4), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 7:
                        new Module(new Cord(i - 5, GetModuleSize() - 10), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 10, i - 5), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 8:
                        new Module(new Cord(i - 6, GetModuleSize() - 9), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 9, i - 6), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 9:
                        new Module(new Cord(i - 6, GetModuleSize() - 11), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 11, i - 6), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 10:
                        new Module(new Cord(i - 7, GetModuleSize() - 10), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 10, i - 7), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 11:
                        new Module(new Cord(i - 8, GetModuleSize() - 9), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 9, i - 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 12:
                        new Module(new Cord(i - 8, GetModuleSize() - 11), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 11, i - 8), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 13:
                        new Module(new Cord(i - 9, GetModuleSize() - 10), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 10, i - 9), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 14:
                        new Module(new Cord(i - 10, GetModuleSize() - 9), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 9, i - 10), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 15:
                        new Module(new Cord(i - 10, GetModuleSize() - 11), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 11, i - 10), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 16:
                        new Module(new Cord(i - 11, GetModuleSize() - 10), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 10, i - 11), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                    case 17:
                        new Module(new Cord(i - 12, GetModuleSize() - 9), informationString[i] ? Color.Black : Color.White, ref _elements);
                        new Module(new Cord(GetModuleSize() - 9, i - 12), informationString[i] ? Color.Black : Color.White, ref _elements);
                        break;
                }
            }
        }

        public void WriteImage(string filename)
        {
            using (var qrBitmap = new Bitmap((GetModuleSize() + 8) * Scale, (GetModuleSize() + 8) * Scale))
            {
                for (int y = 0; y < GetModuleSize() + 8; ++y)
                {
                    for (int x = 0; x < GetModuleSize() + 8; ++x)
                    {
                        if (x < 4 || x >= GetModuleSize() + 4 || y < 4 || y >= GetModuleSize() + 4)
                        {
                            for (int i = 0; i < Scale; ++i)
                            {
                                for (int j = 0; j < Scale; ++j)
                                {
                                    qrBitmap.SetPixel(x * Scale + i, y * Scale + j, Color.White);
                                }
                            }
                        }
                        else if (_elements[x - 4, y - 4] != null)
                        {
                            for (int i = 0; i < Scale; ++i)
                            {
                                for (int j = 0; j < Scale; ++j)
                                {
                                    qrBitmap.SetPixel(x * Scale + i, y * Scale + j, _elements[x - 4, y - 4].PixelColor);
                                }
                            }
                        }
                    }
                }
                qrBitmap.Save(filename);
            }
        }

        private void WriteBitArray(List<BitArray> structuredArray)
        {
            int x = GetModuleSize() - 1;
            int y = GetModuleSize() - 1;            
            int listIndex = 0;
            int listElement = structuredArray[listIndex].Count - 1;
            bool goingUp = true;

            while (listIndex < structuredArray.Count)
            {
                if (x >= TopRightFinderPattern.BottomLeftCord.X - 1 && x < TopRightFinderPattern.BottomRightCord.X && 
                    y == TopRightFinderPattern.BottomLeftCord.Y + 1)
                {
                    new Module(new Cord(x, y), Color.White, ref _elements);
                }
                else if (x >= TopLeftFinderPattern.BottomLeftCord.X && x <= TopLeftFinderPattern.BottomRightCord.X + 1 && 
                    y == TopLeftFinderPattern.BottomLeftCord.Y + 1)
                {
                    new Module(new Cord(x, y), Color.White, ref _elements);
                }
                else if (x == TopLeftFinderPattern.TopRightCord.X + 1 && y >= TopLeftFinderPattern.TopRightCord.Y && 
                    y < TopLeftFinderPattern.BottomRightCord.Y - 1)
                {
                    new Module(new Cord(x, y), Color.White, ref _elements);
                }
                else if (x == TopLeftFinderPattern.TopRightCord.X + 1 && y >= TopLeftFinderPattern.TopRightCord.Y - 1 &&
                    y <= TopLeftFinderPattern.BottomRightCord.Y - 1)
                {
                    
                }
                else if (x == TopLeftFinderPattern.TopRightCord.X + 1 && y >= TopLeftFinderPattern.BottomRightCord.Y &&
                    y <= TopLeftFinderPattern.BottomRightCord.Y)
                {
                    new Module(new Cord(x, y), Color.White, ref _elements);
                }
                else if (x == BottomLeftFinderPattern.TopRightCord.X + 1 && y >= BottomLeftFinderPattern.TopRightCord.Y &&
                    y < BottomLeftFinderPattern.BottomRightCord.Y)
                {
                    new Module(new Cord(x, y), Color.White, ref _elements);
                }
                else if (Version >= 6 && x >= TopRightFinderPattern.TopLeftCord.X - 4 && x < TopRightFinderPattern.TopLeftCord.X - 1 &&
                    y >= TopRightFinderPattern.TopLeftCord.Y && y < TopRightFinderPattern.BottomLeftCord.Y)
                {
                    new Module(new Cord(x, y), Color.White, ref _elements);
                }
                else if (Version >= 6 && x >= BottomLeftFinderPattern.TopLeftCord.X && x < BottomLeftFinderPattern.TopRightCord.X - 1 &&
                    y >= BottomLeftFinderPattern.TopLeftCord.Y - 4 && y < BottomLeftFinderPattern.TopLeftCord.Y - 1)
                {
                    new Module(new Cord(x, y), Color.White, ref _elements);
                }
                else if (_elements[x, y] == null)
                {
                    new Module(new Cord(x, y), structuredArray[listIndex][listElement] ? Color.Black : Color.White, ref _elements);

                    if (listElement == 0)
                    {
                        ++listIndex;
                        if (listIndex < structuredArray.Count)
                        {
                            listElement = structuredArray[listIndex].Count - 1;
                        }                        
                    }
                    else
                    {
                        --listElement;
                    }
                }

                if (x - 1 == LeftTimingPattern.TopLeftCord.X && y == 0)
                {
                    x -= 2;
                    goingUp = !goingUp;
                }
                else if (y == 0 && x <= 5 && x % 2 == 0 && goingUp)
                {
                    --x;
                    goingUp = !goingUp;
                }
                else if (y == GetModuleSize() - 1 && x <= 5 && x % 2 == 0 && !goingUp)
                {
                    --x;
                    goingUp = !goingUp;
                }                
                else if (x <= 5 && x % 2 == 1)
                {
                    --x;                
                }
                else if (x <= 5 && x % 2 == 0 && goingUp)
                {
                    ++x;
                    --y;
                }
                else if (x <= 5 && x % 2 == 0)
                {
                    ++x;
                    ++y;
                }
                else if (y == GetModuleSize() - 1 && x % 2 == 1 && !goingUp)
                {
                    --x;
                    goingUp = !goingUp;
                }
                else if (y == 0 && x % 2 == 1 && goingUp)
                {
                    --x;
                    goingUp = !goingUp;
                }
                else if (x % 2 == 0)
                {
                    --x;
                }
                else if (goingUp)
                {
                    ++x;
                    --y;
                }
                else
                {
                    ++x;
                    ++y;
                }                                
            }

            int prevY = y;
            while (x >= 0)
            {
                while (y < GetModuleSize())
                {
                    if (_elements[x, y] == null)
                    {
                        new Module(new Cord(x, y), Color.White, ref _elements);
                    }
                    ++y;
                }
                --x;
                y = prevY;
            }
        }
    }
}