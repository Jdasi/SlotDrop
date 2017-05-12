using System;
using UnityEngine;
using System.Collections;
using Assets.Classes;
using Rewired;
using Random = UnityEngine.Random;

public class PowerUp : MonoBehaviour
{
    PowerUpType RandomizePowerup()
    {
        PowerUpType tempPowerUpType = PowerUpType.PU_INVALID;
        float time = 0.0f;
        int type = 0;
        while (time <= 5.0f)
        {
            //show something on ui 
            type = Random.Range(2, 3);
            time += Time.deltaTime;
        }
        tempPowerUpType = (PowerUpType) type;
        Debug.Log(tempPowerUpType);
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

            PowerUpType tempPowerUpType = RandomizePowerup();
            Destroy(coll.gameObject.GetComponent<PlayerWeapon>());
            switch (tempPowerUpType)
            {
                case PowerUpType.PU_INVALID:
                    break;
                case PowerUpType.PU_SPEED_BOOST:
                    coll.gameObject.AddComponent<WeaponSpeedBoost>();
                    break;
                case PowerUpType.PU_GLOCK:
                    break;
                case PowerUpType.PU_SHOCKWAVE:
                    coll.gameObject.AddComponent<WeaponShockwave>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Destroy(this);
        }
    }
}