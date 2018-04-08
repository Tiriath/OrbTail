using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GUIMatchmaking : GUIMenuChoose {

    private GameObject ready_button;
    private GameObject not_ready_button;
    private NetworkPlayerBuilder network_builder;
    private GameObject error_message;

    private IList<GameObject> player_icons;

    private const float kReadyScale = 1.33f;
    private const float kScalingTime = 0.5f;
    private Vector3 kLocalScale = 0.15f * Vector3.one;
    private static Color kDisabledColor = Color.grey;

    // Use this for initialization
    public override void Start ()
    {
        base.Start();

        //The icons (disabled by default)
        player_icons = (from icon in GameObject.FindGameObjectsWithTag(Tags.ShipSelector)
                        orderby icon.transform.position.x
                        select icon).ToList();
                               
        foreach (GameObject icon in player_icons)
        {

            icon.transform.localScale = Vector3.zero;

        }

        ready_button = GameObject.Find("ReadyButton");
        not_ready_button = GameObject.Find("NotReadyButton");
        error_message = GameObject.Find("ErrorMessage");

        error_message.SetActive(false);

        ready_button.SetActive(false);
        not_ready_button.SetActive(false);

        //Listens to proper events

        network_builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>().NetworkBuilder;

        network_builder.EventIdAcquired += OnEventIdAcquired;
        network_builder.EventPlayerRegistered += OnEventPlayerRegistered;
        network_builder.EventPlayerUnregistered += OnEventPlayerUnregistered;
        network_builder.EventPlayerReady += OnEventPlayerReady;
        network_builder.EventNoGame += OnEventNoGame;
        network_builder.EventErrorOccurred += OnEventErrorOccurred;
        network_builder.EventDisconnected += OnEventDisconnected;

        network_builder.EventErrorOccurred += OnEventErrorOccurred;
        network_builder.EventNoGame += OnEventNoGame;
        
    }

    void OnEventDisconnected(object sender, string message)
    {

        error_message.GetComponent<TextMesh>().text = "disconnected";
        error_message.SetActive(true);

    }

    void OnEventErrorOccurred(object sender, string message)
    {

        error_message.GetComponent<TextMesh>().text = "error";
        error_message.SetActive(true);

    }

    void OnEventNoGame(object sender)
    {

        error_message.GetComponent<TextMesh>().text = "no game";
        error_message.SetActive(true);

    }

    void OnEventPlayerReady(object sender, int id, bool value)
    {

        //Toggle the ready button
        if (network_builder.Id.HasValue &&
            id == network_builder.Id.Value )
        {
            
            ready_button.SetActive( !value );
            not_ready_button.SetActive( value );
            
        }

        //Update the interface showing ready players
        var icon = player_icons[id];

        icon.GetComponent<SmoothAnimation>().Color = value ? Color.white : kDisabledColor;
        icon.GetComponent<SmoothAnimation>().Scale = value ? kLocalScale * kReadyScale : kLocalScale;

    }

    void OnEventPlayerUnregistered(object sender, int id)
    {

        var icon = player_icons[id];

        icon.GetComponent<SmoothAnimation>().Color = kDisabledColor;
        icon.GetComponent<SmoothAnimation>().Scale = Vector3.zero;

    }

    void OnEventPlayerRegistered(object sender, int id, string name)
    {

        var icon = player_icons[id];

        var icon_resource = Resources.Load<Sprite>("ShipIcons/" + name);

        icon.GetComponent<SpriteRenderer>().sprite = icon_resource;

        icon.GetComponent<Renderer>().material.color = kDisabledColor;
        icon.transform.localScale = Vector3.zero;

        icon.GetComponent<SmoothAnimation>().Color = kDisabledColor;
        icon.GetComponent<SmoothAnimation>().Scale = kLocalScale;

    }

    void OnEventIdAcquired(object sender, int id)
    {

        ready_button.SetActive(true);
        not_ready_button.SetActive(false);

    }


    void OnDestroy()
    {
        
        network_builder.EventIdAcquired -= OnEventIdAcquired;
        network_builder.EventPlayerRegistered -= OnEventPlayerRegistered;
        network_builder.EventPlayerUnregistered -= OnEventPlayerUnregistered;
        network_builder.EventPlayerReady -= OnEventPlayerReady;
        network_builder.EventNoGame -= OnEventNoGame;
        network_builder.EventErrorOccurred -= OnEventErrorOccurred;
        network_builder.EventDisconnected -= OnEventDisconnected;
        
    }


    protected override void OnSelect (GameObject target)
    {
        base.OnSelect(target);

        if (target == ready_button)
        {
            
            network_builder.SetReady(true);
            
        }
        
        if (target == not_ready_button)
        {
            
            network_builder.SetReady(false);
            
        }

        if (target.tag == Tags.BackButton) {

            GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>().Restore();

            SceneManager.LoadScene("MenuChooseShip");

        }
    }


}
