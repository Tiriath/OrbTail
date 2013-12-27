﻿using UnityEngine;
using System.Collections;
using System.Diagnostics;

/// <summary>
/// Represents a game identity, this script shold be attached to every ship
/// </summary>
public class GameIdentity : MonoBehaviour {
    
    /// <summary>
    /// The ordinal number of the player
    /// </summary>
    public int Id;

    public Color Color
    {
        get
        {

            switch (Id)
            {
                case 0:

                    return Color.red;
                    
                case 1:

                    return Color.blue;

                case 2:

                    return Color.green;

                default:

                    return Color.yellow;

            }

        }
    }

    /// <summary>
    /// Is the player local?
    /// </summary>
    public bool IsLocal
    {
        get
        {
            return NetworkHelper.IsOwnerSide(gameObject.networkView);
        }
    }

    /// <summary>
    /// The score of the player
    /// </summary>
    public int Score;

    /// <summary>
    /// Returns the tail length
    /// </summary>
    public int TailLength
    {

        get
        {

            return tail_.GetOrbCount();

        }

    }

    void Start()
    {

        tail_ = GetComponent<Tail>();

    }

    /// <summary>
    /// The tail controller
    /// </summary>
    private Tail tail_;

    [RPC]
    public void RPCSetId(int id)
    {

        Id = id;

    }

}
