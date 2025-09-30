using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button playButton;
    public Button optionButton;
    public Button quitButton;
    public Button backBLevelSelection;
    public Button backButtonOption;
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider effectsVolume;
    public GameObject LevelSelection;
    public GameObject OptionPopUp;
    public Toggle muteToggle;


    private void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        optionButton.onClick.AddListener(OptionsPopUp);
        quitButton.onClick.AddListener(Quit);
        backBLevelSelection.onClick.AddListener(GoBackLobby);
        backButtonOption.onClick.AddListener(GoBack);

        masterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);
        effectsVolume.onValueChanged.AddListener(OnEffectVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggled);

    }

    private void OnEnable()
    {
        if(SoundManager.Instance ==  null) return;

        muteToggle.isOn = SoundManager.Instance.IsMute;
        masterVolume.value = SoundManager.Instance.MasterVolume;
        musicVolume.value = SoundManager.Instance.MusicVolume;
        effectsVolume.value = SoundManager.Instance.EffectVolume;
    }

    private void OnMuteToggled(bool isMute)
    {
        SoundManager.Instance.Mute(isMute);
    }

    private void OnMasterVolumeChanged(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        SoundManager.Instance.SetMusicVolume(value);
    }

    private void OnEffectVolumeChanged(float value)
    {
        SoundManager.Instance.SetEffectVolume(value);
    }

    private void GoBackLobby()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        LevelSelection.SetActive(false);
    }

    private void GoBack()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        OptionPopUp.SetActive(false);
    }

    private void OptionsPopUp()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        OptionPopUp.SetActive(true);
    }

    private void Quit()
    {
        Debug.Log("Application Closed");
        Application.Quit();
    }

    private void PlayGame()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        LevelSelection.SetActive(true);
    }
}