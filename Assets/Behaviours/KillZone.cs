using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.Damage(player.GetHealth());
        }
        else if (other.tag == "Meteor")
        {
            other.GetComponent<Meteor>().Impact(GetComponent<Collider>());
        }
    }

}
