using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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
            Module[,] tempImage = new Module[Size, Size];

            int lowestPenaltyPoints = TryDataMask1(Image, ref tempImage);
            MaskedImage = tempImage;
            MaskVerion = 0;

            int penaltyPoints = TryDataMask2(Image, ref tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 1;
            }

            penaltyPoints = TryDataMask3(Image, ref tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 2;
            }

            penaltyPoints = TryDataMask4(Image, ref tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 3;
            }

            penaltyPoints = TryDataMask5(Image, ref tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 4;
            }

            penaltyPoints = TryDataMask6(Image, ref tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 5;
            }

            penaltyPoints = TryDataMask7(Image, ref tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 6;
            }

            penaltyPoints = TryDataMask8(Image, ref tempImage);
            if (penaltyPoints < lowestPenaltyPoints)
            {
                MaskedImage = tempImage;
                MaskVerion = 7;
            }
        }        

        private int TryDataMask1(Module[,] inputImage, ref Module[,] outImage)
        {
            outImage = inputImage;

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if ((x + y) % 2 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return CalculatePenalty(outImage);
        }

        private int TryDataMask2(Module[,] inputImage, ref Module[,] outImage)
        {
            outImage = inputImage;

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if (x % 2 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return CalculatePenalty(outImage);
        }

        private int TryDataMask3(Module[,] inputImage, ref Module[,] outImage)
        {
            outImage = inputImage;

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if (y % 3 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return CalculatePenalty(outImage);
        }

        private int TryDataMask4(Module[,] inputImage, ref Module[,] outImage)
        {
            outImage = inputImage;

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if ((x + y) % 3 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return CalculatePenalty(outImage);
        }

        private int TryDataMask5(Module[,] inputImage, ref Module[,] outImage)
        {
            outImage = inputImage;

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if ((Math.Floor((decimal) x / 2) + Math.Floor((decimal) y / 3)) % 2 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return CalculatePenalty(outImage);
        }

        private int TryDataMask6(Module[,] inputImage, ref Module[,] outImage)
        {
            outImage = inputImage;

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

            return CalculatePenalty(outImage);
        }

        private int TryDataMask7(Module[,] inputImage, ref Module[,] outImage)
        {
            outImage = inputImage;

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if ((x * y % 2 + x * y % 3) % 2 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return CalculatePenalty(outImage);
        }

        private int TryDataMask8(Module[,] inputImage, ref Module[,] outImage)
        {
            outImage = inputImage;

            for (int y = 0; y < Size; ++y)
            {
                for (int x = 0; x < Size; ++x)
                {
                    if (((x + y) % 2 + x * y % 3) % 2 == 0 && !IsExcludedRegion(x, y))
                    {
                        outImage[x, y].InvertPixelColor();
                    }
                }
            }

            return CalculatePenalty(outImage);
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
                                penalty += 3 + (consecutiveColorCount - 3);                                
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
                                penalty += 3 + (consecutiveColorCount - 3);
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
            throw new NotImplementedException();
        }

        private int CalculateRule3(Module[,] image)
        {
            throw new NotImplementedException();
        }

        private int CalculateRule4(Module[,] image)
        {
            throw new NotImplementedException();
        }
    }
}
