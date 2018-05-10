using UnityEngine;

/// <summary>
/// HUD element used to display the countdown.
/// </summary>
public class HUDGameCountdown : HUDElement
{
    /// <summary>
    /// Scale to use when the timer is pulsating.
    /// </summary>
    public float pulse_scale = 0.05f;

    /// <summary>
    /// Duration of each pulse.
    /// </summary>
    public float pulse_duration = 0.4f;

    /// <summary>
    /// Pulse easing curve.
    /// </summary>
    public iTween.EaseType pulse_ease = iTween.EaseType.easeOutCirc;

    /// <summary>
    /// Time to wait before fading the element away.
    /// </summary>
    public float fade_delay = 0.5f;

    /// <summary>
    /// Time fade duration.
    /// </summary>
    public float fade_duration = 1.0f;

    /// <summary>
    /// Fade easing curve.
    /// </summary>
    public iTween.EaseType fade_ease = iTween.EaseType.easeInCubic;

    /// <summary>
    /// Text to display when the countdown ends.
    /// </summary>
    public string timeout_text = "GO!";

    public void Awake()
    {
        text_mesh = GetComponent<TextMesh>();
        timer = BaseGameMode.Instance.GetComponent<GameTimer>();

        original_scale = gameObject.transform.localScale;
        original_color = text_mesh.color;

        text_mesh.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        BaseGameMode.Instance.MatchCountdownEvent += OnMatchCountdown;
    }

    public void OnDestroy()
    {
        BaseGameMode.Instance.MatchCountdownEvent -= OnMatchCountdown;

        timer.TickEvent -= OnTick;
    }

    /// <summary>
    /// Called whenever the game mode countdown starts.
    /// </summary>
    private void OnMatchCountdown(BaseGameMode game_mode)
    {
        BaseGameMode.Instance.MatchCountdownEvent -= OnMatchCountdown;

        timer.TickEvent += OnTick;

        PulseTimer(0.0f);
    }

    /// <summary>
    /// Called whenever the timer ticks.
    /// </summary>
    private void OnTick(GameTimer timer)
    {
        time = timer.time;                                              // Snapshot the current time since it may change when the tweening coroutine is executed.

        iTween.ValueTo(this.gameObject, iTween.Hash(
            "from", 0f,
            "to", 1f,
            "time", pulse_duration,
            "easeType", pulse_ease,
            "onUpdate", "PulseTimer"));

        if(timer.time <= 0)
        {
            timer.TickEvent -= OnTick;

            iTween.FadeTo(this.gameObject, iTween.Hash(
                "alpha", 0f,
                "time", fade_duration,
                "easeType", fade_ease,
                "delay", fade_delay));
        }
    }

    /// <summary>
    /// Called whenever the timer is pulsating.
    /// </summary>
    private void PulseTimer(float alpha)
    {
        gameObject.transform.localScale = Vector3.Lerp(original_scale, original_scale * pulse_scale, 1.0f - alpha);
        text_mesh.color = original_color;

        text_mesh.text = (time > 0) ? timer.time.ToString() : timeout_text;
    }

    /// <summary>
    /// Countdown timer.
    /// </summary>
    private GameTimer timer;

    /// <summary>
    /// Time to display.
    /// </summary>
    private int time;

    /// <summary>
    /// Element used to displayer the countdown on.
    /// </summary>
    private TextMesh text_mesh;

    /// <summary>
    /// Original element scale.
    /// </summary>
    private Vector3 original_scale;

    /// <summary>
    /// Original element color.
    /// </summary>
    private Color original_color;
}
