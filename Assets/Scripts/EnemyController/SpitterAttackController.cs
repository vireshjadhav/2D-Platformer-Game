using System.Collections;
using UnityEngine;

public class SpitterAttackController : MonoBehaviour
{ 
    [Header("Spitter Attack Settings")]
    public GameObject venomBallPrefab;
    public Transform attackPoint;
    public float attackCooldown = 2f;


    private Transform player;
    private float nexAttackTime;
    private EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();

        if (enemyController == null)
        {
            //Debug.LogError("EnemyController not foundon parent object!");
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            //Debug.LogError("Player not found! Make sure player has 'Player' tag.");
            return ;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyController != null && enemyController.isPlayerInAttackRange && Time.time >= nexAttackTime)
        {
            Attack();
            nexAttackTime = Time.time + attackCooldown;
        }

    }


    void Attack()
    {
        if (venomBallPrefab == null)
        {
            //Debug.LogError("Venom ball prefab not assigned!");
            return;
        }

        if (attackPoint == null)
        {
            //Debug.LogError("Attack point not assigned!");
            return;
        }


        StartCoroutine(SpawnVenomBallWithDelay(1f));
    }

    private IEnumerator SpawnVenomBallWithDelay(float delay)
    {

        yield return new WaitForSeconds(delay);

        if (enemyController == null || !enemyController.isPlayerInAttackRange)
        {
            yield break;
        }

        bool facingRight = player.position.x > transform.position.x;
        float direction = facingRight ? 1f : -1f;

        GameObject venomBall = Instantiate(venomBallPrefab, attackPoint.position, Quaternion.identity);

        Physics2D.IgnoreCollision(venomBall.GetComponent<Collider2D>(), GetComponent<Collider2D>());


        //Rigidbody2D rb = venomBall.GetComponent<Rigidbody2D>();
        //if (rb != null)
        //{

        //    rb.velocity = new Vector2(direction * 2f, 5f);
        //    Debug.Log("Velocity applied: " + rb.velocity);
        //}


        VenomBallProjectile projectile = venomBall.GetComponent<VenomBallProjectile>();
        if (projectile != null)
        {
            projectile.SetDirection(new Vector2(direction, 1f));
        }

        //Debug.Log("Venom ball thrown! Direction: " + direction);
        //Debug.Log("Attack point position: " + attackPoint.position);
        //Debug.Log("Enemy position: " + transform.position);

        Debug.DrawRay(attackPoint.position, new Vector2(direction, 0) * 2f, Color.red, 1f);
    }
}
