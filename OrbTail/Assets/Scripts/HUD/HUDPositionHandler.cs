using UnityEngine;
using System.Collections;

/// <summary>
/// Script used to move a layer of HUD in front of a camera.
/// </summary>
public class HUDPositionHandler : MonoBehaviour
{
    /// <summary>
    /// Index of the player this component should react to.
    /// </summary>
    public int player_index = 0;

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

    public void Awake ()
    {
        FollowCamera.CameraActivatedEvent += OnCameraActivated;
    }

    public void OnDestroy()
    {
        FollowCamera.CameraActivatedEvent -= OnCameraActivated;
    }

    private void OnCameraActivated(FollowCamera camera)
    {
        if(camera.Owner != null && camera.Owner.player_index == player_index)
        {
            camera_transform = camera.CameraTransform;

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
}
