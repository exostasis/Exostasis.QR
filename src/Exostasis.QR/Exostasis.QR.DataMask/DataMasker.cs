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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Exostasis.QR.Common.Extensions;
using Exostasis.QR.Common.Image;

namespace Exostasis.QR.DataMask
{
    public class DataMasker
    {
        public int MaskVerion { get; private set; }

        public Module[,] MaskedImage { get; private set; }

        private int Version { get; set; }
        private int Size { get; set; }

        private List<Element> ExcludedElements { get; set; }
        private Module[,] Image { get; set; }

        public DataMasker(int version, List<Element> excludedElements, Module[,] image, int size)
        {
            Version = version;
            ExcludedElements = excludedElements;
            Image = image;
            Size = size;
        }

        public void CalculateDataMask()
        {
            Module[,] tempImage;

            tempImage = TryDataMask1(Image);
            int lowestPenaltyPoints = CalculatePenalty(tempImage);
            MaskedImage = tempImage;
            MaskVerion = 0;

            tempImage = TryDataMask2(Image);
            int penaltyPoints = CalculatePenalty(tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 1;
                lowestPenaltyPoints = penaltyPoints;
            }

            tempImage = TryDataMask3(Image);
            penaltyPoints = CalculatePenalty(tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 2;
                lowestPenaltyPoints = penaltyPoints;
            }

            tempImage = TryDataMask4(Image);
            penaltyPoints = CalculatePenalty(tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 3;
                lowestPenaltyPoints = penaltyPoints;
            }

            tempImage = TryDataMask5(Image);
            penaltyPoints = CalculatePenalty(Image);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 4;
                lowestPenaltyPoints = penaltyPoints;
            }

            tempImage = TryDataMask6(Image);
            penaltyPoints = CalculatePenalty(tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 5;
                lowestPenaltyPoints = penaltyPoints;
            }

            tempImage = TryDataMask7(Image);
            penaltyPoints = CalculatePenalty(tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 6;
                lowestPenaltyPoints = penaltyPoints;
            }

            tempImage = TryDataMask8(Image);
            penaltyPoints = CalculatePenalty(tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 7;
            }
        }

        private Module[,] TryDataMask1(Module[,] inputImage)
        {
            var outImage = inputImage.DeepCopy();

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if ((x + y)%2 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return outImage;
        }

        private Module[,] TryDataMask2(Module[,] inputImage)
        {
            var outImage = inputImage.DeepCopy();

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if (y%2 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return outImage;
        }

        private Module[,] TryDataMask3(Module[,] inputImage)
        {
            var outImage = inputImage.DeepCopy();

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if (x%3 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return outImage;
        }

        private Module[,] TryDataMask4(Module[,] inputImage)
        {
            var outImage = inputImage.DeepCopy();

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if ((x + y)%3 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return outImage;
        }

        private Module[,] TryDataMask5(Module[,] inputImage)
        {
            var outImage = inputImage.DeepCopy();

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if ((Math.Floor((decimal) y/2) + Math.Floor((decimal) x/3))%2 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return outImage;
        }

        private Module[,] TryDataMask6(Module[,] inputImage)
        {
            var outImage = inputImage.DeepCopy();

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if (x * y % 2 + x * y % 3 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return outImage;
        }

        private Module[,] TryDataMask7(Module[,] inputImage)
        {
            var outImage = inputImage.DeepCopy();

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if ((x*y%2 + x*y%3)%2 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return outImage;
        }

        private Module[,] TryDataMask8(Module[,] inputImage)
        {
            var outImage = inputImage.DeepCopy();

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if (((x + y)%2 + x*y%3)%2 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return outImage;
        }

        private bool IsExcludedRegion(int x, int y)
        {
            return ExcludedElements.Any(element => element.IsWithinSpace(x, y));
        }

        private int CalculatePenalty(Module[,] image)
        {
            int penalty = 0;

            penalty += CalculateRule1(image);
            penalty += CalculateRule2(image);
            penalty += CalculateRule3(image);
            penalty += CalculateRule4(image);

            return penalty;
        }

        private int CalculateRule1(Module[,] image)
        {
            int penalty = 0;
            int consecutiveColorCount = 0;
            Color previousColor = Color.Red;

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if (x == 0)
                    {
                        previousColor = image[x, y].PixelColor;
                        ++consecutiveColorCount;
                    }
                    else
                    {
                        if (previousColor == image[x, y].PixelColor)
                        {
                            ++consecutiveColorCount;
                        }
                        else
                        {
                            previousColor = image[x, y].PixelColor;
                            if (consecutiveColorCount >= 5)
                            {
                                penalty += 3 + (consecutiveColorCount - 5);
                            }

                            consecutiveColorCount = 0;
                        }
                    }
                }

                consecutiveColorCount = 0;
                previousColor = Color.Red;
            }

            for (int x = 0; x < Size; ++x)
            {
                for (int y = 0; y < Size; ++y)
                {
                    if (y == 0)
                    {
                        previousColor = image[x, y].PixelColor;
                        ++consecutiveColorCount;
                    }
                    else
                    {
                        if (previousColor == image[x, y].PixelColor)
                        {
                            ++consecutiveColorCount;
                        }
                        else
                        {
                            previousColor = image[x, y].PixelColor;
                            if (consecutiveColorCount >= 5)
                            {
                                penalty += 3 + (consecutiveColorCount - 5);
                            }

                            consecutiveColorCount = 0;
                        }
                    }
                }

                consecutiveColorCount = 0;
                previousColor = Color.Red;
            }

            return penalty;
        }

        private int CalculateRule2(Module[,] image)
        {
            var penalty = 0;

            for (int y = 0; y < Size - 1; ++y)
            {
                for (int x = 0; x < Size - 1; ++x)
                {
                    if (image[x, y].PixelColor == image[x + 1, y].PixelColor &&
                        image[x, y].PixelColor == image[x, y + 1].PixelColor &&
                        image[x, y].PixelColor == image[x + 1, y + 1].PixelColor)
                    {
                        penalty += 3;
                    }
                }
            }

            return penalty;
        }

        private int CalculateRule3(Module[,] image)
        {
            var penalty = 0;

            for (int y = 0; y < Size - 11; ++y)
            {
                for (int x = 0; x < Size - 11; ++x)
                {
                    if (image[x, y].PixelColor == Color.Black && image[x + 1, y].PixelColor == Color.White &&
                        image[x + 2, y].PixelColor == Color.Black && image[x + 3, y].PixelColor == Color.Black &&
                        image[x + 4, y].PixelColor == Color.Black && image[x + 5, y].PixelColor == Color.White &&
                        image[x + 6, y].PixelColor == Color.Black && image[x + 7, y].PixelColor == Color.White &&
                        image[x + 8, y].PixelColor == Color.White && image[x + 9, y].PixelColor == Color.White &&
                        image[x + 10, y].PixelColor == Color.White)
                    {
                        penalty += 40;
                    }
                    else if (image[x, y].PixelColor == Color.Black && image[x, y + 1].PixelColor == Color.White &&
                             image[x, y + 2].PixelColor == Color.Black && image[x, y + 3].PixelColor == Color.Black &&
                             image[x, y + 4].PixelColor == Color.Black && image[x, y + 5].PixelColor == Color.White &&
                             image[x, y + 6].PixelColor == Color.Black && image[x, y + 7].PixelColor == Color.White &&
                             image[x, y + 8].PixelColor == Color.White && image[x, y + 9].PixelColor == Color.White &&
                             image[x, y + 10].PixelColor == Color.White)
                    {
                        penalty += 40;
                    }
                    else if (image[x, y].PixelColor == Color.White && image[x + 1, y].PixelColor == Color.White &&
                             image[x + 2, y].PixelColor == Color.White && image[x + 3, y].PixelColor == Color.White &&
                             image[x + 4, y].PixelColor == Color.Black && image[x + 5, y].PixelColor == Color.White &&
                             image[x + 6, y].PixelColor == Color.Black && image[x + 7, y].PixelColor == Color.Black &&
                             image[x + 8, y].PixelColor == Color.Black && image[x + 9, y].PixelColor == Color.White &&
                             image[x + 10, y].PixelColor == Color.Black)
                    {
                        penalty += 40;
                    }
                    else if (image[x, y].PixelColor == Color.White && image[x, y + 1].PixelColor == Color.White &&
                             image[x, y + 2].PixelColor == Color.White && image[x, y + 3].PixelColor == Color.White &&
                             image[x, y + 4].PixelColor == Color.Black && image[x, y + 5].PixelColor == Color.White &&
                             image[x, y + 6].PixelColor == Color.Black && image[x, y + 7].PixelColor == Color.Black &&
                             image[x, y + 8].PixelColor == Color.Black && image[x, y + 9].PixelColor == Color.White &&
                             image[x, y + 10].PixelColor == Color.Black)
                    {
                        penalty += 40;
                    }
                }
            }

            return penalty;
        }

        private int CalculateRule4(Module[,] image)
        {
            int numModules = Size*Size;
            int countBlackModule = 0;

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if (image[x, y].PixelColor == Color.Black)
                    {
                        ++countBlackModule;
                    }
                }
            }

            int percentage = (int) ((double) countBlackModule / numModules * 100);

            var lowerMultiple = percentage - percentage % 5;
            var upperMultiple = percentage + (5 - percentage % 5);

            var lowerAbsolute = Math.Abs(lowerMultiple - 50);
            var upperAbsolute = Math.Abs(upperMultiple - 50);

            if (lowerAbsolute / 5 < upperAbsolute / 5)
            {
                return 10 * lowerAbsolute / 5;
            }

            return 10 * upperAbsolute / 5;
        }
    }
}
