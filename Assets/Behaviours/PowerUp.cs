using System;
using UnityEngine;
using System.Collections;
using Assets.Classes;
using Random = UnityEngine.Random;

public class PowerUp : MonoBehaviour
{
    private PowerUpSettings m_settings;

    PowerUpType RandomizePowerup()
    {
        PowerUpType tempPowerUpType = PowerUpType.PU_INVALID;
        float time = 0.0f;
        int type = 0;
        while (time <= 2.0f)
        {
            //show something on ui 
            type = Random.Range(1, 3);
            time += Time.deltaTime;
            Debug.Log(time);
        }

        tempPowerUpType = (PowerUpType)type;
        return tempPowerUpType;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
            coll.gameObject.GetComponent<PlayerWeapon>().weaponType = RandomizePowerup();
            Destroy(this);
        }
    }

   
}