using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : Projectile
{
    private void OnTriggerEnter(Collider otherObject)
    {
        var enemy = otherObject.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Health.HealthValue -= _damage;
        }
    }
}
