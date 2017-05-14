using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCharge : Projectile
{
    private Transform torso_mount;
    private Rigidbody rigid_body;
    private bool force_applied = false;
    private Vector3 absolute_direction;
    private float threshold = 0.5f;


	protected override void Start()
    {
        if (owning_player != null)
        {
            torso_mount = owning_player.transform.FindChild("BodyParts").transform;
            rigid_body = owning_player.GetComponent<Rigidbody>();
        }

        TrackPlayer();
    }


    protected override void Update()
    {
        CalculateRawDirection();
        TrackPlayer();
    }


    protected override void FixedUpdate()
    {
        if (owning_player == null)
            return;

        rigid_body.MovePosition(owning_player.transform.position + 
            (absolute_direction * Time.deltaTime * properties.effect_radius));
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


    void CalculateRawDirection()
    {
        if (owning_player.horizontal == 0 || owning_player.vertical == 0)
            return;
        
        if (owning_player.horizontal > threshold)
        {
            absolute_direction = Vector3.right;
        }
        else if (owning_player.horizontal < -threshold)
        {
            absolute_direction = Vector3.left;
        }
        else if (owning_player.vertical > threshold)
        {
            absolute_direction = Vector3.forward;
        }
        else if (owning_player.vertical < -threshold)
        {
            absolute_direction = Vector3.back;
        }
    }

    
    void TrackPlayer()
    {
        if (torso_mount != null)
    	    transform.position = torso_mount.position;
    }

}
