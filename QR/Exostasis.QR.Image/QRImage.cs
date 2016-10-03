using System.Collections;
using System.Collections.Generic;
using Exostasis.QR.Common.Image;
using System.Drawing;
using System.Linq;
using Exostasis.QR.Common;

namespace Exostasis.QR.Image
{
    public class QrImage
    {
        private int Version { get; }
        private int Scale { get; }
        private List<Element> Elements { get; }

        private FinderPattern TopLeftFinderPattern { get; set; }
        private FinderPattern TopRightFinderPattern { get; set; }
        private FinderPattern BottomLeftFinderPattern { get; set; }

        public QrImage(int version, int scale)
        {
            Version = version;
            Scale = scale;
            Elements = new List<Element>();
            AddFinderPatterns();
            AddSeperators();
            AddAlignmentPatterns();
            AddTimingPatterns();
        }

        private void AddAlignmentPatterns()
        {
            List<Cord> possibleCords = CalculateAlignmentPatternCords();
            List<AlignmentPattern> possibAlignmentPatterns = new List<AlignmentPattern>();

            possibleCords.ForEach(cord => possibAlignmentPatterns.Add(new AlignmentPattern(cord)));

            possibAlignmentPatterns.Where(alignmentPattern => Elements.Any(alignmentPattern.IsWithinSpace));

            possibAlignmentPatterns.ForEach(alignmentPattern => Elements.Add(alignmentPattern));
        }

        private void AddFinderPatterns()
        {
            TopLeftFinderPattern = new FinderPattern(new Cord(0, 0));
            TopRightFinderPattern = new FinderPattern(new Cord(GetModuleSize() - FinderPattern.ModulesWidth, 0));
            BottomLeftFinderPattern = new FinderPattern(new Cord(0, GetModuleSize() - FinderPattern.ModulesHeight));
            Elements.Add(TopLeftFinderPattern);
            Elements.Add(TopRightFinderPattern);
            Elements.Add(BottomLeftFinderPattern);
        }

        private void AddSeperators()
        {
            Elements.Add(new Seperator(new Cord(TopLeftFinderPattern.TopRightCord.X + 1, TopLeftFinderPattern.TopRightCord.Y), 1,
                FinderPattern.ModulesHeight + 1));
            Elements.Add(new Seperator(new Cord(TopLeftFinderPattern.BottomLeftCord.X, TopLeftFinderPattern.BottomLeftCord.Y + 1),
                FinderPattern.ModulesWidth, 1));

            Elements.Add(new Seperator(new Cord(TopRightFinderPattern.TopLeftCord.X - 1, TopRightFinderPattern.TopLeftCord.Y), 1,
                FinderPattern.ModulesHeight + 1));
            Elements.Add(new Seperator(new Cord(TopRightFinderPattern.BottomLeftCord.X, TopRightFinderPattern.BottomLeftCord.Y + 1),
                FinderPattern.ModulesWidth, 1));

            Elements.Add(new Seperator(new Cord(BottomLeftFinderPattern.TopLeftCord.X, BottomLeftFinderPattern.TopLeftCord.Y - 1),
                FinderPattern.ModulesWidth, 1));
            Elements.Add(new Seperator(new Cord(BottomLeftFinderPattern.TopRightCord.X + 1, BottomLeftFinderPattern.TopRightCord.Y - 1), 1,
                FinderPattern.ModulesHeight + 1));
        }

        private void AddTimingPatterns()
        {
            Cord topTimingPatterTopLeftCord = new Cord(TopLeftFinderPattern.BottomRightCord.X + 2, TopLeftFinderPattern.BottomRightCord.Y);           
        }

        private int GetModuleSize()
        {
            return Version * 4 + 21;
        }

        private List<Cord> CalculateAlignmentPatternCords()
        {
            List<Cord> cords = new List<Cord>();
            int[] possibleCords = Constants.AlignmentCords[Version]?.Values;

            if (possibleCords != null)
            {
                for (int i = 0; i < possibleCords.Length; ++i)
                {
                    for (int j = i; j < possibleCords.Length; ++j)
                    {
                        cords.Add(new Cord(possibleCords[i], possibleCords[j]));
                        if (possibleCords[i] != possibleCords[j])
                        {
                            cords.Add(new Cord(possibleCords[j], possibleCords[i]));
                        }
                    }
                }
            }

            return cords;
        }

        public void WriteImage(string filename)
        {
            using (var qrBitmap = new Bitmap(GetModuleSize() * Scale, GetModuleSize() * Scale))
            {

                qrBitmap.Save(filename);
            }
        }

        public void WriteBitArray(List<BitArray> structuredArray)
        {
            throw new System.NotImplementedException();
        }
    }
}