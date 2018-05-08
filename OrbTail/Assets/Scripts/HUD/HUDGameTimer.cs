using UnityEngine;
using System.Collections;

/// <summary>
/// HUD element used to show the current game time.
/// </summary>
public class HUDGameTimer : MonoBehaviour
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

    public void Awake()
    {
        original_scale = gameObject.transform.localScale;

        text_mesh = gameObject.GetComponent<TextMesh>();

        original_color = text_mesh.color;

        timer = BaseGameMode.Instance.GetComponent<GameTimer>();

        FindObjectOfType<GameTimer>().TickEvent += OnTick;
    }

    public void OnDestroy()
    {
        timer.TickEvent -= OnTick;
    }

    /// <summary>
    /// Called whenever the timer ticks.
    /// </summary>
    private void OnTick(GameTimer timer)
    {
        // Start pulsating below the critical time threshold.

        if(timer.time > critical_time)
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
                "easeType", iTween.EaseType.easeOutCirc,
                "onUpdate", "PulseTimer"));
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
        var minutes = timer.time / 60;
        var seconds = timer.time % 60;

        text_mesh.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Game timer to display.
    /// </summary>
    private GameTimer timer;

    /// <summary>
    /// Element used to displayer the game time.
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
