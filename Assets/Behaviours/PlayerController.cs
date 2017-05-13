using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public GameObject body_parts;
    public bool BaseWeapons = true;
    public bool slot_dropping = false;
    public float horizontal_move_speed = 20;
    public float vertical_move_speed = 10;
    public Animator animator;
    public PlayerHUD player_HUD;

    public List<GameObject> mount_points = new List<GameObject>();

    private int id = 0;
    private bool flipped = false;
    private Vector3 last_direction;
    private Vector3 body_parts_default_scale;
    private Player player_input;
    private Transform nearby_slot;

    public int max_player_health = 100;
    private int player_health = 100;
    public int slot_streak = 0;

    public Ability basic_ability;
    public Ability special_ability;


    void Awake()
    {
        body_parts_default_scale = body_parts.transform.localScale;
        player_HUD.SetHealthBarMaxHealth(max_player_health);
        player_health = max_player_health;
        player_HUD.UpdateHealthBar(player_health);

        last_direction = Vector3.right;
    }


    void Update()
    {
        if (player_input != null)
        {
            if(player_input.GetButtonDown("Disconnect"))
            {
                DisconnectPlayer();
            }

            if (!slot_dropping)
            {
                HandleMovement();
                HandleAttack();
                HandleSlotDrop();
            }
        } else {
            Debug.LogWarning("Player is starting in the scene! Please remove");
        }
    }


    void HandleMovement()
    {
        float horizontal = player_input.GetAxis("Horizontal") * Time.deltaTime * horizontal_move_speed;
        float vertical = player_input.GetAxis("Vertical") * Time.deltaTime * vertical_move_speed;

        // Apply the move and store the last facing direction.
        Vector3 move = new Vector3(horizontal, 0, vertical);
        transform.position += move;

        if (move != Vector3.zero)
            last_direction = move.normalized;

        animator.SetBool("walking", horizontal != 0 || vertical != 0);

        if (horizontal < 0 && !flipped)
        {
            flipped = true;
            body_parts.transform.localScale = new Vector3(-body_parts_default_scale.x, body_parts_default_scale.y, body_parts_default_scale.z);
        }

        if (horizontal > 0 && flipped)
        {
            flipped = false;
            body_parts.transform.localScale = new Vector3(body_parts_default_scale.x, body_parts_default_scale.y, body_parts_default_scale.z);
        }
    }


    void HandleAttack()
    {
        if (player_input.GetButtonDown("Attack"))
        {
            if (basic_ability != null)
                basic_ability.Fire();
        }
    }


    void HandleSlotDrop()
    {
        if (player_input.GetButtonDown("SlotDrop"))
        {
            animator.SetTrigger("slot_drop");
            slot_dropping = true;

            Invoke("FireSpecial", 0.6f);
            Invoke("SlotDropped", 0.85f);

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


    void FireSpecial()
    {
        // JTODO -- probably need an alive check here due to Invoke delay..

        if (special_ability != null)
            special_ability.Fire();
    }


    void SlotDropped()
    {
        // JTODO -- probably need an alive check here due to Invoke delay..

        slot_dropping = false;

        if (nearby_slot != null)
        {
            nearby_slot.GetComponent<Slot>().SlotDrop(this);
            slot_streak = player_HUD.AddSlotToken();
        }
    }


    public void Damage(int damage_amount)
    {
        player_health -= damage_amount;
        player_HUD.UpdateHealthBar(player_health);

        if (player_health <= 0)
        {
            KillPlayer();
        }
    }


    public Player GetRewiredPlayer()
    {
        return player_input;
    }


    private void DisconnectPlayer()
    {
        GameObject.FindGameObjectWithTag("Managers").GetComponent<ControllerManager>().RemovePlayer(player_input.id);
    }


    public void KillPlayer()
    {
        // JTODO: -- reset and respawn player here
    }


    public void SetPlayerInput(Player player_input)
    {
        this.player_input = player_input;
        this.id = player_input.id;
    }


    public int GetPlayerID()
    {
        return id;
    }
<<<<<<< HEAD
=======


    public Vector3 GetLastDirection()
    {
        return last_direction;
    }

>>>>>>> origin/master
}
