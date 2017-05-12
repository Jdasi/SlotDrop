using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

namespace Assets.Classes
{
    public class WeaponBaseProjectile : PlayerWeapon
    {
        private Vector3 dir;
        private float bulletspeed = 40.0f;
        public GameObject AmmoPrefab;

        // Use this for initialization
        void Start()
        {
            WeaponSettings.player_input =
                ReInput.players.GetPlayer(this.GetComponentInParent<PlayerController>()
                    .GetPlayerID()); // SHOULD BE EXTRACTED TO BASE BEHAVIOUR
            WeaponSettings.Cooldown = 1.0f;
            WeaponSettings.TimeLeft = 0.0f;
            WeaponSettings.Ready = true;
            WeaponSettings.Type = WeaponType.WEP_BASE_PROJECTILE;
            AmmoPrefab = GameObject.FindGameObjectWithTag("Managers").GetComponent<PowerUpManager>()
                .GetUniqueGameObject(WeaponSettings.Type);
        }


        void OnCollisionEnter(Collision coll)
        {
        }

        void FixedUpdate()
        {
            Shoot();
        }

        private float hor;
        private float temphor;
        private float ver;
        private float tempver;

        protected override void Attack()
        {
            if (!WeaponSettings.player_input.GetButtonDown("SlotDrop") || !WeaponSettings.Ready) return;

            hor = this.gameObject.GetComponent<PlayerController>().horizontal;
            ver = this.gameObject.GetComponent<PlayerController>().vertical;
            dir = new Vector3(hor, 0, ver);
            dir.Normalize();
            dir *= bulletspeed;
            WeaponSettings.Active = true;
            WeaponSettings.Ready = false;
        }

        void Shoot()
        {
            if (!WeaponSettings.Active)
            {
                return;
            }
            GameObject bulletGameObject = Instantiate(AmmoPrefab);
            Vector3 offset = new Vector3(0, 1, 0);
            bulletGameObject.transform.position = transform.position + offset;
            bulletGameObject.gameObject.GetComponent<Rigidbody>().velocity = dir;
            Debug.Log("PEW PEW PEW");
            WeaponSettings.Active = false;
        }
    }
}