using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBarrelBomb : Projectile
{
    public GameObject barrel_prefab;

    protected override void Start()
    {
        for (int i = 0; i < 3; ++i)
        {
            GameObject bomb = Instantiate(barrel_prefab, owning_player.transform.position + new Vector3(0, 5, 0), Quaternion.identity);
            bomb.GetComponent<Barrel>().owner = owning_player.gameObject;

            float random_x = Random.Range(-5000, 5000);
            float random_z = Random.Range(-5000, 5000);

            Vector3 drop_vector = new Vector3(random_x, 10000, random_z);

            bomb.GetComponent<Rigidbody>().AddForce(drop_vector);
        }

        Destroy(gameObject);
    }

}
