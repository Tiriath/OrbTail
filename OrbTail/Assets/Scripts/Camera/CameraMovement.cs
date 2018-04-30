using UnityEngine;

/// <summary>
/// Script used to handle camera movement.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    /// <summary>
    /// Distance of the camera from the target.
    /// </summary>
    public float target_boom_length = 10.0f;

    /// <summary>
    /// Get or set the lobby player owning this camera.
    /// </summary>
    public LobbyPlayer Owner
    {
        get
        {
            return lobby_player;
        }
        set
        {
            lobby_player = value;

            // #TODO Set the proper viewport for the player according to the number of local players in the lobby.
        }
    }

    /// <summary>
    /// Set a new target actor for the camera.
    /// </summary>
    /// <param name="view_target">Object to look at.</param>
    public GameObject ViewTarget
    {
        get
        {
            return gameObject.transform.parent.gameObject;
        }
        set
        {
            transform.SetParent(value.transform);

            player = value.transform;
        }
    }

    public void Start()
    {
        gravity_field = GameObject.FindGameObjectWithTag(Tags.Arena).GetComponent<GravityField>();

        camera_component = GetComponentInChildren<Camera>();

        currentDistanceSmooth = distanceSmooth;
    }

    public void FixedUpdate ()
    {
        // The standard position of the camera is the relative position of the camera from the player.
        //Vector3 standardPos = player.position + relCameraPos;
        Vector3 arenaDown = gravity_field.GetGravityAt(camera_component.transform.position);

        Vector3 standardPos = player.position - player.forward * relDistancePos + relHighPos * -arenaDown;

        if (Physics.Raycast(standardPos, player.position - standardPos, (player.position - standardPos).magnitude, Layers.Obstacles))
        {
            standardPos = player.position + player.forward * relDistancePos / 4f + relHighPos * 5f * -arenaDown;
        }

        newPos = standardPos;
        // Lerp the camera's position between it's current position and it's new position.
        float newDistanceSmooth = distanceSmooth;
        if (Vector3.Dot(player.transform.forward, player.GetComponent<Rigidbody>().velocity) < 0)
        {
            newDistanceSmooth *= 10;
        }

        currentDistanceSmooth = Mathf.Lerp(currentDistanceSmooth, newDistanceSmooth, 0.3f * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, newPos, currentDistanceSmooth * Time.deltaTime);


        // Make sure the camera is looking at the player.
        SmoothLookAt(arenaDown);
    }
    
    void SmoothLookAt (Vector3 arenaDown)
    {
        // Create a vector from the camera towards the player.
        Vector3 relPlayerPosition = player.position + player.transform.up * targetUpOffset - transform.position;

        // Create a rotation based on the relative position of the player being the forward vector.

        Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, -arenaDown);
        
        // Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, rotationSmooth * Time.deltaTime);
    }

    public float distanceSmooth = 10f;	// The relative speed at which the camera will catch up.
    public float rotationSmooth = 10f;
    public float relDistancePos = 7f;
    public float relHighPos = 2.2f;
    public float targetUpOffset = 0f;

    private float currentDistanceSmooth;

    private Transform player;           // Reference to the player's transform.
    private Vector3 newPos;             // The position the camera is trying to reach.

    /// <summary>
    /// Arena source of gravity.
    /// </summary>
    private GravityField gravity_field;

    /// <summary>
    /// Camera component controlled by this boom.
    /// </summary>
    private Camera camera_component;

    /// <summary>
    /// Player owning this camera.
    /// </summary>
    LobbyPlayer lobby_player;
}
