using UnityEngine;
using UnityEngine.Networking;

using System.Collections.Generic;

/// <summary>
/// Base component for each ship.
/// </summary>
public class Ship : NetworkBehaviour
{
    public delegate void DelegateShipEvent(Ship sender);
    public delegate void DelegateOrbEvent(Ship ship, GameObject orb);

    public static event DelegateShipEvent ShipCreatedEvent;
    public static event DelegateShipEvent ShipLocalPlayerEvent;
    public static event DelegateShipEvent ShipDestroyedEvent;

    public event DelegateShipEvent ShipReadyEvent;

    public event DelegateOrbEvent OrbAttachedEvent;
    public event DelegateOrbEvent OrbDetachedEvent;

    /// <summary>
    /// Player index, relative to the match.
    /// </summary>
    [SyncVar]
    public int player_index = -1;

    /// <summary>
    /// Whether this ship is ready.
    /// </summary>
    [SyncVar(hook = "OnSyncIsReady")]
    public bool is_ready = false;

    /// <summary>
    /// Get the lobby player associated to this ship.
    /// </summary>
    public LobbyPlayer LobbyPlayer { get; private set; }

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

    public void Awake()
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

        LobbyPlayer = GameLobby.Instance.lobbySlots[player_index] as LobbyPlayer;

        if(!LobbyPlayer.is_human && isServer)
        {
            gameObject.AddComponent<PlayerAI>();
        }

        RefreshColor();

        // Defer this event until we are sure the ship is properly setup.

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

        Debug.Assert(hasAuthority, "Local player is expected to have authority!");

        if(ShipLocalPlayerEvent != null)
        {
            ShipLocalPlayerEvent(this);
        }

        if(!LobbyPlayer.is_human)
        {
            SetReady();                             // The AI is always ready!
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
    /// Only the server is allowed to attach orbs.
    /// </summary>
    [ClientRpc]
    public void RpcAttachOrb(GameObject orb)
    {
        bool attached = AttachDriver.Top().AttachOrb(orb, OnOrbAttached);

        if (attached && OrbAttachedEvent != null)
        {
            OrbAttachedEvent(this, orb);
        }
    }

    /// <summary>
    /// Detach one orb from the ship.
    /// Only the server is allowed to detach orbs.
    /// </summary>
    [ClientRpc]
    public void RpcDetachOrb()
    {
        var detached_orb = DetachDriver.Top().DetachOrb(OnOrbDetached);

        if (OrbDetachedEvent != null && detached_orb)
        {
            OrbDetachedEvent(this, detached_orb);
        }
    }

    /// <summary>
    /// Set this ship status to "ready".
    /// </summary>
    public void SetReady()
    {
        if(isLocalPlayer)
        {
            CmdSetReady();
        }
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
            orb_material = new Material(orb_controller.DefaultMaterial);

            orb_material.color = LobbyPlayer.Color;
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
        if(orbs.Count > 0)
        {
            var orb = orbs.Pop();

            orb.Unlink();

            return orb.gameObject;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Called whenever a new orb cross the proximity boundaries.
    /// </summary>
    private void OnProximityEnter(object sender, Collider other)
    {
        if(isServer)
        {
            GameObject game_object = other.gameObject;

            if (game_object.tag == Tags.Orb)
            {
                OrbController orb_controller = game_object.GetComponent<OrbController>();

                if (!orb_controller.IsLinked)
                {
                    RpcAttachOrb(game_object);
                }
            }
        }
    }

    /// <summary>
    /// Refresh the ship color.
    /// </summary>
    private void RefreshColor()
    {
        //The material is shared to reduce the draw calls.

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
    }

    /// <summary>
    /// Set this ship status to "ready".
    /// Server-side.
    /// </summary>
    [Command]
    private void CmdSetReady()
    {
        is_ready = true;
    }

    /// <summary>
    /// Set this ship status to "ready."
    /// Client-side.
    /// </summary>
    private void OnSyncIsReady(bool is_ready)
    {
        this.is_ready = is_ready;

        if (is_ready && ShipReadyEvent != null)
        {
            ShipReadyEvent(this);
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
