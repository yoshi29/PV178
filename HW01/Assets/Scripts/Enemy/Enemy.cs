using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementComponent), typeof(HealthComponent), typeof(BoxCollider))]
public class Enemy : MonoBehaviour
{
    [SerializeField] protected MovementComponent _movementComponent;
    [SerializeField] protected HealthComponent _healthComponent;
    [SerializeField] protected ParticleSystem _onDeathParticlePrefab;
    [SerializeField] protected ParticleSystem _onSuccessParticlePrefab;
    [SerializeField] protected LayerMask _attackLayerMask;

    public event Action OnDeath;

    private void Start()
    {
        _healthComponent.OnDeath += HandleDeath;
    }

    private void OnDestroy()
    {
        _healthComponent.OnDeath -= HandleDeath;
    }

    public void Init(EnemyPath path)
    {
        // TODO: Modify this so they have appropriate speed
        _movementComponent.Init(path, 5);
    }

    protected void HandleDeath()
    {
        // TODO: Modify this so they give appropriate reward
        GameObject.FindObjectOfType<Player>().Resources += 0;
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
