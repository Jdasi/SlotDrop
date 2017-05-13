using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Animator animator;
    public BoxCollider box_collider;
    public float time_to_close = 5.0f;
    public float unprotected_chance = 30.0f;
    public bool golden_slot = false;

    private bool unprotected = false;
    private AbilityFactory ability_factory;


    void Start()
    {
        ability_factory = GameObject.FindObjectOfType<AbilityFactory>();

        unprotected = Random.Range(1, 100) < unprotected_chance;

        if (transform.position.y != 0.01f)
        {
            transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
        }
    }


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


    public bool IsUnprotected()
    {
        return unprotected;
    }

    
    public void SlotDrop(PlayerController player)
    {
        // Close the slot.
        Close();

        // JTODO -- Assign new loadout here.
    }

}
