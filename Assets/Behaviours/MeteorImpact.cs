using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorImpact : MonoBehaviour
{
    public float drop_speed = 10f;
    public int meteor_damage = 25;
    [Range(0,1)]
    public float max_random_drop_offset = 0.5f;
    public List<GameObject> particles = new List<GameObject>();
    public float shake_strength = 0.4f;
    public float shake_duration = 0.4f;

    private Vector3 drop_vector;
    private MeshRenderer meteor_mesh;
    private TrailRenderer meteor_trail;
    private bool impacted = false;
    private Ability shock_wave;
    private MeshRenderer[] child_renderers;
   

    private void Awake()
    {
        meteor_mesh = GetComponent<MeshRenderer>();
        meteor_trail = GetComponent<TrailRenderer>();
        shock_wave = GetComponent<Ability>();
        child_renderers = GetComponentsInChildren<MeshRenderer>();

        float random_x = Random.Range(-max_random_drop_offset, max_random_drop_offset);
        float random_z = Random.Range(-max_random_drop_offset, max_random_drop_offset);

        drop_vector = new Vector3(random_x, -1, random_z);
        drop_vector.Normalize();
    }


    private void Update()
    {
        if(!impacted)
        transform.position += drop_vector * drop_speed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        Impact(other);
    }


    private void OnTriggerStay(Collider other)
    {
        Impact(other);
    }


    private void Impact(Collider other)
    {
        if (!impacted)
        {
            impacted = true;

            CameraShake.instance.ShakeCamera(shake_duration, shake_strength);
            PlayerController hit_player = other.GetComponent<PlayerController>();

            if (hit_player)
            {
                hit_player.Damage(meteor_damage);
            }

            foreach(GameObject particle in particles)
            {
                Destroy(particle);
            }


            foreach(MeshRenderer mesh in child_renderers)
            {
                mesh.enabled = false;
            }
            

            meteor_mesh.enabled = false;//hide mesh
            Destroy(gameObject, meteor_trail.time);//destroy after trail has finished
        }
    }
}
