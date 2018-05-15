using UnityEngine;

/// <summary>
/// HUD element used to show the outcome of a match.
/// </summary>
public class HUDShipOutcome : HUDElement
{
    /// <summary>
    /// Message to show when the player wins.
    /// </summary>
    public string win_message = "you win!";

    /// <summary>
    /// Message to show when the player looses.
    /// </summary>
    public string loss_message = "you lose!";

    /// <summary>
    /// Message to show when the match ended with a tie.
    /// </summary>
    public string tie_message = "tie";

    public void Awake()
    {
        text_mesh = gameObject.GetComponent<TextMesh>();

        BaseGameMode.Instance.MatchEndEvent += OnMatchEnd;
    }

    public void OnDestroy()
    {
        var game_mode = BaseGameMode.Instance;

        if (game_mode)
        {
            game_mode.MatchEndEvent -= OnMatchEnd;
        }
    }

    /// <summary>
    /// Called whenever the game mode ends.
    /// </summary>
    private void OnMatchEnd(BaseGameMode game_mode)
    {
        BaseGameMode.Instance.MatchCountdownEvent -= OnMatchEnd;

        var winner = BaseGameMode.Instance.Winner;

        if (winner == null)
        {
            text_mesh.text = tie_message;
        }
        else if (winner == player)
        {
            text_mesh.text = win_message;
        }
        else
        {
            text_mesh.text = loss_message;
        }
    }

    /// <summary>
    /// Called whenever the owner of this HUD changes.
    /// </summary>
    protected override void OnOwnerChanged()
    {
        base.OnOwnerChanged();

        if(Owner)
        {
            var lobby_player = Owner.GetComponent<LobbyPlayer>();
            var ship = Owner.GetComponent<Ship>();
            var spectator = Owner.GetComponent<Spectator>();

            if(lobby_player)
            {
                player = lobby_player;
            }
            else if(ship)
            {
                player = ship.LobbyPlayer;
            }
            else if(spectator)
            {
                player = spectator.LobbyPlayer;
            }
        }
        else
        {
            player = null;
        }
    }

    /// <summary>
    /// Element used to displayer the game time on.
    /// </summary>
    private TextMesh text_mesh;

    /// <summary>
    /// Player this HUD element refers to.
    /// </summary>
    private LobbyPlayer player;
}
