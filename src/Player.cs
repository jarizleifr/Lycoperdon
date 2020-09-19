using System;
using System.Linq;
using Commons.Music.Midi;

namespace Lycoperdon
{
    public class Player
    {
        private IMidiOutput midiOut;
        private Buffer buffer;
        private Song song;
        private DateTime lastTime;
        private float delta;

        public bool Running { get; private set; }

        public Player(int midiDevice, Song song)
        {
            var access = MidiAccessManager.Default;
            midiOut = access.OpenOutputAsync(access.Outputs.ToList()[midiDevice].Id).Result;

            buffer = new Buffer();
            this.song = song;
        }

        public void Play()
        {
            Running = true;
            lastTime = DateTime.Now;

            while (Running)
            {
                var now = DateTime.Now;
                delta = (now.Ticks - lastTime.Ticks) / 10000f;
                lastTime = now;

                if (!song.Finished)
                {
                    song.Tick(buffer, delta);
                }
                buffer.Tick(midiOut, delta);

                if (song.Finished && buffer.Empty)
                {
                    Running = false;
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("Usage: lycoperdon [song name] [optional midi device #]");
                return;
            }

            var song = args[0];
            var midiDevice = args.Length == 2 ? int.Parse(args[1]) : 0;

            var player = new Player(midiDevice, new SongParser($"./songs/{song}.yaml").Song);
            player.Play();
            Console.WriteLine("Playback finished.");
        }
    }
}