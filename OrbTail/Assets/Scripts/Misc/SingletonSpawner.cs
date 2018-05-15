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

    /// <summary>
    /// If true any existing instances are destroyed and recreated, if false any existing instance is preserved and no new instance is created.
    /// </summary>
    public bool recreate = true;

    void Start ()
    {
        var existing_instance = GameObject.FindGameObjectWithTag(singleton_prefab.tag);

        if(existing_instance != null && recreate)
        {
            DestroyImmediate(existing_instance);

            existing_instance = null;
        }

        if (existing_instance == null)
        {
            var instance = GameObject.Instantiate(singleton_prefab);

            DontDestroyOnLoad(instance);
        }
    }
}
