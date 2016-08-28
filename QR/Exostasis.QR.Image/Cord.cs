using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exostasis.QR.Image
{
    public class Cord
    {
        private int _x { get; set; }
        private int _y { get; set; }

        public Cord (int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}
