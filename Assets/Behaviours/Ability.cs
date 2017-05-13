using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public AbilityProperties properties; // Properties of the ability.

    private float cooldown_timer = 0;


	void Start()
    {
		DerivedStart();
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

		DerivedUpdate();
	}


    public void Fire()
    {
        // If ability has already been fired, abort.
        if (cooldown_timer > 0)
            return;

        cooldown_timer = properties.cooldown;

        // Create particle effect.
        if (properties.activate_effect != null)
        {
            GameObject particle_effect = Instantiate(properties.activate_effect);
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

        DerivedFire();
    }


    // Returns true if the ability is off cooldown, otherwise returns false.
    public bool IsReady()
    {
        return cooldown_timer <= 0;
    }


    // Functions to be overriden by derived classes.
    protected virtual void DerivedStart() {}
    protected virtual void DerivedUpdate() {}
    protected virtual void DerivedFire() {}

}
