using UnityEngine;
using System.Collections.Generic;

public class HUDTutorialHandler : MonoBehaviour
{
    protected void OnSelect (GameObject target)
    {
        if (target.tag == Tags.MenuSelector)
        {
            if (target.name == "Dismiss")
            {
                /*builder.PlayerReady ();*/
                Destroy (gameObject);
            }
            else if (target.name == "PowerUpButton")
            {
                ActivateTab ("PowerTab");
            }
            else if (target.name == "GameModeButton")
            {
                ActivateTab ("GameModeTab");
            }
        }
    }

    private void Awake()
    {
        var game_mode = FindObjectOfType<BaseGameMode>();

        if(game_mode.tutorial != null)
        {
            var tutorial = Instantiate(game_mode.tutorial);

            tutorial.transform.SetParent(transform);

            tutorial.transform.localPosition = Vector3.zero;
            tutorial.transform.localRotation = Quaternion.identity;

            foreach(Transform child in tutorial.transform)
            {
                tabs.Add(child.gameObject);
            }

            ActivateTab("GameModeTab");
        }
    }

    /// <summary>
    /// Activate a tutorial tab by name.
    /// </summary>
    /// <param name="tab_name">Name of the tab to activate.</param>
    private void ActivateTab (string tab_name)
    {
        foreach (var tab in tabs)
        {
            tab.SetActive (tab.name == tab_name);
        }
    }

    /// <summary>
    /// Tabs in this tutorial.
    /// </summary>
    private List<GameObject> tabs = new List<GameObject>();
}
