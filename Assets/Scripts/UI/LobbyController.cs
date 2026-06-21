using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button playButton;
    public Button optionButton;
    public Button instructionButton;
    public Button quitButton;
    public Button backBLevelSelection;
    public Button backButtonOption;
    public Button backButtonInstruction;
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider effectsVolume;
    public GameObject levelSelection;
    public GameObject optionPopUp;
    public GameObject instructionPanel;
    public Toggle muteToggle;


    private void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        optionButton.onClick.AddListener(OptionsPopUp);
        instructionButton.onClick.AddListener(Instruction);
        quitButton.onClick.AddListener(Quit);
        backBLevelSelection.onClick.AddListener(GoBackLobby);
        backButtonOption.onClick.AddListener(GoBack);
        backButtonInstruction.onClick.AddListener(GoBackInstruct);

        masterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);
        musicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);
        effectsVolume.onValueChanged.AddListener(OnEffectVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggled);


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

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

        if (isMute)
        {
            masterVolume.interactable = false;
        }
        else
        {
            masterVolume.interactable = true;
        }
    }

    private void OnMasterVolumeChanged(float value)
    {
        SoundManager.Instance.SetMasterVolume(value);

        if (value <= 0.01f)
        {
            muteToggle.isOn = true;
        }
        else
        {
            muteToggle.isOn = false;
        }
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
        levelSelection.SetActive(false);
    }

    private void GoBack()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        optionPopUp.SetActive(false);
    }

    private void OptionsPopUp()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        optionPopUp.SetActive(true);
    }

    private void Quit()
    {
        Debug.Log("Application Closed");
        SoundManager.Instance.Play(Sounds.ButtonClick);
        Application.Quit();
    }

    private void PlayGame()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        levelSelection.SetActive(true);
    }

    private void Instruction()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        instructionPanel.SetActive(true);
    }

    private void GoBackInstruct()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        instructionPanel.SetActive(false);
    }
}