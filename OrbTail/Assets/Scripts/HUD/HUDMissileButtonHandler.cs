using UnityEngine;
using System.Collections;

public class HUDMissileButtonHandler : MonoBehaviour {
//     private GameBuilder builder;

    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().enabled = false;
//         builder = GameObject.FindGameObjectWithTag(Tags.Master).GetComponent<GameBuilder>();
//         builder.EventGameBuilt += OnGameBuilt;
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    private void OnGameBuilt(object sender) {
        //Game game = GameObject.FindGameObjectWithTag(Tags.Game).GetComponent<Game>();
//         GameObject player = game.ActivePlayer;
//         player.GetComponent<PowerController>().OnPowerAttachedEvent += EventPowerAttached;
        
/*        builder.EventGameBuilt -= OnGameBuilt;*/
    }

    private void EventPowerAttached(object sender, GameObject ship, Power power) {
        if (power.Group == PowerGroups.Main && power.Name == "Missile") {
            GetComponent<Renderer>().enabled = true;
            power.OnDeactivatedEvent += PowerDestroyed;
        }
    }

    private void PowerDestroyed(object sender) {

        var power = (Power)sender;

        power.OnDeactivatedEvent -= PowerDestroyed;

        if (power.Group == PowerGroups.Main) {
            GetComponent<Renderer>().enabled = false;
        }
    }
}
