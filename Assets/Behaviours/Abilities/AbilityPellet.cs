using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPellet : Ability
{

    protected override void DerivedStart()
    {

    }


    protected override void DerivedUpdate()
    {

    }


    protected override void DerivedFire()
    {
        PlayerController firing_player = GetComponent<PlayerController>();

        Vector3 origin = transform.FindChild("BodyParts").position;
        Vector3 facing = firing_player.GetLastDirection();

        GameObject projectile = Instantiate(properties.projectile, origin, Quaternion.identity);

        projectile.GetComponent<ProjectilePellet>().Init(
            firing_player.GetPlayerID(), origin, facing, properties.projectile_speed, properties.damage);
    }

}
