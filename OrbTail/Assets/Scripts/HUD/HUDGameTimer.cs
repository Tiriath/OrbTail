using UnityEngine;

/// <summary>
/// HUD element used to show the current game time.
/// </summary>
public class HUDGameTimer : HUDElement
{
    /// <summary>
    /// Time for which the HUD element starts pulsating.
    /// </summary>
    public int critical_time = 10;

    /// <summary>
    /// Color of the timer when under the critical time.
    /// </summary>
    public Color critical_color = Color.red;

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

    public void Awake()
    {
        text_mesh = gameObject.GetComponent<TextMesh>();
        timer = BaseGameMode.Instance.GetComponent<GameTimer>();

        original_scale = gameObject.transform.localScale;
        original_color = text_mesh.color;

        text_mesh.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        BaseGameMode.Instance.MatchCountdownEvent += OnMatchCountdown;
        BaseGameMode.Instance.MatchStartEvent += OnMatchStart;
    }

    public void OnDestroy()
    {
        BaseGameMode.Instance.MatchCountdownEvent -= OnMatchCountdown;
        BaseGameMode.Instance.MatchCountdownEvent -= OnMatchStart;

        timer.TickEvent -= OnTick;
    }

    /// <summary>
    /// Called whenever the game mode countdown starts.
    /// </summary>
    private void OnMatchCountdown(BaseGameMode game_mode)
    {
        BaseGameMode.Instance.MatchCountdownEvent -= OnMatchCountdown;

        gameObject.transform.localScale = original_scale;
        text_mesh.color = original_color;

        time = BaseGameMode.Instance.duration;

        UpdateTime();
    }

    /// <summary>
    /// Called whenever the game mode starts.
    /// </summary>
    private void OnMatchStart(BaseGameMode game_mode)
    {
        BaseGameMode.Instance.MatchCountdownEvent -= OnMatchStart;

        timer.TickEvent += OnTick;

        gameObject.transform.localScale = original_scale;
        text_mesh.color = original_color;

        UpdateTime();
    }

    /// <summary>
    /// Called whenever the timer ticks.
    /// </summary>
    private void OnTick(GameTimer timer)
    {
        time = timer.time;                                              // Snapshot the current time since it may change when the tweening coroutine is executed.

        // Start pulsating below the critical time threshold.

        if (time > critical_time)
        {
            gameObject.transform.localScale = original_scale;
            text_mesh.color = original_color;

            UpdateTime();
        }
        else
        {
            iTween.ValueTo(this.gameObject, iTween.Hash(
                "from", 0f,
                "to", 1f,
                "time", pulse_duration,
                "easeType", pulse_ease,
                "onUpdate", "PulseTimer"));

            if(time <= 0)
            {
                timer.TickEvent -= OnTick;

                iTween.FadeTo(this.gameObject, iTween.Hash(
                    "alpha", 0f,
                    "time", fade_duration,
                    "easeType", fade_ease,
                    "delay", fade_delay));
            }
        }
    }

    /// <summary>
    /// Called whenever the timer is pulsating.
    /// </summary>
    private void PulseTimer(float alpha)
    {
        gameObject.transform.localScale = Vector3.Lerp(original_scale, original_scale * pulse_scale, 1.0f - alpha);
        text_mesh.color = Color.Lerp(original_color, critical_color, alpha);

        UpdateTime();
    }

    /// <summary>
    /// Update the timer value.
    /// </summary>
    private void UpdateTime()
    {
        var minutes = time / 60;
        var seconds = time % 60;

        text_mesh.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Game timer to display.
    /// </summary>
    private GameTimer timer;

    /// <summary>
    /// Time to display.
    /// </summary>
    private int time;

    /// <summary>
    /// Element used to displayer the game time on.
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
