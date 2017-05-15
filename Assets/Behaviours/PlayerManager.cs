using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : MonoBehaviour
{
    const int MAX_PLAYERS = 32;
    PlayerFactory player_factory;

    private Dictionary<int, ConnectedPlayer> connected_players;


    void Awake()
    {
        player_factory = GetComponent<PlayerFactory>();
        connected_players = new Dictionary<int, ConnectedPlayer>();

        // Subscribe to controller connected events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
    }


    void Start()
    {
        AssignAllConnectedJoysticks();
    }


    void Update()
    {
        foreach (ConnectedPlayer connected_player in connected_players.Values)
        {
            // Player initial join.
            if (connected_player.rewired.GetButtonDown("Connect") && !connected_player.rewired.isPlaying)
            {
                player_factory.CreatePlayer(connected_player);
            }

            // Player respawn
            if (connected_player.player_obj == null && connected_player.rewired.isPlaying)
            {
                player_factory.CreatePlayer(connected_player);
            }
        }
    }


    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (args.controllerType != ControllerType.Joystick)
            return;

        CreateConnectedPlayer(args.controllerId);
    }


    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("Controller disconnected: Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);

        int player_id = args.controllerId;

        // Find the player related to the controller.
        ConnectedPlayer connected_player = connected_players[player_id];
        connected_player.rewired.controllers.ClearAllControllers();

        // And destroy them!
        KillPlayer(player_id);

        connected_players.Remove(player_id);
    }


    void AssignAllConnectedJoysticks()
    {
        foreach (Joystick joystick in ReInput.controllers.Joysticks)
        {
            CreateConnectedPlayer(joystick.id);
        }
    }


    private void CreateConnectedPlayer(int controller_id)
    {
        ConnectedPlayer connected_player = new ConnectedPlayer();
        connected_player.joystick = ReInput.controllers.Joysticks[controller_id];

        connected_player.rewired = ReInput.players.GetPlayer(controller_id);
        connected_player.rewired.controllers.AddController(connected_player.joystick, true);
        connected_player.rewired.isPlaying = false;

        connected_players.Add(controller_id, connected_player);
    }


    public void PlayerLeave(int player_id)
    {
        KillPlayer(player_id);
        connected_players[player_id].rewired.isPlaying = false;
    }


    public void KillPlayer(int player_id)
    {
        Destroy(connected_players[player_id].player_obj);
    }


    public bool AnyPlayerTitan()
    {
        foreach (ConnectedPlayer connected_player in connected_players.Values)
        {
            if (connected_player.player_obj == null)
                continue;

            PlayerController player = connected_player.player_obj.GetComponent<PlayerController>();

            if (player.IsTitan())
            {
                return true;
            }
        }

        return false;
    }

}
