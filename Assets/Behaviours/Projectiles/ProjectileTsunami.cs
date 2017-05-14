using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTsunami : Projectile
{
    public GameObject particle_effect;
    public float move_delay;
    public float move_spacing;
    public int max_moves;

    private float timer;
    private float move_times;

	protected override void Start()
    {
        origin = owning_player != null ? 
            owning_player.transform.FindChild("BodyParts").position + (facing * move_spacing) : transform.position;

        CreateEffect(particle_effect, origin, Vector3.zero);
        ++move_times;
        
        if (owning_player != null)
            transform.position = origin;
    }


    protected override void Update()
    {
        if (move_times < max_moves)
        {
            timer += Time.deltaTime;

            if (timer >= move_delay)
            {
                timer = 0;
                ++move_times;

                transform.position += facing * move_spacing;
                CreateBlast();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void CreateBlast()
    {
        CreateEffect(particle_effect, transform.position, Vector3.zero);

        CreateExplosionForce(owning_player != null ? owning_player.gameObject : null, 
            transform.position, properties.effect_radius, properties.knockback_force);

        CameraShake.instance.ShakeCamera(properties.camera_shake_strength, properties.camera_shake_duration);
    }

}
