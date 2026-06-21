using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteController : MonoBehaviour
{
    [SerializeField] private Transform doorCenterPoint;
    [SerializeField] PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        if (doorCenterPoint == null)
            doorCenterPoint = transform;

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            StartCoroutine(CompleteLevelSequence(other.gameObject));
        }
    }

    private IEnumerator CompleteLevelSequence(GameObject player)
    {
        Animator playerAnimator = player.GetComponent<Animator>();
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", 0f);
        }

        yield return StartCoroutine(MovePlayerToDoorCenter(player));

        playerController.SetGameWon();
        LevelManager.Instance.MarkCurrentLevelComplete();

        yield return new WaitForSeconds(2f);
        LoadNextLevel();
    }

    private IEnumerator MovePlayerToDoorCenter(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        Rigidbody2D playerRb = playerController.GetComponent<Rigidbody2D>();
        Animator playerAnimator = playerController.GetComponent<Animator>();

        playerController.enabled = false;
        playerRb.velocity = Vector2.zero;
        playerRb.isKinematic = true;

        if(playerAnimator != null)
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

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int totalNumberOfScene = SceneManager.sceneCountInBuildSettings;
        SoundManager.Instance.RestartMusic();

        if (nextSceneIndex < totalNumberOfScene)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
