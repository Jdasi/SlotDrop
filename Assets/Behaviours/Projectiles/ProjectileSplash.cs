using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSplash : Projectile
{
    public GameObject particle_effect;

	protected override void Start()
    {
        if (owning_player != null)
            origin = owning_player.transform.FindChild("BodyParts").position;

        Vector3 offset_pos = origin + (facing * 3);

        CreateEffect(particle_effect, offset_pos, offset_pos + facing);

        transform.position = offset_pos + (facing * 2);
    }


    protected override void OnTriggerEnter(Collider other)
    {
        // Only collide with players.
        if (other.tag != "Player")
            return;

        PlayerController colliding_player = other.GetComponent<PlayerController>();

        // Don't collide with self.
        if (owning_player != null)
        {
            if (colliding_player.GetPlayerID() == owning_player.GetPlayerID())
                return;
        }

        colliding_player.Damage(properties.damage);
    }

}
