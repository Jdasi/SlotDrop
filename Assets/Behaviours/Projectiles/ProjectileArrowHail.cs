using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileArrowHail : Projectile
{
    public GameObject particle_effect;
    public float damage_delay = 0.5f;

    private List<Collider> colliding_objects = new List<Collider>();
    private float timer;
    private bool can_damage;


    protected override void Start()
    {
        CreateEffect(particle_effect, origin, Vector3.zero);
        Invoke("EnableDamage", 2.0f);
    }


    protected override void Update()
    {
        colliding_objects.RemoveAll(null);

        timer += Time.deltaTime;

        if (timer >= damage_delay)
        {
            timer = 0;

            if (!can_damage)
                return;

            foreach (Collider obj in colliding_objects)
            {
                obj.GetComponent<PlayerController>().Damage(properties.damage);
            }
        }
    }


    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        if (other.GetComponent<Rigidbody>() == owning_player.GetComponent<Rigidbody>())
            return;

        if (!colliding_objects.Contains(other))
            colliding_objects.Add(other);
    }


    protected override void OnTriggerLeave(Collider other)
    {
        if (other.tag != "Player")
            return;

        if (colliding_objects.Contains(other))
            colliding_objects.Remove(other);
    }


    void EnableDamage()
    {
        can_damage = true;
    }


}
