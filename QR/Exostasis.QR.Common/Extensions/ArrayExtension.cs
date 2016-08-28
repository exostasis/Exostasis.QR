using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exostasis.QR.Common.Extensions
{
    public static class ArrayExtension
    {
        public static T[] SubArray<T>(this T[] source, int start, int length)
        {
            T[] destination = new T[length];

            if (source == null)
            {
                throw new NullReferenceException("Source cannot be null");
            }
            else if (source.Length - start - length < 0)
            {
                throw new ArgumentException("Cannot index out of range");
            }

            Array.Copy(source, start, destination, 0, length);

            return destination;
        }
    }
}
