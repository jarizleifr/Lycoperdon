using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpYaml.Serialization;

namespace Lycoperdon
{
    public class PlayCommand : ICommand
    {
        public string Command { get; private set; }
        public bool Immediate => false;

        private Beat pattern;

        public PlayCommand(string command, Serializer serializer, int patternLength, string name)
        {
            Command = command;

            if (name == null)
            {
                pattern = new Beat(new List<(Drum, Track)>(), null, patternLength);
                return;
            }
            else
            {
                var beat = serializer.Deserialize<BeatData>(File.ReadAllText($"./beats/{name}.yaml"));
                var beatLength = beat.Beat.Aggregate(0, (acc, cur) => cur.Value.Length > acc
                    ? cur.Value.Length
                    : acc
                );

                var tracks = beat.Beat
                    .Select(b => (b.Key, CreateTrack(b.Value, patternLength, beatLength)))
                    .ToList();

                var accent = CreateTrack(beat.Accent, patternLength, beatLength);

                pattern = new Beat(tracks, accent, patternLength);
            }
        }

        public bool Do(Buffer buffer, Song song, bool elapsed) =>
            pattern.Play(buffer, song);

        private Track CreateTrack(string track, int patternLength, int beatLength)
        {
            if (track == "" || track is null)
            {
                return new Track(new Dictionary<int, int>());
            }

            var notes = new Dictionary<int, int>();
            for (int i = 0; i < patternLength; i++)
            {
                var remainder = i % beatLength;
                if (remainder < track.Length)
                {
                    var velocity = ParseVelocity(track[remainder]);
                    notes.Add(i, velocity);
                }
            }
            return new Track(notes);
        }

        private static int ParseVelocity(char c) => c switch
        {
            '.' => 80,
            '+' => 90,
            'x' => 100,
            'X' => 127,
            _ => 0
        };
    }
}