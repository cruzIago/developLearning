using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Manages things that system may need like language and sending logs. 
 */
public class game_manager : MonoBehaviour
{
    public static game_manager instance;
    private static Global globalSettings;
    private static file_writer writer;
    private static mono_gmail sender;

    public Sprite[] texts_backgrounds;
    public Sprite[] console_backgrounds;

    //Singleton
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            globalSettings = new Global();
            writer = new file_writer();
            writer.writeOnFile("prueba");
            sender = new mono_gmail();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }


        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Application.quitting += OnExitApplication;
    }

    void Update()
    {

    }

    //Used to write from everywhere on the log file
    public static void writeOnFile(string text)
    {
        writer.writeOnFile(text);
    }

    //When app is closed, it sends an email through mono_gmail.cs
    void OnExitApplication()
    {
        writer.closeStream();
        sender.SendLog(writer.path);
    }

    //Retrieves strings from language settings
    public static string getStringFromLang(int id) {
        string idd = "" + id;
        return globalSettings.languages.getString(idd);
    }

    //To change console and text style
    public void changeStyle(Image texts, Image console) {
        if (texts != null) {
            int temp_text_background = 0;
            temp_text_background = PlayerPrefs.GetInt("text_backgrounds");
            texts.sprite = texts_backgrounds[temp_text_background];
        }
        if (console != null) {
            int temp_console_background = 0;
            temp_console_background = PlayerPrefs.GetInt("console_backgrounds");
            console.sprite = console_backgrounds[temp_console_background];
        }
    }
    
}
