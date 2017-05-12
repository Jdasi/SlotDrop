using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PowerUpManager : MonoBehaviour
{
    public List<GameObject> particles = new List<GameObject>();


    public GameObject GetParticle(int particle_id)
    {
        return particles[particle_id];
    }
}
