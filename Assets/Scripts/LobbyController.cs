using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public GameObject LevelSelection;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        playButton.onClick.AddListener(Quit);

    }

    private void Quit()
    {
        Application.Quit();
    }

    private void PlayGame()
    {
        LevelSelection.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
