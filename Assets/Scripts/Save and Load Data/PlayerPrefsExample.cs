using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPrefsExample : MonoBehaviour
{
    [SerializeField] private TMP_InputField input; // Username
    [SerializeField] private TMP_Text txtDisplay;

    public void SaveData()
    {
        PlayerPrefs.SetString("SAVE_DATA", input.text);
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("SAVE_DATA"))
        {
            txtDisplay.SetText(PlayerPrefs.GetString("SAVE_DATA"));
        }
        else
        {
            {
                Debug.Log("No data to load.");
            }
        }
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteKey("SAVE_DATA");
    }
}
