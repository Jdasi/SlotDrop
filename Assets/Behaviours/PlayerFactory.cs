using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerFactory : MonoBehaviour
{
    public Transform player_spawn;
    public GameObject player_prefab;

    public GameObject CreatePlayer(Player player_input)
    {
        GameObject player_clone = Instantiate(player_prefab);
        player_clone.GetComponent<PlayerController>().SetPlayerInput(player_input);
        player_clone.GetComponent<PlayerController>().SetPlayerID(player_input.id);
        player_clone.transform.position = player_spawn.position;
        return player_clone;
    }
}
