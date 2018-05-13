using UnityEngine;
using System.Collections;

/// <summary>
/// Script used to handle the HUD of a player.
/// </summary>
public class HUDHandler : MonoBehaviour
{
    /// <summary>
    /// Movement smoothing factor.
    /// </summary>
    public float movement_smooth = 30f;

    /// <summary>
    ///  Rotation smoothing factor.
    /// </summary>
    public float rotation_smooth = 30f;

    /// <summary>
    /// Distance of the object from the camera.
    /// </summary>
    public float distance = 10.7f;

    /// <summary>
    /// Set the owner of this HUD.
    /// </summary>
    public GameObject Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;

            foreach(var hud_element in GetComponentsInChildren<HUDElement>(true))
            {
                hud_element.Owner = owner;
            }
        }
    }

    /// <summary>
    /// Set the camera this HUD component is attached to.
    /// </summary>
    public Camera Camera
    {
        set
        {
            camera_transform = value.transform;

            transform.position = camera_transform.position;
            transform.rotation = camera_transform.rotation;
        }
    }

    void FixedUpdate ()
    {
        if(camera_transform != null)
        {
            transform.position = Vector3.Lerp(transform.position, camera_transform.position + camera_transform.forward * distance, Time.fixedDeltaTime * movement_smooth);
            transform.rotation = Quaternion.Lerp(transform.rotation, camera_transform.rotation, Time.fixedDeltaTime * rotation_smooth);
        }
    }

    /// <summary>
    /// Transform of the camera this component is bound to.
    /// </summary>
    private Transform camera_transform;

    /// <summary>
    /// Owner of this HUD.
    /// </summary>
    private GameObject owner;
}
