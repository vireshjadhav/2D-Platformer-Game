using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetProgressOnLaunch : MonoBehaviour
{
    [SerializeField] private bool resetOnNextLaunch = true;
    private const string RESET_FLAG_KEY = "HasResetBefore";

    void Awake()
    {
        bool hasResetBefore = PlayerPrefs.GetInt(RESET_FLAG_KEY, 0) == 1;

        if (resetOnNextLaunch && !hasResetBefore)
        {

            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            PlayerPrefs.SetInt("Level1", (int)LevelStatus.Unlocked);
            PlayerPrefs.Save();

            PlayerPrefs.SetInt(RESET_FLAG_KEY, 1);
            PlayerPrefs.Save();

            resetOnNextLaunch = false;
        }
        else if(hasResetBefore)
        {
            Debug.Log("NOT first launch - Progress preserved (Level2 should be unlocked if completed)");
        }
        else
        {
            Debug.Log("Reset Disable in Inspector - Progress preserved");
        }
    }
}
