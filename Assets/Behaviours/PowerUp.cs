using System;
using UnityEngine;
using System.Collections;
using Assets.Classes;
using Rewired;
using Random = UnityEngine.Random;

public class PowerUp : MonoBehaviour
{
    LoadoutType RandomizeLoadout()
    {
        int type = Random.Range(0, Enum.GetNames(typeof(LoadoutType)).Length);
        return (LoadoutType) type;
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
        if (coll.gameObject.GetComponent<PlayerController>().BaseWeapons)
        {
            LoadoutType tempLoadoutType = RandomizeLoadout();
            GameObject temPlayerLoadoutManager = GameObject.FindGameObjectWithTag("Managers");
            PlayerLoadoutManager loadoutManager = temPlayerLoadoutManager.GetComponent<PlayerLoadoutManager>();
            PlayerLoadout newlLoadout = loadoutManager.PlayerLoadouts[(int) tempLoadoutType];

            //destroy base weapons
            PlayerWeapon[] weapons = coll.gameObject.GetComponents<PlayerWeapon>();
            foreach (var i in weapons)
            {
                Destroy(i);
            }

            switch (newlLoadout.BasePlayerWeapon)
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

            switch (newlLoadout.SpecialPlayerWeapon)
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
        }
        Destroy(this);
    }
}