using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    private void OnTriggerEnter(Collider otherObject)
    {
        var enemy = otherObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            DamageEnemyAndOthersInRadius(enemy);
        }
    }

    private void DamageEnemyAndOthersInRadius(Enemy enemy)
    {
        var enemiesToDamage = Physics.OverlapSphere(enemy.transform.position, 5)
            .Where(o => o.gameObject.GetComponent<Enemy>() != null)
            .Select(c => c.gameObject.GetComponent<Enemy>())
            .ToList();

        enemiesToDamage.Add(enemy);

        foreach (var enemyToDamage in enemiesToDamage)
        {
            enemyToDamage.Health.HealthValue -= _damage;
        }
    }
}
