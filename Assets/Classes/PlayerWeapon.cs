using UnityEngine;

namespace Assets.Classes
{
    public class PlayerWeapon : MonoBehaviour
    {
        protected WeaponSettings WeaponSettings;

        protected virtual void Attack()
        {
        }

        public virtual void Update()
        {
            Attack(); //to be called from player 
            if (WeaponSettings.Ready) return;
            if (WeaponSettings.TimeLeft >= WeaponSettings.Cooldown)
            {
                WeaponSettings.TimeLeft = 0.0f;
                WeaponSettings.Ready = true;
            }
            WeaponSettings.TimeLeft += Time.deltaTime;
        }
    }
}