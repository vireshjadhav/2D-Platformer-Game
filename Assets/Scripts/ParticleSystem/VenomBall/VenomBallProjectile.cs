using System;
using System.Collections;
using UnityEngine;

public class VenomBallProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 5f;
    public float lifeTime = 3f;
    public float knockBackForce = 2f;
    //public float arcHeight = 5f;

    //[SerializeField] private PlayerController playerController;
    //[SerializeField] private LivesController livesController;

    private Rigidbody2D rb;
    private Collider2D col;
    public float arcHeight = 8f;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = rb.GetComponent<Collider2D>();

        if (col != null)
        {
            col.enabled = false;
            Invoke(nameof(EnableCollider), 0.1f);
        }
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {

        if (rb == null) return;


        Vector2 moveDirection = new Vector2(dir.x * speed, arcHeight);
        rb.velocity = moveDirection;
    }

    void EnableCollider()
    {
        if (rb != null)
            col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController hitPlayer = collision.GetComponent<PlayerController>();

            if (hitPlayer == null || !hitPlayer.canDealDamage) return;

            //hitPlayer.canDealDamage = false;

            SoundManager.Instance.Play(Sounds.PlayerDamage);

            //if (hitPlayer != null && hitPlayer.canDealDamage)
            //{
            LivesController lives = FindObjectOfType<LivesController>();
            if (lives != null && !lives.IsDead())
            {
                lives.ReduceLives(1);
            }

            hitPlayer.HurtAnimation();

            //Vector2 knockDir = (hitPlayer.transform.position - transform.position).normalized;
            //hitPlayer.PushPlayerAway(knockDir * knockBackForce);

            float directionX = collision.transform.position.x > transform.position.x ? 1f : -1f;
            Vector2 pushDirection = new Vector2(directionX, 0.3f);
            hitPlayer.PushPlayerAway(pushDirection);


            hitPlayer.DamageCoolDown();
            //}

            Destroy(gameObject);
        }

        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
