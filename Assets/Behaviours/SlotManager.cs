using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public float min_open_delay = 2;
    public float max_open_delay = 10;

    private Slot[] slots;
    private bool random_slot_queued = false;


	void Start()
    {
		slots = GameObject.FindObjectsOfType<Slot>();
	}
	

	void Update()
    {
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
        Slot slot = slots[Random.Range(0, slots.Length)];

        if (!slot.IsOpen())
            slot.Open();

        random_slot_queued = false;
    }

}
