using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShockwave : Projectile
{

    protected override void Start()
    {
        CreateExplosionForce(owning_player.gameObject, origin, 
            properties.effect_radius, properties.knockback_force);

        Destroy(gameObject);
    }

}
