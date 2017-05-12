using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourRandomiser : MonoBehaviour
{
    static int colour_rand_boost = 0;
    private void Awake()
    {
        Random.InitState((int)Time.time + (colour_rand_boost++));
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 150f, 200f);
    }
}
