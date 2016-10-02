using Exostasis.QR.Common.Image;

namespace Exostasis.QR.Image
{
    public class FinderPattern : Element
    {
        public static int ModulesWidth
        {
            get { return 7; }
        }

        public static int ModulesHeight
        {
            get { return 7; }
        }

        public FinderPattern(Cord topLeftCord)
        {
            TopLeftCord = topLeftCord;
            TopRightCord = new Cord(topLeftCord.X + ModulesWidth, topLeftCord.Y);
            BottomLeftCord = new Cord(topLeftCord.X, topLeftCord.Y + ModulesHeight);
            BottomRightCord = new Cord(topLeftCord.X + ModulesWidth, topLeftCord.Y + ModulesHeight);
        }
    }
}
