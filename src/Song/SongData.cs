using System.Collections.Generic;

namespace Lycoperdon
{
    public class SongData
    {
        public int Tempo { get; set; } = 120;
        public int Swing { get; set; } = 0;
        public int Humanize { get; set; } = 0;
        public int Variance { get; set; } = 0;
        public Drum First { get; set; } = Drum.None;
        public Drum Accent { get; set; } = Drum.None;
        public Dictionary<string, List<string>> Patterns { get; set; } =
            new Dictionary<string, List<string>>();

        public List<string> Song { get; set; }
    }
}