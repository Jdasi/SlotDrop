using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public GameObject body_parts;
    public GameObject stun_effect;
    public bool BaseWeapons = true;
    public bool slot_dropping = false;
    public float horizontal_move_speed = 15;
    public float vertical_move_speed = 15;
    public Animator animator;
    public PlayerHUD player_HUD;
    public List<GameObject> mount_points = new List<GameObject>();
    public SpriteRenderer hat;
    public GameObject damage_flash;
    public Renderer body_renderer;
    public float damage_flash_duration = 0.5f;
    public string loadout_name;
    public float snap_distance = 2.0f;
    public GameObject face_indicator;
    public Vector3 titan_size;

    private int id = 0;
    private bool flipped = false;
    private Vector3 last_facing;
    private Vector3 move = Vector2.zero; 
    private Vector3 body_parts_default_scale;
    private Player player_input;
    private Slot nearby_slot;
    private Vector3 slot_attempt_pos;
    private Rigidbody player_rigidbody;
    private bool spawning = true;
    private bool controls_disabled = true;
    private bool face_locked = false;

    public int max_player_health = 100;
    private int player_health = 100;
    public int slot_streak = 0;
    private const float TITAN_STREAK_REQUIREMENT = 4;
    private bool is_titan = false;

    private PlayerManager player_manager;
    public Ability basic_ability;
    public Ability special_ability;

    public float horizontal;
    public float vertical;

    void Awake()
    {
        body_parts_default_scale = body_parts.transform.localScale;
        damage_flash.SetActive(false);

        player_HUD.SetHealthBarMaxHealth(max_player_health);
        player_health = max_player_health;
        player_HUD.UpdateHealthBar(player_health);

        player_rigidbody = GetComponent<Rigidbody>();
        stun_effect.SetActive(false);
        last_facing = Vector3.right;

        body_renderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        face_indicator.GetComponent<SpriteRenderer>().material.color = body_renderer.material.color;
    }


    void Start()
    {
        player_manager = GameObject.FindObjectOfType<PlayerManager>();
    }


    void Update()
    {
        if (player_input != null)
        {
            DebugControls();
            if (player_input.GetButtonDown("Disconnect"))
            {
                player_manager.PlayerLeave(player_input.id);
            }

            if (!slot_dropping && !controls_disabled)
            {
                HandleMovement();
                HandleAttack();
                HandleSlotDrop();
            }
        }

        Vector3 look_at = face_indicator.transform.position + (last_facing * 3);
        face_indicator.transform.LookAt(look_at);
        face_indicator.transform.Rotate(new Vector3(90, 0, 0));
    }


    void DebugControls()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            EnableTitan();
        }
    }


    void FixedUpdate()
    {
        if (!slot_dropping && !controls_disabled)
        {
            player_rigidbody.MovePosition(transform.position + move);
        }
        else
        {
            player_rigidbody.velocity = new Vector3(0, player_rigidbody.velocity.y, 0);
        }
    }


    void HandleMovement()
    {
        horizontal = player_input.GetAxis("Horizontal");
        vertical = player_input.GetAxis("Vertical");

        // Apply the move and store the last facing direction.
        move = new Vector3(horizontal  * Time.deltaTime * horizontal_move_speed, 0,
                           vertical * Time.deltaTime * vertical_move_speed);


        face_locked = player_input.GetButton("FaceLock");

        if (move != Vector3.zero)
        {
            if (!face_locked)
                last_facing = move.normalized;
        }

        animator.SetBool("walking", horizontal != 0 || vertical != 0);

        if (horizontal < 0 && !flipped && !face_locked)
        {
            flipped = true;
            body_parts.transform.localScale = new Vector3(-body_parts_default_scale.x, body_parts_default_scale.y, body_parts_default_scale.z);
        }

        if (horizontal > 0 && flipped && !face_locked)
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
        if (special_ability != null)
        {
            if (!special_ability.IsReady())
                return;
        }

        if (player_input.GetButtonDown("SlotDrop"))
        {
            animator.SetTrigger("slot_drop");
            slot_dropping = true;

            Invoke("FireSpecial", 0.6f);
            Invoke("SlotDropped", 0.85f);

            SnapToNearestSlot();
        }
    }


    void SnapToNearestSlot()
    {
        RaycastHit[] sphere = Physics.SphereCastAll(transform.position, snap_distance, Vector3.forward, 0);

        foreach (var elem in sphere)
        {
            if (elem.collider.tag != "Slot")
                continue;

            nearby_slot = elem.collider.GetComponent<Slot>();

            // Check slot matching depending on titan status.
            if ((nearby_slot.golden_slot && !is_titan) ||
                !nearby_slot.golden_slot && is_titan)
                break;

            slot_attempt_pos = new Vector3(nearby_slot.transform.position.x, transform.position.y, nearby_slot.transform.position.z);

            transform.position = slot_attempt_pos;

            break;
        }
    }


    void OnCollisionEnter(Collision collision)
    { 
        if (spawning)
        {
            if (collision.collider.CompareTag("Floor") || collision.collider.CompareTag("Prop"))
            {  
                spawning = false;
                controls_disabled = false;
            }
        }
    }


    void FireSpecial()
    {
        if (special_ability != null)
            special_ability.Fire();
    }


    void SlotDropped()
    {
        slot_dropping = false;

        if (nearby_slot != null)
        {
            if (transform.position != slot_attempt_pos)
                return;

            nearby_slot.GetComponent<Slot>().SlotDrop(this);
            slot_streak = player_HUD.AddSlotToken();
        }
    }


    public void Damage(int damage_amount)
    {
        player_health -= damage_amount;
        player_HUD.UpdateHealthBar(player_health);
        DamageFlash();

        if (player_health <= 0)
        {
            player_manager.KillPlayer(player_input.id);
        }
    }


    private void DamageFlash()
    {
        damage_flash.SetActive(true);
        Invoke("EndDamageFlash", damage_flash_duration);
    }


    private void EndDamageFlash()
    {
        damage_flash.SetActive(false);
    }


    public void Stun(float stun_duration)
    {
        controls_disabled = true;
        stun_effect.SetActive(true);
        animator.SetBool("walking", false);
        Invoke("EndStun", stun_duration);
    }


    private void EndStun()
    {
        controls_disabled = false;
        stun_effect.SetActive(false);
    }


    public void SetPlayerInput(Player player_input)
    {
        this.player_input = player_input;
        this.id = player_input.id;
    }


    public bool TitanReady()
    {
        return slot_streak >= TITAN_STREAK_REQUIREMENT;
    }


    public void EnableTitan()
    {
        transform.localScale = titan_size;
        is_titan = true;

        max_player_health = 300;
        player_health = max_player_health;

        face_indicator.gameObject.SetActive(false);
        player_HUD.SetHealthBarMaxHealth(max_player_health);
    }


    public int GetPlayerID()
    {
        return id;
    }


    public Vector3 GetLastFacing()
    {
        return last_facing;
    }


    public void SetControlsDisabled(bool is_disabled)
    {
        controls_disabled = is_disabled;
    }


    public bool GetControlsDisabled()
    {
        return controls_disabled;
    }


    public void SetSpawning(bool is_spawning)
    {
        spawning = is_spawning;
    }


    public bool GetSpawning()
    {
        return spawning;
    }


    public int GetHealth()
    {
        return player_health;
    }

}
