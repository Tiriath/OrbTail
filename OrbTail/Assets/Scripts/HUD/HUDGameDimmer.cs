using UnityEngine;

/// <summary>
/// HUD element used to dim the interface when the match ends.
/// </summary>
public class HUDGameDimmer : HUDElement
{
    /// <summary>
    /// Time fade duration.
    /// </summary>
    public float fade_duration = 0.3f;

    /// <summary>
    /// Fade easing curve.
    /// </summary>
    public iTween.EaseType fade_ease = iTween.EaseType.easeInCubic;

    public void Awake()
    {
        gameObject.SetActive(false);

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
        gameObject.SetActive(true);

        BaseGameMode.Instance.MatchCountdownEvent -= OnMatchEnd;

        iTween.FadeFrom(gameObject, 0.0f, fade_duration);
    }
}
