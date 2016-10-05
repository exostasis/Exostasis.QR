namespace Exostasis.QR.Common.Image
{
    public abstract class Element
    {
        public Cord TopLeftCord { get; protected set; }
        public Cord TopRightCord { get; protected set; }
        public Cord BottomLeftCord { get; protected set; }
        public Cord BottomRightCord { get; protected set; }

        public bool IsWithinSpace(Element other)
        {
            if (other.TopLeftCord.X <= TopRightCord.X && other.TopLeftCord.X >= TopLeftCord.X &&
                other.TopLeftCord.Y <= BottomLeftCord.Y && other.TopLeftCord.Y >= TopLeftCord.Y)
            {
                return true;
            }

            if (other.TopRightCord.X <= TopRightCord.X && other.TopRightCord.X >= TopLeftCord.X &&
                other.TopRightCord.Y <= BottomLeftCord.Y && other.TopRightCord.Y >= TopLeftCord.Y)
            {
                return true;
            }

            if (other.BottomLeftCord.X <= TopRightCord.X && other.BottomLeftCord.X >= TopLeftCord.X &&
                other.BottomLeftCord.Y <= BottomLeftCord.Y && other.BottomLeftCord.Y >= TopLeftCord.Y)
            {
                return true;
            }

            return other.BottomRightCord.X <= TopRightCord.X && other.BottomRightCord.X >= TopLeftCord.X &&
                   other.BottomRightCord.Y <= BottomLeftCord.Y && other.BottomRightCord.Y >= TopLeftCord.Y;
        }

        public bool IsWithinSpace(int x, int y)
        {
            return x <= TopRightCord.X && x >= TopLeftCord.X && y <= BottomLeftCord.Y && y >= TopLeftCord.Y;
        }
    }
}
