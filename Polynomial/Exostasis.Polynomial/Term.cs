using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exostasis.Equations
{
    public class Term
    {
        public int _constant { get; private set; }
        public Variable _variable { get; private set; }

        public Term (int constant)
        {
            _constant = constant;
        }
    }
}
