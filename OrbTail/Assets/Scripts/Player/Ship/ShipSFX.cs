using UnityEngine;
using System.Collections;

/// <summary>
/// Handles ship sound effects.
/// </summary>
public class ShipSFX : MonoBehaviour
{
    /// <summary>
    /// SFX when the ship crashes into something.
    /// </summary>
    public AudioClip sfx_crash;

    /// <summary>
    /// SFX when something is collected.
    /// </summary>
    public AudioClip sfx_gather;

    /// <summary>
    /// Minimum magnitude for an impact.
    /// </summary>
    public float crash_min_magnitude = 10.0f;

    /// <summary>
    /// Maximum magnitude for an impact.
    /// </summary>
    public float crash_max_magnitude = 20.0f;

    /// <summary>
    /// Minimum pitch for the engine (no throttle).
    /// </summary>
    public float engine_min_pitch = 2.0f;

    /// <summary>
    /// Maximum pitch for the engine (full throttle).
    /// </summary>
    public float engine_max_pitch = 4.0f;

    public void Awake ()
    {
        audio_source = GetComponent<AudioSource>();

        movement_controller = GetComponent<MovementController>();

        engine_pitch = audio_source.pitch;

        GetComponent<Ship>().OrbAttachedEvent += OnOrbAttached;
    }

    public void OnDestroy()
    {
        GetComponent<Ship>().OrbAttachedEvent -= OnOrbAttached;
    }

    public void Update ()
    {
        if(audio_source != null)
        {
            var engine = movement_controller.EngineDriver.Top();

            audio_source.pitch = Mathf.Abs(engine.Input) * (engine_max_pitch - engine_min_pitch) + engine_min_pitch;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(sfx_crash)
        {
            var crash_magnitude = collision.impulse.magnitude;

            if (collision.collider.gameObject.tag != Tags.Orb && crash_magnitude > crash_min_magnitude)
            {
                var sfx_volume = (crash_magnitude - crash_min_magnitude) * (crash_max_magnitude - crash_min_magnitude);

                AudioSource.PlayClipAtPoint(sfx_crash, collision.transform.position, sfx_volume);
            }
        }
    }

    private void OnOrbAttached(Ship ship, GameObject orb)
    {
        if(sfx_gather)
        {
            AudioSource.PlayClipAtPoint(sfx_gather, transform.position);
        }
    }

    /// <summary>
    /// Movement controller.
    /// </summary>
    private MovementController movement_controller;

    /// <summary>
    /// Ship audio source.
    /// </summary>
    private AudioSource audio_source;

    /// <summary>
    /// Default engine pitch.
    /// </summary>
    private float engine_pitch;
}
