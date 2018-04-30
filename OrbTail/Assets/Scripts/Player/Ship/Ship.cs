using UnityEngine;
using UnityEngine.Networking;

using System.Collections.Generic;

/// <summary>
/// Base component for each ship.
/// </summary>
public class Ship : NetworkBehaviour
{
    public delegate void DelegateShipEvent(Ship sender);
    public delegate void DelegateOrbEvent(Ship ship, List<GameObject> orbs);

    public static event DelegateShipEvent ShipCreatedEvent;
    public static event DelegateShipEvent ShipLocalPlayerEvent;
    public static event DelegateShipEvent ShipDestroyedEvent;

    public event DelegateOrbEvent OrbAttachedEvent;
    public event DelegateOrbEvent OrbDetachedEvent;

    /// <summary>
    /// Player index, relative to the match.
    /// </summary>
    [SyncVar]
    public int player_index = -1;

    /// <summary>
    /// Determine how orbs are attached to the tail.
    /// </summary>
    public DriverStack<IAttacherDriver> AttachDriver { get; private set; }

    /// <summary>
    /// Determine how orbs are detached from the tail.
    /// </summary>
    public DriverStack<IDetacherDriver> DetachDriver { get; private set; }

    /// <summary>
    /// Get the number of orbs in the ship's tail.
    /// </summary>
    public int TailLength
    {
        get
        {
            return orbs.Count;
        }
    }

    /// <summary>
    /// Get the lobby player associated to this ship.
    /// </summary>
    public LobbyPlayer LobbyPlayer { get; private set; }
    
    void Awake()
    {
        AttachDriver = new DriverStack<IAttacherDriver>();
        DetachDriver = new DriverStack<IDetacherDriver>();

        AttachDriver.SetDefaultDriver(new DefaultAttacherDriver());
        DetachDriver.SetDefaultDriver(new DefaultDetacherDriver());

        GetComponentInChildren<ProximityHandler>().OnProximityEvent += OnProximityEnter;
    }

    /// <summary>
    /// Called on each client after being activated.
    /// </summary>
    public override void OnStartClient()
    {
        base.OnStartClient();

        Debug.Assert(player_index >= 0, "Player index should be set when the client starts!");

        // Link this ship to its lobby player.

        LobbyPlayer = (LobbyPlayer)GameLobby.Instance.lobbySlots[player_index];
        LobbyPlayer.Ship = this;

        //Colorize this ship. The material is shared to reduce the draw calls.

        Material material = null;

        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            if (renderer.gameObject.tag.Equals(Tags.ShipDetail))
            {
                if (material == null)
                {
                    material = renderer.material;
                    material.color = LobbyPlayer.Color;
                }

                renderer.material = material;
            }
        }

        // Notify after the ship is properly setup.

        if (ShipCreatedEvent != null)
        {
            ShipCreatedEvent(this);
        }
    }

    /// <summary>
    /// Called whenever a local player starts controlling this ship.
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if(ShipLocalPlayerEvent != null)
        {
            ShipLocalPlayerEvent(this);
        }
    }

    public void OnDestroy()
    {
        if (ShipDestroyedEvent != null)
        {
            ShipDestroyedEvent(this);
        }
    }

    /// <summary>
    /// Attach one orb to the ship.
    /// </summary>
    public bool AttachOrb(GameObject orb)
    {
        return AttachOrb(new List<GameObject>() { orb }).Contains(orb);
    }

    /// <summary>
    /// Attach one or more orbs to the ship.
    /// </summary>
    public List<GameObject> AttachOrb(List<GameObject> orbs)
    {
        orbs = AttachDriver.Top().AttachOrbs(orbs, OnOrbAttached);

        if (OrbAttachedEvent != null)
        {
            OrbAttachedEvent(this, orbs);
        }

        return orbs;
    }

    /// <summary>
    /// Detach one or more orbs from the ship.
    /// </summary>
    public List<GameObject> DetachOrbs(int count)
    {
        var detached_orbs = DetachDriver.Top().DetachOrbs(Mathf.Min(count, orbs.Count), OnOrbDetached);

        if (OrbDetachedEvent != null && detached_orbs.Count > 0)
        {
            OrbDetachedEvent(this, detached_orbs);
        }

        return detached_orbs;
    }

    /// <summary>
    /// Called whenever a new orb is attached to the ship.
    /// </summary>
    /// <param name="orb">Orb being attached.</param>
    /// <return>Return true if the orb could be attached, returns false otherwise.</return>
    private bool OnOrbAttached(GameObject orb)
    {
        var orb_controller = orb.GetComponent<OrbController>();

        if(orb_controller.IsLinked)
        {
            return false;                   // We may not attach a linked orb since it may linked to something else.
        }

        if (orb_material == null)
        {
            orb_material = new Material(orb_controller.DefaultMaterial)
            {
                color = LobbyPlayer.Color
            };
        }

        // Either link to the last orb or to the ship itself.

        orb_controller.Link(orbs.Count > 0 ? orbs.Peek().gameObject : gameObject, orb_material);

        orbs.Push(orb_controller);

        return true;
    }

    /// <summary>
    /// Called whenever an orb is detached from the ship.
    /// </summary>
    private GameObject OnOrbDetached()
    {
        var orb = orbs.Pop();

        orb.Unlink();

        return orb.gameObject;
    }

    /// <summary>
    /// Called whenever a new orb cross the proximity boundaries.
    /// </summary>
    private void OnProximityEnter(object sender, Collider other)
    {
        GameObject game_object = other.gameObject;

        if (game_object.tag == Tags.Orb)
        {
            OrbController orb_controller = game_object.GetComponent<OrbController>();

            if (!orb_controller.IsLinked)
            {
                AttachOrb(game_object);
            }
        }
    }

    /// <summary>
    /// Orbs attached to the tail.
    /// </summary>
    private Stack<OrbController> orbs = new Stack<OrbController>();

    /// <summary>
    /// Orb material when attached to the ship.
    /// </summary>
    private Material orb_material;
}
