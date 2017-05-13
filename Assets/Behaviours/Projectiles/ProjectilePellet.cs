using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePellet : Projectile
{

    protected override void Start()
    {

    }


	protected override void Update()
    {
		transform.position += direction * Time.deltaTime * properties.projectile_speed;
	}


    protected override void OnTriggerEnter(Collider other)
    {
        // Only collide with players.
        if (other.tag != "Player")
            return;

        PlayerController colliding_player = other.GetComponent<PlayerController>();

        // Don't collide with self.
        if (colliding_player.GetPlayerID() == owning_player.GetPlayerID())
            return;

        colliding_player.Damage(properties.damage);
        Destroy(gameObject);
    }

}
