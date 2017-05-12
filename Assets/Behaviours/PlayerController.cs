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
    public float move_speed = 20;

    public List<GameObject> mount_points = new List<GameObject>();

    public Animator player_animator;
    private bool flipped = false;
    private Vector3 scale;

    private void Awake()
    {
        //player_input = ReInput.players.GetPlayer(id);
        scale = transform.localScale;
    }


    void Start()
    {
    }


    void Update()
    {
        HandleMovement();
    }


    void FixedUpdate()
    {
    }

    void HandleMovement()
    {


        float horizontal = player_input.GetAxis("Horizontal") * Time.deltaTime * move_speed;
        float vertical = player_input.GetAxis("Vertical") * Time.deltaTime * move_speed;

        transform.position += new Vector3(horizontal, 0, vertical);

        player_animator.SetBool("walking", horizontal != 0 || vertical != 0);

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

        if(player_input.GetButtonDown("SlotDrop"))
        {
            player_animator.SetTrigger("slot_drop");
        }
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

    
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "PowerUp")
        {
        }
    }
}