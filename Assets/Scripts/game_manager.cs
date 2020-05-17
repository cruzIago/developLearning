using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class game_manager : MonoBehaviour
{
    public static game_manager instance;
    private static Global globalSettings;
    private static file_writer writer;
    private static mono_gmail sender;

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
        

        print(globalSettings.languages.getString("-1")); //To test if languages are working
        
        //Application.quitting += OnExitApplication;
    }

    void Update()
    {

    }

    //Used to write from everywhere on the log file
    public static void writeOnFile(string text)
    {
        writer.writeOnFile(text);
    }

    void OnExitApplication()
    {
        writer.closeStream();
        sender.SendLog(writer.path);
    }

    public static string getStringFromLang(int id) {
        string idd = "" + id;
        return globalSettings.languages.getString(idd);
    }





}
