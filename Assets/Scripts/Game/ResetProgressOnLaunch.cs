using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetProgressOnLaunch : MonoBehaviour
{
    [SerializeField] private bool resetOnNextLaunch = false;
    private const string RESET_FLAG_KEY = "HasResetBefore";
    private const string FORCE_RESET_KEY = "ForceResetOnNextLaunch";

    void Awake()
    {
        bool forceReset = PlayerPrefs.GetInt(FORCE_RESET_KEY, 0) == 1;
        bool hasResetBefore = PlayerPrefs.GetInt(RESET_FLAG_KEY, 0) == 1;

        if ( forceReset)
        {

            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            PlayerPrefs.SetInt("Level1", (int)LevelStatus.Unlocked);
            PlayerPrefs.Save();

            PlayerPrefs.SetInt(RESET_FLAG_KEY, 1);
            PlayerPrefs.DeleteKey(FORCE_RESET_KEY);
            PlayerPrefs.Save();

            Debug.Log("Force reset complete - Level1 unlocked");
        }
        else if(!hasResetBefore && resetOnNextLaunch)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            PlayerPrefs.SetInt("Level1", (int)LevelStatus.Unlocked);
            PlayerPrefs.Save();


            PlayerPrefs.SetInt(RESET_FLAG_KEY, 1);
            PlayerPrefs.Save();

            Debug.Log("First launch reset complete - Level1 unlocked");
        }
        else
        {
            Debug.Log("No reset needed - Progress preserved");
            Debug.Log("forceReset: " + forceReset + ", hasResetBefore: " + hasResetBefore + ", resetOnNextLaunch: " + resetOnNextLaunch);
        }
    }

    public static void SetForceResetOnNextLaunch()
    {
        PlayerPrefs.SetInt(FORCE_RESET_KEY, 1);
        PlayerPrefs.Save();
        Debug.Log("Force reset set for next launch");
    }
}
