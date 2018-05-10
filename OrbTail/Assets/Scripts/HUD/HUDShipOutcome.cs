using UnityEngine;

/// <summary>
/// HUD element used to show the outcome of a match.
/// </summary>
public class HUDShipOutcome : HUDElement
{
    /// <summary>
    /// Message to show when a player wins.
    /// </summary>
    public string win_message = "{0} wins!";

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

        text_mesh.text = "Game over";
    }

    /// <summary>
    /// Element used to displayer the game time on.
    /// </summary>
    private TextMesh text_mesh;
}
