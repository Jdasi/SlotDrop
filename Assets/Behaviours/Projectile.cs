﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected PlayerController owning_player;
    protected AbilityProperties properties;
    protected Vector3 origin;
    protected Vector3 facing;

    public void Init(PlayerController owning_player, AbilityProperties properties, Vector3 origin, Vector3 facing)
    {
        this.owning_player = owning_player;
        this.properties = properties;
        this.origin = origin;
        this.facing = facing;

        Destroy(gameObject, properties.lifetime);
    }


    // Instantiates a particle prefab and attaches a garbage collection script to it.
    public static void CreateEffect(GameObject particle_effect, Vector3 position, Vector3 direction)
    {
        GameObject particle = Instantiate(particle_effect);

        particle.transform.position = position;

        if (direction != Vector3.zero)
            particle.transform.LookAt(direction);

        particle.AddComponent<TempParticle>();
    }


    // Applies an explosion force to all objects in the radius. Does not affect creator by default.
    public static RaycastHit[] CreateExplosionForce(GameObject creator, Vector3 position, 
        float radius, float force, bool affect_creator = false)
    {
        RaycastHit[] sphere = Physics.SphereCastAll(position, radius, Vector3.forward, 0);

        foreach (var elem in sphere)
        {
            Rigidbody collided_body = elem.collider.gameObject.GetComponent<Rigidbody>();

            if (collided_body == null)
                continue;

            // Don't affect creator.
            if (creator != null && !affect_creator)
            {
                if (collided_body == creator.GetComponent<Rigidbody>())
                    continue;
            }

            collided_body.AddExplosionForce(force, position, radius);
        }

        return sphere;
    }


	protected virtual void Start() {}
	protected virtual void Update() {}
    protected virtual void FixedUpdate() {}

    protected virtual void OnCollisionEnter(Collision other) {}
    protected virtual void OnCollisionLeave(Collision other) {}

    protected virtual void OnTriggerEnter(Collider other) {}
    protected virtual void OnTriggerLeave(Collider other) {}

}
