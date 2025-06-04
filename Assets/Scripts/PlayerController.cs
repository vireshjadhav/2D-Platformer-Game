using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rd2d;
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpForce = 25f;

    private float horizontalInput, verticalInput;

    public LevelOverController levelOverController;
    public float speed = 5f;
    public BoxCollider2D boxCol;
    private bool crouch = false;
    private bool isGrounded = false;

    private Vector2 boxColInitSize;
    private Vector2 boxColInitOffset;

    private void Awake()
    {
        Debug.Log("Player Controller awake");
        rd2d = GetComponent<Rigidbody2D>();
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
        CrouchAnimantion();
        PlayMovementAnimation(horizontalInput, verticalInput);
        MovePlayer(horizontalInput, verticalInput);
        MovePlayerVertically(verticalInput);
    }

    public void MovePlayerVertically(float vertical)
    {
        if (vertical > 0 && isGrounded)
        {
            animator.SetTrigger("Jump");
            Debug.Log("Jumping 1");
            rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("PlayerDeath"))
        {
            PlayerDied();
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

        if (other.transform.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void PlayerDied()
    {
        Destroy(gameObject);
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //        if(other.transform.tag == "Ground")
    //        {
    //            isGrounded = true;
    //        }
    //}

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag == "Ground")
        {
            isGrounded = false;
        }
    }



    private void CrouchAnimantion()
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

    private void MovePlayer(float horizontal, float vertical)
    {
        Vector3 position = transform.position;
        position.x += horizontal* speed * Time.deltaTime;
        transform.position = position;
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

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void PlayMovementAnimation(float horizontal, float vertical)
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
}
