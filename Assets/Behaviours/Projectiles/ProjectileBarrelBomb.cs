﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBarrelBomb : Projectile
{
    public GameObject barrel_prefab;
    public int max_bombs = 3;

    public float min_range = 2000;
    public float max_range = 3000;

    private float min_variation = 10;
    private List<Vector3> prev_drop_vectors = new List<Vector3>();

    protected override void Start()
    {
        // Spawn bombs!
        for (int i = 0; i < max_bombs; ++i)
        {
            GameObject obj = Instantiate(barrel_prefab, owning_player.transform.position + new Vector3(0, 5, 0), Quaternion.identity);
            
            Barrel bomb = obj.GetComponent<Barrel>();
            bomb.owning_player = owning_player;

            GenerateDropVector(bomb);
        }

        Destroy(gameObject);
    }


    void GenerateDropVector(Barrel bomb)
    {
        Vector3 drop_vector = Vector3.zero;

        bool vector_valid = false;

        do
        {
            float random_x = Random.Range(-max_range, max_range);
            random_x = Mathf.Clamp(random_x, -min_range, max_range);

            float random_z = Random.Range(-max_range, max_range);
            random_z = Mathf.Clamp(random_z, -min_range, max_range);

            drop_vector = new Vector3(random_x, 10000, random_z);

            int test_passes = 0;
            foreach (Vector3 v in prev_drop_vectors)
            {
                if (Vector3.Distance(drop_vector, v) >= min_range)
                    ++test_passes;
            }

            if (test_passes >= prev_drop_vectors.Count)
                vector_valid = true;

        } while (!vector_valid);

        bomb.AddForce(drop_vector);

        prev_drop_vectors.Add(drop_vector);
    }

}
