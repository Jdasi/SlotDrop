using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{
    public Meteor meteor_prefab;
    public SpawnAreaCircle meteor_spawn_area;

    public void  SpawnMeteor()
    {
        GameObject meteor_clone = Instantiate(meteor_prefab).gameObject;
        Vector2 random_circle_location = Random.insideUnitCircle * meteor_spawn_area.spawn_radius;

        meteor_clone.transform.position = new Vector3(meteor_spawn_area.transform.position.x + random_circle_location.x,
            meteor_spawn_area.transform.position.y, meteor_spawn_area.transform.position.z + random_circle_location.y); // spawn meteor at random position
    }

}
