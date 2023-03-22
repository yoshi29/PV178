using System.Linq;
using UnityEngine;

public class BasicTower : Tower
{
    private new void Update()
    {
        base.Update();

        if (ShouldSelectNewEnemy())
        {
            _selectedEnemy = _enemiesInRange
                .OrderBy(o => Vector3.Distance(o.transform.position, _towerPosition))
                .First();
        }

        ShootOrWaitForReload();
    }
}
