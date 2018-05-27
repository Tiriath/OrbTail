using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GUI element used to display the lobby countdown.
/// </summary>
public class GUILobbyCountdown : MonoBehaviour
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
    /// Time fade duration.
    /// </summary>
    public float fade_duration = 0.3f;

    /// <summary>
    /// Pulse easing curve.
    /// </summary>
    public iTween.EaseType pulse_ease = iTween.EaseType.easeOutCirc;

    public void Awake()
    {
        text_mesh = GetComponent<TextMesh>();

        original_scale = gameObject.transform.localScale;

        LobbyCountdown.LobbyCountdownStartedEvent += OnLobbyCountdownStarted;

        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called whenever the lobby countdown starts.
    /// </summary>
    public void OnLobbyCountdownStarted(LobbyCountdown timer)
    {
        this.timer = timer;

        this.timer.TickEvent += OnLobbyCountdownTick;
        this.timer.TimeOutEvent += OnLobbyCountdownTimeOut;

        gameObject.SetActive(true);

        text_mesh.text = timer.time.ToString();

        iTween.FadeFrom(gameObject, 0.0f, fade_duration);
    }

    /// <summary>
    /// Called whenever the lobby countdown ticks.
    /// </summary>
    /// <param name="timer"></param>
    private void OnLobbyCountdownTick(GameTimer timer)
    {
        time = timer.time;                                              // Snapshot the current time since it may change when the tweening coroutine is executed.

        iTween.ValueTo(this.gameObject, iTween.Hash(
            "from", 0f,
            "to", 1f,
            "time", pulse_duration,
            "easeType", pulse_ease,
            "onUpdate", "PulseTimer"));
    }

    /// <summary>
    /// Called whenever the lobby countdown finishes.
    /// </summary>
    private void OnLobbyCountdownTimeOut(GameTimer timer)
    {
        this.timer.TickEvent -= OnLobbyCountdownTick;
        this.timer.TimeOutEvent -= OnLobbyCountdownTimeOut;
    }

    /// <summary>
    /// Called whenever the timer is pulsating.
    /// </summary>
    private void PulseTimer(float alpha)
    {
        gameObject.transform.localScale = Vector3.Lerp(original_scale, original_scale * pulse_scale, 1.0f - alpha);

        text_mesh.text = timer.time > 0 ? timer.time.ToString() : "";
    }

    /// <summary>
    /// Original element scale.
    /// </summary>
    private Vector3 original_scale;

    /// <summary>
    /// Element used to display the countdown on.
    /// </summary>
    private TextMesh text_mesh;

    /// <summary>
    /// Countdown timer.
    /// </summary>
    private LobbyCountdown timer;

    /// <summary>
    /// Time to display.
    /// </summary>
    private int time;
}
