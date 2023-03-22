using UnityEngine;

public class LazyEnemy : Enemy
{
    private const float MOVEMENT_TIME_LIMIT = 5f;
    private const float WAIT_TIME_LIMIT = 1f;

    private float _movementTime;
    private float _waitTime;

    private void Update()
    {
        if (_waitTime > 0) // Wait
        {
            _waitTime -= Time.deltaTime;
            if (_waitTime < 0 ) _movementComponent.MoveAlongPath();
        }
        else if (_movementTime >= MOVEMENT_TIME_LIMIT) // Stop moving
        {
            _movementTime = 0;
            _waitTime = WAIT_TIME_LIMIT;
            _movementComponent.CancelMovement();
        }
        else // Move
        {
            _movementTime += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision otherObject)
    {
        if (TryToDealDamage(otherObject, _damage, 2 * _damage))
        {
            HandleDeathWithoutReward();
        }
    }
}
