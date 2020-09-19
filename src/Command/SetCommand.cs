using System;

namespace Lycoperdon
{
    public class SetCommand : ICommand
    {
        public string Command { get; private set; }
        public bool Immediate => true;

        private SetCommandType type;
        private string value;

        public SetCommand(string command, SetCommandType type, string value) =>
            (Command, this.type, this.value) = (command, type, value);

        public bool Do(Buffer buffer, Song song, bool elapsed)
        {
            switch (type)
            {
                case SetCommandType.TEMPO:
                    song.Tempo = int.Parse(value);
                    break;
                case SetCommandType.SWING:
                    song.Swing = int.Parse(value);
                    break;
                case SetCommandType.HUMANIZE:
                    song.Humanize = int.Parse(value);
                    break;
                case SetCommandType.VARIANCE:
                    song.Variance = int.Parse(value);
                    break;
                case SetCommandType.FIRST:
                    song.First = Enum.Parse<Drum>(value);
                    break;
                case SetCommandType.ACCENT:
                    song.Accent = Enum.Parse<Drum>(value);
                    break;
            }
            return true;
        }
    }
}