using System.Collections.Generic;

namespace Lycoperdon
{
    public class Track
    {
        private Dictionary<int, int> velocities;

        public int GetVelocity(int position) => velocities.TryGetValue(position, out var velocity)
            ? velocity
            : 0;

        public Track(Dictionary<int, int> velocities) => this.velocities = velocities;
    }
}