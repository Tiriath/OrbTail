using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used in spectator mode to cycle among different players.
/// </summary>
public class Spectator : MonoBehaviour
{
    /// <summary>
    /// Get the lobby player associated to this spectator.
    /// </summary>
    public LobbyPlayer LobbyPlayer { get; set; }

    public void Awake()
    {
        input = this.GetComponent<InputProxy>();
    }

    public void OnDestroy()
    {
        Ship.ShipDestroyedEvent -= OnShipDestroyed;
    }

    public void OnEnable()
    {
        Ship.ShipDestroyedEvent += OnShipDestroyed;

        ships = new List<Ship>(FindObjectsOfType<Ship>());

        JumpNext();
    }

    void Update()
    {
        if(input.FireInput)
        {
            enable_change_target = false;

            JumpNext();
        }
        else
        {
            enable_change_target = true;
        }
    }

    /// <summary>
    /// Jump to the next ship.
    /// </summary>
    private void JumpNext()
    {
        if (ships.Count > 0)
        {
            transform.SetParent(ships[0].gameObject.transform);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            var next = ships[0];

            ships.RemoveAt(0);

            ships.Add(next);
        }
    }

    /// <summary>
    /// Called whenever a ship is destroyed.
    /// </summary>
    private void OnShipDestroyed(Ship ship)
    {
        ships.Remove(ship);

        // Jump to the next ship if the current ship being followed has been destroyed.

        if (transform.parent == ship.gameObject.transform)
        {
            JumpNext();
        }
    }

    /// <summary>
    /// Whether to change the target.
    /// </summary>
    private bool enable_change_target = true;

    /// <summary>
    /// List of all ships in the game.
    /// </summary>
    private List<Ship> ships;

    /// <summary>
    /// Current camera movement.
    /// </summary>
    private FollowCamera camera_movement_;

    /// <summary>
    /// Proxy used to read user or AI input.
    /// </summary>
    private InputProxy input;
}
