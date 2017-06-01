using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Animator animator;
    public BoxCollider box_collider;
    public SpriteRenderer hazard_symbol;
    public GameObject light_shaft;

    public float time_to_close = 5.0f;
    public float infection_chance = 30.0f;
    public bool golden_slot = false;

    private LoadoutFactory loadout_factory;
    private PCManager pc_manager;
    private const int percentile = 100;


    void Start()
    {
        loadout_factory = GameObject.FindObjectOfType<LoadoutFactory>();
        pc_manager = GameObject.FindObjectOfType<PCManager>();
        light_shaft.SetActive(false);

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
        if (IsOpen())
            return;

        if (animator != null)
            animator.SetBool("open", true);

        if (!golden_slot)
        {
            Invoke("Close", time_to_close);
        }

        light_shaft.SetActive(true);
    }


    public void Close()
    {
        CancelInvoke();

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
        if (animator != null)
            return animator.GetBool("open");

        return false;
    }

    
    public bool CanBeSlotted()
    {
        return box_collider.enabled && !pc_manager.IsGameEnding();
    }

    
    public void SlotDrop(PlayerController player)
    {
        // Game ending slot drop.
        if (golden_slot && player.IsTitan())
        {
            pc_manager.EndGame();

            return;
        }
        else
        {
            // Close the slot.
            Close();
        }

        // Chance to infect the PC.
        if (Random.Range(0, percentile) < infection_chance)
        {
            pc_manager.IncrementVirusBar();
            GameObject.FindObjectOfType<AudioManager>().PlayOneShot("laughing_skull");

            loadout_factory.AssignLoadout(player, "Broken");
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

}
