using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Classes;
using UnityEngine;


public class PowerUpManager : MonoBehaviour
{
    public List<GameObject> _uniqueGameObjects = new List<GameObject>(Enum.GetNames(typeof(WeaponType)).Length);


    void Start()
    {
        Debug.Log(Enum.GetNames(typeof(WeaponType)).Length);
    }

    public GameObject GetUniqueGameObject(WeaponType weaponType)
    {
        return _uniqueGameObjects[(int) weaponType];
    }
}