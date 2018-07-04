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
    public delegate void DelegateOrbsEvent(Ship ship, List<GameObject> orbs);

    public static event DelegateShipEvent ShipCreatedEvent;
    public static event DelegateShipEvent ShipLocalPlayerEvent;
    public static event DelegateShipEvent ShipDestroyedEvent;

    public event DelegateShipEvent ShipReadyEvent;

    public event DelegateOrbEvent OrbAttachedEvent;
    public event DelegateOrbsEvent OrbDetachedEvent;

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
    /// Livery to use, one for each possible player index value.
    /// </summary>
    public TextureField[] liveries;

    /// <summary>
    /// Get the lobby player associated to this ship.
    /// </summary>
    public LobbyPlayer LobbyPlayer { get; private set; }

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
        GetComponentInChildren<ProximityHandler>().OnProximityEvent -= OnProximityEnter;

        if (ShipDestroyedEvent != null)
        {
            ShipDestroyedEvent(this);
        }
    }

    /// <summary>
    /// Attach an orb to the ship.
    /// </summary>
    /// <param name="orb">Orb to attach.</param>
    /// <returns>Returns true if the orb could be attached, returns false otherwise.</returns>
    public void AttachOrb(GameObject orb)
    {
        Debug.Assert(isServer);

        if(!orb.GetComponent<OrbController>().IsLinked)
        {
            RpcAttachOrb(orb);
        }
    }

    /// <summary>
    /// Detach one or more orbs from the ship.
    /// </summary>
    /// <param name="count">Number of orbs to detach.</param>
    public void DetachOrbs(int count)
    {
        Debug.Assert(isServer);

        count = Mathf.Min(orbs.Count, count);

        if(count > 0 && !GetComponent<Invincibility>())
        {
            RpcDetachOrb(count);
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
    /// Attach one orb to the ship.
    /// </summary>
    [ClientRpc]
    private void RpcAttachOrb(GameObject orb)
    {
        var orb_controller = orb.GetComponent<OrbController>();

        if (orb_material == null)
        {
            orb_material = new Material(orb_controller.DefaultMaterial);

            orb_material.SetColor("_Albedo", LobbyPlayer.Color);
        }

        // Either link to the last orb or to the ship itself.

        orb_controller.Link(orbs.Count > 0 ? orbs.Peek().gameObject : gameObject, orb_material);

        orbs.Push(orb_controller);

        if (OrbAttachedEvent != null)
        {
            OrbAttachedEvent(this, orb);
        }
    }

    /// <summary>
    /// Detach one orb from the ship.
    /// Only the server is allowed to detach orbs.
    /// </summary>
    [ClientRpc]
    private void RpcDetachOrb(int count)
    {
        var detached_orbs = new List<GameObject>();

        for(;count > 0; --count)
        {
            var orb = orbs.Pop();

            orb.Unlink();

            detached_orbs.Add(orb.gameObject);
        }
        
        if (OrbDetachedEvent != null)
        {
            OrbDetachedEvent(this, detached_orbs);
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
        var material = GetComponentInChildren<MeshRenderer>().material;

        material.SetColor("_Color", LobbyPlayer.Color);
        material.SetFloat("_Desaturate", 0.0f);

        if (liveries.Length > LobbyPlayer.player_index)
        {
            string livery_path = liveries[LobbyPlayer.player_index];

            var livery_texture = Resources.Load<Texture>(livery_path);

            material.SetTexture("_Diffuse", livery_texture);
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
