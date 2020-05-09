using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class game_manager : MonoBehaviour
{
    public static game_manager instance;
    public static Global globalSettings;
    private static file_writer writer;
    private static mono_gmail sender;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        globalSettings = new Global();

        writer = new file_writer();
        writer.writeOnFile("prueba");
        sender = new mono_gmail();

        print(globalSettings.languages.getString("101")); //To test if languages are working

        Application.quitting += OnExitApplication;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    void OnExitApplication()
    {
        writer.closeStream();
        sender.SendLog(writer.path);
    }





}
