using Exostasis.QR.Common.Image;

namespace Exostasis.QR.Image
{
    public class AlignmentPattern : Element
    {
        public int ModulesWide
        {
            get { return 5; }
        }

        public int ModulesHeigh
        {
            get { return 5; }
        }

        public AlignmentPattern(Cord topLeftCord)
        {
            TopLeftCord = topLeftCord;
            TopRightCord = new Cord(topLeftCord.X + ModulesWide, topLeftCord.Y);
            BottomLeftCord = new Cord(topLeftCord.X, topLeftCord.Y + ModulesHeigh);
            BottomRightCord = new Cord(topLeftCord.X + ModulesWide, topLeftCord.Y + ModulesHeigh);
        }
    }
}
