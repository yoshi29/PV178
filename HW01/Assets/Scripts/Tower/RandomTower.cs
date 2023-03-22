using UnityEngine;
using Random = System.Random;

public class RandomTower : Tower
{
    private static readonly Random _random = new();

    private new void Update()
    {
        base.Update();

        if (ShouldSelectNewEnemy())
        {
            var randomIndex = _random.Next(0, _enemiesInRange.Length);
            _selectedEnemy = _enemiesInRange[randomIndex];
        }

        TryAttackWithRandomStrategy();
    }

    private void TryAttackWithRandomStrategy()
    {
        if (!IsEnemySelectedAndIsInRange()) return;

        int percent = _random.Next(0, 100);

        if (percent < 20) ShootOrWaitForReload(2, 0f);
        else if (percent < 60 + 20) ShootOrWaitForReload();
        else _reloadTimeCountdown -= Time.deltaTime;
    }
}