using Rewired;
using UnityEngine;

namespace Assets.Classes
{
    public class WeaponSpecialShockwave : PlayerWeapon
    {
        private readonly float _cameraShockwaveShakeStrength = 0.4f;
        private readonly float _cameraShockwaveShakeDuration = 0.4f;
        private GameObject _shockwavePrefab;

        protected override void Attack()
        {
            if (!WeaponSettings.player_input.GetButtonDown("SlotDrop") || !WeaponSettings.Ready) return;

            CameraShake.instance.ShakeCamera(_cameraShockwaveShakeDuration, _cameraShockwaveShakeStrength);
            WeaponSettings.Active = true;
            WeaponSettings.Ready = false;

            GameObject shockParticle = Instantiate(_shockwavePrefab);
            shockParticle.transform.position = transform.position;
            Destroy(shockParticle, shockParticle.GetComponent<ParticleSystem>().main.duration);
        }

        // Use this for initialization
        void Start()
        {
            WeaponSettings.player_input =
                ReInput.players.GetPlayer(this.GetComponentInParent<PlayerController>()
                    .GetPlayerID()); // SHOULD BE EXTRACTED TO BASE BEHAVIOUR
            WeaponSettings.WeaponEffectRadius = 20.0f;
            WeaponSettings.KnockbackForce = 20.0f;
            WeaponSettings.KnockbackForce *= 1000;
            WeaponSettings.Cooldown = 3.0f;
            WeaponSettings.TimeLeft = 0.0f;
            WeaponSettings.Ready = true;

            WeaponSettings.Type = WeaponType.WEP_SHOCKWAVE_RADIAL;
            _shockwavePrefab = GameObject.FindGameObjectWithTag("Managers").GetComponent<PowerUpManager>()
                .GetUniqueGameObject(WeaponSettings.Type);
        }


        void FixedUpdate()
        {
            Shockwave();
        }

        void Shockwave()
        {
            if (!WeaponSettings.Active)
            {
                return;
            }

            RaycastHit[] sphere = Physics.SphereCastAll(transform.position, WeaponSettings.WeaponEffectRadius,
                Vector3.forward, 0);

            foreach (var elem in sphere)
            {
                Rigidbody rigidBody = elem.collider.gameObject.GetComponent<Rigidbody>();

                if (rigidBody == this.GetComponent<Rigidbody>())
                    continue;

                if (rigidBody != null)
                    rigidBody.AddExplosionForce(WeaponSettings.KnockbackForce, transform.position,
                        WeaponSettings.WeaponEffectRadius);
            }
            WeaponSettings.Active = false;
        }
    }
}