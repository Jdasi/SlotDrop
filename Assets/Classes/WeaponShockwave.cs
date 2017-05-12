using UnityEngine;
using System.Collections;
using Rewired;

public class WeaponShockwave : PlayerWeapon
{
    public float CameraShockwaveShakeStrength = 0.4f;
    public GameObject shockwave_prefab;

    // Use this for initialization
    void Start()
    {
        weaponSettings.player_input =
            ReInput.players.GetPlayer(this.GetComponentInParent<PlayerController>()
                .id); // SHOULD BE EXTRACTED TO BASE BEHAVIOUR
        weaponSettings.duration = 0.4f;
        weaponSettings.active = false;
        weaponSettings.radius = 20.0f;
        weaponSettings.force = 20.0f;
        weaponSettings.force *= 1000;
        Debug.Log("HO");
    }

    // Update is called once per frame
    void Update()
    {
        HandleShockwave();
    }

    void FixedUpdate()
    {
        Shockwave();
    }

    void HandleShockwave()
    {
        if (weaponSettings.player_input.GetButtonDown("SlotDrop") && !weaponSettings.active)
        {
            CameraShake.instance.ShakeCamera(weaponSettings.duration, CameraShockwaveShakeStrength);
            weaponSettings.active = true;

            GameObject shock_particle = Instantiate(shockwave_prefab);
            shock_particle.transform.position = transform.position;
            Destroy(shock_particle, shock_particle.GetComponent<ParticleSystem>().main.duration);
        }
    }


    void Shockwave()
    {
        if (!weaponSettings.active)
            return;

        RaycastHit[] sphere = Physics.SphereCastAll(transform.position, weaponSettings.radius, Vector3.forward, 0);

        foreach (var elem in sphere)
        {
            Rigidbody rigid_body = elem.collider.gameObject.GetComponent<Rigidbody>();

            if (rigid_body == this.GetComponent<Rigidbody>())
                continue;

            if (rigid_body != null)
                rigid_body.AddExplosionForce(weaponSettings.force, transform.position, weaponSettings.radius);
        }

        weaponSettings.active = false;
    }
}