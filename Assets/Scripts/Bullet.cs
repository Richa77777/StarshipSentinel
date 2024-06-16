using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public Action<Bullet> OnDisableAction;

    [SerializeField] protected float _bulletSpeed;
    [SerializeField] protected Direction _direction = Direction.Up;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, (int)_direction * _bulletSpeed);
    }

    private void OnDisable()
    {
        OnDisableAction?.Invoke(this);
    }

    protected enum Direction
    {
        Up = 1,
        Down = -1
    }
}
