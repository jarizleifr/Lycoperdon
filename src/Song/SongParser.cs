using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpYaml.Serialization;

namespace Lycoperdon
{
    public class SongParser
    {
        public Song Song { get; private set; }

        private Dictionary<string, Func<List<ICommand>>> patterns;

        public SongParser(string path)
        {
            var serializer = new Serializer();
            var songData = serializer.Deserialize<SongData>(File.ReadAllText(path));

            patterns = songData.Patterns.Aggregate(new Dictionary<string, Func<List<ICommand>>>(), (acc, cur) =>
            {
                acc.Add(cur.Key, () => cur.Value.Select(commandString => Parse(commandString)).ToList());
                return acc;
            });

            var commands = songData.Song.Select(commandString => Parse(commandString)).ToList();
            Song = new Song(commands)
            {
                Tempo = songData.Tempo,
                Swing = songData.Swing,
                Humanize = songData.Humanize,
                Variance = songData.Variance,
                First = songData.First,
                Accent = songData.Accent,
            };
        }

        private ICommand Parse(string commandString)
        {
            var c = commandString.Split(' ');
            var command = c[0];

            var serializer = new Serializer();

            return command switch
            {
                "SET" => new SetCommand(commandString, Enum.Parse<SetCommandType>(c[1]), c[2]),
                "SILENCE" => new PlayCommand(commandString, null, int.Parse(c[1]), null),
                "PLAY" => (patterns != null && patterns.TryGetValue(c[2], out var pattern))
                    ? (ICommand)new BatchCommand(commandString, pattern.Invoke(), (cmd) => { })
                    : new PlayCommand(commandString, serializer, int.Parse(c[1]), c[2]),
                _ => throw new Exception("Unknown command")
            };
        }
    }
}