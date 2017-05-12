using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    private Player player_input;
    private int id = 0;
    public bool BaseWeapons = true;
    public bool slot_dropping = false;
    public float move_speed = 20;

    public List<GameObject> mount_points = new List<GameObject>();

    public Animator animator;
    private bool flipped = false;
    private Vector3 scale;

    public float horizontal;
    public float vertical;

    private Transform nearby_slot;


    void Awake()
    {
        scale = transform.localScale;
    }


    void Update()
    {
        if (player_input != null)
        {
            if (!slot_dropping)
            {
                HandleMovement();
                HandleSlotDrop();
            }
        } else {
            Debug.LogWarning("Player is starting in the scene! Please remove");
        }
    }


    void HandleMovement()
    {
        horizontal = player_input.GetAxis("Horizontal") * Time.deltaTime * move_speed;
        vertical = player_input.GetAxis("Vertical") * Time.deltaTime * move_speed;

        transform.position += new Vector3(horizontal, 0, vertical);

        animator.SetBool("walking", horizontal != 0 || vertical != 0);

        if (horizontal < 0 && !flipped)
        {
            flipped = true;
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }

        if (horizontal > 0 && flipped)
        {
            flipped = false;
            transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        }
    }


    void HandleSlotDrop()
    {
        if (player_input.GetButtonDown("SlotDrop"))
        {
            animator.SetTrigger("slot_drop");
            slot_dropping = true;

            Invoke("DisableSlotDropping", 0.85f);

            if (nearby_slot != null)
            {
                if (!nearby_slot.GetComponent<BoxCollider>().enabled)
                {
                    nearby_slot = null;
                } else {
                    transform.position = new Vector3(nearby_slot.position.x, transform.position.y, nearby_slot.position.z);
                }
            }
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Slot")
        {
            nearby_slot = other.transform;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Slot")
        {
            nearby_slot = null;
        }
    }


    void SlotDropped()
    {
        slot_dropping = false;

        if (nearby_slot != null)
            nearby_slot.GetComponent<Slot>().Close();
    }


    public void SetPlayerInput(Player player_input)
    {
        this.player_input = player_input;
    }


    public void SetPlayerID(int id)
    {
        this.id = id;
    }


    public int GetPlayerID()
    {
        return id;
    }

}
