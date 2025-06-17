using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D boxCol;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public LevelCompleteController levelCompleteController;
    public GameOverController gameOverController;
    public ScoreController scoreController;

    [Header("Movement")]
    public float speed = 5f;
    [SerializeField] private float jumpPower;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpMultiplier;


    private float horizontalInput;
    private bool isGrounded;
    private bool isJumping;
    private float jumpCounter;
    private bool isDead = false;
    private bool crouch = false;


    private Vector2 vecGravity;
    private Vector2 boxColInitSize;
    private Vector2 boxColInitOffset;


    private void Awake()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        boxColInitSize = boxCol.size;
        boxColInitOffset = boxCol.offset;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        CrouchAnimation();
        PlayMovementAnimation(horizontalInput);
        MovePlayer(horizontalInput);
        HandleJump();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void MovePlayer(float horizontal)
    {
        if (isDead) return;

        Vector3 position = transform.position;
        position.x += horizontal * speed * Time.deltaTime;
        transform.position = position;
    }

    public void HandleJump()
    {
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.7f, 0.2f), CapsuleDirection2D.Horizontal, 0f, groundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded && !isDead)
        {
            animator.SetTrigger("Jump");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
            isJumping = true;
            jumpCounter = 0;
        }

        if(rb2d.velocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime)
            {
                isJumping = false;
            }

            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > 0.5f)
            {
                currentJumpM = jumpMultiplier * (1 - t);
            }

            rb2d.velocity += vecGravity * currentJumpM * Time.deltaTime;
        }

        if(Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpCounter = 0;


            if(rb2d.velocity.y > 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y*0.6f);
            }
        }

        if(rb2d.velocity.y <0)
        {
            rb2d.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
    }

    private void CrouchAnimation()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Crouch(false);
        }
    }

    public void Crouch(bool crouch)
    {
        if (crouch)
        {
            float offX = -0.0978f;
            float offY = 0.5947f;

            float sizeX = 0.6988f;
            float sizeY = 1.3398f;

            boxCol.size = new Vector2(sizeX, sizeY);
            boxCol.offset = new Vector2(offX, offY);
        }
        else
        {
            boxCol.size = boxColInitSize;
            boxCol.offset = boxColInitOffset;
        }

        this.crouch = crouch;
        animator.SetBool("Crouch", crouch);
    }

    private void PlayMovementAnimation(float horizontal)
    {
        Vector3 scale = transform.localScale;
        if (horizontal < 0)
        {
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (horizontal > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("KillZone"))
        {
            Debug.Log("Player entered the killzone");
            PlayerDied();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    private void PlayerDied()
    {
        Destroy(gameObject);
        gameOverController.PlayerDied();
    }

    public void PickUpKey()
    {
        scoreController.IncreseScore(10);
    }

    public void KillPlayer()
    {
        Debug.Log("Player is killed by enemy.");
        animator.SetBool("IsDead", true);
        isDead = true;
        StartCoroutine(DestroyAfterDelay(2f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        gameOverController.PlayerDied();
    }
}
