using Rewired;

namespace Assets.Classes
{
    public struct PowerUpSettings
    {
        public Player player_input;
        public float duration;
        public PowerUpType type;
        public bool active;
        public float force;
        public float radius;
    }
}