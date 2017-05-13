using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerFactory : MonoBehaviour
{
    public Transform player_spawn;
    public GameObject player_prefab;
    public AbilityFactory ability_factory;


    public void CreatePlayer(ConnectedPlayer connected_player)
    {
        GameObject player_clone = Instantiate(player_prefab);
        PlayerController player_controller = player_clone.GetComponent<PlayerController>();

        player_controller.name = "Player" +  connected_player.rewired.id.ToString();
        player_controller.SetPlayerInput(connected_player.rewired);
        player_controller.transform.position = player_spawn.position;

        connected_player.player_obj = player_clone;
        connected_player.rewired.isPlaying = true;

        ability_factory.AssignLoadout(player_controller);
    }

}
