using System;
using UnityEngine;
using System.Collections;
using Assets.Classes;
using Random = UnityEngine.Random;

public class PowerUp : MonoBehaviour
{
    PowerUpSettings RandomizePowerup()
    {
        PowerUpType tempPowerUpType = PowerUpType.PU_INVALID;
        float time = 0.0f;
        int type = 0;
        while (time <= 2.0f)
        {
            //show something on ui 
            type = Random.Range(1, 3);
            time += Time.deltaTime;
        }

        tempPowerUpType = (PowerUpType) type;
        PowerUpSettings temPowerUpSettings = new PowerUpSettings();

        switch (tempPowerUpType)
        {
            case PowerUpType.PU_INVALID:
                temPowerUpSettings.type = tempPowerUpType;
                temPowerUpSettings.duration = 0;
                break;
            case PowerUpType.PU_SPEED_BOOST:
                temPowerUpSettings.type = tempPowerUpType;
                temPowerUpSettings.duration = 4;
                break;
            case PowerUpType.PU_GLOCK:
                temPowerUpSettings.type = tempPowerUpType;
                temPowerUpSettings.duration = 3;
                break;
        }
        Debug.Log(tempPowerUpType);
        return temPowerUpSettings;
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
            coll.gameObject.GetComponent<PlayerWeapon>().weaponSettings = RandomizePowerup();
            Destroy(this);
        }
    }
}