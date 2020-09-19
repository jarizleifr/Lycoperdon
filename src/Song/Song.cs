using System;
using System.Collections.Generic;

namespace Lycoperdon
{
    public class Song
    {
        public int Tempo { get; set; }
        public int Swing { get; set; }
        public int Humanize { get; set; }
        public int Variance { get; set; }

        public Drum First { get; set; }
        public Drum Accent { get; set; }

        public bool Finished { get; private set; }

        private double accumulator = 0;
        private BatchCommand commands;

        public Song(List<ICommand> commands) =>
            this.commands = new BatchCommand("", commands, (cmd) => Console.WriteLine(cmd.Command));

        public void Tick(Buffer buffer, float delta)
        {
            double t = 60000d / Tempo;
            var ms = t / 4;

            accumulator += delta;

            bool elapsed = accumulator >= ms;
            if (!commands.Do(buffer, this, elapsed))
            {
                Finished = true;
            }

            if (elapsed)
            {
                accumulator -= ms;
            }
        }
    }
}