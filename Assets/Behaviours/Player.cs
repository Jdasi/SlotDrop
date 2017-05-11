using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id = 0;
    public float move_speed = 20;

    public GameObject test_eyes;

    private Dictionary<MountPoint.ID, MountPoint> mount_points = new Dictionary<MountPoint.ID, MountPoint>();
    private GameObject mount_origin;
 void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "PowerUp")
        {

        }
    }
    void Start()
    {
        mount_origin = transform.FindChild("Body").gameObject;

        MountPoint mount_point = new MountPoint();
        mount_point.mount_object = Instantiate(test_eyes, transform.position, Quaternion.identity, transform) as GameObject;
        mount_point.mount_offset = new Vector3(0, 1, 0);
        
        mount_points.Add(MountPoint.ID.eyes, mount_point);

		Vector3 look_pos = Camera.main.transform.position - transform.position;
        look_pos.x = 0;

        transform.LookAt(look_pos);
	}
	

	void Update()
    {
		float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * move_speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * move_speed;

        transform.position += new Vector3(horizontal, 0, vertical);

        UpdateMountPoints();
	}


    void UpdateMountPoints()
    {
        foreach (var elem in mount_points)
        {
            GameObject mount_object = elem.Value.mount_object;
            Vector3 mount_offset = elem.Value.mount_offset;

            if (mount_object == null)
            {
                continue;
            }

            mount_object.transform.position = mount_origin.transform.position;
            mount_object.transform.localPosition += mount_offset;
        }
    }
}
