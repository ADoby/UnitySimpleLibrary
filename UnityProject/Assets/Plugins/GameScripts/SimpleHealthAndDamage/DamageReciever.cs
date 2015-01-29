using UnityEngine;
using System.Collections;

namespace SimpleLibrary
{
    public class DamageReciever : MonoBehaviour
    {
        public HealthHandler reciever;

        public float DamageMultiply = 1.0f;

        public void Damage(DamageInfo damageInfo)
        {
            if (reciever)
            {
                damageInfo.Value *= DamageMultiply;
                reciever.Damage(damageInfo);
            }
        }

        public void Damage(float damage)
        {
            if (reciever)
            {
                damage *= DamageMultiply;
                reciever.Damage(damage);
            }
        }
    }
}

