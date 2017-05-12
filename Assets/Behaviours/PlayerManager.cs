using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class PlayerManager : MonoBehaviour
{
    public GameObject player_prefab;

    void Awake()
    {
        // Listen for controller connection events
        ReInput.ControllerConnectedEvent += OnControllerConnected;

        // Assign each Joystick to a Player initially
        foreach (Joystick j in ReInput.controllers.Joysticks)
        {
            if (ReInput.controllers.IsJoystickAssigned(j)) continue; // Joystick is already assigned

            // Assign Joystick to first Player that doesn't have any assigned
            AssignJoystickToNextOpenPlayer(j);
        }
    }


    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (args.controllerType != ControllerType.Joystick) return; // skip if this isn't a Joystick

        AssignJoystickToNextOpenPlayer(ReInput.controllers.GetJoystick(args.controllerId));
    }

    void AssignJoystickToNextOpenPlayer(Joystick j)
    {
        foreach (Player p in ReInput.players.Players)
        {
            if (p.controllers.joystickCount > 0) continue; // player already has a joystick
            p.controllers.AddController(j, true); // assign joystick to player
            return;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        ReInput.ControllerConnectedEvent -= OnControllerConnected;
        //ReInput.ControllerDisconnectedEvent -= OnControllerDisconnected;
        //ReInput.ControllerPreDisconnectEvent -= OnControllerPreDisconnect;
    }
}
