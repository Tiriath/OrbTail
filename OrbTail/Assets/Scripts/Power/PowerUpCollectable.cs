using System;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Represents a power-up on a map any ship can collect upon collision.
/// </summary>
public class PowerUpCollectable : MonoBehaviour
{
    public System.Type type;

    /// <summary>
    /// Spinning speed in degrees per second for each axis
    /// </summary>
    public Vector3 spin_speed = new Vector3(180.0f, 180.0f, 180.0f);

    /// <summary>
    /// Time needed to reactivate this power.
    /// </summary>
    public float cooldown = 5.0f;

    /// <summary>
    /// Fade duration, used when activating and deactivating the power.
    /// </summary>
    public float fade_duration = 0.5f;

    /// <summary>
    /// Fade easing type.
    /// </summary>
    public iTween.EaseType fade_easing = iTween.EaseType.easeInBounce;

    /// <summary>
    /// Activate or deactivate this power.
    /// </summary>
    public bool IsActive { get; private set; }

    public void Awake()
    {
        IsActive = true;

        original_scale = gameObject.transform.localScale;
    }

    public void Update()
    {
        gameObject.transform.localRotation *= Quaternion.Euler(spin_speed * Time.deltaTime);
    }

    /// <summary>
    /// Collect this power and temporarily disable it.
    /// </summary>
    /// <returns>Returns the collected power up prefab.</returns>
    public GameObject Collect()
    {
        if(IsActive)
        {
            IsActive = false;

            iTween.ValueTo(this.gameObject, iTween.Hash(
                "from", 0.0f,
                "to", 1.0f,
                "time", fade_duration,
                "easetype", fade_easing,
                "onUpdate", "OnScaleChanged",
                "onComplete", "OnDeactivated"));

            // Pick a random powerup from the game mode.

            var power_ups = BaseGameMode.Instance.power_ups;

            if(power_ups.Length > 0)
            {
                return power_ups[UnityEngine.Random.Range(0, power_ups.Length)];
            }
        }

        return null;
    }

    /// <summary>
    /// Called when the power is deactivated.
    /// </summary>
    private void OnDeactivated()
    {
        GetComponent<Renderer>().enabled = false;

        iTween.ValueTo(this.gameObject, iTween.Hash(
                "from", 1.0f,
                "to", 0.0f,
                "time", fade_duration,
                "easetype", fade_easing,
                "delay", cooldown,
                "onStart", "OnStartActivating",
                "onUpdate", "OnScaleChanged",
                "onComplete", "OnActivated"));
    }

    /// <summary>
    /// Called whenever the power is starting being activated.
    /// </summary>
    private void OnStartActivating()
    {
        GetComponent<Renderer>().enabled = true;
    }

    /// <summary>
    /// Called when the power is activated.
    /// </summary>
    private void OnActivated()
    {
        IsActive = true;
    }

    /// <summary>
    /// Called when the power is being activated or deactivated.
    /// </summary>
    private void OnScaleChanged(float scale_alpha)
    {
        gameObject.transform.localScale = Vector3.Lerp(original_scale, Vector3.one * 0.01f, scale_alpha);
    }

    /// <summary>
    /// Original scale of this object.
    /// </summary>
    private Vector3 original_scale;
}
