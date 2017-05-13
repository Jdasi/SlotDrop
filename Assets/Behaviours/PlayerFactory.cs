using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerFactory : MonoBehaviour
{
    public Transform player_spawn;
    public GameObject player_prefab;
    public AbilityFactory ability_factory;


    public GameObject CreatePlayer(Player player_input)
    {
        GameObject player_clone = Instantiate(player_prefab);

        PlayerController player = player_clone.GetComponent<PlayerController>();

        player.name = "Player" + player_input.id.ToString();
        player.SetPlayerInput(player_input);
        player.transform.position = player_spawn.position;

        ability_factory.AssignLoadout(player);

        return player_clone;
    }

}
