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
        BaseGameMode.Instance.MatchEndEvent -= OnMatchEnd;
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
        else if (winner == Owner.GetComponent<Ship>().LobbyPlayer)
        {
            text_mesh.text = win_message;
        }
        else
        {
            text_mesh.text = loss_message;
        }
    }

    /// <summary>
    /// Element used to displayer the game time on.
    /// </summary>
    private TextMesh text_mesh;
}
