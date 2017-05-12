using UnityEngine;
using System.Collections;
using Assets.Classes;

public class WeaponSpeedBoost : PlayerWeapon
{

    // Use this for initialization
    void Start()
    {
        weaponSettings.duration = 5.0f;
        weaponSettings.type = PowerUpType.PU_SPEED_BOOST;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
