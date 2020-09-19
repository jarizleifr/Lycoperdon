namespace Lycoperdon
{
    public class Note
    {
        public readonly Drum drum;
        public readonly int velocity;
        public float delay;

        public Note(Drum drum, int velocity, int delay) =>
            (this.drum, this.velocity, this.delay) = (drum, velocity, delay);

        public readonly static Note None = new Note(Drum.None, 0, 0);
    }
}