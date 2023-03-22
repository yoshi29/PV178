using System.Linq;

public class BurstTower : Tower
{
    private new void Update()
    {
        base.Update();

        if (ShouldSelectNewEnemy())
        {
            _selectedEnemy = _enemiesInRange
                .OrderBy(o => o.gameObject.GetComponent<Enemy>().Health.HealthValue)
                .First();
        }

        ShootOrWaitForReload(2, 0.2f);
    }
}
