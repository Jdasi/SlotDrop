using Rewired;

namespace Assets.Classes
{
    public struct WeaponSettings
    {
        public Player player_input; //to be deleted
        public WeaponType Type;

        public bool Active;
        public bool Ready;

        public float SlowDuration;
        public float SlowModifier;

        public float StunDuration; // may be same as slow - speed modifier -100% for slowduration

        public float KnockbackForce;

        public float WeaponEffectRadius;

        public float Cooldown;
        public float TimeLeft;
    }
}