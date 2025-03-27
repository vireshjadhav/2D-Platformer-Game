using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Runtime.CompilerServices;
using UnityEditor;
=======
>>>>>>> 03429261b8644fb86db73bfe784c56af70b2c552
using UnityEngine;

public class PlayerController : MonoBehaviour
{
<<<<<<< HEAD
    private float horizontalInput, verticalInput;
    public Animator animator;
    public float speed = 5f;
    public BoxCollider2D boxCol;

    private Vector2 boxColInitSize;
    private Vector2 boxColInitOffset;
=======
    public Animator animator;
>>>>>>> 03429261b8644fb86db73bfe784c56af70b2c552

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        boxColInitSize = boxCol.size;
        boxColInitOffset = boxCol.offset;
=======

>>>>>>> 03429261b8644fb86db73bfe784c56af70b2c552
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
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
=======
        float speed = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(speed));

        Vector3 scale = transform.localScale;
        if (speed < 0)
        {
            scale.x = -1f * Mathf.Abs(scale.x);
        }
        else if (speed > 0)
>>>>>>> 03429261b8644fb86db73bfe784c56af70b2c552
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
<<<<<<< HEAD

        animator.SetFloat("Speed", horizontal);
    }

    public void PlayJumpAnimation(float vertical)
    {
        if (vertical > 0)
        {
            animator.SetTrigger("Jump");
        }
=======
>>>>>>> 03429261b8644fb86db73bfe784c56af70b2c552
    }
}
