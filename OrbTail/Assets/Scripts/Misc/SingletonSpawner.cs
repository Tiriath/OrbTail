using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used to spawn a singleton that is preserved across levels.
/// </summary>
public class SingletonSpawner : MonoBehaviour
{
    /// <summary>
    /// Prefab of the singleton to spawn.
    /// </summary>
    public GameObject singleton_prefab;

    void Start ()
    {
        if(GameObject.FindGameObjectWithTag(singleton_prefab.tag) == null)
        {
            var instance = GameObject.Instantiate(singleton_prefab);

            DontDestroyOnLoad(instance);
        }
    }
}
