using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShockwave : Ability
{

    public override void DerivedStart()
    {

    }


    public override void DerivedUpdate()
    {

    }


    public override void DerivedFire()
    {
        RaycastHit[] sphere = Physics.SphereCastAll(transform.position, properties.effect_radius,
        Vector3.forward, 0);

        foreach (var elem in sphere)
        {
            Rigidbody rigidBody = elem.collider.gameObject.GetComponent<Rigidbody>();

            // Don't apply force to self.
            if (rigidBody == this.GetComponent<Rigidbody>())
                continue;

            if (rigidBody != null)
                rigidBody.AddExplosionForce(properties.knockback_force, transform.position, properties.effect_radius);
        }
    }

}
