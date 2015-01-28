﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class ColliderHit : MonoBehaviour {

    public HealthHandler healthScript;

    public DamageColliderManager owner;

    public float damageMultiplier;

    public float minForceForRagdoll;

    public float forceMultToDamage = 1.0f;

    public void Damage(vp_DamageInfo damageInfo)
    {
        if (healthScript)
        {
            damageInfo.Damage *= damageMultiplier;
            healthScript.Damage(damageInfo);
        }
    }

    public void Damage(float damage)
    {
        Damage(new vp_DamageInfo(damage, null));
    }

    private List<HitInfo> hitForces = new List<HitInfo>();

    public void AddHitForce(Vector3 position, Vector3 force)
    {

        ForceForRagdoll(force.magnitude);
        hitForces.Add(new HitInfo() { force = force, position = position });
        AddCachedForces();
    }

    public void AddCachedForces()
    {
        if (hitForces.Count > 0 && !rigidbody.isKinematic)
        {
            foreach (var item in hitForces)
            {
                rigidbody.AddForceAtPosition(item.force, item.position, ForceMode.Impulse);
            }

            hitForces.Clear();
        }
    }

    void FixedUpdate()
    {
        AddCachedForces();
    }

    void OnCollisionEnter(Collision info)
    {
        if (!owner.PartOfMe(info.transform))
        {
            if (info.relativeVelocity.magnitude > minForceForRagdoll)
            {
                float damage = (info.relativeVelocity.magnitude - minForceForRagdoll) * forceMultToDamage;
                vp_DamageInfo damageInfo = new vp_DamageInfo(damage, info.transform);
                Damage(damageInfo);

                ForceForRagdoll(info.relativeVelocity.magnitude);

                if (rigidbody.isKinematic)
                {
                    hitForces.Add(new HitInfo() { force = info.relativeVelocity, position = info.contacts[0].point });
                }
            }
        }
    }

    public void ForceForRagdoll(float amount)
    {
        if(healthScript)
            healthScript.HitForce(amount, minForceForRagdoll);
    }
}

public struct HitInfo
{
    public Vector3 position;
    public Vector3 force;
}
