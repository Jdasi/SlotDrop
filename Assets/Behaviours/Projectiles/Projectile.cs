using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected PlayerController owning_player;
    protected AbilityProperties properties;
    protected Vector3 origin;
    protected Vector3 direction;

    public void Init(PlayerController owning_player, AbilityProperties properties, Vector3 origin, Vector3 direction)
    {
        this.owning_player = owning_player;
        this.properties = properties;
        this.origin = origin;
        this.direction = direction;

        Destroy(gameObject, properties.lifetime);
    }

	protected virtual void Start() {}
	protected virtual void Update() {}

    protected virtual void OnCollisionEnter(Collision other) {}
    protected virtual void OnTriggerEnter(Collider other) {}

}
