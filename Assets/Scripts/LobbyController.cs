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
        SoundManager.Instance.Play(Sounds.ButtonClick);
        LevelSelection.SetActive(true);
    }
}
