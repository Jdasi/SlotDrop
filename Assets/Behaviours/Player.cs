using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id = 0;
    public float move_speed = 20;
    public float shockwave_force = 20.0f;
    public float shockwave_radius = 20.0f;

    public List<GameObject> mount_points = new List<GameObject>();

    public Animator player_animator;
    private bool flipped = false;
    private bool firing_shockwave = false;

    void Start()
    {
        shockwave_force *= 1000;
    }


    void Update()
    {
        HandleMovement();
        HandleShockwave();
    }


    void FixedUpdate()
    {
        if (firing_shockwave)
        {
            Shockwave();
        }
    }


    void HandleShockwave()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !firing_shockwave)
        {
            firing_shockwave = true;
        }
    }


    void Shockwave()
    {
        RaycastHit[] sphere = Physics.SphereCastAll(transform.position, shockwave_radius, Vector3.forward, 0);

        foreach (var elem in sphere)
        {
            Rigidbody rigid_body = elem.collider.gameObject.GetComponent<Rigidbody>();

            if (rigid_body == this.GetComponent<Rigidbody>())
                continue;

            if (rigid_body != null)
                rigid_body.AddExplosionForce(shockwave_force, transform.position, shockwave_radius);
        }

        firing_shockwave = false;
    }


    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * move_speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * move_speed;

        transform.position += new Vector3(horizontal, 0, vertical);

        player_animator.SetBool("walking", horizontal != 0 || vertical != 0);

        if (horizontal < 0 && !flipped)
        {
            flipped = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (horizontal > 0 && flipped)
        {
            flipped = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }


    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "PowerUp")
        {
        }
    }
}
