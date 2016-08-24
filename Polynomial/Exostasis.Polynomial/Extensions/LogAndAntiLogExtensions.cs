using System.Collections.Generic;
using System.Linq;

namespace Exostasis.Polynomial.Extensions
{
    public static class LogAndAntiLogExtensions
    {
        private static Dictionary<AlphaTerm, int> _antiLogs = new Dictionary<AlphaTerm, int>();

        public static int AntiLog (this AlphaTerm self)
        {
            if (_antiLogs.ContainsKey(self))
            {
                return _antiLogs[self];
            }

            int value = 2 * (self - 1).AntiLog();

            if (value > 255)
            {
                value ^= 285;
            }

            _antiLogs.Add(self, value);

            return value;
        }

        public static AlphaTerm Log(this int self)
        {
            var value = _antiLogs.Where(d => d.Value == self)
                .Select(d => d.Key)
                .SingleOrDefault();
            if (value != null)
            {
                return value;
            }

            for (int i = 0; i <= 255; ++i)
            {
                value = new AlphaTerm(i);

                if (value.AntiLog() == self)
                {
                    break;
                }
            }

            return value;
        }
    }
}
