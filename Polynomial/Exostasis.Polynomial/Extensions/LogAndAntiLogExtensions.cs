using System;
using System.Collections.Generic;
using System.Linq;

namespace Exostasis.Polynomial.Extensions
{
    public static class LogAndAntiLogExtensions
    {
        private static Dictionary<int, AlphaTerm> _logs = new Dictionary<int, AlphaTerm>();

        public static int AntiLog (this AlphaTerm self)
        {
            var search = _logs.Where(d => d.Value.EqualExponent(self))
                .Select(d => d.Key);
            if (search.Count() == 1)
            {
                return search.ElementAt(0);
            }

            int value;
            if (self._exponent == 0)
            {
                value = 1;
                _logs.Add(value, self);
            }
            else
            {
                value = 2 * (self - 1).AntiLog();

                if (value > 255)
                {
                    value ^= 285;
                }
                if (!_logs.ContainsKey(value))
                {
                    _logs.Add(value, self);
                }
            }

            return value;
        }

        public static AlphaTerm Log(this int self, string variable, int exponent)
        {
            AlphaTerm value = null;

            if (_logs.ContainsKey(self))
            {
                return new AlphaTerm(_logs[self]._exponent, variable, exponent);
            }

            for (int i = 0; i <= 255; ++i)
            {
                value = new AlphaTerm(i, variable, exponent);

                if (value.AntiLog() == self)
                {
                    break;
                }
            }

            return value;
        }
    }
}
