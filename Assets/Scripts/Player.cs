using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _xLimit = 1.5f;

    [Header("Bullet")]
    [SerializeField] private List<Transform> _bulletSpawnPoints;
    [SerializeField] private float _delayBtwnShots;
    [SerializeField] private Bullet _bulletPrefab;

    private Camera _camera;
    private Vector2 _touchStartPosition;
    private Vector2 _touchEndPosition;
    private Vector2 _touchDeltaPosition;

    private ObjectPool<Bullet> _bulletsPool;

    private void Awake()
    {
        _bulletsPool = new ObjectPool<Bullet>(_bulletPrefab, 20);
        _camera = Camera.main;

        StartCoroutine(Shooting());
    }

    private void OnEnable()
    {
        foreach (Bullet bullet in _bulletsPool.PoolQueue)
        {
            bullet.OnDisableAction += _bulletsPool.ReturnToPool;
        }
    }

    private void OnDisable()
    {
        foreach (Bullet bullet in _bulletsPool.PoolQueue)
        {
            bullet.OnDisableAction -= _bulletsPool.ReturnToPool;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _touchStartPosition = _camera.ScreenToWorldPoint(touch.position);
                _touchEndPosition = _touchStartPosition;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                _touchEndPosition = _camera.ScreenToWorldPoint(touch.position);
                _touchDeltaPosition = _touchEndPosition - _touchStartPosition;

                Vector2 newPosition = new Vector2(transform.position.x + _touchDeltaPosition.x * _moveSpeed, transform.position.y);

                float clampedX = Mathf.Clamp(newPosition.x, -_xLimit, _xLimit);
                transform.position = new Vector3(clampedX, transform.position.y);

                _touchStartPosition = _touchEndPosition;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            GameController.Instance.SubtractHealth();
            collision.gameObject.SetActive(false);
        }
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delayBtwnShots);

            for (int i = 0; i < _bulletSpawnPoints.Count; i++)
            {
                Bullet bullet = _bulletsPool.GetObject();
                bullet.transform.position = _bulletSpawnPoints[i].position;
            }
        }
    }
}
