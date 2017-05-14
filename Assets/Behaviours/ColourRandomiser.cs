using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourRandomiser : MonoBehaviour
{
    static int _colourRandBoost = 0;

    private void Awake()
    {
        Random.InitState((int)Time.time + (_colourRandBoost++));
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

}
