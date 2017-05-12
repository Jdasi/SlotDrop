using System;
using Assets.Classes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Behaviours
{
    public class PowerUp : MonoBehaviour
    {
        public void NewLoadout(GameObject playerGameObject)
        {
            //randomize loadout
            LoadoutType tempLoadoutType = RandomizeLoadout();

            //load manager
            GameObject temPlayerLoadoutManager = GameObject.FindGameObjectWithTag("Managers");
            PlayerLoadoutManager loadoutManager = temPlayerLoadoutManager.GetComponent<PlayerLoadoutManager>();

            //get loadout
            PlayerLoadout newlLoadout = loadoutManager.PlayerLoadouts[(int) tempLoadoutType];

            //destroy base weapons
            PlayerWeapon[] weapons = playerGameObject.gameObject.GetComponents<PlayerWeapon>();
            foreach (var i in weapons)
            {
                Destroy(i);
            }

            //add new base waepon
            switch (newlLoadout.BasePlayerWeapon)
            {
                case WeaponType.WEP_BASE_PROJECTILE:
                    playerGameObject.gameObject.AddComponent<WeaponBaseProjectile>();
                    break;
                default:
                    Debug.Log("UNABLE TO CREATE WEAPON, CHECK RANDOM RANGES IN RANDOMIZEPOWERUP");
                    throw new ArgumentOutOfRangeException();
            }

            //add new special weapon
            switch (newlLoadout.SpecialPlayerWeapon)
            {
                case WeaponType.WEP_CONE_KNOCKBACK:
                    break;
                case WeaponType.WEP_SHOCKWAVE_RADIAL:
                    playerGameObject.gameObject.AddComponent<WeaponSpecialShockwave>();
                    break;
                case WeaponType.WEP_SLOWING_BOMB:
                    break;
                case WeaponType.WEP_STUN_NEAREST_PLAYER:
                    break;
                default:
                    Debug.Log("UNABLE TO CREATE WEAPON, CHECK RANDOM RANGES IN RANDOMIZEPOWERUP");
                    throw new ArgumentOutOfRangeException();
            }
        }

        LoadoutType RandomizeLoadout()
        {
            int type = Random.Range(0, Enum.GetNames(typeof(LoadoutType)).Length);
            return (LoadoutType) type;
        }
    }
}