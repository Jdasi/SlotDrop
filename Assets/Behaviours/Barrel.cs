using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject shockwave_particle;
    public GameObject owner;
	

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddTorque(new Vector3(3, 0, 6));
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Barrel")
            return;

        if (other.gameObject == owner)
            return;

        GameObject.FindObjectOfType<AudioManager>().PlayOneShot("explosion");

        CameraShake.instance.ShakeCamera(0.4f, 0.4f);

        Projectile.CreateEffect(shockwave_particle, transform.position, Vector3.zero);
        RaycastHit[] elems = Projectile.CreateExplosionForce(gameObject, transform.position, 5, 0);

        foreach (var elem in elems)
        {
            PlayerController player = elem.collider.GetComponent<PlayerController>();

            if (player == null)
                continue;

            player.Stun(2.0f);
        }
    }


}
