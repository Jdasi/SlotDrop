using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShockwave : Projectile
{

    protected override void Start()
    {
        RaycastHit[] sphere = Physics.SphereCastAll(transform.position, properties.effect_radius, Vector3.forward, 0);

        foreach (var elem in sphere)
        {
            Rigidbody collided_body = elem.collider.gameObject.GetComponent<Rigidbody>();

            if (collided_body == null)
                continue;

            // Don't apply force to self.
            if (collided_body == owning_player.GetComponent<Rigidbody>())
                continue;

            collided_body.AddExplosionForce(properties.knockback_force, transform.position, properties.effect_radius);
        }

        Destroy(gameObject);
    }

}
