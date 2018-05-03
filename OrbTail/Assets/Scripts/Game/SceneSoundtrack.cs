using UnityEngine;
using System.Collections;

/// <summary>
/// Script used to handle scene soundtracks.
/// </summary>
public class SceneSoundtrack : MonoBehaviour
{
    /// <summary>
    /// Volume of the sound.
    /// </summary>
    public const float volume = 0.3f;
    
    public const float fadeTime = 1f;
    public const float longFadeTime = 4f;

    //private GameBuilder builder;
    private GameObject battleSound;

    // Use this for initialization
    void Start ()
    {
        //builder = GetComponent<GameBuilder>();
        //builder.EventGameBuilt += OnGameBuilt;
        battleSound = transform.Find("BattleMusic").gameObject;
    }

    private void OnGameBuilt(object sender) {
        iTween.AudioTo(gameObject, iTween.Hash(
            "volume", 0f,
            "time", fadeTime,
            "onComplete", "StartBattleMusic"
            ));
        //game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
        //game.EventEnd += OnMatchEnd;
    }

    private void OnMatchEnd(object sender, GameObject winner, int info) {
        iTween.AudioTo(battleSound, iTween.Hash(
            "volume", 0f,
            "time", longFadeTime
            ));
    }

    private void StartBattleMusic() {
        battleSound.GetComponent<AudioSource>().volume = volume;
        battleSound.GetComponent<AudioSource>().Play();
    }
}
