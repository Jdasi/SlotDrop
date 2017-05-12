using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    const int MAX_PLAYERS = 32;

    public Transform player_spawn;
    public GameObject player_prefab;

	void Start()
    {
		
	}
	
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Instantiate(player_prefab, 
        }
	}
}
