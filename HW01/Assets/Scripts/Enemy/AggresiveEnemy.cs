using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class AggresiveEnemy : Enemy
{
    private const float TOWER_DETECTION_RANGE = 10f;
    [CanBeNull] private Tower _selectedTower;
    private Tower[] _towersInRange;

    private bool _moveTowardsTowerStarted;
    private bool _moveAlongPathStarted;

    private void Update()
    {
        _towersInRange = GetTowersInRange(TOWER_DETECTION_RANGE);

        if (!IsTowerSelectedAndIsInRange() && _towersInRange.Length != 0)
        {
            _selectedTower = _towersInRange
                .OrderBy(o => Vector3.Distance(o.transform.position, transform.position))
                .First();
        }

        Move();
    }

    private void OnCollisionEnter(Collision otherObject)
    {
        if (TryToDealDamage(otherObject, _damage,  _damage))
        {
            HandleDeathWithoutReward();
        }
    }

    private Tower[] GetTowersInRange(float range)
    {
        return Physics
            .OverlapSphere(transform.position, range)
            .Where(o => o.gameObject.GetComponent<Tower>() != null)
            .Select(c => c.GetComponent<Tower>())
            .ToArray();
    }

    private bool IsTowerSelectedAndIsInRange()
    {
        return _selectedTower != null && _towersInRange.Contains(_selectedTower);
    }


    private void Move()
    {
        if (_selectedTower != null && !_moveTowardsTowerStarted)
        {
            _movementComponent.MoveTowards(_selectedTower.transform);
            _moveTowardsTowerStarted = true;
            _moveAlongPathStarted = false;
        }
        else if (_selectedTower == null && !_moveAlongPathStarted)
        {
            _movementComponent.MoveAlongPath();
            _moveAlongPathStarted = true;
            _moveTowardsTowerStarted = false;
        }
    }
}
