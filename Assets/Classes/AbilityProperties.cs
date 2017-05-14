using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityProperties
{
    public AudioClip audio_clip;                // Sound that plays on activation.

    public float cooldown = 0;                  // Ability recharge time.
    public int damage = 0;                      // Ability damage dealt to players.

    public float effect_radius = 0;             // Ability AoE radius.
    public float knockback_force = 0;           // Ability knockback against targets within AoE.
    
    public GameObject projectile;               // Object that is launched.
    public float projectile_speed = 0;          // Speed of the projectile.
    public float lifetime = 5;                  // Lifetime of the projectile.

    public float camera_shake_strength = 0;     // Strength of the camera shake on activation.
    public float camera_shake_duration = 0;     // Duration of the camera shake on activation.
}
