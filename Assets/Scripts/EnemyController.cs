using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{

    public float patrolSpeed = 0f;
    public Vector3 pointA;
    public Vector3 pointB;
    public int currentScale;
    private int currentScaleX;
    private Vector3 targetPoint;

    [SerializeField]private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        setTargetPoint();
        currentScaleX = currentScale;
    }

    private void setTargetPoint()
    {
        transform.position = pointA;
        targetPoint = pointB;
    }

    // Update is called once per frame
    void Update()
    {
        PatrolEnemy();
    }

    private void PatrolEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, patrolSpeed*Time.deltaTime);
        animator.SetFloat("PatrolSpeed", patrolSpeed);
        if(transform.position == targetPoint )
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
            currentScaleX *= -1;
            SetApplyCurrentScale();
        }
    }

    private void SetApplyCurrentScale()
    {
        transform.localScale = new Vector3(currentScaleX, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.KillPlayer();
        }
    }

}
