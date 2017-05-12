using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanRotator : MonoBehaviour
{
    RectTransform rect_transform;
    public float rotation_speed = 5f;
	// Use this for initialization
	void Start ()
    {
        rect_transform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        rect_transform.Rotate(Vector3.forward * Time.deltaTime * rotation_speed);
    }
}
