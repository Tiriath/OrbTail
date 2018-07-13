using UnityEngine;

/// <summary>
/// HUD element used to show the spectating message and used to cycle remaining players.
/// </summary>
public class HUDSpectator : HUDElement
{
    /// <summary>
    /// Message to show when spectating.
    /// </summary>
    public string spectating_message = "spectating";

    public void Awake()
    {
        text_mesh = gameObject.GetComponent<TextMesh>();
        input_collider = gameObject.GetComponent<Collider>();

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

        text_mesh.text = "";
    }

    protected override void OnOwnerChanged()
    {
        var spectator = Owner.GetComponent<Spectator>();
        input = Owner.GetComponent<InputProxy>();

        if (spectator != null)
        {
            text_mesh.text = spectating_message;
            input_collider.enabled = true;
            is_spectating = true;
        }
        else
        {
            text_mesh.text = "";
            input_collider.enabled = false;
            is_spectating = false;
        }
    }

    public override void OnInputConfirm()
    {
        if (input && is_spectating)
        {
            input.PowerUpSignal = true;
        }
    }

    /// <summary>
    /// Element used to displayer the game time on.
    /// </summary>
    private TextMesh text_mesh;

    /// <summary>
    /// Whether the player is currently spectating.
    /// </summary>
    private bool is_spectating = false;

    /// <summary>
    /// Used to read user input.
    /// </summary>
    private InputProxy input;

    /// <summary>
    /// HUD element collider.
    /// </summary>
    private Collider input_collider;
}
