using Exostasis.QR.Common.Image;

namespace Exostasis.QR.Image
{
    public class Seperator : Element
    {

        public Seperator(Cord topLeftCord, int width, int height)
        {
            TopLeftCord = topLeftCord;
            TopRightCord = new Cord(topLeftCord.X + width, topLeftCord.Y);
            BottomLeftCord = new Cord(topLeftCord.X, topLeftCord.Y + height);
            BottomRightCord = new Cord(topLeftCord.X + width, topLeftCord.Y + height);
        }
    }
}
