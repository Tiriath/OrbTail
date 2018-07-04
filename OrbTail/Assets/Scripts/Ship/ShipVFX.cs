using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component used to handle the physical aspect of a ship.
/// </summary>
public class ShipVFX : MonoBehaviour
{
    /// <summary>
    /// Liveries.
    /// </summary>
    public TextureField[] liveries;

    /// <summary>
    /// Set the livery color.
    /// </summary>
    public void SetLivery(Color color, int livery_index)
    {
        var material = GetComponentInChildren<MeshRenderer>().material;

        material.SetColor("_Color", color);
        material.SetFloat("_Desaturate", 0.0f);

        if (liveries.Length > livery_index)
        {
            string livery_path = liveries[livery_index];

            var livery_texture = Resources.Load<Texture>(livery_path);

            material.SetTexture("_Diffuse", livery_texture);
        }
    }
}
