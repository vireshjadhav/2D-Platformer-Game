using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCompleteController : MonoBehaviour
{
    [Header("Game Complete:UI Reference")]
    [SerializeField] private Button quitButton;
    [SerializeField] private Button lobbyButton;
    [SerializeField] public GameObject gameCompletePanel;

    [Header("Door Reference")]
    [SerializeField] private Transform doorCenterPoint;

    private int currentSceneIndex;

    [SerializeField] private ParticleController confettiEffect1;
    [SerializeField] private ParticleController confettiEffect2;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (doorCenterPoint == null)
            doorCenterPoint = transform;
    }


    public void Awake()
    {
        quitButton.onClick.AddListener(QuitGame);
        lobbyButton.onClick.AddListener(loadLobby);
    }

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Player Hit Game Complete Object");
            StartCoroutine(CompleteGameSequence(collision.gameObject));
        }
    }

    private IEnumerator CompleteGameSequence(GameObject player)
    {
        Animator playerAnimator = player.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);
        }

        yield return StartCoroutine(MovePlayerToDoorCenter(player));

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.SetGameWon();


        confettiEffect1.PlayPlayerWinEffect();
        confettiEffect2.PlayPlayerWinEffect();

        yield return new WaitForSeconds(3f);
        gameCompletePanel.SetActive(true);
        Debug.Log("GameCompletePanel" + gameCompletePanel.activeInHierarchy);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private IEnumerator MovePlayerToDoorCenter(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        Animator playerAnimator = playerController.GetComponent<Animator>();

        playerController.enabled = false;
        playerRb.velocity = Vector2.zero;
        playerRb.isKinematic = true;

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);
            playerAnimator.SetBool("Crouch", false);
        }

        Vector3 targetPosition = new Vector3(doorCenterPoint.position.x, doorCenterPoint.position.y, doorCenterPoint.position.z);

        float duration = 0.5f;
        float elapsedTime = 0f;
        Vector3 startPosition = player.transform.position;

        while (elapsedTime < duration)
        {
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = targetPosition;

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        SoundManager.Instance.Play(Sounds.ButtonClick);
        Application.Quit();
    }


    private void loadLobby()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene(0);
    }
}
