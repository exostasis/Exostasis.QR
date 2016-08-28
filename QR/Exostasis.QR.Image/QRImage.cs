namespace Exostasis.QR.Image
{
    public class QRImage
    {
        private int _version { get; set; }

        public QRImage (int version)
        {
            _version = version;
        }

        private int GetModuleSize ()
        {
            return ((_version - 1) * 4 + 21);
        } 

        private Cord GetTopRightFinderPatterTopLeftCornerLoc ()
        {
            return new Cord(GetModuleSize() - 7, 0);
        }

        private Cord GetBottomLeftFinderPatterTopLeftCornerLoc ()
        {
            return new Cord(0, GetModuleSize() - 7);
        }

        private void CalculateAlignmentPatternCords ()
        {

        }
    }
}