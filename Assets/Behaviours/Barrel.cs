using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public GameObject shockwave_particle;
    public PlayerController owning_player;
    public Rigidbody rigid_body;


    void FixedUpdate()
    {
        rigid_body.AddTorque(new Vector3(15, 0, 12));
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Barrel")
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

            if (owning_player != null)
            {
                if (player == owning_player)
                    continue;
            }

            player.Stun(2.0f);
        }

        Destroy(gameObject);
    }


    public void AddForce(Vector3 force)
    {
        rigid_body.AddForce(force);
    }


}
