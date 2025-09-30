using UnityEngine;

public class EnemyAttackRangeController : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;

    private void Awake()
    {
        if(enemyController == null)
            enemyController = GetComponent<EnemyController>();
    }

    private void Start()
    {
        if (enemyController == null)
        {
            Debug.LogError("EnemyController not assigned and not found in parent!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (enemyController != null)
            {
                enemyController.PlayerEnteredAttackRange(other.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (enemyController != null)
            {
                enemyController.PlayerExitedAttackRange();
            }
        }
    }
}