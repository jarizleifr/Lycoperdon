using System.Collections.Generic;
using Commons.Music.Midi;

namespace Lycoperdon
{
    public class Buffer
    {
        public bool Empty => buffer.Count == 0;

        private List<Note> buffer = new List<Note>();

        public void Tick(IMidiOutput midiOut, float delta)
        {
            if (buffer.Count == 0) return;
            for (int i = buffer.Count - 1; i >= 0; i--)
            {
                var note = buffer[i];
                note.delay -= delta;
                if (note.delay <= 0)
                {
                    var drum = TryGetAlternateDrum(note.drum);
                    midiOut.Send(new byte[] { 0x99, (byte)drum, (byte)note.velocity }, 0, 3, 0);
                    buffer.RemoveAt(i);
                }
            }
        }

        public void Enqueue(Note note) => buffer.Add(note);

        private Drum TryGetAlternateDrum(Drum drum) => drum switch
        {
            Drum.ClumsySnare => RNG.Int(0, 100) < 95 ? Drum.Snare : Drum.Rimshot,
            Drum.RandomCymbal => RNG.PickRandom(Drum.Crash1, Drum.Crash2, Drum.China, Drum.Splash),
            Drum.RandomHighTom => RNG.PickRandom(Drum.HighTom, Drum.HighMidTom),
            Drum.RandomMidTom => RNG.PickRandom(Drum.LowMidTom, Drum.LowTom),
            Drum.RandomLowTom => RNG.PickRandom(Drum.HighFloorTom, Drum.LowFloorTom),
            _ => drum
        };
    }
}