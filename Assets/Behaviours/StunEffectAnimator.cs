using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffectAnimator : MonoBehaviour
{
    public float rotation_speed = -50;


	void Update ()
    {
		transform.Rotate(Vector3.up * Time.deltaTime * rotation_speed);
    }
}
