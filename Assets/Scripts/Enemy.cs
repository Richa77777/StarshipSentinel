using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    public Action<Enemy> OnDisableAction;

    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] protected Direction _direction = Direction.Down;

    [Header("Bullet")]
    [SerializeField] private List<Transform> _bulletSpawnPoints;
    [SerializeField] private float _delayBtwnShots;

    [SerializeField] private AudioClip _clip;

    private float _health = 2f;
    private Rigidbody2D _rb;
    private Coroutine _shootingCor;
    private AudioSource _audioSource;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _clip;
    }

    private void OnEnable()
    {
        _health = 2f;
        _rb.velocity = new Vector2(_rb.velocity.x, (int)_direction * _moveSpeed);

        foreach (Bullet bullet in GameController.Instance._enemyBulletsPool.PoolQueue)
        {
            bullet.OnDisableAction += GameController.Instance._enemyBulletsPool.ReturnToPool;
        }

        if (_shootingCor == null)
        {
            _shootingCor = StartCoroutine(Shooting());
        }
    }

    private void OnDisable()
    {
        if (_shootingCor != null)
        {
            StopCoroutine(_shootingCor);
            _shootingCor = null;
        }

        foreach (Bullet bullet in GameController.Instance._enemyBulletsPool.PoolQueue)
        {
            bullet.OnDisableAction -= GameController.Instance._enemyBulletsPool.ReturnToPool;
        }

        OnDisableAction?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            _audioSource.Play();
            _health -= 0.5f;

            if (_health <= 0)
            {
                GameController.Instance.AddScore();
                gameObject.SetActive(false);
            }

            collision.gameObject.SetActive(false);
        }
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, _delayBtwnShots));

            for (int i = 0; i < _bulletSpawnPoints.Count; i++)
            {
                Bullet bullet = GameController.Instance._enemyBulletsPool.GetObject();
                bullet.transform.position = _bulletSpawnPoints[i].position;
            }
        }
    }

    protected enum Direction
    {
        Up = 1,
        Down = -1
    }
}
