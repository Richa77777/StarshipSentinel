using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _delayBtwnSpawn = 2f;
    [SerializeField] private float _xSpawnLimit = 1.5f;
    [SerializeField] private float _ySpawnPos = 0f;

    [SerializeField] private Enemy _enemyPrefab;

    private ObjectPool<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(_enemyPrefab, 10);

        StartCoroutine(SpawnCycle());
    }

    private void OnEnable()
    {
        foreach (Enemy enemy in _enemyPool.PoolQueue)
        {
            enemy.OnDisableAction += _enemyPool.ReturnToPool;
        }
    }

    private void OnDisable()
    {
        foreach (Enemy enemy in _enemyPool.PoolQueue)
        {
            enemy.OnDisableAction -= _enemyPool.ReturnToPool;
        }
    }

    private IEnumerator SpawnCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delayBtwnSpawn);

            float xPos = Random.Range(_xSpawnLimit, -_xSpawnLimit);
            Enemy enemy = _enemyPool.GetObject();
            enemy.transform.position = new Vector2(xPos, _ySpawnPos);
        }
    }
}
