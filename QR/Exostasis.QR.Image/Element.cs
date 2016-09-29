using Exostasis.QR.Common.Image;

namespace Exostasis.QR.Image
{
    public abstract class Element
    {
        protected Cord _topLeftCord { get; set; }
        protected Cord _topRightCord { get; set; }
        protected Cord _bottomLeftCord { get; set; }
        protected Cord _bottomRightCord { get; set; }

        public bool IsWithinSpace (Element other)
        {
            if(other._topLeftCord._x <= _topRightCord._x && other._topLeftCord._x >= _topLeftCord._x && other._topLeftCord._y <= _bottomLeftCord._y && other._topLeftCord._y >= _topLeftCord._y)
            {
                return true;
            }

            if(other._topRightCord._x <= _topRightCord._x && other._topRightCord._x >= _topLeftCord._x && other._topRightCord._y <= _bottomLeftCord._y && other._topRightCord._y >= _topLeftCord._y)
            {
                return true;
            }

            if(other._bottomLeftCord._x <= _topRightCord._x && other._bottomLeftCord._x >= _topLeftCord._x && other._bottomLeftCord._y <= _bottomLeftCord._y && other._bottomLeftCord._y >= _topLeftCord._y)
            {
                return true;
            }

            if(other._bottomRightCord._x <= _topRightCord._x && other._bottomRightCord._x >= _topLeftCord._x && other._bottomRightCord._y <= _bottomLeftCord._y && other._bottomRightCord._y >= _topLeftCord._y)
            {
                return true;
            }

            return false;
        }
    }
}
