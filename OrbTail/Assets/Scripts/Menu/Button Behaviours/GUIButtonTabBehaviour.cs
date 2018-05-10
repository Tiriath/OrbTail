using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Button used to cycle multiple tabs when the button is pressed.
/// </summary>
public class GUIButtonTabBehaviour : GUIElement
{
    /// <summary>
    /// Name of the tabs to cycle through.
    /// </summary>
    public string[] tabs;

    public void Awake()
    {
        // Kinda inefficient, but we are dealing with a handful of elements.

        tab_objects = new Queue<GameObject>();

        foreach (var tab_name in tabs)
        {
            foreach (Transform child in transform.parent)
            {
                if(child.gameObject.name == tab_name)
                {
                    child.gameObject.SetActive(false);

                    tab_objects.Enqueue(child.gameObject);
                }
            }
        }

        // Enable the first tab and hide everything else.

        tab_objects.Peek().SetActive(true);
    }

    public override void OnInputConfirm()
    {
        var old_tab = tab_objects.Dequeue();

        tab_objects.Enqueue(old_tab);

        old_tab.SetActive(false);

        tab_objects.Peek().SetActive(true);
    }

    /// <summary>
    /// List of tab objects to cycle through.
    /// </summary>
    public Queue<GameObject> tab_objects;
}
