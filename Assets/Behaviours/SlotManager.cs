using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public float min_open_delay = 2;
    public float max_open_delay = 10;

    public Slot golden_slot;

    private Slot[] slots;
    private bool random_slot_queued = false;
    private PlayerManager player_manager;

    bool invoke_called = false;


	void Start()
    {
        player_manager = GameObject.FindObjectOfType<PlayerManager>();
		slots = GameObject.FindObjectsOfType<Slot>();
	}
	

	void Update()
    {
        bool titan_exists = player_manager.AnyPlayerTitan();

        if (titan_exists)
        {
            if (!golden_slot.IsOpen() && !invoke_called)
            {
                invoke_called = true;
                Invoke("OpenGoldenSlot", 1.0f);
            }
        }
        else
        {
            if (golden_slot.IsOpen())
            {
                golden_slot.Close();
                CancelInvoke();
            }
        }

		if (!random_slot_queued)
        {
            Invoke("OpenRandomSlot", Random.Range(min_open_delay, max_open_delay));
            random_slot_queued = true;
        }
	}


    void CloseAllSlots()
    {
        foreach (Slot slot in slots)
        {
            slot.Close();
        }
    }


    // Open a random slot if it is not open.
    void OpenRandomSlot()
    {
        Slot slot;

        do
        {
            slot = slots[Random.Range(0, slots.Length)];
        } while (slot.IsGolden());

        if (!slot.IsOpen())
            slot.Open();

        random_slot_queued = false;
    }


    void OpenGoldenSlot()
    {
        invoke_called = false;
        golden_slot.Open();
    }

}
