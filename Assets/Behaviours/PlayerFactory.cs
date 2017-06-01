using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerFactory : MonoBehaviour
{
    public Transform player_spawn;
    public GameObject player_prefab;
    public LoadoutFactory ability_factory;
    private SpawnAreaCircle spawn_area;


    private void Start()
    {
        spawn_area = player_spawn.GetComponent<SpawnAreaCircle>();
    }


    public void CreatePlayer(ConnectedPlayer connected_player)
    {
        GameObject player_clone = Instantiate(player_prefab, player_spawn);
        PlayerController player_controller = player_clone.GetComponent<PlayerController>();

        player_controller.name = "Player" + connected_player.rewired.id.ToString();
        player_controller.SetPlayerColor(connected_player.color);
        player_controller.SetPlayerInput(connected_player.rewired);

        Vector2 random_circle_location = Random.insideUnitCircle * spawn_area.spawn_radius;

        player_controller.transform.position = new Vector3(spawn_area.transform.position.x + random_circle_location.x,
            spawn_area.transform.position.y, spawn_area.transform.position.z + random_circle_location.y); // spawn meteor at random position

        connected_player.player_obj = player_clone;
        connected_player.rewired.isPlaying = true;

        ability_factory.AssignLoadout(player_controller, "Base");
    }

}
