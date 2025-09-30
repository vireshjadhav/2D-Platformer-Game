using System;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
            Debug.Log("[Chase] chasing player at: " + player.position);
            ChaseAndAttack();
        }
    }


    void ChaseAndAttack()
    {
        float minX = Mathf.Min(enemyController.pointA.x, enemyController.pointB.x);
        float maxX = Mathf.Max(enemyController.pointA.x, enemyController.pointB.x);

        Debug.Log("[Chase] Enemy at: " + transform.position + " | Player at: " + player.position);

        float playerX = player.position.x;
        float enemyX = transform.position.x;
        Vector3 targetPos;

        if (playerX < minX)
        {
            targetPos = new Vector3(minX, transform.position.y, transform.position.z);
            Debug.Log("[Chase] Player left of patrol bounds. Moving to PointA.");
        }
        else if (playerX > maxX)
        {
            targetPos = new Vector3(maxX, transform.position.y, transform.position.z);
            Debug.Log("[Chase] Player right of patrol bounds. Moving to PointB.");
        }
        else
        {
            targetPos = new Vector3(playerX, transform.position.y, transform.position.z);
            Debug.Log("[Chase] Player inside patrol bounds. Chasing player.");
        }

            //float targetX = Mathf.Clamp(player.position.x, minX, maxX);

            //Vector3 targetPos = new Vector3(targetX, transform.position.y, transform.position.z);

            float distance = Vector2.Distance(transform.position, targetPos);

            if (distance > stopDistance)
        {
            Debug.Log("[Chase] Moving towards player. Distance: " + distance);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
            animator.SetFloat("PatrolSpeed", chaseSpeed);
        }
        else
        {
            if (playerX >= minX && playerX <= maxX)
            {
                Debug.Log("[Attack] Player in bite range! Attacking now.");
                animator.SetFloat("PatrolSpeed", 0);
                animator.SetTrigger("Attack");
            }
            else
            {
                Debug.Log("[Chase] Reached patrol boundary, stopping chase.");
                animator.SetFloat("PatrolSpeed", 0);
            }
        }
    }
}