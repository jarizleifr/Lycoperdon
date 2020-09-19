using System;
using System.Collections.Generic;
using System.Linq;

namespace Lycoperdon
{
    public class BatchCommand : ICommand
    {
        public string Command { get; private set; }
        public bool Immediate => commands.All(c => c.Immediate);

        private List<ICommand> commands;
        private Action<ICommand> onChange;
        private int current = -1;

        public BatchCommand(string command, List<ICommand> commands, Action<ICommand> onChange) =>
            (Command, this.commands, this.onChange) = (command, commands, onChange);

        public bool Do(Buffer buffer, Song song, bool elapsed)
        {
            if (current == -1)
            {
                current = 0;
                onChange(commands[current]);
            }

            while (current < commands.Count && commands[current].Immediate)
            {
                commands[current].Do(buffer, song, elapsed);
                current++;
                onChange(commands[current]);
            }

            if (current < commands.Count && elapsed)
            {
                if (!commands[current].Do(buffer, song, elapsed))
                {
                    current++;
                    if (current < commands.Count)
                    {
                        onChange(commands[current]);
                    }
                }
            }
            return current < commands.Count;
        }
    }
}