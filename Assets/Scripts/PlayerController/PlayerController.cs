using System.Collections;
using UnityEngine;
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
    public GameCompleteController gameCompleteController;

    [Header("Movement")]
    public float speed = 5f;
    [SerializeField] private float jumpPower = 5.0f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float jumpTime = 0.3f;
    [SerializeField] private float jumpMultiplier = 2f;

    [Header("KnockBack")]
    [SerializeField] private float pushStrenght = 0.5f;
    [SerializeField] private float knockbackDuration = 0.05f;
    [SerializeField] private float knockBackMultiplier = 0.5f;

    [Header("Invincibility")]
    [SerializeField] private float damageCooldown = 1.5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float blinkDuration = 0.1f;
    public bool canDealDamage = true;

    private bool isKnockedBack = false;
    private bool isDead = false;
    private bool crouch = false;
    private float horizontalInput;
    private bool isGrounded;
    private bool isJumping;
    private float jumpCounter;
    private Vector2 vecGravity;
    private Vector2 boxColInitSize;
    private Vector2 boxColInitOffset;


    public bool gameWon { get; private set; } = false;

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
        if (!isKnockedBack)
        {
            HandleCrouch();
            PlayMovementAnimation(horizontalInput);
            HandleMovement(horizontalInput);
            HandleJump();
        }
    }
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }
    private void HandleMovement(float horizontal)
    {
        if (isDead || gameWon) return;
       
        if (isKnockedBack)
        {
            speed = Mathf.MoveTowards(speed, 5f, 0.1f * Time.deltaTime);
        }
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
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower); isJumping = true;
            jumpCounter = 0;
        }
        if (rb2d.velocity.y > 0 && isJumping)
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
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpCounter = 0;
            if (rb2d.velocity.y > 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.6f);
            }
        }
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
    }
    private void HandleCrouch()
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
        transform.localScale = scale; animator.SetFloat("Speed", Mathf.Abs(horizontal));
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
    public void HurtAnimation()
    {
        CameraShakeController.instance.TriggerShake(CameraShakeController.instance.duration, CameraShakeController.instance.magnitude);
        animator.SetTrigger("Hurt");
    }

    private void PlayerDied()
    {
        gameOverController.PlayerDied();
        gameObject.SetActive(false);
        //Destroy(gameObject);
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
        PlayerDied();
    }
    public void SetGameWon()
    {
        Debug.Log("Game Won");
        gameWon = true;
        rb2d.velocity = Vector3.zero;
        rb2d.bodyType = RigidbodyType2D.Static;
    }
    public void PushPlayerAway(Vector2 direction)
    {

        isKnockedBack = true; 
        rb2d.velocity = Vector2.zero;

        Vector2 knockDir = new Vector2(direction.x * pushStrenght, direction.y * pushStrenght * knockBackMultiplier);
        rb2d.AddForce(knockDir, ForceMode2D.Impulse);

        HurtAnimation();

        StartCoroutine(knockBackCoroutine());
    }
    private IEnumerator knockBackCoroutine()
    {
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
    }

    public void TakeDamage()
    {
        isKnockedBack = true;
    }


    public void DamageCoolDown()
    {

        StartCoroutine(WaitForSometime(damageCooldown));
    }

    private IEnumerator WaitForSometime(float delay)
    {
        float elapsedTime = 0f;

        while (elapsedTime < delay)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            elapsedTime += blinkDuration;
            yield return new WaitForSeconds(blinkDuration);
        }
        spriteRenderer.enabled = true;

        //yield return new WaitForSeconds(delay);
        canDealDamage = true;
    }
}