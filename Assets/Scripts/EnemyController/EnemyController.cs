using System;
using System.Collections;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [Header ("Patrol Setting")]
    public float patrolSpeed = 2f;
    private float originalSpeed;
    public Vector3 pointA;
    public Vector3 pointB;
    private Vector3 targetPoint;
    private bool isWaiting = false;

    [Header("Enemy Stats")]
    private int live = 1;
    public bool isPlayerInAttackRange = false;

    private Vector3 baseScale;
    public int currentScale = 1;

    [SerializeField] private Animator animator;
    [SerializeField] private LivesController livesController;

    private Transform playerTarget;

    [SerializeField] private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
        currentScale = baseScale.x >= 0 ? 1 : -1;

        setTargetPoint();
        originalSpeed = patrolSpeed;
        FaceTargetPoint();
    }


    // Update is called once per frame
    void Update()
    {
        if (!isWaiting && !isPlayerInAttackRange)
        {
            PatrolEnemy();
        }

        if (isPlayerInAttackRange && playerTarget != null)
        {
            FacePlayer(playerTarget);
        }
    }

    private void setTargetPoint()
    {
        transform.position = pointA;
        targetPoint = pointB;
    }

    private void PatrolEnemy()
    {
        if (!isPlayerInAttackRange)
        {
            patrolSpeed = originalSpeed;
            FaceTargetPoint();
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, patrolSpeed * Time.deltaTime);
            animator.SetFloat("PatrolSpeed", patrolSpeed);
            if (transform.position == targetPoint)
            {
                patrolSpeed = 0f;
                animator.SetFloat("PatrolSpeed", patrolSpeed);
                StartCoroutine(WaitForSeconds(2.5f));
            }
        }
        else
        {
            patrolSpeed = 0;
        }
    }

    private IEnumerator WaitForSeconds(float delay)
    {
        isWaiting = true;
        animator.SetBool("IsWaiting", isWaiting);

        yield return new WaitForSeconds(delay);

        targetPoint = (targetPoint == pointA) ? pointB : pointA;
        FaceTargetPoint();

        patrolSpeed = originalSpeed;
        isWaiting = false;
        animator.SetFloat("PatrolSpeed", patrolSpeed);
        animator.SetBool("IsWaiting", isWaiting);
    }

    private void SetApplyCurrentScale()
    {
        transform.localScale = new Vector3(currentScale, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null && playerController.canDealDamage)
        {
            //playerController.canDealDamage = false;

            SoundManager.Instance.Play(Sounds.PlayerDamage);
            playerController.HurtAnimation();

            float directionX = collision.transform.position.x > transform.position.x ? 1f : -1f;
            Vector2 pushDirection =new Vector2 (directionX, 0.3f);

            playerController.PushPlayerAway(pushDirection);
            livesController.ReduceLives(live);

            playerController.DamageCoolDown();
        }
    }

    private void PlayAttackAnimation(Transform player)
    {
        FacePlayer(player);
        animator.SetBool("IsCollided", true);
    }


    private void StopAttackAnimation()
    {
        animator.SetBool("IsCollided", false);
        animator.SetFloat("PatrolSpeed", 0);
    }

    private void FacePlayer(Transform player)
    {
        if (player == null) return;

        //float direction = player.position.x - transform.position.x;
        bool shouldFaceRight = player.position.x > transform.position.x;  //direction > 0;
        bool currentlyFacingRight = currentScale > 0;
        if (shouldFaceRight != currentlyFacingRight)
        {
            currentScale *= -1;
            SetApplyCurrentScale();
        }
    }

    private void FaceTargetPoint()
    {
        float dx = targetPoint.x - transform.position.x;
        int desired = dx > 0 ? 1 : -1;
        if (currentScale != desired)
        {
            currentScale *= -1;
            SetApplyCurrentScale();
        }

    }

    public void PlayerEnteredAttackRange(Transform player)
    {
        isPlayerInAttackRange = true;
        playerTarget = player;
        PlayAttackAnimation(player.transform);
    }

    public void PlayerExitedAttackRange()
    {
        isPlayerInAttackRange = false;
        playerTarget = null;
        StopAttackAnimation();

        FaceTargetPoint();
    }
}