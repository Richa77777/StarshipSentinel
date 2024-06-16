using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class DestroyZone : MonoBehaviour
{
    [SerializeField] private DestroyZoneType _destroyZoneType = DestroyZoneType.Enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_destroyZoneType == DestroyZoneType.Enemy)
        {
            if (collision.GetComponent<Enemy>())
            {
                GameController.Instance.SubtractHealth();
            }
        }

        if (collision.GetComponent<Bullet>())
        {
            collision.gameObject.SetActive(false);
        }
    }

    private enum DestroyZoneType
    {
        PlayerBullets,
        Enemy
    }
}
