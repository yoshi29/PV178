using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MovementComponent), typeof(HealthComponent), typeof(BoxCollider))]
public class Enemy : MonoBehaviour
{
    [SerializeField] protected MovementComponent _movementComponent;
    [SerializeField] protected HealthComponent _healthComponent;
    [SerializeField] protected ParticleSystem _onDeathParticlePrefab;
    [SerializeField] protected ParticleSystem _onSuccessParticlePrefab;
    [SerializeField] protected LayerMask _attackLayerMask;

    [SerializeField] protected int _damage;
    [SerializeField] protected int _reward;
    [SerializeField] protected int _speed;

    public HealthComponent Health => _healthComponent;

    public event Action OnDeath;

    protected void Start()
    {
        _healthComponent.OnDeath += HandleDeath;
    }

    public void Init(EnemyPath path)
    {
        _movementComponent.Init(path, _speed);
        _movementComponent.MoveAlongPath();
    }

    protected void HandleDeath()
    {
        GameObject.FindObjectOfType<Player>().Resources += _reward;
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    protected void HandleDeathWithoutReward()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    protected bool TryToDealDamage(Collision otherObject, int damageToCastle, int damageToTower)
    {
        var damageDealt = true;

        if (otherObject.gameObject.GetComponent<Castle>() != null)
        {
            otherObject.gameObject.GetComponent<Castle>().Health.HealthValue -= damageToCastle;
        }
        else if (otherObject.gameObject.GetComponent<Tower>() != null)
        {
            otherObject.gameObject.GetComponent<Tower>().Health.HealthValue -= damageToTower;
        }
        else damageDealt = false;

        return damageDealt;
    }
}
