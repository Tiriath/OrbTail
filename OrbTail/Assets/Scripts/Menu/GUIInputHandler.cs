using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script used to handle GUI input events.
/// </summary>
public class GUIInputHandler : MonoBehaviour
{
    public virtual void Update ()
    {
        if(Input.touchCount > 0)
        {
            HandleInput(Input.touches[0].position, Input.touches[0].phase == TouchPhase.Ended);
        }
        else if(Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            HandleInput(Input.mousePosition, Input.GetMouseButtonUp(0));
        }
    }

    /// <summary>
    /// Handle a single input.
    /// </summary>
    private void HandleInput(Vector2 position, bool confirm_input)
    {
        var input_ray = Camera.main.ScreenPointToRay(position);

        RaycastHit raycast_hit;

        bool hit = Physics.Raycast(input_ray, out raycast_hit, Mathf.Infinity, Layers.MenuButton);

        // Check whether the input stopped interacting with the previous button it was interacting with.

        var previous_button = interacting_button;

        if (!hit)
        {
            interacting_button = null;
        }
        else
        {
            interacting_button = raycast_hit.collider.gameObject;

            // A new button is being interacted with.

            if (previous_button != interacting_button)
            {
                foreach (var behaviour in interacting_button.GetComponents<GUIButtonBehaviour>())
                {
                    behaviour.OnInputInteract();
                }
            }

            // The button was confirmed.

            if (confirm_input)
            {
                foreach (var behaviour in interacting_button.GetComponents<GUIButtonBehaviour>())
                {
                    behaviour.OnInputConfirm();
                }

                interacting_button = null;
            }
        }

        // Leave any previous button if changed.

        if (previous_button != interacting_button && previous_button != null)
        {
            foreach (var behaviour in previous_button.GetComponents<GUIButtonBehaviour>())
            {
                behaviour.OnInputLeave();
            }
        }
    }

    /// <summary>
    /// Last button the input was interacting with.
    /// </summary>
    private GameObject interacting_button = null;
}
