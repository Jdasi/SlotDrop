using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public AbilityProperties properties; // Properties of the ability.

    private float cooldown_timer = 0;


    void Start()
    {
        // Cool stuff here ..
    }


    void Update()
    {
        if (cooldown_timer > 0)
        {
            cooldown_timer -= Time.deltaTime;
        }
        else
        {
            cooldown_timer = 0;
        }
	}


    public void Fire()
    {
        // If ability has already been fired, abort.
        if (cooldown_timer > 0)
            return;

        cooldown_timer = properties.cooldown;

        // Create particle effect.
        if (properties.particle != null)
        {
            GameObject particle_effect = Instantiate(properties.particle);
            particle_effect.transform.position = transform.position;
            Destroy(particle_effect, particle_effect.GetComponent<ParticleSystem>().main.duration);
        }

        // Play audio clip.
        if (properties.audio_clip != null)
        {
            // JTODO -- need to get an audio source from somewhere..
        }

        // Shake camera.
        if (properties.camera_shake_strength > 0 || properties.camera_shake_duration > 0)
            CameraShake.instance.ShakeCamera(properties.camera_shake_duration, properties.camera_shake_strength);

        CreateProjectile();
    }


    void CreateProjectile()
    {
        PlayerController firing_player = GetComponent<PlayerController>();

        Vector3 facing = firing_player.GetLastDirection();
        Vector3 origin = transform.FindChild("BodyParts").position + (facing * 2);

        GameObject projectile = Instantiate(properties.projectile, origin, Quaternion.identity);
        Debug.Log(projectile);

        projectile.GetComponent<Projectile>().Init(firing_player, properties, origin, facing);
    }


    // Returns true if the ability is off cooldown, otherwise returns false.
    public bool IsReady()
    {
        return cooldown_timer <= 0;
    }

}
