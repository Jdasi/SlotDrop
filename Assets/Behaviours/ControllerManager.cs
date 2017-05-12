using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ControllerManager : MonoBehaviour
{
    void Awake()
    {
        // Listen for controller connection events
        ReInput.ControllerConnectedEvent += OnControllerConnected;

        // Assign each Joystick to a Player initially
        foreach (Joystick joystick in ReInput.controllers.Joysticks)
        {
            if (ReInput.controllers.IsJoystickAssigned(joystick))
                continue; // Joystick is already assigned

            // Assign Joystick to first Player that doesn't have any assigned
            AssignJoystickToNextOpenPlayer(joystick);
        }
    }


    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (args.controllerType != ControllerType.Joystick)
            return; // skip if this isn't a Joystick

        AssignJoystickToNextOpenPlayer(ReInput.controllers.GetJoystick(args.controllerId));
    }


    void AssignJoystickToNextOpenPlayer(Joystick joystick)
    {
        foreach (Player player in ReInput.players.Players)
        {
            if (player.controllers.joystickCount > 0)
                continue; // player already has a joystick

            player.controllers.AddController(joystick, true); // assign joystick to player

            return; // joystick assigned, abort
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
