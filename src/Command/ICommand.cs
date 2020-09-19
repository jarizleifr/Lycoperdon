namespace Lycoperdon
{
    public interface ICommand
    {
        string Command { get; }
        bool Immediate { get; }
        bool Do(Buffer buffer, Song song, bool elapsed);
    }
}