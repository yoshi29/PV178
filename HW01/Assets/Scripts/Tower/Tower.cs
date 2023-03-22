using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(HealthComponent))]
public class Tower : MonoBehaviour
{
    [SerializeField] protected LayerMask _enemyLayerMask;
    [SerializeField] private HealthComponent _healthComponent;
    [SerializeField] protected Projectile _projectilePrefab;
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] protected Transform _objectToPan;
    [SerializeField] protected Transform _projectileSpawn;
    [SerializeField] private GameObject _previewPrefab;

    [SerializeField] protected int _price;
    [SerializeField] protected int _range;
    [SerializeField] protected string _name;
    [SerializeField] protected float _reloadTime;

    protected Enemy _selectedEnemy;
    protected Vector3 _towerPosition;
    protected float _reloadTimeCountdown;
    protected Enemy[] _enemiesInRange;
    private bool _shootingInProcess;

    [CanBeNull] public Enemy SelectedEnemy => _selectedEnemy;

    public HealthComponent Health => _healthComponent;
    public BoxCollider Collider => _boxCollider;
    public GameObject BuildingPreview => _previewPrefab;

    public int Price => _price;

    private void Start()
    {
        _healthComponent.OnDeath += HandleDeath;
        _towerPosition = transform.position;
    }

    protected void Update()
    {
        _enemiesInRange = GetEnemiesInRange();

        if (_selectedEnemy != null)
        {
            _objectToPan.LookAt(_selectedEnemy.transform); // Rotate tower top
        }
    }

    private void OnDestroy()
    {
        _healthComponent.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }

    protected Enemy[] GetEnemiesInRange()
    {
        return Physics
            .OverlapSphere(_towerPosition, _range)
            .Where(o => o.gameObject.GetComponent<Enemy>() != null)
            .Select(c => c.gameObject.GetComponent<Enemy>())
            .ToArray();
    }

    protected bool ShouldSelectNewEnemy()
    {
        return !IsEnemySelectedAndIsInRange() && _enemiesInRange.Length != 0;
    }

    protected bool IsEnemySelectedAndIsInRange()
    {
        return _selectedEnemy != null && _enemiesInRange.Contains(_selectedEnemy);
    }

    protected void ShootOrWaitForReload(int numberOfProjectiles = 1, float delayBetweenProjectiles = 0)
    {
        if (!IsEnemySelectedAndIsInRange()) return;

        if (_reloadTimeCountdown <= 0)
        {
            if (!_shootingInProcess) StartCoroutine(Shoot(numberOfProjectiles, delayBetweenProjectiles));
        }
        else
        {
            _reloadTimeCountdown -= Time.deltaTime;
        }
    }

    private IEnumerator Shoot(int numberOfProjectiles = 1, float delayBetweenProjectiles = 0f)
    {
        _shootingInProcess = true;

        while (numberOfProjectiles > 0)
        {
            var projectile = Instantiate(_projectilePrefab, _projectileSpawn.position, Quaternion.identity);
            projectile.Target = _selectedEnemy;

            numberOfProjectiles--;

            yield return new WaitForSeconds(delayBetweenProjectiles);
        }

        _reloadTimeCountdown = _reloadTime;
        _shootingInProcess = false;
    }
}
