﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script used to handle GUI input events.
/// </summary>
public class GUIInputHandler : MonoBehaviour
{
    /// <summary>
    /// Layer containing the GUI elements.
    /// </summary>
    public string gui_layer = "MenuButton";

    /// <summary>
    /// Maximum time between two consecutive interactions that register as a single "double tap".
    /// </summary>
    public float double_tap_time = 2.0f;

    /// <summary>
    /// Camera owning this input handler. If none is specified the main camera will be used instead.
    /// </summary>
    public Camera OwningCamera { get; set; }

    /// <summary>
    /// Get or set the layer the GUI elements belong to.
    /// </summary>
    public int GUILayer
    {
        get
        {
            return gui_layer_bitmask;
        }
        set
        {
            gui_layer_bitmask = 1 << value;

            gui_layer = LayerMask.LayerToName(value);
        }
    }

    public virtual void Awake()
    {
        Debug.Log("GUIInputHandler: " + gameObject.name);

        GUILayer = LayerMask.NameToLayer(gui_layer);
    }

    public virtual void Update ()
    {
        if(Input.touchCount > 0)
        {
            HandleInput(Input.touches[0].position, Input.touches[0].phase);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            HandleInput(Input.mousePosition, TouchPhase.Began);
        }
        else if (Input.GetMouseButton(0))
        {
            HandleInput(Input.mousePosition, TouchPhase.Moved);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            HandleInput(Input.mousePosition, TouchPhase.Ended);
        }
    }
    
    /// <summary>
    /// Handle a single input.
    /// </summary>
    private void HandleInput(Vector2 position, TouchPhase touch_phase)
    {
        var camera = OwningCamera ? OwningCamera : Camera.main;

        if(camera)
        {
            RaycastHit raycast_hit;

            bool hit = Physics.Raycast(camera.ScreenPointToRay(position), out raycast_hit, Mathf.Infinity, gui_layer_bitmask);

            var hit_button = hit ? raycast_hit.collider.gameObject : null;

            // Handle inputs.

            if (touch_phase == TouchPhase.Began)
            {
                interacting_button = hit_button;

                InputEnter();

            }
            else if (touch_phase == TouchPhase.Ended)
            {
                if (hit_button == interacting_button)
                {
                    InputConfirm();
                }

                InputLeave();

                timestamp = Time.realtimeSinceStartup;
            }
            else if (touch_phase == TouchPhase.Canceled)
            {
                InputLeave();
            }
        }
    }

    /// <summary>
    /// Start interacting with the current button.
    /// </summary>
    private void InputEnter()
    {
        if (interacting_button)
        {
            foreach (var behaviour in interacting_button.GetComponents<GUIElement>())
            {
                behaviour.OnInputEnter();
            }
        }
    }

    /// <summary>
    /// Confirm the input on the current button.
    /// </summary>
    private void InputConfirm()
    {
        if (interacting_button)
        {
            foreach (var behaviour in interacting_button.GetComponents<GUIElement>())
            {
                if (!behaviour.double_tap || (Time.time - timestamp < double_tap_time))
                {
                    behaviour.OnInputConfirm();
                }
            }
        }
    }

    /// <summary>
    /// End interacting with the current button.
    /// </summary>
    private void InputLeave()
    {
        if (interacting_button)
        {
            foreach (var behaviour in interacting_button.GetComponents<GUIElement>())
            {
                behaviour.OnInputLeave();
            }
        }
    }

    /// <summary>
    /// Button the input is currently interacting with.
    /// </summary>
    private GameObject interacting_button = null;

    /// <summary>
    /// Last time an input was confirmed.
    /// </summary>
    private float timestamp = 0.0f;

    /// <summary>
    /// Bitmask of the GUI layer.
    /// </summary>
    private int gui_layer_bitmask = 0;
}
