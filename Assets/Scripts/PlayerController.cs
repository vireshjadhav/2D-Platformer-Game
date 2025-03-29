using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    public Animator animator;
    public float speed = 5f;
    public float jumpForce = 25f;
    public BoxCollider2D boxCol;
    private Rigidbody2D rd2d;

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
    }

    private void CrouchAnimantion()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Crouch(true);
        }
        else
        {
            Crouch(false);
        }
    }

    private void MovePlayer(float horizontal, float vertical)
    {
        Vector3 position = transform.position;
        position.x += horizontal* speed * Time.deltaTime;
        transform.position = position;

        if (verticalInput > 0)
        {
            rd2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Force);
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

        animator.SetBool("Crouch", crouch);
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
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

        if (vertical > 0)
        {
            animator.SetTrigger("Jump");
        }
    }

}
