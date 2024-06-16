using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Game Settings")]
    [SerializeField] private int _scoreForShip = 1;
    [SerializeField] private int _subHpForCollisOrMiss = 1;

    [Header("Components")]
    [SerializeField] private GameObject _loseTab;
    [SerializeField] private ScoreController _scoreController;
    [SerializeField] private HealthController _healthController;
    [SerializeField] private Player _player;

    public ObjectPool<Bullet> _enemyBulletsPool;
    [SerializeField] private Bullet _enemyBulletPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _enemyBulletsPool = new ObjectPool<Bullet>(_enemyBulletPrefab, 40);
    }

    public void SubtractHealth()
    {
        _healthController.SubtractHealth(_subHpForCollisOrMiss);
    }

    public void AddScore()
    {
        _scoreController.AddScore(_scoreForShip);
    }

    public void Lose()
    {
        _player.enabled = false;
        StopAllCoroutines();
        _loseTab.SetActive(true);
        Time.timeScale = 0f;
    }
}
