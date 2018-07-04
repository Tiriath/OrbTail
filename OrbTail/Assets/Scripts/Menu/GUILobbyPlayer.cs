using UnityEngine;
using System.Linq;

/// <summary>
/// Script used to show the status of a lobby player inside the lobby scene.
/// </summary>
public class GUILobbyPlayer : MonoBehaviour
{
    /// <summary>
    /// Index of the player this GUI element should react to.
    /// </summary>
    public int index = -1;

    /// <summary>
    /// Scale of the element when the player is ready.
    /// </summary>
    public float ready_scale = 1.0f;

    /// <summary>
    /// Scale of the element when the player is not ready.
    /// </summary>
    public float not_ready_scale = 0.5f;

    /// <summary>
    /// Time needed to tween the element.
    /// </summary>
    public float tween_time = 0.2f;

    /// <summary>
    /// Ships this element can display.
    /// </summary>
    public ShipBinding[] ships;

    public void Start ()
    {
        LobbyPlayer.PlayerJoinedEvent += OnLobbyPlayerJoined;
        LobbyPlayer.PlayerLeftEvent += OnLobbyPlayerLeft;

        original_scale = gameObject.transform.localScale;

        gameObject.transform.localScale = Vector3.zero;
    }

    public void OnDestroy()
    {
        LobbyPlayer.PlayerJoinedEvent -= OnLobbyPlayerJoined;
        LobbyPlayer.PlayerLeftEvent -= OnLobbyPlayerLeft;

        if(bound_lobby_player)
        {
            bound_lobby_player.PlayerIndexChangedEvent -= OnPlayerIndexChanged;
            bound_lobby_player.PlayerShipChangedEvent -= OnPlayerShipChanged;
            bound_lobby_player.PlayerReadyEvent -= OnPlayerReadyChanged;
        }
    }

    /// <summary>
    /// Called whenever a new player joins the lobby.
    /// </summary>
    private void OnLobbyPlayerJoined(LobbyPlayer lobby_player)
    {
        lobby_player.PlayerIndexChangedEvent += OnPlayerIndexChanged;

        OnPlayerIndexChanged(lobby_player);
    }

    /// <summary>
    /// Called whenever a player index changes.
    /// </summary>
    private void OnPlayerIndexChanged(LobbyPlayer lobby_player)
    {
        if(lobby_player.player_index == index)
        {
            Debug.Assert(bound_lobby_player == null, "The element cannot be bound to more than one player!");

            bound_lobby_player = lobby_player;

            lobby_player.PlayerShipChangedEvent += OnPlayerShipChanged;
            lobby_player.PlayerReadyEvent += OnPlayerReadyChanged;

            OnPlayerShipChanged(lobby_player);
            OnPlayerReadyChanged(lobby_player);
        }
    }

    /// <summary>
    /// Called whenever a player ship changes.
    /// </summary>
    private void OnPlayerShipChanged(LobbyPlayer lobby_player)
    {
        Debug.Assert(lobby_player == bound_lobby_player);

        var player_ship = this.ships.Where(ship => ship.GameShip.name == lobby_player.player_ship).ToArray();

        Destroy(ship);

        if (player_ship.Length > 0)
        {
            ship = GameObject.Instantiate(player_ship[0].MenuShip, transform);

            ship.transform.localPosition = Vector3.zero;

            ship.GetComponent<ShipVFX>().SetLivery(lobby_player.Color, lobby_player.player_index);
        }
    }

    /// <summary>
    /// Called whenever the the player ready status changes.
    /// </summary
    private void OnPlayerReadyChanged(LobbyPlayer lobby_player)
    {
        Debug.Assert(lobby_player == bound_lobby_player);

        var target_scale = lobby_player.readyToBegin ? ready_scale : not_ready_scale;

        iTween.ScaleTo(gameObject, original_scale * target_scale, tween_time);
    }

    /// <summary>
    /// Called whenever a a player leaves the lobby.
    /// </summary>
    private void OnLobbyPlayerLeft(LobbyPlayer lobby_player)
    {
        if (bound_lobby_player && lobby_player == bound_lobby_player)
        {
            lobby_player.PlayerIndexChangedEvent -= OnPlayerIndexChanged;
            lobby_player.PlayerShipChangedEvent -= OnPlayerShipChanged;
            lobby_player.PlayerReadyEvent -= OnPlayerReadyChanged;

            bound_lobby_player = null;

            iTween.ScaleTo(gameObject, Vector3.zero, tween_time);

            Destroy(ship);

            ship = null;
        }
    }

    /// <summary>
    /// Lobby player this element is currently bound to.
    /// </summary>
    private LobbyPlayer bound_lobby_player = null;

    /// <summary>
    /// Original element scale.
    /// </summary>
    private Vector3 original_scale;

    /// <summary>
    /// Ship instance used to show the ship selected by this player.
    /// </summary>
    private GameObject ship = null;
}

/// <summary>
/// Binds a game ship to its visual representation inside the matchmaking.
/// </summary>
[System.Serializable]
public class ShipBinding
{
    /// <summary>
    /// Game ship selected by the player.
    /// </summary>
    public GameObject GameShip;

    /// <summary>
    /// Menu ship to show.
    /// </summary>
    public GameObject MenuShip;
}