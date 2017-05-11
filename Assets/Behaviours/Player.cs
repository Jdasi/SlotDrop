using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id = 0;
    public float move_speed = 20;

    public List<GameObject> mount_points = new List<GameObject>();

    void Start()
    {
    }


    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * move_speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * move_speed;

        transform.position += new Vector3(horizontal, 0, vertical);
    }


    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "PowerUp")
        {
        }
    }
}
