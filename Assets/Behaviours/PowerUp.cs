using System;
using UnityEngine;
using System.Collections;
using Assets.Classes;
using Rewired;
using Random = UnityEngine.Random;

public class PowerUp : MonoBehaviour
{
    WeaponType RandomizePowerup()
    {
        int type = Random.Range(1, 2);
        return (WeaponType) type;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag != "Player") return;

        this.gameObject.SetActive(false);
        int weaponsCount = coll.gameObject.GetComponents<PlayerWeapon>().Length;
        WeaponType oldWeaponType = WeaponType.WEP_INVALID;
        int newWeapons = 0;
        if (coll.gameObject.GetComponent<PlayerController>().BaseWeapons)
        {
            while (newWeapons < 1) // should be 2, but we have only shockwave for now
            {
                var newWeaponType = WeaponType.WEP_INVALID;
                do
                {
                    newWeaponType = RandomizePowerup();
                } while (newWeaponType == oldWeaponType); // randomize weapon if it is the same as prev one

                //destroy base weapons
                PlayerWeapon[] weapons = coll.gameObject.GetComponents<PlayerWeapon>();
                foreach (var i in weapons)
                {
                    Destroy(i);
                }
                switch (newWeaponType)
                {
                    case WeaponType.WEP_INVALID:
                        Debug.Log("UNABLE TO CREATE WEAPON, CHECK RANDOM RANGES IN RANDOMIZEPOWERUP");
                        break;
                    case WeaponType.WEP_CONE_KNOCKBACK:
                        break;
                    case WeaponType.WEP_SHOCKWAVE_RADIAL:
                        coll.gameObject.AddComponent<WeaponSpecialShockwave>();
                        break;
                    case WeaponType.WEP_BASE_PROJECTILE:
                        coll.gameObject.AddComponent<WeaponBaseProjectile>();
                        break;
                    case WeaponType.WEP_SLOWING_BOMB:
                        break;
                    case WeaponType.WEP_STUN_NEAREST_PLAYER:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                coll.gameObject.GetComponent<PlayerController>().BaseWeapons = false;
                newWeapons++;
            }
        }
        Destroy(this);
    }
}