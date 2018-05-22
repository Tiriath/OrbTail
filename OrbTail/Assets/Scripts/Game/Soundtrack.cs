using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Script used to play a soundtrack for the current scene.
/// </summary>
public class Soundtrack : MonoBehaviour
{
    public delegate void DelegateSoundtrackEvent(Soundtrack sender);

    public event DelegateSoundtrackEvent StopEvent;

    /// <summary>
    /// Fade in time, in seconds.
    /// </summary>
    public float fade_in = 0.5f;

    /// <summary>
    /// Fade out time, in seconds.
    /// </summary>
    public float fade_out = 0.5f;

    // Use this for initialization
    public void Awake ()
    {
        audio_source = GetComponent<AudioSource>();
        
        DontDestroyOnLoad(gameObject);

        Play();
    }

    /// <summary>
    /// Fade in this soundtrack.
    /// </summary>
    public void Play()
    {
        // Stop every other soundtrack.
        fadeout_counter = 0;

        foreach (var soundtrack in FindObjectsOfType<Soundtrack>())
        {
            if (soundtrack != this)
            {
                soundtrack.StopEvent += OnStopped;
                soundtrack.Stop();

                ++fadeout_counter;
            }
        }

        if(fadeout_counter == 0)
        {
            FadeIn();                   // No other soundtrack playing: fade in right away!
        }
    }

    /// <summary>
    /// Stop this soundtrack.
    /// </summary>
    public void Stop()
    {
        iTween.AudioTo(gameObject, iTween.Hash(
            "volume", 0f,
            "time", fade_out,
            "onComplete", "OnFadeOut"));
    }

    /// <summary>
    /// Fade in this soundtrack.
    /// </summary>
    private void FadeIn()
    {
        iTween.AudioFrom(gameObject, iTween.Hash(
            "volume", 0f,
            "time", fade_in));

        audio_source.Play();
    }

    /// <summary>
    /// Called whenever another soundtrack is stopped.
    /// </summary>
    /// <param name="other"></param>
    private void OnStopped(Soundtrack other)
    {
        --fadeout_counter;

        if (fadeout_counter == 0)
        {
            FadeIn();                   // No other soundtrack playing: fade in right away!
        }
    }

    /// <summary>
    /// Called whenever this soundtrack is completely faded-out.
    /// </summary>
    private void OnFadeOut()
    {
        audio_source.Stop();

        if (StopEvent != null)
        {
            StopEvent(this);
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Audio source component.
    /// </summary>
    private AudioSource audio_source;

    /// <summary>
    /// Number of soundtrack to wait for before starting playing this one.
    /// </summary>
    private int fadeout_counter;
}
