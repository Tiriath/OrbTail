using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script used in spectator mode to cycle among different players
/// </summary>
public class SpectatorMode : MonoBehaviour
{
    public void Start ()
    {
        camera_movement_ = GetComponent<FollowCamera>();

        Ship.ShipCreatedEvent += OnShipCreated;
        Ship.ShipDestroyedEvent += OnShipDestroyed;

        LookNext();
    }

    void Update()
    {
        if (Input.GetAxis(Inputs.Fire) <= 0.0f && Input.touchCount <= 0)
        {
            toggle_change_target = true;
        }
        else if (toggle_change_target)
        {
            toggle_change_target = false;

            LookNext();
        }
    }

    /// <summary>
    /// Looks to the next ship
    /// </summary>
    private void LookNext()
    {
        if (ships_.Count > 0)
        {
            var element = ships_[0];

            ships_.RemoveAt(0);

            ships_.Add(element);

            camera_movement_.ViewTarget = ships_[0];
        }
    }

    /// <summary>
    /// Called whenever a new ship is created.
    /// </summary>
    private void OnShipCreated(Ship ship)
    {
        ships_.Add(ship.gameObject);
    }

    /// <summary>
    /// Called whenever a ship is destroyed.
    /// </summary>
    private void OnShipDestroyed(Ship ship)
    {
        ships_.Remove(ship.gameObject);

        if (camera_movement_.ViewTarget == ship)
        {
            LookNext();
        }
    }
    
    /// <summary>
    /// Whether to change the target.
    /// </summary>
    private bool toggle_change_target = true;
    
    /// <summary>
    /// List of all ships in the game.
    /// </summary>
    private IList<GameObject> ships_ = new List<GameObject>();

    /// <summary>
    /// Current camera movement.
    /// </summary>
    private FollowCamera camera_movement_;
}
