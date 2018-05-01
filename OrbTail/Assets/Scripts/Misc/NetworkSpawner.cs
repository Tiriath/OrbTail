using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Script used to spawn a networked object on the server and each client.
/// </summary>
public class NetworkSpawner : NetworkBehaviour
{
    /// <summary>
    /// Prefab to spawn.
    /// </summary>
    public GameObject prefab;

    /// <summary>
    /// Whether to automatically destroy the object after spawning.
    /// </summary>
    public bool auto_destroy = true;

    /// <summary>
    /// Called on the server.
    /// </summary>
    public override void OnStartServer()
    {
        GameObject instance = Instantiate(prefab, transform.position, transform.localRotation);

        NetworkServer.Spawn(instance);

        if(auto_destroy)
        {
            Destroy(gameObject);
        }
    }
}
