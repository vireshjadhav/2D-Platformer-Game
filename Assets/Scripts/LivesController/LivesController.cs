using UnityEngine;


public class LivesController : MonoBehaviour
{
    private int life;
    public GameObject[] hearts;
    private bool dead = false;


    [SerializeField] public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        life = hearts.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true)
        {
            //Debug.Log("Death animation will be play");
            playerController.KillPlayer();
        }
    }

    public void ReduceLives(int heart)
    {
        if (dead) return;

        life -= heart;

        if (life >= 0 && life < hearts.Length && hearts[life] != null)
        {
            Destroy(hearts[life].gameObject);
        }

        if (life < 1)
        {
            dead = true;
            life = 0;

            int playerLayer = LayerMask.NameToLayer("Player");
            int enemyLayer = LayerMask.NameToLayer("Enemy");

            Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);


            //Collider2D playerCol = playerController.GetComponent<Collider2D>();
            //EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            //foreach (EnemyController enemy in enemies)
            //{
            //    Collider2D enemyCol = enemy.GetComponent<Collider2D>();
            //    if (enemyCol != null && playerCol != null)
            //    {
            //        Physics2D.IgnoreCollision(playerCol, enemyCol, true);
            //    }
            //}

            //VenomBallProjectile[] venomBalls = FindObjectsOfType<VenomBallProjectile>();
            //foreach (VenomBallProjectile ball in venomBalls)
            //{
            //    Collider2D ballCol = ball.GetComponent<Collider2D>();
            //    if (ballCol != null && playerCol != null)
            //    {
            //        Physics2D.IgnoreCollision(playerCol, ballCol, true);
            //    }
            //}

        }
    }

    public void ResetPlayer()
    {
        life = hearts.Length;
        dead = false;

        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

    }

    public bool IsDead()
    {
        return dead;
    }
}
