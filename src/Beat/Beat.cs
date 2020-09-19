using System.Collections.Generic;

namespace Lycoperdon
{
    public class Beat
    {
        public int Length { get; private set; }

        private Track accent;
        private List<(Drum, Track)> tracks;
        private int current = 0;

        public Beat(List<(Drum, Track)> tracks, Track accent, int length) =>
            (this.tracks, this.accent, Length) = (tracks, accent, length);

        public bool Play(Buffer buffer, Song song)
        {
            foreach (var track in tracks)
            {
                var velocity = track.Item2.GetVelocity(current);
                var drum = track.Item1;
                if (velocity > 0)
                {
                    buffer.Enqueue(new Note(drum, velocity, RNG.Int(0, song.Humanize)));
                }
            }

            var playFirst = current == 0 && song.First != Drum.None;
            if (playFirst)
            {
                buffer.Enqueue(new Note(song.First, 127, RNG.Int(0, song.Humanize)));
            }

            if (!playFirst && accent != null && song.Accent != Drum.None)
            {
                var velocity = accent.GetVelocity(current);
                if (song.Accent != Drum.None && velocity > 0)
                {
                    buffer.Enqueue(new Note(song.Accent, velocity, RNG.Int(0, song.Humanize)));
                }
            }
            return (++current < Length);
        }
    }
}