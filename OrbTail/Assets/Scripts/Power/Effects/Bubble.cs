using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour
{
    void Update ()
    {
        // Too heavy
        // float bubblingFactor = Mathf.Cos(Time.time * 2.0f);
        this.transform.localScale = Vector3.one * 2f;
    }
}
