using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePellet : MonoBehaviour
{
    private PlayerController owning_player;
    private Vector3 direction;
    private float move_speed;
    private int damage;
    private float lifetime = 5.0f;


	void Update()
    {
		transform.position += direction * Time.deltaTime * move_speed;
	}


    private void OnTriggerEnter(Collider other)
    {
        // Only collide with players.
        if (other.tag != "Player")
            return;

        PlayerController colliding_player = other.GetComponent<PlayerController>();

        // Don't collide with self.
        if (colliding_player.GetPlayerID() == owning_player.GetPlayerID())
            return;

        colliding_player.Damage(damage);
        Destroy(gameObject);
    }


    public void Init(PlayerController firing_player, Vector3 start_pos, Vector3 direction, float move_speed, int damage)
    {
        this.owning_player = firing_player;
        transform.position = start_pos;
        this.direction = direction;
        this.move_speed = move_speed;
        this.damage = damage;

        Destroy(gameObject, lifetime);
    }

}
