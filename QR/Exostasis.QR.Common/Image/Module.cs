﻿using System.Drawing;

namespace Exostasis.QR.Common.Image
{
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