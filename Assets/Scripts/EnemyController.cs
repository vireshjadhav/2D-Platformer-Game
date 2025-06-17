using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{

    public float patrolSpeed = 0f;
    private float originalSpeed;
    public Vector3 pointA;
    public Vector3 pointB;
    public int currentScale;
    private int currentScaleX;
    private Vector3 targetPoint;
    private bool isWaiting = false;
    private int death = 1;

    [SerializeField]private Animator animator;
    [SerializeField] private LivesController livesController;

    // Start is called before the first frame update
    void Start()
    {
        setTargetPoint();
        currentScaleX = currentScale;
        originalSpeed = patrolSpeed;
    }

    private void setTargetPoint()
    {
        transform.position = pointA;
        targetPoint = pointB;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting)
        {
            PatrolEnemy();
        }
    }

    private void PatrolEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, patrolSpeed*Time.deltaTime);
        animator.SetFloat("PatrolSpeed", patrolSpeed);
        if(transform.position == targetPoint )
        {
            patrolSpeed = 0f;
            animator.SetFloat("PatrolSpeed", patrolSpeed);   
            StartCoroutine(WaitForSeconds(2.5f));
        }
    }

    private IEnumerator WaitForSeconds(float delay)
    {
        isWaiting = true;
        animator.SetBool("IsWaiting", isWaiting);

        yield return new WaitForSeconds(delay);
        
        targetPoint = (targetPoint == pointA) ? pointB : pointA;
        currentScaleX *= -1;
        SetApplyCurrentScale();

        patrolSpeed = originalSpeed;
        isWaiting = false;
        animator.SetFloat("PatrolSpeed", patrolSpeed);
        animator.SetBool("IsWaiting", isWaiting);
    }

    private void SetApplyCurrentScale()
    {
        transform.localScale = new Vector3(currentScaleX, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Player hit the enemy collider");
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            livesController.ReduceLives(death);
        }
    }

}
