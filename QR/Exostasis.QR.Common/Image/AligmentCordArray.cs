namespace Exostasis.QR.Common.Image
{
    public class AlignmentCordArray
    {
        public int[] Values { get; private set; }

        public AlignmentCordArray(params int[] values)
        {
            Values = values;
        }
    }
}
