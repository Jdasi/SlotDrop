using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Animator animator;
    public BoxCollider box_collider;
    public float time_to_close = 5.0f;
	

	void Update()
    {
        if (animator.IsInTransition(0))
            box_collider.enabled = false;

        if (!animator.IsInTransition(0) && animator.GetBool("open"))
            box_collider.enabled = true;
	}

    
    public void Open()
    {
        animator.SetBool("open", true);

        Invoke("Close", time_to_close);
    }


    public void Close()
    {
        animator.SetBool("open", false);
    }


    public bool IsOpen()
    {
        return animator.GetBool("open");
    }

}
