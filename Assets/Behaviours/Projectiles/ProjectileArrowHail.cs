using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileArrowHail : Projectile
{
    public GameObject particle_effect;
    public float damage_delay = 0.5f;
    public float stun_chance = 0;

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
        timer += Time.deltaTime;

        if (timer >= damage_delay)
        {
            timer = 0;

            if (!can_damage)
                return;

            DamageAllInRadius();
        }
    }


    void EnableDamage()
    {
        can_damage = true;
    }


    void DamageAllInRadius()
    {
        RaycastHit[] sphere = Physics.SphereCastAll(transform.position, properties.effect_radius, Vector3.forward, 0);

        foreach (var elem in sphere)
        {
            if (elem.collider.tag != "Player")
                continue;

            PlayerController player = elem.collider.gameObject.GetComponent<PlayerController>();

            if (player == null ||
                player == owning_player)
                continue;

            if (Random.Range(1, 100) < stun_chance)
                player.Stun(properties.stun_duration);

            player.Damage(properties.damage);
        }
    }


}
