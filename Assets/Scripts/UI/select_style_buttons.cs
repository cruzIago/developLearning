using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class select_style_buttons : MonoBehaviour
{
    public Button[] console_bg; //References to change if any is not interactable if its on use
    public Button[] text_bg;

    private int last_text_bg;
    private int last_console_bg;

    void Start()
    {
        if (PlayerPrefs.HasKey("text_backgrounds"))
        {
            last_text_bg = PlayerPrefs.GetInt("text_backgrounds");
        }
        else
        {
            last_text_bg = 0;
        }

        if (PlayerPrefs.HasKey("console_backgrounds"))
        {
            last_console_bg = PlayerPrefs.GetInt("console_backgrounds");
        }
        else
        {
            last_console_bg = 0;
        }

        setupConsoleButtons();
        setupBGButtons();
    }

    public void OnChangeTextBG()
    {
        GameObject temp_button = EventSystem.current.currentSelectedGameObject;
        if (temp_button != null)
        {
            if (temp_button.name.Contains("1"))
            {
                PlayerPrefs.SetInt("text_backgrounds", 0);
                last_text_bg = 0;
            }
            else if (temp_button.name.Contains("2"))
            {
                PlayerPrefs.SetInt("text_backgrounds", 1);
                last_text_bg = 1;
            }
            else
            {
                PlayerPrefs.SetInt("text_backgrounds", 2);
                last_text_bg = 2;
            }
        }
        else
        {
            Debug.LogError("There is an error on select_style_buttons. Button may not exist");
        }
        setupBGButtons();
    }


    /*
     * To make buttons interactable or not 
     */
    void setupBGButtons()
    {
        for (int i = 0; i < text_bg.Length; i++)
        {
            if (last_text_bg == i)
            {
                text_bg[i].interactable = false;
            }
            else
            {
                text_bg[i].interactable = true;
            }
        }
    }

    public void OnChangeConsoleBG()
    {
        GameObject temp_button = EventSystem.current.currentSelectedGameObject;
        if (temp_button != null)
        {
            if (temp_button.name.Contains("1"))
            {
                PlayerPrefs.SetInt("console_backgrounds", 0);
                last_console_bg = 0;
            }
            else if (temp_button.name.Contains("2"))
            {
                PlayerPrefs.SetInt("console_backgrounds", 1);
                last_console_bg = 1;
            }
            else
            {
                PlayerPrefs.SetInt("console_backgrounds", 2);
                last_console_bg = 2;
            }
        }
        else
        {
            Debug.LogError("There is an error on select_style_buttons. Button may not exist");
        }
        setupConsoleButtons();
    }

    void setupConsoleButtons()
    {
        for (int i = 0; i < console_bg.Length; i++)
        {
            if (last_console_bg == i)
            {
                console_bg[i].interactable = false;
            }
            else
            {
                console_bg[i].interactable = true;
            }
        }
    }

}
