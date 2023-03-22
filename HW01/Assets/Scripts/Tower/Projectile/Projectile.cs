using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected LayerMask _enemyLayerMask;
    [SerializeField] protected ParticleSystem _onHitParticleSystem;

    [SerializeField] protected int _damage;
    [SerializeField] protected int _speed;
    [SerializeField] protected float _lifespan;

    protected Enemy _target;
    protected float _lifespanCountdown;
    
    public int Damage => _damage;

    public Enemy Target
    {
        get => _target;
        set => _target = value;
    }

    protected void Start()
    {
        _lifespanCountdown = _lifespan;
    }

    protected void Update()
    {
        if (_lifespanCountdown <= 0 || _target == null) Destroy(transform.gameObject);
        else _lifespanCountdown -= Time.deltaTime;
    }

    protected void FixedUpdate()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, 
                Time.fixedDeltaTime * _speed);
        }
    }
}
