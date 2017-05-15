using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slot : MonoBehaviour
{
    public Animator animator;
    public BoxCollider box_collider;
    public SpriteRenderer hazard_symbol;
    public GameObject light_shaft;

    public float time_to_close = 5.0f;
    public float unprotected_chance = 30.0f;
    public bool unprotected = false;
    public bool golden_slot = false;

    public GameObject end_game_canvas;

    private LoadoutFactory loadout_factory;
    private PCManager pc_manager;
    private const int percentile = 100;


    void Start()
    {
        loadout_factory = GameObject.FindObjectOfType<LoadoutFactory>();
        pc_manager = GameObject.FindObjectOfType<PCManager>();
        light_shaft.SetActive(false);

        unprotected = Random.Range(1, percentile) < unprotected_chance;
        hazard_symbol.enabled = unprotected;

        if (transform.position.y != 0.2f)
        {
            transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
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
        if(animator != null)
            animator.SetBool("open", true);

        if (!golden_slot)
            Invoke("Close", time_to_close);

        light_shaft.SetActive(true);
    }


    public void Close()
    {
        animator.SetBool("open", false);
        light_shaft.SetActive(false);
    }


    public void PostponeClose()
    {
        CancelInvoke();

        if (!golden_slot)
            Invoke("Close", time_to_close / 2);
    }


    public bool IsOpen()
    {
        if(animator != null)
            return animator.GetBool("open");

        return false;
    }


    public bool IsUnprotected()
    {
        return unprotected;
    }

    
    public bool CanBeSlotted()
    {
        return box_collider.enabled;
    }

    
    public void SlotDrop(PlayerController player)
    {
        // Close the slot.
        Close();

        // Game ending slot drop.
        if (golden_slot && player.IsTitan())
        {
            Time.timeScale = 0.3f;

            Invoke("EnableCanvas", 1.0f);
            Invoke("ResetLevel", 4.0f);

            return;
        }

        // Infect the PC.
        if (unprotected)
        {
            pc_manager.IncrementVirusBar();

            // Assign initial loadout.
            if (player.loadout_name == "Base")
            {
                    loadout_factory.AssignRandomLoadout(player);
            }
            // Chance to reset the player's loadout.
            else
            {
                if (Random.Range(0, percentile) < unprotected_chance)
                    loadout_factory.AssignLoadout(player, "Base");
            }
        }
        else
        {
            loadout_factory.AssignRandomLoadout(player);
        }

        if (player.TitanReady())
        {
            loadout_factory.AssignLoadout(player, "Gold");
            player.EnableTitan();
        }

        // Fully heal player.
        player.Damage(-(player.max_player_health - player.GetHealth()));

    }


    public bool IsGolden()
    {
        return golden_slot;
    }


    void EnableCanvas()
    {
        Time.timeScale = 1;
        end_game_canvas.SetActive(true);
    }


    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
