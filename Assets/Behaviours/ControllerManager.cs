using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ControllerManager : MonoBehaviour
{
    const int MAX_PLAYERS = 32;
    private int player_id_counter = 0;
    PlayerFactory player_factory;

    // Track which Joysticks we've seen before in this session so we can tell new joysticks vs ones that have already been assigned to a Player
    private Dictionary<int, GameObject> assigned_joystick;

    void Awake()
    {
        player_factory = GetComponent<PlayerFactory>();
        assigned_joystick = new Dictionary<int, GameObject>();

        // Subscribe to controller connected events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
    }

    void Start()
    {
        AssignAllJoysticksToSystemPlayer(true);
    }


    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (args.controllerType != ControllerType.Joystick) return;

        // Check if this Joystick has already been assigned. If so, just let Auto-Assign do its job.
        if (assigned_joystick.ContainsKey(args.controllerId)) return;

        // Joystick hasn't ever been assigned before. Make sure it's assigned to the System Player until it's been explicitly assigned
        ReInput.players.GetSystemPlayer().controllers.AddController<Joystick>(args.controllerId, true);
    }


    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);

        RemovePlayer(args.controllerId);
    }

    void RemovePlayer(int player_id)
    {       
        Destroy(assigned_joystick[player_id]);
        assigned_joystick.Remove(player_id);
    }

    void AssignAllJoysticksToSystemPlayer(bool removeFromOtherPlayers)
    {
        foreach (Joystick j in ReInput.controllers.Joysticks)
        {
            ReInput.players.GetSystemPlayer().controllers.AddController(j, removeFromOtherPlayers);
        }
    }


    void Update()
    {
        // Watch for JoinGame action in System Player
        if (ReInput.players.GetSystemPlayer().GetButtonDown("SlotDrop"))
        {
            AssignNextPlayer();
        }
    }


    void AssignNextPlayer()
    {
        if (player_id_counter >= MAX_PLAYERS)
        {
            Debug.Log("Max player limit already reached!");
            return;
        }

        // Get the next Rewired Player Id
        int rewiredPlayerId = GetNextGamePlayerId();

        // Get the Rewired Player
        Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

        // Determine which Controller was used to generate the JoinGame Action
        Player systemPlayer = ReInput.players.GetSystemPlayer();
        var inputSources = systemPlayer.GetCurrentInputSources("SlotDrop");

        foreach (var source in inputSources)
        {  
            if (source.controllerType == ControllerType.Joystick)
            {
                AssignJoystickToPlayer(rewiredPlayer, source.controller as Joystick);
                break;
            }
            else
            { // Custom Controller
                throw new System.NotImplementedException();
            }
        }
    }


    private void AssignJoystickToPlayer(Player player, Joystick joystick)
    {
        // Assign the joystick to the Player, removing it from System Player
        player.controllers.AddController(joystick, true);

        // Mark this joystick as assigned so we don't give it to the System Player again
        assigned_joystick.Add(joystick.id, player_factory.CreatePlayer(player));

        Debug.Log("Assigned " + joystick.name + " to Player " + player.name);
       
    }


    private int GetNextGamePlayerId()
    {
        return player_id_counter++;
    }
}

