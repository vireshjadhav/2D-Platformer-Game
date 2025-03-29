using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    public Animator animator;
    public float speed = 5f;
    public BoxCollider2D boxCol;

    private Vector2 boxColInitSize;
    private Vector2 boxColInitOffset;

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


        PlayJumpAnimation(verticalInput);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Crouch(true);
        }
        else
        {
            Crouch(false);
        }

        PlayRunWalkOrIdleAnimation(horizontalInput);
  
    }

    public void Crouch(bool crouch)
    {
        if (crouch == true)
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

    private float GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Debug.Log($"horizontal: {horizontalInput}, vertical: {verticalInput}");
        return Mathf.Max(horizontalInput, verticalInput);
    }

    private void PlayRunWalkOrIdleAnimation(float horizontal)
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

    public void PlayJumpAnimation(float vertical)
    {
        if (vertical > 0)
        {
            animator.SetTrigger("Jump");
        }
    }
}
