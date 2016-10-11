using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Exostasis.QR.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static T DeepCopy<T>(this T objectToCopy)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binaryFomatter = new BinaryFormatter();
                binaryFomatter.Serialize(memStream, objectToCopy);
                memStream.Position = 0;

                return (T) binaryFomatter.Deserialize(memStream);
            }
        }
    }
}
