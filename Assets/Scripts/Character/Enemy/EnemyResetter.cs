using UnityEngine;

public class EnemyResetter : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(nameof(Enemy)))
            return;

        if (collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
