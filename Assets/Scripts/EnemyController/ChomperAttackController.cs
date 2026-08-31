using UnityEngine;

public class ChomperAttackController : MonoBehaviour
{
    [Header("Chase Attack Settings")]
    public float chaseSpeed = 1.6f;
    public float stopDistance = 0.5f;


    private Transform player;
    private EnemyController enemyController;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        animator = GetComponent<Animator>();

        if (enemyController == null)
        {
            Debug.LogError("EnemyController not foundon parent object!");
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure player has 'Player' tag.");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyController.isPlayerInAttackRange && player != null)
        {
            ChaseAndAttack();
        }
    }


    void ChaseAndAttack()
    {
        float minX = Mathf.Min(enemyController.pointA.x, enemyController.pointB.x);
        float maxX = Mathf.Max(enemyController.pointA.x, enemyController.pointB.x);


        float playerX = player.position.x;
        float enemyX = transform.position.x;
        Vector3 targetPos;

        if (playerX < minX)
        {
            targetPos = new Vector3(minX, transform.position.y, transform.position.z);
        }
        else if (playerX > maxX)
        {
            targetPos = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        else
        {
            targetPos = new Vector3(playerX, transform.position.y, transform.position.z);
        }


            float distance = Vector2.Distance(transform.position, targetPos);

        if (distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
            animator.SetFloat("PatrolSpeed", chaseSpeed);
        }
        else
        {
            if (playerX >= minX && playerX <= maxX)
            {
                animator.SetFloat("PatrolSpeed", 0);
                animator.SetBool("IsCollided", true);
            }
            else
            {
                animator.SetFloat("PatrolSpeed", 0);
            }
        }
    }
}