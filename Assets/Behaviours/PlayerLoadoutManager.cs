using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Classes;

public class PlayerLoadoutManager : MonoBehaviour
{
    public List<PlayerLoadout> PlayerLoadouts = new List<PlayerLoadout>();


    void Start()
    {
        //loadout 1
        PlayerLoadout temPlayerLoadout = new PlayerLoadout
        {
            LoadoutTypes = LoadoutType.LO_NOOB,
            BasePlayerWeapon = WeaponType.WEP_BASE_PROJECTILE,
            SpecialPlayerWeapon = WeaponType.WEP_SHOCKWAVE_RADIAL
        };
        PlayerLoadouts.Add(temPlayerLoadout);

        //loadout 2
        temPlayerLoadout = new PlayerLoadout
        {
            LoadoutTypes = LoadoutType.LO_PHISHERMAN,
            BasePlayerWeapon = WeaponType.WEP_BASE_PROJECTILE,
            SpecialPlayerWeapon = WeaponType.WEP_SHOCKWAVE_RADIAL
        };
        PlayerLoadouts.Add(temPlayerLoadout);
    }

    public PlayerLoadout GetUniqueGameObject(int id)
    {
        return PlayerLoadouts[id];
    }
}